namespace pharm4me7.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ChangeLog : DbContext
    {
        // Your context has been configured to use a 'ChangeLog' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'pharm4me7.Models.ChangeLog' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ChangeLog' 
        // connection string in the application configuration file.
        public ChangeLog()
            : base("name=ChangeLog")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}