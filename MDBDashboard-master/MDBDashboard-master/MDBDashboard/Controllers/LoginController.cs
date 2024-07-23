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
    public class LoginController : Controller
    {

        //private readonly HttpClient _httpClient;
        private static readonly HttpClient client = new HttpClient();

        public ActionResult Index()
        {
            //ViewBag.Message = "Sample page for Login1.";
            return View();
        }

        public ActionResult Login(FormCollection form)
        {

            string username = form["username"];
            string password = form["password"];

            var resultado = Validate(username,password);
            if (resultado.isSuccess)
            {
                var session = HttpContext.Session;
                var datos = JsonConvert.DeserializeObject<JsonUser>(resultado.objectResp.ToString());
                session["idusuario"] = datos.Id;
                session["username"] = datos.Nombre_Usuario;
                session["nombres"]  = String.Concat(datos.Nombres," ", datos.Apellidos);
                return RedirectToAction("Index", "Home");
            }
            {
                ViewBag.Message = "Error de usuario y/o contraseña";
                ViewBag.isSuccess = resultado.isSuccess;
                return View("Index");
            }
            
        }

        public static RespuestaAPI Validate(string username, string password)
        {

            var respuestaAPI = new RespuestaAPI();
            
            if (username != string.Empty && password != string.Empty)
            {

                try
                {
                    Usuarios usuario = new Usuarios();
                    usuario.Nombre_Usuario = username;
                    usuario.Clave = password;

                    var requestMessage = ApiConection.GetHttpRequestMessage(HttpMethod.Post, "Login");
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
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return respuestaAPI;
                }

            }
            return respuestaAPI;
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }


    }
}