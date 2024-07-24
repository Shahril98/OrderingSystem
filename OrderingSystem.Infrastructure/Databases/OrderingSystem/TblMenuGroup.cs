using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblMenuGroup: TblBaseSoftDelete
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<TblMenu> Menus { get; init; } = [];
    }
}
