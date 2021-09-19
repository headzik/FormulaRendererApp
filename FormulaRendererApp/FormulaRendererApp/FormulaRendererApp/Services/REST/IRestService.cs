using FormulaRendererApp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace FormulaRendererApp.Services
{
    public interface IRestService
    {
        Task<HttpResponseMessage> PostTexFormula(Formula formula);
        Task<HttpResponseMessage> GetTexFormula(string hashID);
        Task<HttpResponseMessage> GetRenderedImage(string hashID);

    }
}
