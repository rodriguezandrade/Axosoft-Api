using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;
using SS.Mvc.AxosoftApi.Model;

namespace SS.Mvc.AxosoftApi.Data
{
    public class ModelContext : DbContext
    {
        public ModelContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public IDbSet<Role> Roles { get; set; }

        public IDbSet<UserRole> UserRoles { get; set; }

        public IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ModelNamespaceConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Add mappings
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var builder = new StringBuilder(ex.Message);

                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    builder.AppendLine().Append(entityValidationErrors.Entry.Entity.GetType())
                        .AppendLine(" errors:");

                    foreach (var error in entityValidationErrors.ValidationErrors)
                    {
                        builder
                            .Append("\t")
                            .Append(error.PropertyName)
                            .Append(": ")
                            .AppendLine(error.ErrorMessage);
                    }
                }

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(builder.ToString(), ex.EntityValidationErrors);
            }
        }
    }
}
