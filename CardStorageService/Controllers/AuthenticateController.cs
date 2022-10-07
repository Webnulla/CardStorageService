using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CardStorageService.Models;
using CardStorageService.Models.Requests;
using CardStorageService.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IValidator<AuthenticationRequest> _validator;

        public AuthenticateController(IAuthenticateService authenticateService, IValidator<AuthenticationRequest> validator)
        {
            _authenticateService = authenticateService;
            _validator = validator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthenticationRequest authenticationRequest)
        {
            ValidationResult validationResult = _validator.Validate(authenticationRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }
            
            AuthenticationResponse authenticationResponse = _authenticateService.Login(authenticationRequest);

            if (authenticationResponse.Status == Models.AuthenticationStatus.Success)
            {
                Response.Headers.Add("X-Session-Token", authenticationResponse.SessionInfo.SessionToken);
            }

            return Ok(authenticationResponse);
        }

        [HttpGet]
        [Route("session")]
        [ProducesResponseType(typeof(SessionInfo), StatusCodes.Status200OK)]
        public IActionResult GetSessionInfo()
        {
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                //Bearer
                var scheme = headerValue.Scheme;
                //Token
                var sessionToken = headerValue.Parameter;

                if (string.IsNullOrEmpty(sessionToken))
                {
                    return Unauthorized();
                }

                SessionInfo sessionInfo = _authenticateService.GetSessionInfo(sessionToken);
                if (sessionInfo == null)
                {
                    return Unauthorized();
                }

                return Ok(sessionInfo);
            }

            return Unauthorized();
        }
    } 
}
