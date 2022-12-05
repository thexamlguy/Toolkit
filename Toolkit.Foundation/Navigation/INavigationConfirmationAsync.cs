
namespace Kromek.Framework.Core.Extensions
{
    public interface INavigationConfirmationAsync
    {
        Task<bool> CanConfirmAsync();
    }
}