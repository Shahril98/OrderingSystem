using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblMenuImage: TblBase
    {
        public Guid MenuId { get; set; }
        public TblMenu Menu { get; init; } = null!;
        public Guid FileId { get; set; }
        public TblFile Image { get; init; } = null!;
        public int Order {  get; set; }
    }
}
