using IdentityMvc.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.Data.EntityConfiguration
{
    public class UserAccountConfigration : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountConfigration()
        {
            Property(p => p.DisplayName).HasMaxLength(100).HasColumnType("nvarchar");
        }
    }
}
