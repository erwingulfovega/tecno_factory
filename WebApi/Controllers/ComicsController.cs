using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Db;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComicsController : ControllerBase
    {

        internal AppDb Db { get; set; }
        public ComicsController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("{idusuario}")]
        public IActionResult GetComicsFavoritesByUser(int idusuario)
        {
            Db.Connection.Open();
            try
            {
                var query = new ComicsDB(Db);
                var result = query.GetComicsFavoritesByUser(idusuario);
                if (result is null)
                    return NotFound(new { error = "No hay comics registrados" });
                return new OkObjectResult(result);
            }
            catch (Exception ex) {
                return new BadRequestObjectResult(ex.Message);
            }
            
        }

 
        [HttpPost]
        public IActionResult Post([FromBody] Comics body)
        {
            var respuestaAPI = new RespuestaAPI();
            try
            {
                Db.Connection.Open();
                var query = new ComicsDB(Db);

                var insertedID = query.Insert(body);

                var result = query.GetId(insertedID);

                respuestaAPI.isSuccess = true;
                respuestaAPI.id = insertedID;
                respuestaAPI.message = "Datos Guardados!";
                respuestaAPI.objectResp = JsonConvert.SerializeObject(result);

                return new OkObjectResult(respuestaAPI);
            }
            catch (Exception ex)
            {
                respuestaAPI.isSuccess = false;
                respuestaAPI.id = 0;
                respuestaAPI.message = "Error al Guardar los datos - "+ex;
                respuestaAPI.objectResp = null;
                return new BadRequestObjectResult(respuestaAPI);
            }
        }

 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                
                Db.Connection.Open();
                var query = new UsuariosDB(Db);
                var result = query.GetId(id);
                if (result is null)
                    return NotFound(new { Id = id, error = "Error al eliminar el usuario: " + id });
                query.Delete(id);
                return new OkObjectResult(new { Message = "Datos Eliminados !" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
