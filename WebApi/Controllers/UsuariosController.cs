using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Db;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {

        internal AppDb Db { get; set; }
        public UsuariosController(AppDb db)
        {
            Db = db;
        }

        [HttpGet("{DocumentoIdentidad}")]
        public IActionResult GetUserByDocumentIdentification(string DocumentoIdentidad)
        {
            Db.Connection.Open();
            var query = new UsuariosDB(Db);
            var result = query.GetUserByDocumentIdentification(DocumentoIdentidad);
            if (result is null)
                return NotFound(new { error = "No hay usuarios registrados" });
            return new OkObjectResult(result);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            Db.Connection.Open();
            var query = new UsuariosDB(Db);
            var result = query.GetUsuarios();
            if (result is null)
                return NotFound(new { error = "No hay usuarios registrados" });
            return new OkObjectResult(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Usuarios body)
        {
            var respuestaAPI = new RespuestaAPI();
            try
            {
                Db.Connection.Open();
                var query = new UsuariosDB(Db);

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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Usuarios body)
        {
            try
            {

                Db.Connection.Open();
                var query = new UsuariosDB(Db);
                var result = query.GetId(id);
                if (result is null)
                    return NotFound(new { Id = id, error = "Error al actualizar los datos " + id });
                
                query.Update(body);
                return new OkObjectResult(new { Message = "Datos Actualizados !" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
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
