using CatalogApi.Domain.Entities;

namespace CatalogApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDto productDto)
        {
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Quantity = productDto.Quantity,
                Price = productDto.Price,
            };
        }
        public static (ProductDto?, IEnumerable<ProductDto>?) FromEntity(Product? product, IEnumerable<Product>? products)
        {
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDto
                (
                    Id: product.Id,
                    Name: product.Name,
                    Quantity: product.Quantity,
                    Price: product.Price
                    );

                return (singleProduct, null);
            }
            //return list of
            if (products is not null || product is null)
            {
                var lov = products!.Select(p => new ProductDto(Id: p.Id, Name: p.Name!, Quantity: p.Quantity, Price: p.Price)).ToList();
                return (null, lov);
            }
            return (null, null);
        }
    }
}
