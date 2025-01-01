using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Products.Events;
using Common.Linq;

namespace CleanArchitecture.Domain.Products.Entites
{
    public class Product : AuditableEntity, IAggregateRoot, ISoftDeletable
    {
        #region Constructor
        public Product(string nameAr,
                       string nameEn,
                       string nameFr)
        {
            NameEn = nameEn;
            NameAr = nameAr;
            NameFr = nameFr;
            categories = [];
            productItems = [];
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public string NameAr { get; private set; }
        public string NameEn { get; private set; }
        public string NameFr { get; private set; }


        private readonly List<Category> categories;
        public IReadOnlyCollection<Category> Categories => categories.AsReadOnly();

        private readonly List<ProductItem> productItems;
        public IReadOnlyCollection<ProductItem> ProductItems => productItems.AsReadOnly();

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }


        #endregion

        #region Methods

        public void Update(string nameAr, string nameEn, string nameFr)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameAr);
            ArgumentException.ThrowIfNullOrEmpty(nameEn);
            ArgumentException.ThrowIfNullOrEmpty(nameFr);

            NameEn = nameEn;
            NameAr = nameAr;
            NameFr = nameFr;
        }


        #region Manage Categories List
        public void AddCategory(Category category)
        {
            categories.Add(category);
        }
        public void AddCategory(List<Category> categorylst)
        {
            categories.AddRange(categorylst);
        }
        public void UpdateCategory(List<Category> categorylst)
        {
            categories.Clear();
            categories.AddRange(categorylst);
        }
        public void RemoveCategory(Category category)
        {
            categories.Remove(category);
        }
        public void RemoveCategory(List<Category> categorylst)
        {
            categories.RemoveAll(c => categorylst.Contains(c));
        }
        #endregion

        #region Manage Product Items List
        public void AddProductItem(ProductItem productItem)
        {
            productItems.Add(productItem);
        }
        public void AddProductItems(List<ProductItem> productItemsLst)
        {
            productItems.AddRange(productItemsLst);
        }
        public void UpdateProductItems(List<ProductItem> productItemsLst)
        {
            productItems.RemoveAll(productItems => !productItemsLst.Exists(p => p.Id == productItems.Id));

            productItemsLst.ForEach(p =>
            {
                if (p.Id == Guid.Empty)
                {
                    productItems.Add(p);
                }
                else
                {
                    var productItem = productItems.Find(i => i.Id == p.Id);
                    productItem?.Update(p);
                }

            });
        }
        public void RemoveProductItem(ProductItem productItem)
        {
            productItems.Remove(productItem);
            AddDomainEvent(new ProductItemDeletedEvent(productItem));

        }
        public void RemoveProductItems(List<ProductItem> productDetailsLst)
        {
            var itemsToRemove = productItems.ToList(p => productDetailsLst.Contains(p));
            productItems.RemoveAll(p => productDetailsLst.Contains(p));

            if (itemsToRemove.Count > 0)
            {
                AddDomainEvent(new ProductItemsDeletedEvent(itemsToRemove));
            }

        }
        #endregion

        #endregion

    }
}
