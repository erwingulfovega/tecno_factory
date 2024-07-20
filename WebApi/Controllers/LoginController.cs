using Microsoft.AspNetCore.Mvc;
using WebApi.Db;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        internal AppDb Db { get; set; }
        public LoginController(AppDb db)
        {
            Db = db;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Login body)
        {
            var respuestaAPI = new RespuestaAPI();
            try
            {
                Db.Connection.Open();
                var query = new LoginDB(Db);

                var status = query.Login(body);

                if (status)
                {
                    respuestaAPI.isSuccess = true;
                    respuestaAPI.message = "Login correcto!";
                    return new OkObjectResult(respuestaAPI);
                }
                else
                {
                    respuestaAPI.isSuccess = true;
                    respuestaAPI.message = "Login incorrecto!";
                    return new OkObjectResult(respuestaAPI);
                }
                
            }
            catch (Exception ex)
            {
                respuestaAPI.isSuccess = false;
                respuestaAPI.id = 0;
                respuestaAPI.message = "Error al Guardar los datos - "+ex;
                return new BadRequestObjectResult(respuestaAPI);
            }
        }

    }
}
