using CatalogApi.Application.Interfaces;
using CatalogApi.Domain.Entities;
using CatalogApi.Infrastructure.Data;
using Dukkani.Shared.Logs;
using Dukkani.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext dbContext) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var product = await GetByAsync(_ => _.Name.Equals(entity.Name));
                if (product is not null && !string.IsNullOrEmpty(product.Name))
                    return new Response(false, $"Product with name {entity.Name} is already exists");

                var newProduct = dbContext.Products.Add(entity).Entity;
                await dbContext.SaveChangesAsync();
                if (newProduct is not null && newProduct.Id > 0)
                    return new Response(true, $"{entity.Name} has been added successfully");
                else
                    return new Response(false, $"Error while adding {entity.Name}");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occurred while adding the product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} Not found");
                dbContext.Products.Remove(entity);
                await dbContext.SaveChangesAsync();
                return new Response(true, $"{entity.Name} Deleted successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occurred while deleting the product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FindAsync(id);
                return product is not null ? product : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while retrieving the product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await dbContext.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while retrieving the products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await dbContext.Products.Where(predicate).FirstOrDefaultAsync();
                return product is not null ? product : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occurred while retrieving product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = FindByIdAsync(entity.Id);
                if (product is null)
                    return new Response(false, $"{entity.Name} Not found");

                dbContext.Entry(product).State = EntityState.Detached;
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
                return new Response(true, $"{entity.Name} is updated successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occurred while retrieving product");
            }
        }
    }
}
