using EasyLease.Model.Data;
using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services
{
    public class OrganizationService : Interface.IOrganizationService
    {
        readonly EasyLeaseDbContext _dbContext;
        public OrganizationService(EasyLeaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddOrganization(Organization organization, long orgId, long userId)
        {
            if (orgId != 1) return -2;  // making sure that only super admins can add new org
            organization.CreatedBy = userId;
            organization.CreatedTime = DateTime.Now;
            organization.UpdatedBy = userId;
            organization.UpdatedTime = DateTime.Now;
            _dbContext.Organizations.Add(organization);
            _dbContext.SaveChanges();
            return organization.OrganizationId;
        }

        public Organization GetOrganizationById(long organizationId)
        {
            return _dbContext.Organizations.Where(x => x.OrganizationId == organizationId).FirstOrDefault() ?? new Organization();
        }

        public IEnumerable<Organization> GetOrganizations()
        {
            return _dbContext.Organizations.ToList();
        }

        public bool InActivateOrganization(long organizationId, long userId)
        {
            var organization = _dbContext.Organizations.Where(x => x.OrganizationId == organizationId).FirstOrDefault() ?? new Organization();
            if (organization.OrganizationId == 0) return false;
            organization.IsActive = false;
            organization.UpdatedBy = userId;
            organization.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public Organization UpdateOrganization(Organization organization, long userId)
        {
            var dbOrganization = _dbContext.Organizations.Where(x => x.OrganizationId == organization.OrganizationId).FirstOrDefault() ?? new Organization();
            dbOrganization = dbOrganization.CopyOrganizationDetails(organization, userId);
            _dbContext.SaveChanges();
            return dbOrganization;
        }
    }
}
