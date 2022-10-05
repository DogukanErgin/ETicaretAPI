using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistent.Contexts
{
    public class ETicaretAPIDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ETicaretAPIDbContext(DbContextOptions options) : base(options)
        {
            //IoC container da doldurulacak
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ETicaretAPI.Domain.Entities.File> Files { get; set; }

        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketsItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasKey(b=>b.Id);

            builder.Entity<Basket>().HasOne(b=>b.Order).WithOne(o=>o.Basket).HasForeignKey<Order>(b=>b.Id);
            base.OnModelCreating(builder);
        }



        public override async Task<int> SaveChangesAsync(CancellationToken
            cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch  //alttakileri kapsamasa bile en alttaki kondisyon data nın yeni state i olacaktır. discard verirsek herhangi bir sıkıntıyla karşılaşmayız örnek sıkıntı ise şu şekilde eğer bir veri silersek data state deleted olarak gelecek ve eski haline göre modified state i deleted bir yapıya eklenmek istenecektir bu da patlamaya sebep verir.
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,

                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow


                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        
    }


}
