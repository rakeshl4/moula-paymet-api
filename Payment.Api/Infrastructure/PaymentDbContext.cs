using Microsoft.EntityFrameworkCore;

namespace Payment.Api.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext
            (DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Payment> PaymentCollection { get; set; }
        public DbSet<Models.Customer> CustomerCollection { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Models.Payment>().ToTable("Payment");
            builder.Entity<Models.Payment>().HasKey(p => p.Id);
            builder.Entity<Models.Payment>().Property(p => p.Id).IsRequired();
            builder.Entity<Models.Payment>().Property(p => p.CustomerId).IsRequired();
            builder.Entity<Models.Payment>().Property(p => p.Amount).IsRequired();
            builder.Entity<Models.Payment>().Property(p => p.TransactionDate).IsRequired();
            builder.Entity<Models.Payment>().Property(p => p.StatusId).IsRequired();
            builder.Entity<Models.Payment>().Property(p => p.Comments).HasMaxLength(512);

            builder.Entity<Models.Customer>().ToTable("Customer");
            builder.Entity<Models.Customer>().HasKey(p => p.Id);
            builder.Entity<Models.Customer>().Property(p => p.Id).IsRequired();
            builder.Entity<Models.Customer>().Property(p => p.FirstName).IsRequired().HasMaxLength(512);
            builder.Entity<Models.Customer>().Property(p => p.LastName).IsRequired().HasMaxLength(512);
            builder.Entity<Models.Customer>().Property(p => p.TotalBalance).IsRequired();

            var customerIdOne = new System.Guid("eb07ea19-38cc-4579-892c-510da1eca613");
            var customerIdTwo = new System.Guid("639485dc-edb9-4e0d-abc5-b164db1aa497");
            builder.Entity<Models.Customer>().HasData(
            new Models.Customer
            {
                Id = customerIdOne,
                FirstName = "David",
                LastName = "Snedakar",
                TotalBalance = 4000
            },
            new Models.Customer
            {
                Id = customerIdTwo,
                FirstName = "Curtis",
                LastName = "Peltz",
                TotalBalance = 28000
            });

            builder.Entity<Models.Payment>().HasData(
          new Models.Payment
          {
              Id = System.Guid.NewGuid(),
              CustomerId = customerIdOne,
              Amount = 323,
              StatusId = 1,
              Comments = "Processed",
              TransactionDate = System.DateTime.Now
          },
          new Models.Payment
          {
              Id = System.Guid.NewGuid(),
              CustomerId = customerIdOne,
              Amount = 400,
              StatusId = 0,
              Comments = "Pending",
              TransactionDate = System.DateTime.Now
          },
            new Models.Payment
            {
                Id = System.Guid.NewGuid(),
                CustomerId = customerIdTwo,
                Amount = 222,
                StatusId = 1,
                Comments = "Processed",
                TransactionDate = System.DateTime.Now
            },
          new Models.Payment
          {
              Id = System.Guid.NewGuid(),
              CustomerId = customerIdTwo,
              Amount = 900,
              StatusId = 0,
              Comments = "Pending",
              TransactionDate = System.DateTime.Now
          });
        }
    }
}
