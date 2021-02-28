using BDC.Core.Models;
using System;

namespace BDC.Core.Adapters
{
    public class FxAdapter : IFxAdapter
    {
        public Result<string> PostFx(Transaction transaction)
        {
            // make api call to post fx payment
            // ..............


            // simulate payment response
            Random rnd = new Random();
            int val = rnd.Next(1, 4);

            if (val == 1)
                return Result<string>.Success("SWFT-" + DateTime.Now.Ticks, "Payment successful");
            else if (val == 2)
                return Result<string>.Failure(null, "Source account is blacklisted");
            else if (val == 3)
                return Result<string>.Failure(null, "PNC: Account cannot receive fund from your country");

            return Result<string>.Failure("Payment failed");
        }
    }
}
