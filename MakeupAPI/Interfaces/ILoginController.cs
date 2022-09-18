using MakeupAPI.AuthorizationAndAuthentication;
using Microsoft.AspNetCore.Mvc;

namespace MakeupAPI.Interfaces
{
    public interface ILoginController
    {
        Task<IActionResult> Post(Authenticate authInfo);
    }
}
