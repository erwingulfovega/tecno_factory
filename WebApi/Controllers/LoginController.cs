using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            string jsonString = string.Empty;
            try
            {
                Db.Connection.Open();
                
                var query = new LoginDB(Db);
                var status = query.Login(body);

                var usuario = new UsuariosDB(Db);
                Usuarios usuarios = new Usuarios();
                var ObtenerUsuario = usuario.GetUserByUserName(body.Nombre_Usuario);
                usuarios.Nombre_Usuario = ObtenerUsuario.Nombre_Usuario;
                usuarios.DocumentoIdentidad = ObtenerUsuario.DocumentoIdentidad;
                usuarios.Nombres = ObtenerUsuario.Nombres;
                usuarios.Email=ObtenerUsuario.Email;
                jsonString = JsonConvert.SerializeObject(usuarios);

                if (status && ObtenerUsuario!=null && jsonString!=string.Empty)
                {
                    respuestaAPI.isSuccess = true;
                    respuestaAPI.message = "Login correcto!";
                    respuestaAPI.CodigoResultado = 200;
                    respuestaAPI.objectResp = jsonString;
                    return new OkObjectResult(respuestaAPI);
                }
                else
                {
                    respuestaAPI.isSuccess = true;
                    respuestaAPI.message = "Login incorrecto!";
                    respuestaAPI.CodigoResultado = 400;
                    respuestaAPI.objectResp = jsonString;
                    return new BadRequestObjectResult(respuestaAPI);
                }
                
            }
            catch (Exception ex)
            {
                respuestaAPI.isSuccess = false;
                respuestaAPI.id = 0;
                respuestaAPI.message = "Error al Guardar los datos - "+ex;
                respuestaAPI.objectResp = jsonString;
                return new BadRequestObjectResult(respuestaAPI);
            }
        }

    }
}
