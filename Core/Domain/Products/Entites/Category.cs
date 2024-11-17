using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Common.Exceptions;

namespace CleanArchitecture.Domain.Products.Entites
{
    public class Category : AuditableEntity, IAggregateRoot
    {
        #region Constructor
        public Category(string nameAr,
                        string nameEn,
                        string nameFr,
                        string briefAr,
                        string briefEn,
                        string briefFr,
                        DateTime applyingDate,
                        bool? isAvailable = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameAr);
            NameAr = nameAr;
            
            ArgumentException.ThrowIfNullOrEmpty(nameEn);
            NameEn = nameEn;

            ArgumentException.ThrowIfNullOrEmpty(nameFr);
            NameFr = nameFr;

            ArgumentException.ThrowIfNullOrEmpty(briefAr);
            BriefAr = briefAr;

            ArgumentException.ThrowIfNullOrEmpty(briefEn);

            BriefEn = briefEn;
            ArgumentException.ThrowIfNullOrEmpty(briefFr);

            BriefFr = briefFr;

            ArgumentDefaultDateTimeException.ThrowIfDateTimeIsDefault(applyingDate);
            ApplyingDate = applyingDate;

            IsAvailable = isAvailable;
            availableProducts = [];
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public string NameAr { get; private set; }
        public string NameEn { get; private set; }
        public string NameFr { get; private set; }
        public string BriefAr { get; private set; }
        public string BriefEn { get; private set; }
        public string BriefFr { get; private set; }
        public bool? IsAvailable { get; private set; }
        public DateTime ApplyingDate { get; private set; }

        private readonly List<Product> availableProducts;
        public IReadOnlyCollection<Product> AvailableProducts => availableProducts.AsReadOnly();

        #endregion

        #region Methods
        public void ChangeAvailablityStatus()
        {
            IsAvailable = !IsAvailable;
        }

        public void SetApplyingDate(DateTime applyingDate)
        {
            ArgumentDefaultDateTimeException.ThrowIfDateTimeIsDefault(applyingDate);
            ApplyingDate = applyingDate;
        }
        public void AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            ArgumentNullOrEmptyEnumerableException.ThrowIfNullOrEmptyEnumerable(product.ProductItems, $"Item list of product {product.NameEn}");

            availableProducts.Add(product);
        }

        public void AddProduct(List<Product> productLst)
        {
            ArgumentNullOrEmptyEnumerableException.ThrowIfNullOrEmptyEnumerable(productLst, $"{nameof(productLst)}");

            availableProducts.AddRange(productLst);
        }

        public void RemoveProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            availableProducts.Remove(product);
        }

        #endregion

    }
}
