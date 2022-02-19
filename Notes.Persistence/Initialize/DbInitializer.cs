using Parcus.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Initialize
{
    public class DbInitializer
    {
        public static void Initilize(AppDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
