using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Product.Entites
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
            NameAr = nameAr;
            NameEn = nameEn;
            NameFr = nameFr;
            BriefAr = briefAr;
            BriefEn = briefEn;
            BriefFr = briefFr;
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
            if (applyingDate != default)
            {
                ApplyingDate = applyingDate;
            }
        }
        public void AddProduct(Product product) => availableProducts.Add(product);
        public void AddProduct(List<Product> productLst) => availableProducts.AddRange(productLst);
        public void RemoveProduct(Product product) => availableProducts.Remove(product);

        #endregion

    }
}
