using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblBase
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimestampCreated { get; set; }
        public DateTimeOffset TimestampUpdated { get; set; }
    }
}
