namespace EasyLease.Model.Enums
{
    public enum ProductStatusTypeValues
    {
        Mortgaged,  // when the product is mortgaged. Initial step
        Ceased, // when the customer has faulted on repayment and product is ceased
        Returned, // when the payment is completed and the product is returned to the customer
        Sold, // The product has been ceased and now sold to recover the amount
        Other   // Other
    }
}
