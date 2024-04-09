using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.Repositories.DBContext
{
    public class CommonDBContext : DbContext
    {
        public CommonDBContext(DbContextOptions<CommonDBContext> options) : base(options)
        {
        }

        public DbSet<RobotFx.DoMain.Models.User> Users { get; set; }
        public DbSet<RobotFx.DoMain.Models.AccountFx> AccountFxs { get; set; }
    }
}
