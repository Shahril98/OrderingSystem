using Mapster;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public interface ITableService
    {
        Task<TableDto> AddTable(AddTableDto table);
         FileDto CreateTableQrCode(Guid tableId, string link);
        Task DeleteTable(Guid tableId);
        Task<List<TableDto>> GetAllTables();
        Task<TableDto> GetTableById(Guid tableId);
        Task<TableDto> UpdateTable(UpdateTableDto table, Guid tableId);
    }

    public class TableService(IBaseRepository baseRepository) : ITableService
    {
        public async Task<TableDto> AddTable(AddTableDto table)
        {
            var toSave = table.Adapt<TblTable>();
            baseRepository.AddData(toSave);
            await baseRepository.SaveChanges();
            return toSave.Adapt<TableDto>();
        }
        public async Task<TableDto> UpdateTable(UpdateTableDto table, Guid tableId)
        {
            var dbData = await baseRepository.GetDataById<TblTable>(tableId);
            table.Adapt(dbData);
            await baseRepository.SaveChanges();
            return dbData.Adapt<TableDto>();
        }
        public async Task<List<TableDto>> GetAllTables()
        {
            var data = await baseRepository.GetAllData<TblTable>();
            return data.Adapt<List<TableDto>>();
        }
        public async Task<TableDto> GetTableById(Guid tableId)
        {
            var data = await baseRepository.GetDataById<TblTable>(tableId);
            return data.Adapt<TableDto>();
        }
        public async Task DeleteTable(Guid tableId)
        {
            var toDelete = await baseRepository.GetDataById<TblTable>(tableId);
            baseRepository.DeleteData(toDelete);
            await baseRepository.SaveChanges();
        }
        public FileDto CreateTableQrCode(Guid tableId, string link)
        {
            using QRCodeGenerator qrGenerator = new();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(50);
            return new FileDto($"{tableId}", ".png", "image/png", qrCodeImage); 
        }
    }
}
