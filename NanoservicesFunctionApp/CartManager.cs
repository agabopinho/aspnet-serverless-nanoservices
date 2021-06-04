using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nanoservices.Entities;
using Nanoservices.Infrastructure.Definitions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NanoservicesFunctionApp
{
    public class CartManager
    {
        private readonly ICheckOutService _checkoutService;
        public CartManager(ICheckOutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [FunctionName("IsItemAvailable")]
        public async Task<IActionResult> IsItemAvailable(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            string productCode = req.Query["productcode"];
            return new OkObjectResult(await _checkoutService.IsItemAvailable(productCode));
        }

        [OpenApiOperation(operationId: "getName", tags: new[] { "name" }, Summary = "Gets the name", Description = "This gets the name.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The name", Description = "The name", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
        [FunctionName("GetAllCartItems")]
        public async Task<IActionResult> GetAllCartItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            return new OkObjectResult(await _checkoutService.GetAllCartItems());
        }

        [FunctionName("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            return new OkObjectResult(await _checkoutService.GetAllCustomers());
        }

        [FunctionName("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            return new OkObjectResult(await _checkoutService.GetAllProducts());
        }

        [FunctionName("UpdateStock")]
        public async Task<IActionResult> UpdateStock(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", Route = null)]
            HttpRequestMessage req, ILogger log)
        {
            string jsonContent = await req.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonContent))
            {
                return new BadRequestErrorMessageResult("Invalid input.");
            }

            CartItem cartItem = JsonConvert.DeserializeObject<CartItem>(jsonContent);
            return new OkObjectResult(await _checkoutService.UpdateStock(cartItem));
        }

        [FunctionName("AddItemToCart")]
        public async Task<IActionResult> AddItemToCart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequestMessage req, ILogger log)
        {
            string jsonContent = await req.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonContent))
            {
                return new BadRequestErrorMessageResult("Invalid input.");
            }

            CartItem cartItem = JsonConvert.DeserializeObject<CartItem>(jsonContent);
            return new OkObjectResult(await _checkoutService.AddItemToCart(cartItem));
        }
    }
}