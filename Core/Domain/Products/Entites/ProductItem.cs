using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Products.Events;

namespace CleanArchitecture.Domain.Products.Entites
{
    public class ProductItem : AuditableEntity
    {
        #region Constructor
        public ProductItem(string? description, decimal price, int amount)
        {
            Description = description;
            Price = price;
            Amount = amount;
        }

        internal ProductItem()
        {

        }
        #endregion

        #region Properties 
        public Guid Id { get; set; }
        public Guid ProductId { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int Amount { get; private set; }
        public virtual Product? Product { get; private set; }
        #endregion

        #region Methods
        public void SetProduct(Product product)
        {
            Product = product;
            ProductId = product.Id;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice < 0)
            {
                throw new ArgumentException($"'{nameof(Price)}' cannot be less than zero.", nameof(newPrice));
            }
            Price = newPrice;
            AddDomainEvent(new ProductItemPriceChangedEvent(this));
        }

        public void Update(ProductItem productItem)
        {
            Description = productItem.Description;

            if (Price != productItem.Price)
            {
                ChangePrice(productItem.Price);
            }

            if (Amount != productItem.Amount)
            {
                ChangeAmount(productItem.Amount);
            }
        }
        public void ChangeAmount(int newAmount)
        {
            if (newAmount < 0)
            {
                throw new ArgumentException($"'{nameof(Amount)}' cannot be less than zero.", nameof(newAmount));
            }
            Amount = newAmount;
            AddDomainEvent(new ProductItemAmountChangedEvent(this));
        }
        #endregion
    }
}
