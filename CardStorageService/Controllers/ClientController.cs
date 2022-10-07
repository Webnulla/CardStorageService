using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CardStorageService.Data;
using CardStorageService.Models.Requests;
using CardStorageService.Services;
using CardStorageService.Services.Impl;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepositoryService _repository;
        private readonly IValidator<CreateClientRequest> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientRepository> _logger;

        public ClientController(
            ILogger<ClientRepository> logger, 
            IClientRepositoryService repository, 
            IValidator<CreateClientRequest> validator,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateClientResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] CreateClientRequest request)
        {
            try
            {
                ValidationResult validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }
                
                // var clientId = _repository.Create(new Client
                // {
                //     FirstName = request.FirstName,
                //     Surname = request.Surname,
                //     Patronymic = request.Patronymc
                // });
                var clientId = _repository.Create(_mapper.Map<Client>(request));
                return Ok(new CreateClientResponse
                {
                    ClientId = clientId
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create client error");
                return Ok(new CreateClientResponse
                {
                    ErrorCode = 912,
                    ErrorMessage = "Create client error"
                });
            }
        }
    }
}