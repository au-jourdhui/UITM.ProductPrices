using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public interface IApplicationDbContext : IDisposable, IAsyncDisposable
    {
        DbSet<ContractorProductPrice> ContractorProductPrices { get; set; }
        DbSet<Contractor> Contractors { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductType> ProductTypes { get; set; }

        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
    }
}