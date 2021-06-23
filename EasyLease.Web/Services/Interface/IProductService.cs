using EasyLease.Model.Enums;
using EasyLease.Model.Models;
using EasyLease.Web.BusinessModels;
using System.Collections.Generic;

namespace EasyLease.Web.Services.Interface
{
    public interface IProductService
    {
        IEnumerable<ProductDetails> GetAllProducts(long orgId);
        IEnumerable<ProductDetails> GetAllProductsByCustomerId(long customerId);
        ProductDetails GetProductById(long productId);
        long AddProductDetails(ProductDetails productDetails, long orgId, long userId);
        bool InActivateProduct(long productId, long userId);
        ProductDetails UpdateProduct(ProductDetails productDetails, long updatedBy);
        ProductMortgageDetails UpdateProductMortgageDetails(long productId, ProductPaymentStatusTypeValues paymentStatusType, decimal pendingAmount, long userId);
    }
}
