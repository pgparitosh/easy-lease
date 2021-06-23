using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services.Interface
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(long userId);
        IEnumerable<User> GetUsersByOrganizationId(long organizationId);
        long AddUser(User user, long createdBy);
        User UpdateUser(User user, long updatedBy);
        User ValidateLogin(string userName, string password);
        bool ChangePassword(long userId, string newPassword, long updatedBy);
        bool InActivateUser(long userId, long updateBy);
        bool LockUser(long userId, long updatedBy);
        bool UnlockUser(long userId, long updatedBy);
        bool UnlockUserUsingPhoneNo(string phone, long updatedBy);
        string ResetPasswordToDefault(long userId, long updatedBy);
        bool IsSuperUser(long userId);
    }
}
