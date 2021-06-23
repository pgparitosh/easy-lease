using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Model.Extensions
{
    public static class Helper
    {
        public static Customer CopyCustomerDetails(this Customer target, Customer source, long updatedBy)
        {
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.CreatedBy = source.CreatedBy;
            target.CreatedTime = source.CreatedTime;
            target.CustomerId = source.CustomerId;
            target.CustomerIdentificationNumber = source.CustomerIdentificationNumber;
            target.CustomerIdentificationType = source.CustomerIdentificationType;
            target.CustomerImageUrl = source.CustomerImageUrl;
            target.IsActive = source.IsActive;
            target.Name = source.Name;
            target.Pincode = source.Pincode;
            target.PrimaryPhone = source.PrimaryPhone;
            target.SecondaryPhone = source.SecondaryPhone;
            target.State = source.State;
            target.UpdatedBy = updatedBy;
            target.UpdatedTime = DateTime.Now;
            return target;
        }

        public static Organization CopyOrganizationDetails(this Organization target, Organization source, long updatedById)
        {
            target.IsActive = source.IsActive;
            target.MaxAllowedUsers = source.MaxAllowedUsers;
            target.MaxLoginRetrtyAttempts = source.MaxLoginRetrtyAttempts;
            target.Name = source.Name;
            target.UpdatedBy = updatedById;
            target.UpdatedTime = DateTime.Now;
            return target;
        }
        
        public static User CopyUserDetails(this User target, User source, long updatedBy)
        {
            target.FailedAttemptsCount = source.FailedAttemptsCount;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.OrganizationId = source.OrganizationId;
            target.PhoneNumber = source.PhoneNumber;
            target.UpdatedBy = updatedBy;
            target.UpdatedTime = DateTime.Now;
            target.UserName = source.UserName;
            return target;
        }
        
        public static Product CopyProductDetails(this Product target, Product source, long updatedBy)
        {
            target.FullDescription = source.FullDescription;
            target.IsActive = source.IsActive;
            target.ProductImageUrls = source.ProductImageUrls;
            target.ProductType = source.ProductType;
            target.ProductWeightInGrams = source.ProductWeightInGrams;
            target.Quantity = source.Quantity;
            target.ShortDescription = source.ShortDescription;
            target.UpdatedBy = updatedBy;
            target.UpdatedTime = DateTime.Now;
            return target;
        }

        public static ProductMortgageDetails CopyProductMortgageDetails(this ProductMortgageDetails target, ProductMortgageDetails source, long updatedBy)
        {
            target.ApproximateIncome = source.ApproximateIncome;
            target.ApproximateProfit = source.ApproximateProfit;
            target.ApproximateValueAsOfCurrentDate = source.ApproximateValueAsOfCurrentDate;
            target.CurrentStatus = source.CurrentStatus;
            target.CustomerId = source.CustomerId;
            target.InterestRate = source.InterestRate;
            target.LendedAmount = source.LendedAmount;
            target.PaymentStatus = source.PaymentStatus;
            target.PendingAmount = source.PendingAmount;
            target.ProductMortgageDate = source.ProductMortgageDate;
            target.ProductMortgageEndDate = source.ProductMortgageEndDate;
            target.RepaymentTenureInMonths = source.RepaymentTenureInMonths;
            target.RepaymentType = source.RepaymentType;
            target.UpdatedBy = updatedBy;
            target.UpdatedTime = DateTime.Now;
            return target;
        }

        public static Transaction CoptTransactionDetails(this Transaction target, Transaction source, long userId)
        {
            //target.CurrentProductStatus = source.CurrentProductStatus;
            //target.PreviousProductStatus = source.PreviousProductStatus;
            target.TransactionDate = source.TransactionDate;
            target.TransactionAmount = source.TransactionAmount;
            target.UpdatedBy = userId;
            target.UpdatedTime = DateTime.Now;
            return target;
        }

        public static Tuple<long, long> LoggedInUserDetails(this ClaimsPrincipal currentUser)
        {
            if (currentUser.HasClaim(c => c.Type == "Org") && currentUser.HasClaim(c => c.Type == "Id"))
            {
                return new Tuple<long, long>
                    (long.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Org").Value)
                    ,long.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Id").Value));
            }
            return new Tuple<long, long>(0, 0);
        }
    }
}
