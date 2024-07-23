using MDBinASP.NET.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class ComicsController : Controller
    {
        public async Task<ActionResult> Index()
        {

            ViewBag.Message = "Comics y Series Marvel";

            if (Session["username"] != string.Empty && Session["username"] != null)
            {
                ViewBag.username = Session["username"];
                var public_key = System.Configuration.ConfigurationManager.AppSettings["public_key"];
                var private_key = System.Configuration.ConfigurationManager.AppSettings["private_key"];
                var url_apimarvel = System.Configuration.ConfigurationManager.AppSettings["url_apimarvel"];
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

                        // Pass the list of comics to the view
                        return View("Index", comics);
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

        public ActionResult FavoriteComics()
        {
            
            ViewBag.Message = "Mis Comics Favoritos";

            if (Session["username"] != string.Empty && Session["username"] != null)
            {
                ViewBag.username = Session["username"];
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

    }
}