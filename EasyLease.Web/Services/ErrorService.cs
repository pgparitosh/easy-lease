using EasyLease.Model.Data;
using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services
{
    public class ErrorService : Interface.IErrorService
    {
        readonly EasyLeaseDbContext _dbContext;
        public ErrorService(EasyLeaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogError(string controller, string action, long userId, string inputData, Exception ex)
        {
            var error = new ApplicationError()
            {
                Action = action,
                Controller = controller,
                InnerException = ex.InnerException?.Message,
                InputDataString = inputData,
                LoggedAt = DateTime.Now,
                Message = ex?.Message,
                UserId = userId
            };
            _dbContext.Errors.Add(error);
            _dbContext.SaveChanges();
        }
    }
}
