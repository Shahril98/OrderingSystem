using Mapster;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public interface IFileService
    {
        Task<AddFileResultDto> AddFile(AddFileDto addFile);
        Task<List<AddFileResultDto>> AddFiles(List<AddFileDto> addFiles);
        Task<FileDto> GetPublicFileData(Guid fileId);
        Task RemoveFiles(IEnumerable<Guid> fileIds);
    }

    public class FileService(IBaseRepository baseRepository, IFileRepository fileRepository) : IFileService
    {
        public async Task<AddFileResultDto> AddFile(AddFileDto addFile)
        {
            if (addFile.FileSize > FileSize.MaxSize)
                throw new BadRequestException("Saiz fail tidak boleh melebihi 10mb");
            var toSave = addFile.Adapt<TblFile>();
            baseRepository.AddData(toSave);
            await baseRepository.SaveChanges();
            return toSave.Adapt<AddFileResultDto>();
        }
        /// <summary>
        /// this method may consume a lot of memory if use concurrently
        /// </summary>
        /// <param name="addFiles"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<List<AddFileResultDto>> AddFiles(List<AddFileDto> addFiles)
        {
            if (addFiles.Any(o => o.FileSize > FileSize.MaxSize))
                throw new BadRequestException("Saiz fail tidak boleh melebihi 10mb");
            var toSave = addFiles.Adapt<List<TblFile>>();
            baseRepository.AddDataBatch(toSave);
            await baseRepository.SaveChanges();
            return toSave.Adapt<List<AddFileResultDto>>();
        }
        public async Task RemoveFiles (IEnumerable<Guid> fileIds)
        {
            var toRemoves = new List<TblFile>();
            foreach(var fileId in fileIds)
            {
                toRemoves.Add(new()
                {
                    Id = fileId,
                });
            }
            baseRepository.DeleteDataBatch(toRemoves);
            await baseRepository.SaveChanges();
        }

        public async Task<FileDto> GetPublicFileData(Guid fileId)
        {
            var data = await fileRepository.GetPublicFileData(fileId);
            return data;
        }
    }
}
