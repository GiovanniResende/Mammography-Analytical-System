using SistemaTCC.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTCC.data
{

    public class context : DbContext
    {
        public context() 
        : base("name=SAMconn")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<tblusuario> Usuario { get; set; }
    }
    public class tblusuario
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }        
       
    }
    
}
