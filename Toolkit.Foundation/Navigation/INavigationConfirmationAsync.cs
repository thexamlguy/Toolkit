
namespace Toolkit.Foundation
{
    public interface INavigationConfirmationAsync
    {
        Task<bool> CanConfirmAsync();
    }
}