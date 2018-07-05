namespace Shared.Domain
{
    public class Money : ValueObject<Money>
    {
        //concession to entity framework
        private Money()
        {

        }
        public Money(decimal amount)
        {
            Amount = amount;
            Exchange = "USD";
        }

        public Money(decimal amount, string exchange)
        {
            Amount = amount;
            Exchange = exchange;
        }

        public decimal Amount { get; private set; }
        public string Exchange { get; private set; }
    }
}