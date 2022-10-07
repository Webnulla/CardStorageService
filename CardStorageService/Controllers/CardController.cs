using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CardStorageService.Data;
using CardStorageService.Models;
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
    [Route("api/card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardRepositoryService _repository;
        private readonly ILogger<CardRepository> _logger;
        private readonly IValidator<CreateCardRequest> _validator;
        private readonly IMapper _mapper;

        public CardController(
            ICardRepositoryService repository, 
            ILogger<CardRepository> logger, 
            IValidator<CreateCardRequest> validator,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] CreateCardRequest request)
        {
            try
            {
                ValidationResult validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }
                
                // var cardId = _repository.Create(new Card
                // {
                //     ClientId = request.ClientId,
                //     CardNo = request.CardNo,
                //     ExDate = request.ExpDate,
                //     CVV2 = request.CVV2
                // });
                var cardId = _repository.Create(_mapper.Map<Card>(request));
                return Ok(new CreateCardResponse
                {
                    CardId = cardId.ToString()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create card error");
                return Ok(new CreateCardResponse
                {
                    ErrorCode = 1012,
                    ErrorMessage = "Create card error"
                });
            }
        }

        [HttpGet("get-by-client-id")]
        [ProducesResponseType(typeof(GetCardResponse), StatusCodes.Status200OK)]
        public IActionResult GetByClientId([FromQuery] int clientId)
        {
            try
            {
                var cards = _repository.GetByClientId(clientId);
                return Ok(new GetCardResponse
                {
                    Cards = _mapper.Map<List<CardDto>>(cards)
                    // Cards = cards.Select(card => new CardDto
                    // {
                    //     CardId = card.CardId,
                    //     CardNo = card.CardNo,
                    //     CVV2 = card.CVV2,
                    //     Name = card.Name,
                    //     ExpDate = card.ExDate.ToString("MM/yy")
                    // }).ToList()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get cards error");
                return Ok(new GetCardResponse
                {
                    ErrorCode = 1013,
                    ErrorMessage = "Get cards error"
                });
            }
        }
        
        [HttpGet("all")]
        public ActionResult<IList<Card>> GetAllCards()
        {
            try
            {
                var card = _repository.GetAll();
                return Ok(new GetCardResponse
                {
                    Cards = _mapper.Map<List<CardDto>>(card)
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get cards error");
                return Ok(new GetCardResponse
                {
                    ErrorCode = 1013,
                    ErrorMessage = "Get cards error"
                });
            }
        }
    }
}