namespace BDC.Core.Adapters
{
    public interface IPaymentEngineAdapter
    {
        Result<string> PostPaymentFromBankAccount(string bankCode, string accountNo, decimal amount);
    }
}
