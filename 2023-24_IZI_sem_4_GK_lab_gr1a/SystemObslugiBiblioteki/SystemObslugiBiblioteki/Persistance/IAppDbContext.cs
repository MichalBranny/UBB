using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObslugiBiblioteki.Model;
using SystemObslugiBiblioteki.Models;

namespace SystemObslugiBiblioteki.Persistence
{
    public interface IAppDbContext
    {
        DbSet<Books> Books { get; set; }
        DbSet<CheckOuts> CheckOuts { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync();
    }
}
