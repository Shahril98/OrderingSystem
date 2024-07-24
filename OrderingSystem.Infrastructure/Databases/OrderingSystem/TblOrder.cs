using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblOrder : TblBaseSoftDelete
    {
        public int Total { get; set; }
        public string Note { get; set; } = null!;
        public OrderStatus Status { get; set; }
        public Guid MenuId { get; set; }
        public TblMenu Menu { get; init; } = null!;
        public Guid TableId { get; set; }
        public TblTable Table { get; init; } = null!;
        public TblOrderToReceipt OrderToReceipt { get; init; } = null!;
    }
}
