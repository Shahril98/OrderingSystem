using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Dtos
{
    public record AddFileDto(string Name, string Extension, string ContentType, byte[] Data, bool IsPublic)
    {
        public long FileSize = Data.LongLength;
    }
    public record FileDto(string Name, string Extension, string ContentType, byte[] Data)
    {
        [AdaptIgnore(MemberSide.Destination)]
        public string FullName { get { return Name + Extension; } }
    }
    public record AddFileResultDto(Guid? Id, string Name, string Extension, bool IsSuccess = true);

    public class SimpleFileDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullName { get { return Name + Extension; } }
    }

}
