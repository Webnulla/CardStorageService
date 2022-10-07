using System.Collections.Generic;

namespace CardStorageService.Models.Requests
{
    public class GetCardResponse :IOperationResult
    {
        public IList<CardDto>? Cards { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}