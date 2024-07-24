using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblReceipt: TblBaseSoftDelete
    {
        public double Total {  get; set; }
        public PaymentType PaymentType { get; set; }
        public string TransactionId { get; set; } = null!;
        public ICollection<TblOrderToReceipt> OrderToReceipts { get; init; } = [];
    }
}
