using MDBinASP.NET.Clases;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
//using Linea = GEA_MODELS.Linea;

namespace MDBinASP.NET.Controllers
{
    public class UsuariosController : Controller
    {

        //private readonly HttpClient _httpClient;
        private static readonly HttpClient client = new HttpClient();

        public ActionResult Index()
        {
            //ViewBag.Message = "Sample page for Login1.";
            return View();
        }

        public ActionResult Register(FormCollection form)
        {
            var resultado = InsertarUsuario(form);
            if (resultado.isSuccess)
            {
                var session = HttpContext.Session;
                var datos = JsonConvert.DeserializeObject<JsonUser>(resultado.objectResp.ToString());
                session["username"] = datos.Nombre_Usuario;
                session["nombres"] = String.Concat(datos.Nombres, " ", datos.Apellidos);
                return RedirectToAction("Index", "Home");
            }
            {
                HttpContext.Session.Remove("username");
                HttpContext.Session.Remove("nombres");
                return RedirectToAction("Error", "Usuarios");
            }
            
        }

        public ActionResult Error()
        {
            ViewBag.Message = "Error al crear el usuario, las claves no coinciden!";
            ViewBag.isFailed = false;
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("nombres");
            ViewData["ErrorMessage"] = "Error al crear el usuario, las claves no coinciden!";
            return RedirectToAction("Login", "Login");
        }

        public static RespuestaAPI InsertarUsuario(FormCollection form)
        {

            var respuestaAPI = new RespuestaAPI();
            string documento_identidad = form["documento_identidad"];
            string nombres = form["nombres"];
            string apellidos = form["apellidos"];
            string email = form["email"];
            string username = form["username"];
            string password1 = form["password1"];
            string password2 = form["password2"];
            Usuarios usuario = new Usuarios();

            if (documento_identidad != string.Empty && 
                nombres != string.Empty &&
                apellidos!=string.Empty &&
                email!=string.Empty &&
                username!=string.Empty&&
                password1!=string.Empty&&
                password2!=string.Empty){

                try
                {
                    if (password1==password2)
                    {
                        
                        usuario.DocumentoIdentidad = documento_identidad;
                        usuario.Nombres = nombres;
                        usuario.Apellidos = apellidos;
                        usuario.Email = email;
                        usuario.Nombre_Usuario = username;
                        usuario.Clave = password1;

                        var requestMessage = ApiConection.GetHttpRequestMessage(HttpMethod.Post, "Usuarios");
                        requestMessage.Content =
                            new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
                        var task = Task.Run(() => client.SendAsync(requestMessage));
                        task.Wait();
                        var response = task.Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var taskResp = Task.Run(() => response.Content.ReadAsStringAsync());
                            taskResp.Wait();
                            respuestaAPI = JsonConvert.DeserializeObject<RespuestaAPI>(taskResp.Result);
                            return respuestaAPI;
                        }
                        else
                        {
                            return respuestaAPI;
                        }
                    }
                    
                    
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return respuestaAPI;
                }

            }
            return respuestaAPI;
        }


    }
}