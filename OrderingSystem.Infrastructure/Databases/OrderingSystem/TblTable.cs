using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblTable: TblBaseSoftDelete
    {
        public string Number { get; set; } = null!;
        public ICollection<TblOrder> Orders { get; init; } = [];
    }
}
