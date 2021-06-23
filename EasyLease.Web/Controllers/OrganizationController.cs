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
using System.Threading.Tasks;

namespace EasyLease.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IErrorService _errorService;
        const string CONTROLLER_NAME = "Organization";
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost]
        [Route("AddOrganization")]
        public long AddOrganization(Organization organization)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _organizationService.AddOrganization(organization, orgId, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "AddOrganization", userId, JsonConvert.SerializeObject(organization), ex);
                return -1;
            }
        }

        [HttpPost]
        [Route("UpdateOrganization")]
        public Organization UpdateOrganization(Organization organization)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _organizationService.UpdateOrganization(organization, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UpdateOrganization", userId, JsonConvert.SerializeObject(organization), ex);
                return new Organization();
            }
        }

        [HttpPost]
        [Route("InactivateOrganization/{id}")]
        public bool InactivateOrganization(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _organizationService.InActivateOrganization(id, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "InactivateOrganization", userId, id.ToString(), ex);
                return false;
            }
        }

        [HttpGet]
        [Route("GetOrganizations")]
        public IEnumerable<Organization> GetOrganizations()
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _organizationService.GetOrganizations();
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetOrganizations", userId, "", ex);
                return new List<Organization>();
            }
        }

        [HttpGet]
        [Route("GetOrganizationById/{id}")]
        public Organization GetOrganizationById(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _organizationService.GetOrganizationById(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetOrganizationById", userId, id.ToString(), ex);
                return new Organization();
            }
        }
    }
}
