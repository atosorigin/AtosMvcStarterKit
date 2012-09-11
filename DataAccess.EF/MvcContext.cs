using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Customer.Project.Domain.Entities;

namespace Customer.Project.DataAccessEF
{
    public class MvcContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<DataAccess.EF.Mvc4Context>());

        public MvcContext()
        {
            Database.SetInitializer(new DbInitializer());

            // for returning entity lists as Json
            // prevents error: You must write an attribute 'type'='object' after writing the attribute with local name '__type'
            this.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// create here your database model.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            CreateModel(modelBuilder);
        }

        public void CreateModel(DbModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

                //modelBuilder.Entity<Branch>().HasKey(s => s.Id);
                //modelBuilder.Entity<Scheme>()
                //    .HasRequired(s => s.Branch)
                //    .WithMany()
                //    .HasForeignKey(s => s.BranchId);
            }
        }
    }
}