using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ReadLaterDataContext>
    {
        public ReadLaterDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReadLaterDataContext>();
            optionsBuilder.UseSqlServer("Server=tcp:ptmsqldatauk.database.windows.net,1433;Database=AHDev;User ID=Demo;Password=ReadLater5;Trusted_Connection=False;Encrypt=True;");

            return new ReadLaterDataContext(optionsBuilder.Options);
        }
    }
}
