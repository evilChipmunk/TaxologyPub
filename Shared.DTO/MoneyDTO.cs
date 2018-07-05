namespace Shared.DTO
{
    public class MoneyDTO
    {
        public MoneyDTO()
        {
            Exchange = DefaultExchange.USD;
        }

        public MoneyDTO(decimal amount)
        {
            Amount = amount;
            Exchange = DefaultExchange.USD;
        }

        public MoneyDTO(decimal amount, string exchange)
        {
            Amount = amount;
            Exchange = exchange;
        }

        
        public decimal Amount { get; set; }
        public string Exchange { get; set; }
    }
}
