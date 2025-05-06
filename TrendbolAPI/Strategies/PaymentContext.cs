using TrendbolAPI.Models;

namespace TrendbolAPI.Strategies
{
    public class PaymentContext
    {
        private readonly IPaymentStrategy _paymentStrategy;

        public PaymentContext(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public async Task<bool> ExecutePayment(Order order, decimal amount)
        {
            return await _paymentStrategy.ProcessPayment(order, amount);
        }
    }
} 