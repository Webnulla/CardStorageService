using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardStorageService.Data;
using Microsoft.Extensions.Logging;

namespace CardStorageService.Services.Impl
{
    public class ClientRepository : IClientRepositoryService
    {
        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(CardStorageServiceDbContext context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public IList<Client> GetAll()
        {
            var client = _context.Clients.ToList();
            return client;
        }

        public Client GetById(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                throw new Exception("Card not found");
            }

            return client;
        }

        public int Create(Client data)
        {
            _context.Clients.Add(data);
            _context.SaveChanges();
            return data.ClientId;
        }

        public Task Update(Client data)
        {
            var client = _context.Clients.Find(data.ClientId);
            if (client == null)
            {
                throw new Exception("Card not found");
            }

            client.FirstName = data.FirstName;
            client.Surname = data.Surname;
            client.Patronymic = data.Patronymic;
            client.Cards = data.Cards;

            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                throw new Exception("Card not found");
            }

            _context.Remove(client);
            _context.SaveChanges();
            
            return Task.CompletedTask;
        }
    }
}