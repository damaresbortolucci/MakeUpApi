using MakeupAPI.AuthorizationAndAuthentication;
using MakeupAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakeupAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase, ILoginController
    {
        private readonly ILoginRepository _repository;
        private readonly GenerateToken _generateToken;

        public LoginController(ILoginRepository repository, GenerateToken generateToken)
        {
            _repository = repository;
            _generateToken = generateToken;
        }


        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] Authenticate authInfo)
        {
            var validation = await _repository.Login(authInfo);

            if (!validation)
                return NotFound(new { message = "Usuário ou Senha inválidos" });

            var token = _generateToken.GenerateJwt(authInfo);
            return Ok(token);
        }
    }
}
