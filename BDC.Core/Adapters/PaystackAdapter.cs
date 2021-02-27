using System;

namespace BDC.Core.Adapters
{
    public class PaystackAdapter : IPaymentEngineAdapter
    {
        public Result<string> PostPaymentFromBankAccount(string bankCode, string accountNo, decimal amount)
        {
            // make api call to post payment
            // ..............


            // simulate payment response
            Random rnd = new Random();
            int val = rnd.Next(1, 4);

            if(val == 1)
                return Result<string>.Success("PSTK-" + DateTime.Now.Ticks, "Payment successful");
            else if (val == 2)
                return Result<string>.Failure(null, "Debit failed: Insufficient fund");
            else if (val == 3)
                return Result<string>.Failure(null, "Debit failed: PND - Account cannot be debited");

            return Result<string>.Failure("Payment authorization failed");
        }
    }
}
