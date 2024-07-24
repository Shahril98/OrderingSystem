using Mapster;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Repositories
{
    public interface IFileRepository
    {
        Task<FileDto> GetPublicFileData(Guid fileId);
    }

    public class FileRepository(OrderingSystemDbContext context) : IFileRepository
    {
        public async Task<FileDto> GetPublicFileData(Guid fileId)
        {
            var data = await context.TblFile.AsNoTracking().Where(o => o.Id == fileId && o.IsPublic)
                .Select(o => new FileDto(o.Name, o.Extension, o.ContentType, o.Data))
                .SingleOrDefaultAsync() ?? throw new RecordNotFoundException($"Tiadak rekod ditemui untuk id = '{fileId}'");
            return data;
        }
    }
}
