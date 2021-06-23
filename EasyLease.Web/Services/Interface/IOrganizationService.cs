using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services.Interface
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetOrganizations();
        Organization GetOrganizationById(long organizationId);
        bool InActivateOrganization(long organizationId, long userId);
        long AddOrganization(Organization organization, long orgId, long userId);
        Organization UpdateOrganization(Organization organization, long userId);
    }
}
