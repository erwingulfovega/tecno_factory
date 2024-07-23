using MDBinASP.NET.Clases;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MDBinASP.NET.Clases.MarvelAPI;

namespace MDBinASP.NET.Controllers
{
    public class ComicsFavoritesController : Controller
    {

        private static readonly HttpClient client = new HttpClient();
        public async Task<ActionResult> Index()
        {

            var result = MiListaComics(Convert.ToInt32(Session["idusuario"]));
            ViewBag.Message = "Mis Comics Favoritos";
            ViewBag.IdUsuario = Session["idusuario"];
            
            if (Session["username"] != string.Empty && Session["username"] != null)
            {
                ViewBag.username = Session["username"];
                var public_key = System.Configuration.ConfigurationManager.AppSettings["public_key"];
                var private_key = System.Configuration.ConfigurationManager.AppSettings["private_key"];
                var url_apimarvel = System.Configuration.ConfigurationManager.AppSettings["url_apimarvel"];
                var url_webapi = System.Configuration.ConfigurationManager.AppSettings["url_webapi"];
                ViewBag.UrlWebApi = url_webapi;
                var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var hash= GenerateHash(public_key, private_key,ts);
                var url = string.Format(url_apimarvel + "?ts={0}&apikey={1}&hash={2}", ts, public_key, hash);

                using (var httpClient = new HttpClient())
                {
                    // Send a GET request
                    var response = await httpClient.GetAsync(url);

                    // Check the response status code
                    if (response.IsSuccessStatusCode)
                    {
                        // Process the successful response
                        var json = await response.Content.ReadAsStringAsync();
                        var rootObject = JsonConvert.DeserializeObject<RootObject>(json);

                        // Access comics data
                        var comics = rootObject.Data.Results;
                        var idComicsToFilter = result.Select(item => item.IdComics).Distinct().ToList();
                        var filteredComics = comics.Where(comic => idComicsToFilter.Contains(comic.Id)).ToList();
                        // Pass the list of comics to the view
                        return View("Index", filteredComics);
                    }
                    else
                    {
                        // Handle the error response
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        //return View("Error", new ErrorViewModel { Message = errorMessage });
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public string GenerateHash(string public_key, string private_key, string ts)
        {
           
            try
            {
                if (public_key != string.Empty && private_key != string.Empty && ts != string.Empty)
                {

                    var cadena = ts + private_key + public_key;

                    string algorithmName = "MD5"; // Replace with your chosen algorithm
                    HashAlgorithm hashAlgorithm = HashAlgorithm.Create(algorithmName);

                    byte[] inputBytes = Encoding.UTF8.GetBytes(cadena);
                    byte[] hashBytes = hashAlgorithm.ComputeHash(inputBytes);

                    string hashString = "";

                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        hashString += hashBytes[i].ToString("x2");
                    }

                    return hashString;
                }
                else
                {
                    Console.WriteLine("Error de hash");
                    return null;
                }
            }
            catch (Exception ex)
            {
                return  ex.Message;
            }

            
        }

        public class ComicsResponse
        {
          public List<ComicsFavoritos> Comics { get; set; } // Assuming a "Comics" property with a list
        }

        public static List<ComicsFavoritos> MiListaComics(int idusuario)
        {

            List<ComicsFavoritos> lista = new List<ComicsFavoritos>();

            try
            {
                var requestMessage = ApiConection.GetHttpRequestMessage(HttpMethod.Get, string.Format("Comics/{0}", idusuario));
                var task = Task.Run(() => client.SendAsync(requestMessage));
                task.Wait();
                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    var taskResp = Task.Run(() => response.Content.ReadAsStringAsync());
                    taskResp.Wait();

                    // Deserialize based on actual response structure
                    if (taskResp.IsCompleted) // Check if simple array
                    {
                        lista = JsonConvert.DeserializeObject<List<ComicsFavoritos>>(taskResp.Result);
                        return lista;
                    }
                    else // Assume nested structure
                    {
                        var listaResponse = JsonConvert.DeserializeObject<ComicsResponse>(taskResp.Result);
                        return listaResponse.Comics; // Return nested list
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return lista;
        }

        public static RespuestaAPI InsertarComicsFavoritos(int id_comic, int id_usuario)
        {

            var respuestaAPI = new RespuestaAPI();
                       
            ComicsFavorites comics = new ComicsFavorites();

            if (id_comic!=0 && id_usuario!=0)
            {

                try
                {
                    comics.IdComics  = id_comic;
                    comics.IdUsuario = id_usuario;
                        
                    var requestMessage = ApiConection.GetHttpRequestMessage(HttpMethod.Post, "Comics");
                    requestMessage.Content =
                        new StringContent(JsonConvert.SerializeObject(comics), Encoding.UTF8, "application/json");
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

    }
}