using EasyLease.Model.Data;
using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Web.Services
{
    public class UserService : Interface.IUserService
    {
        readonly EasyLeaseDbContext _dbContext;
        readonly IOrganizationService _organizationService;

        public UserService(EasyLeaseDbContext dbContext, IOrganizationService organizationService)
        {
            _dbContext = dbContext;
            _organizationService = organizationService;
        }

        public bool ChangePassword(long userId, string newPassword, long updatedBy)
        {
            var user = _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user == null) return false;
            var hashPassword = CreateMD5(newPassword);
            user.PreviousPassword = user.Password;
            user.FailedAttemptsCount = 0;
            user.IsLocked = false;
            user.IsActive = true;
            user.Password = hashPassword;
            user.UpdatedBy = updatedBy;
            user.UpdatedTime = DateTime.Now;
            user.LastPasswordChangeDate = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public User GetUserById(long userId)
        {
            return _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault() ?? new User();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public IEnumerable<User> GetUsersByOrganizationId(long organizationId)
        {
            return _dbContext.Users.Where(x => x.OrganizationId == organizationId).ToList() ?? new List<User>();
        }

        public bool InActivateUser(long userId, long updatedBy)
        {
            if (!IsSuperUser(updatedBy)) return false;
            var user = _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user == null) return false;
            user.IsActive = false;
            user.UpdatedBy = updatedBy;
            user.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public bool LockUser(long userId, long updatedBy)
        {
            if (!IsSuperUser(updatedBy)) return false;
            var user = _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user == null) return false;
            user.IsLocked = true;
            user.UpdatedBy = updatedBy;
            user.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public string ResetPasswordToDefault(long userId, long updatedBy)
        {
            if (!IsSuperUser(updatedBy)) return "Not a super user";
            var user = _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user == null) return "No user found";
            var newPassword = string.Concat(user.FirstName,user.LastName,"@",DateTime.Now.Year.ToString());
            var hashPassword = CreateMD5(newPassword);
            user.PreviousPassword = user.Password;
            user.FailedAttemptsCount = 0;
            user.IsLocked = false;
            user.IsActive = true;
            user.Password = hashPassword;
            user.UpdatedBy = updatedBy;
            user.UpdatedTime = DateTime.Now;
            user.LastPasswordChangeDate = DateTime.Now;
            _dbContext.SaveChanges();
            return newPassword;
        }

        public bool UnlockUser(long userId, long updatedBy)
        {
            if (!IsSuperUser(updatedBy)) return false;
            var user = _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user == null) return false;
            user.IsLocked = false;
            user.UpdatedBy = updatedBy;
            user.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public bool UnlockUserUsingPhoneNo(string phone, long updatedBy)
        {
            if (!IsSuperUser(updatedBy)) return false;
            var user = _dbContext.Users.Where(x => x.PhoneNumber == phone).FirstOrDefault();
            if (user == null) return false;
            user.IsLocked = true;
            user.UpdatedBy = updatedBy;
            user.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public User ValidateLogin(string userName, string password)
        {
            var hashPassword = CreateMD5(password);
            var user = _dbContext.Users.Where(x => x.UserName == userName).FirstOrDefault() ?? new User();
            if(user.UserId == 0)
            {
                // no user found
                return user;
            }
            else if(user.Password != hashPassword)
            {
                // invalid login
                var totalFaieldCount = user.FailedAttemptsCount;
                var maxFailedCountLimit = _organizationService.GetOrganizationById(user.OrganizationId)?.MaxLoginRetrtyAttempts;
                if (totalFaieldCount + 1 > maxFailedCountLimit)
                    user.IsLocked = true;
                else
                    user.FailedAttemptsCount = totalFaieldCount + 1;
                user.UpdatedBy = 1;
                user.UpdatedTime = DateTime.Now;
                user.LastLoginAttemptDate = DateTime.Now;
                _dbContext.SaveChanges();
                return new User();
            }
            else
            {
                // successful login
                user.FailedAttemptsCount = 0;
                user.LastSuccessfulLogin = DateTime.Now;
                _dbContext.SaveChanges();
                return user;
            }
        }

        public static string CreateMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("X2"));
            return sb.ToString();
        }

        public long AddUser(User user, long createdBy)
        {
            if (!IsSuperUser(createdBy)) return -2;
            var dbUser = _dbContext.Users.Where(x => x.UserName.ToLower().Equals(user.UserName.ToLower())).FirstOrDefault();
            if (dbUser != null) return -1;
            user.Password = CreateMD5(user.Password);
            user.CreatedBy = createdBy;
            user.CreatedTime = DateTime.Now;
            user.UpdatedBy = createdBy;
            user.UpdatedTime = DateTime.Now;
            user.FailedAttemptsCount = 0;
            user.IsActive = true;
            user.IsLocked = false;
            user.LastLoginAttemptDate = DateTime.Now;
            user.LastPasswordChangeDate = DateTime.Now;
            user.LastSuccessfulLogin = DateTime.Now;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user.UserId;
        }

        public User UpdateUser(User user, long updatedBy)
        {
            var dbUser = _dbContext.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            if (dbUser == null) return new User();
            dbUser = dbUser.CopyUserDetails(user, updatedBy);
            _dbContext.SaveChanges();
            return dbUser;
        }

        public bool IsSuperUser(long userId)
        {
            var orgId = _dbContext.Users.Where(x => x.UserId == userId).Select(y => y.OrganizationId).FirstOrDefault();
            return orgId == 1;
        }
    }
}
