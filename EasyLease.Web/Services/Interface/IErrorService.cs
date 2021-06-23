using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services.Interface
{
    public interface IErrorService
    {
        void LogError(string controller, string action, long userId, string inputData, Exception ex);
    }
}
