using TrendbolAPI.Models;

namespace TrendbolAPI.Strategies
{
    public interface IPaymentStrategy
    {
        Task<bool> ProcessPayment(Order order, decimal amount);
    }

    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public async Task<bool> ProcessPayment(Order order, decimal amount)
        {
            await Task.Delay(1000); 
            return true;
        }
    }

    public class BankTransferPaymentStrategy : IPaymentStrategy
    {
        public async Task<bool> ProcessPayment(Order order, decimal amount)
        {
            await Task.Delay(1000); 
            return true;
        }
    }

    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        public async Task<bool> ProcessPayment(Order order, decimal amount)
        {
            await Task.Delay(1000); 
            return true;
        }
    }
} 