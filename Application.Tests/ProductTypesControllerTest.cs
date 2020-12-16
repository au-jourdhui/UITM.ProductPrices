using Application.Server.Controllers;
using Application.Shared;
using Application.Tests.Extensions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class ProductTypesControllerTest
    {
        private const int productTypeCount = 10;
        private readonly List<ProductType> productTypes = 
            Enumerable.Range(1, productTypeCount).Select(x => new ProductType
            {
                ID = x,
                Name = $"{nameof(ProductType)} {x}"
            }).ToList();
        private readonly ProductTypesController controller;

        public ProductTypesControllerTest()
        {
            var context = new Mock<IApplicationDbContext>();
            context.Setup(x => x.ProductTypes).Returns(() => productTypes.ToMockDbSet());
            controller = new ProductTypesController(context.Object);
        }

        [Fact]
        public async Task Get_All()
        {
            // Arrange
            var size = (int?)null;
            var page = (int?)null;

            // Act
            var result = (await controller.GetProductTypes(size, page)).Value;

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(productTypeCount, result.Count());
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(5, 1)]
        [InlineData(6, 0)]
        [InlineData(8, 0)]
        [InlineData(1, 8)]
        public async Task Get_Size_Page(int? size = null, int? page = null)
        {
            // Arrange

            // Act
            var result = (await controller.GetProductTypes(size, page)).Value;

            // Assert
            Assert.NotEmpty(result);
        }
    }
}
