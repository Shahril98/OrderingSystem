using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblMenu : TblBaseSoftDelete
    {
        public string Name { get; set; } = null!;
        public MenuType MenuType { get; set; }
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public MenuStatus MenuStatus { get; set; }
        public ICollection<TblOrder> Orders { get; init; } = [];
        public Guid? MenuGroupId { get; set; }
        public TblMenuGroup? MenuGroup { get; init; } = null!;
        public ICollection<TblMenuImage> MenuImages { get; init; } = [];
    }
}
