using System;
using System.Threading.Tasks;
using CardStorageService.Data;
using Castle.Core.Logging;
using ClientServiceProtos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CardStorageService.Services.Impl;

public class ClientService : ClientServiceProtos.ClientService.ClientServiceBase
{
    private readonly IClientRepositoryService _clientRepositoryService;
    private readonly ILogger<ClientService> _logger;

    public ClientService(IClientRepositoryService clientRepositoryService, ILogger<ClientService> logger)
    {
        _clientRepositoryService = clientRepositoryService;
        _logger = logger;
    }

    public override Task<CreateClientResponse> Create(CreateClientRequest request, ServerCallContext context)
    {
        try
        {
            var clientId = _clientRepositoryService.Create(new Client
            {
                FirstName = request.FirstName,
                Surname = request.Surname,
                Patronymic = request.LastName
            });

            var response = new CreateClientResponse
            {
                ClientId = clientId,
                ErrorCode = 0,
                ErrorMessage = String.Empty
            };

            return Task.FromResult(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Create client error");
            var response = new CreateClientResponse
            {
                ClientId = -1,
                ErrorCode = 912,
                ErrorMessage = "Create client error"
            };

            return Task.FromResult(response);
        }
    }
}