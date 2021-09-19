using FormulaRendererApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FormulaRendererApp.Services
{
    public class RestService : IRestService
    {
        const string BASE_URL = "https://en.wikipedia.org/api/rest_v1/media/math";
        const string POST_TEX_URL = "/check/{0}";
        const string GET_FORMULA_URL = "/formula/{0}";
        const string GET_IMAGE_URL = "/render/{0}/{1}";
        const string TEX_TYPE = "tex";
        const string SVG_TYPE = "svg";
        const string PNG_TYPE = "png";

        public static string BaseAddress;
        
        readonly HttpClient client = new HttpClient();
        public async Task<HttpResponseMessage> PostTexFormula(Formula formula)
        {
            var uri = new Uri(BASE_URL + string.Format(POST_TEX_URL, TEX_TYPE));
            var json = JsonConvert.SerializeObject(formula);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);
            return response;
        }

        public async Task<HttpResponseMessage> GetTexFormula(string hashID)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> GetRenderedImage(string hashID)
        {
            var uri = new Uri(BASE_URL + string.Format(GET_IMAGE_URL, PNG_TYPE, hashID));
            var response = await client.GetAsync(uri);
            return response;
        }
    }
}
