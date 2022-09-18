using MakeupAPI.Interfaces;
using MakeupAPI.AuthorizationAndAuthentication;

namespace MakeupAPI.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IConfiguration _configuration;

        public LoginRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public Task<bool> Login(Authenticate authInfo)
        {
            return Task.Run(() =>
            {
                var login = _configuration.GetValue<string>("autenticacao:login");
                var password = _configuration.GetValue<string>("autenticacao:senha");

                if (authInfo.Username == login && authInfo.Password == password)
                    return true;

                return false;
            });
        }
    }
}
