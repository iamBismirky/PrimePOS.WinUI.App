using System.Threading.Tasks;

namespace PrimePOS.WinUI.Contracts
{
    public interface IOverlayViewModel
    {
        Task<bool> WaitTask { get; }

        void Close(bool result);
    }
}
