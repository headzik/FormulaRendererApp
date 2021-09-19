using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaRendererApp.Services;
using System;
using System.Net.Http;
using System.Net;
using FormulaRendererApp.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
* It seems that if verbatim string is passed, the string has additional slashes \
* Looks like verbatim @ works only in one source code?
*/
namespace FormulaRendererAppTest
{
    [TestClass]
    public class TestRestService
    {
        IRestService RestService;

        [TestInitialize]
        public void BeforeTest()
        {
            RestService = new RestService();
        }

        [TestMethod]
        public void TestPostTexFormula_StatusOK()
        {
            var formula = new Formula
            {
                q = @"\lim_{h \to 0} \frac{f(a+h)-f(a)}{h}"
            };
            var response = RestService.PostTexFormula(formula);
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }

        [TestMethod]
        public void TestPostTexFormula_BadRequest()
        {
            var formula = new Formula
            {
                q = @"lim_{h  \\frac{f(a+h)-f(a)}{h}"
            };
            var response = RestService.PostTexFormula(formula);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
        }

        //Xamarin does not support async unit test methods, so workaround is done
        //https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/november/async-programming-unit-testing-asynchronous-code
        [TestMethod]
        public void TestGetRenderedImage_StatusOK()
        {
            var getResponse = Task.Run(async () =>
            {
                var formula = new Formula
                {
                    q = @"\lim_{h \to 0} \frac{f(a+h)-f(a)}{h}"
                };
                var postResponse = await RestService.PostTexFormula(formula);
                postResponse.Headers.TryGetValues("x-resource-location", out IEnumerable<string> values);

                string hashID = values.First();
                var getResponse = RestService.GetRenderedImage(hashID);
                return getResponse;

            }).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, getResponse.Result.StatusCode);
        }

        [TestMethod]
        public void TestGetRenderedImage_NotFound()
        {
            string hashID = "ASDASDASD";
            var getResponse = RestService.GetRenderedImage(hashID);
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.Result.StatusCode);
        }

        [TestMethod]
        public void TestGetRenderedImage_BadRequest()
        {
            var getResponse = RestService.GetRenderedImage(null);
            Assert.AreEqual(HttpStatusCode.BadRequest, getResponse.Result.StatusCode);
        }
    }

}