
namespace CATZureMobileApp.Authentication
{
    using System.Threading.Tasks;

    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
}
