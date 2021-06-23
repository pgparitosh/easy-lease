using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.BusinessModels;
using EasyLease.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private const string CONTROLLER_NAME = "Product";
        readonly IProductService _productService;
        readonly IErrorService _errorService;
        public ProductController(IProductService productService, IErrorService errorService)
        {
            _productService = productService;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("GetProducts")]
        public IEnumerable<ProductDetails> GetProducts()
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _productService.GetAllProducts(orgId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetProducts", userId, "", ex);
                return new List<ProductDetails>();
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public long AddProduct(ProductDetails productDetails)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _productService.AddProductDetails(productDetails, orgId, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "AddProduct", userId, JsonConvert.SerializeObject(productDetails), ex);
                return -1;
            }
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public ProductDetails UpdateProduct(ProductDetails productDetails)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _productService.UpdateProduct(productDetails, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UpdateProduct", userId, JsonConvert.SerializeObject(productDetails), ex);
                return new ProductDetails();
            }
        }

        [HttpGet]
        [Route("GetProductDetailsByProductId/{id}")]
        public ProductDetails GetProductDetailsByProductId(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _productService.GetProductById(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetProductDetailsByProductId", userId, Convert.ToString(id), ex);
                return new ProductDetails();
            }
        }

        [HttpGet]
        [Route("GetProductsForCustomer/{id}")]
        public IEnumerable<ProductDetails> GetProductsForCustomer(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _productService.GetAllProductsByCustomerId(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetProductsForCustomer", userId, Convert.ToString(id), ex);
                return new List<ProductDetails>();
            }
        }

        [HttpPost]
        [Route("InactivateProduct/{id}")]
        public bool InactivateProduct(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _productService.InActivateProduct(id, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "InactivateProduct", userId, Convert.ToString(id), ex);
                return false;
            }
        }
    }
}
