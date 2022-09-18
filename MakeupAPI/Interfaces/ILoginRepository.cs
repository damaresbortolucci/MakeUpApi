using MakeupAPI.AuthorizationAndAuthentication;

namespace MakeupAPI.Interfaces
{
    public interface ILoginRepository
    {
       Task<bool> Login(Authenticate authInfo);
    }
}
