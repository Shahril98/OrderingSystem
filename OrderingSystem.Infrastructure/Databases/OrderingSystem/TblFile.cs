using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
    public class TblFile: TblBase
    {
        public string Name { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
        public long FileSize { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<TblMenuImage> MenuImages { get; init; } = [];
    }
}
