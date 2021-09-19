using FormulaRendererApp.Models;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using FormulaRendererApp.Exceptions;

namespace FormulaRendererApp.Services
{
    public class FormulaManager
    {
        readonly IRestService RestService;

        public FormulaManager(IRestService restService)
        {
            RestService = restService;
        }

        public Task<Stream> RenderImageFromFormula(string formula)
        {

            return Task.Run(async () =>
            {
                var result = Stream.Null;

                var Formula = new Formula { q = formula };
                var response = await RestService.PostTexFormula(Formula);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("x-resource-location",
                        out IEnumerable<string> values))
                    {
                        string hashID = values?.First();
                        response = await RestService.GetRenderedImage(hashID);
                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsStreamAsync();
                        }
                        else
                        {
                            HandleStatusCode(response.StatusCode);
                        }
                    }
                }
                else
                {
                    HandleStatusCode(response.StatusCode);
                }


                return result;
            });
        }

        private void HandleStatusCode(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException();

                case HttpStatusCode.NotFound:
                    throw new NotFoundException();

                case HttpStatusCode.InternalServerError:
                    throw new InternalServerErrorException();

                default:
                    break;

            }
        }
    }
}
