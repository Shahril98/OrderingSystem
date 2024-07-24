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
    public interface IReceiptRepository
    {
        Task<ReceiptFullDetailsDto> GetReceiptFullDetails(Guid receiptId);
    }

    public class ReceiptRepository(OrderingSystemDbContext dbContext) : IReceiptRepository
    {
        public async Task<ReceiptFullDetailsDto> GetReceiptFullDetails(Guid receiptId)
        {
            var data = await (from receipt in dbContext.TblReceipt
                              join orderReceipt in dbContext.TblOrderToReceipt on receipt.Id equals orderReceipt.ReceiptId
                              join order in dbContext.TblOrder on orderReceipt.OrderId equals order.Id
                              where receipt.Id == receiptId
                              select new { receipt, order = order.Adapt<OrderDto>() }
                              ).ToListAsync();
            if (data.Count > 0)
                throw new RecordNotFoundException();
            var receiptDetails = data.Select(o => o.receipt).First();
            ReceiptFullDetailsDto result = new(receiptId, receiptDetails.Total, receiptDetails.PaymentType, receiptDetails.TransactionId, data.Select(o => o.order).ToList());
            return result;
        }
    }
}
