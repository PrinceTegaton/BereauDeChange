using BDC.Core.Models;

namespace BDC.Core.Adapters
{
    public interface IFxAdapter
    {
        Result<string> PostFx(Transaction transaction);
    }
}
