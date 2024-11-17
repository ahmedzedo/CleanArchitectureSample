using CleanArchitecture.Domain.Common.Exceptions;
using CleanArchitecture.Domain.Products.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Test.Entities.Products
{
    public class CategoryTest
    {
        #region Constructor test
        [Fact]
        public void Constructor_nameArIsEmpty_ThrowArgumentException()
        {
            //Act
            var ctor = () => new Category("", "categoryEn", "categoryfr", "breifAr", "briefEn", "breifFr", DateTime.Now, true);

            //Assert
            Assert.Throws<ArgumentException>(ctor);
        }

        [Fact]
        public void Constructor_nameEnIsEmpty_ThrowArgumentException()
        {
            //Act
            var ctor = () => new Category("categoryAr", "", "categoryfr", "breifAr", "briefEn", "breifFr", DateTime.Now, true);

            //Assert
            Assert.Throws<ArgumentException>(ctor);
        }
        [Fact]
        public void Constructor_nameFrIsEmpty_ThrowArgumentException()
        {
            //Act
            var ctor = () => new Category("categoryAr", "categoryEn", "", "breifAr", "briefEn", "breifFr", DateTime.Now, true);

            //Assert
            Assert.Throws<ArgumentException>(ctor);
        }

        [Fact]
        public void Constructor_breifArIsEmpty_ThrowArgumentException()
        {
            //Act
            var ctor = () => new Category("categoryAr", "categoryEn", "categoryfr", "", "briefEn", "breifFr", DateTime.Now, true);

            //Assert
            Assert.Throws<ArgumentException>(ctor);
        }
        [Fact]
        public void Constructor_briefEnIsEmpty_ThrowArgumentException()
        {
            //Act
            var ctor = () => new Category("categoryAr", "categoryEn", "categoryfr", "breifAr", "", "breifFr", DateTime.Now, true);

            //Assert
            Assert.Throws<ArgumentException>(ctor);
        }
        [Fact]
        public void Constructor_breifFrIsEmpty_ThrowArgumentException()
        {
            //Act
            var ctor = () => new Category("categoryAr", "categoryEn", "categoryfr", "breifAr", "briefEn", "", DateTime.Now, true);

            //Assert
            Assert.Throws<ArgumentException>(ctor);
        }
        [Fact]
        public void Constructor_applyingDateIsEmpty_ThrowArgumentDefaultDateTimeException()
        {
            //Act
            var ctor = () => new Category("categoryAr", "categoryEn", "categoryfr", "breifAr", "briefEn", "breifFr", default, true);

            //Assert
            Assert.Throws<ArgumentDefaultDateTimeException>(ctor);
        }
        #endregion

        #region Set Applying Date Test
        [Fact]
        public void SetApplyingDate_ApplyingDateIsNull_ThrowArgumentDefaultDateTimeException()
        {
            //Arrange
            var category = new Category("categoryAr", "categoryEn", "categoryfr", "breifAr", "briefEn", "breifFr", DateTime.Now, true);
            //Act
            var setapplyingDate = () => category.SetApplyingDate(default);
            //Assert
            Assert.Throws<ArgumentDefaultDateTimeException>(setapplyingDate);
        }

        #endregion

        #region Add Product test
        [Fact]
        public void AddProduct_ProductIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var category = new Category("categoryAr", "categoryEn", "categoryfr", "breifAr", "briefEn", "breifFr", DateTime.Now, true);
            Product? product = null;
            //Act
            var addProduct = () => category.AddProduct(product);
            //Assert
            Assert.Throws<ArgumentNullException>(addProduct);
        }

        [Fact]
        public void AddProduct_ProductItemsIsEmpty_ArgumentEmptyEnumerableException()
        {
            //Arrange
            var category = new Category("categoryAr", "categoryEn", "categoryfr", "breifAr", "briefEn", "breifFr", DateTime.Now, true);
            Product product = new("productAr", "productEn", "PrdoductFr");
           
            //Act
            var addProduct = () => category.AddProduct(product);
           
            //Assert
            Assert.Throws<ArgumentNullOrEmptyEnumerableException>(addProduct);
        }
        #endregion
    }
}
