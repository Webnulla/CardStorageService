using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardStorageService.Data;
using Microsoft.Extensions.Logging;

namespace CardStorageService.Services.Impl
{
    public class CardRepository : ICardRepositoryService
    {
        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<CardRepository> _logger;

        public CardRepository(CardStorageServiceDbContext context, ILogger<CardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Card> GetAll()
        {
            var card = _context.Cards.ToList();
            return card;
        }

        public Card GetById(string id)
        {
            var card = _context.Cards.Find(id);
            if (card == null)
            {
                throw new Exception("Card not found");
            }

            return card;
        }

        public string Create(Card data)
        {
            var client = _context.Clients.FirstOrDefault(x => x.ClientId == data.ClientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }

            _context.Cards.Add(data);
            _context.SaveChanges();

            return data.CardId.ToString();
        }

        public Task Update(Card data)
        {
            var card = _context.Cards.Find(data.CardId);
            if (card == null)
            {
                throw new Exception("Card not found");
            }

            card.Name = data.Name;
            card.CardNo = data.CardNo;
            card.CVV2 = data.CVV2;
            card.ExDate = data.ExDate;
            card.Client = data.Client;

            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(string id)
        {
            var card = _context.Cards.Find(id);
            if (card == null)
            {
                throw new Exception("Card not found");
            }

            _context.Remove(card);
            _context.SaveChanges();
            
            return Task.CompletedTask;
        }

        public IList<Card> GetByClientId(int id)
        {
            return _context.Cards.Where(card => card.ClientId == id).ToList();
        }
    }
}