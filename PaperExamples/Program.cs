using System;

namespace PaperExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }


    public interface IGetPayAmountStrategy
    {
        decimal GetPay();
    }
    public class HourlyStrategy : IGetPayAmountStrategy
    {
        private decimal PayRate { get; set; }
        private double HoursWorked { get; set; }
        public decimal GetPay()
        {
            return PayRate * (decimal) HoursWorked;
        }
    }
    public class SalesStrategy : IGetPayAmountStrategy
    {
        private decimal PayRate { get; set; }
        private double HoursWorked { get; set; }
        private decimal CommissionAmount { get; set; }
        public decimal GetPay()
        {
            return (PayRate * (decimal)HoursWorked) + CommissionAmount;
        }
    }
    public class PayrollService
    {
        private readonly IGetPayAmountStrategy strategy;
        public PayrollService(IGetPayAmountStrategy strategy)
        {
            this.strategy = strategy;
        }
        public decimal GetTotalAfterDeductions()
        {
            decimal deductions = 1000;
            decimal amount = strategy.GetPay();
            return amount - deductions;
        }
    }





    public class BadConsumerPayrollService
    {
        private readonly IGetPayAmountStrategy strategy;

        public BadConsumerPayrollService(IGetPayAmountStrategy strategy)
        {
            this.strategy = strategy;
        }
        public decimal GetTotal()
        {
            decimal amount = strategy.GetPay();
            decimal deductions = 1000; 

            //intimate knowledge of the type has to be known
            //this downcast would make Barbara Liskov frown
            if (strategy is SalesStrategy)  
            {
                decimal bonus = 1000;
                amount = amount + bonus;
                return amount - deductions;
            }
            else if (strategy is HourlyStrategy)
            {
                return amount - deductions;
            }

            return 0; //oops! 
        }
    }
}
