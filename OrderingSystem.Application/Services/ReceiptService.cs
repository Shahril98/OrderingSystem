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
    public interface IReceiptService
    {
        Task<Guid> AddReceipt(AddReceiptDto addReceipt, Guid userId);
        Task<ReceiptDto> GetReceipt(Guid receiptId);
        Task<ReceiptFullDetailsDto> GetReceiptFullDetails(Guid receiptId);
    }

    public class ReceiptService(IBaseRepository baseRepository, IReceiptRepository receiptRepository, IOrderService orderService) : IReceiptService
    {
        public async Task<Guid> AddReceipt(AddReceiptDto addReceipt, Guid userId)
        {
            var ordersIsValid = await orderService.AllOrderIsValidToComplete(addReceipt.OrderIds);
            if (ordersIsValid)
                throw new ActionNotValidException("Pesanan tidak dijumpai atau pesanan tidak sah");
            using var t = await baseRepository.StartTransaction();
            var receiptToSave = addReceipt.Adapt<TblReceipt>();
            baseRepository.AddData(receiptToSave);
            await baseRepository.SaveChanges(userId);
            List<TblOrderToReceipt> orderReceiptToUpdate = [];
            foreach (var orderId in addReceipt.OrderIds)
            {
                await orderService.UpdateOrderStatus(orderId, OrderStatus.Completed, userId);
                orderReceiptToUpdate.Add(new() { OrderId = orderId, ReceiptId = receiptToSave.Id });
            }
            baseRepository.AddDataBatch(orderReceiptToUpdate);
            await baseRepository.SaveChanges();
            return receiptToSave.Id;
        }
        public async Task<ReceiptDto> GetReceipt(Guid receiptId)
        {
            var result = await baseRepository.GetDataById<TblReceipt>(receiptId);
            return result.Adapt<ReceiptDto>();
        }
        public async Task<ReceiptFullDetailsDto> GetReceiptFullDetails(Guid receiptId)
        {
            var result = await receiptRepository.GetReceiptFullDetails(receiptId);
            return result;
        }
    }
}
