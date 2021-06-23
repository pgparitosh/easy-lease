using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyLease.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        readonly ICustomerService _customerService;
        readonly IErrorService _errorService;
        const string CONTROLLER_NAME = "Customer";
        public CustomerController(ICustomerService customerService, IErrorService errorService)
        {
            _customerService = customerService;
            _errorService = errorService;
        }

        [Route("GetCustomers")]
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _customerService.GetAccessibleCustomers(orgId);
            }
            catch(Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetCustomers", userId, "", ex);
                return new List<Customer>();
            }  
        }

        [HttpGet]
        [Route("GetCustomer/{id}")]
        public Customer GetCustomerById(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _customerService.GetCustomerById(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetCustomers", userId, Convert.ToString(id), ex);
                return new Customer();
            }
        }

        [Route("AddCustomer")]
        [HttpPost]
        public HttpResponseMessage AddCustomer(Customer customer)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                var result = _customerService.AddCustomer(customer, userId);
                var statusCode = result > 0 ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
                var response = new HttpResponseMessage()
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(result))
                };
                return response;
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "AddCustomer", userId, JsonConvert.SerializeObject(customer), ex);
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };
                return response;
            }
        }

        [Route("InActivateCustomer/{id}")]
        [HttpPost]
        public bool InActivateCustomer(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _customerService.InActivateCustomer(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "InActivateCustomer", userId, Convert.ToString(id), ex);
                return false;
                throw;
            }
        }

        [Route("UpdateCustomer")]
        [HttpPost]
        public Customer UpdateCustomer(Customer customer)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _customerService.UpdateCustomer(customer, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UpdateCustomer", userId, JsonConvert.SerializeObject(customer), ex);
                return new Customer();
            }
        }
    }
}
