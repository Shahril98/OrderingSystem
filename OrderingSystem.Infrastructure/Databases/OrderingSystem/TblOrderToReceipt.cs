using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblOrderToReceipt: TblBase
    {
        public Guid OrderId { get; set; }
        public TblOrder Order { get; init; } = null!;
        public Guid ReceiptId { get; set; }
        public TblReceipt Receipt { get; init; } = null!;
    }
}
