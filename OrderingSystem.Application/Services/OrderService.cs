using Mapster;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public interface IOrderService
    {
        Task<bool> AllOrderIsValidToComplete(List<Guid> orderIds);
        Task<List<OrderDto>> GetActiveTableOrder(Guid tableId);
        Task PlaceOrder(Guid tableId, List<PlaceOrderDto> orders);
        Task UpdateOrderStatus(Guid orderId, OrderStatus orderStatus, Guid userId);
    }

    public class OrderService(IBaseRepository baseRepository, IOrderRepository orderRepository) : IOrderService
    {
        public async Task PlaceOrder(Guid tableId, List<PlaceOrderDto> orders)
        {
            var toSave = orders.Adapt<List<TblOrder>>();
            foreach (var t in toSave)
            {
                t.TableId = tableId;
            }
            baseRepository.AddDataBatch(toSave);
            await baseRepository.SaveChanges();
        }
        public async Task<List<OrderDto>> GetActiveTableOrder(Guid tableId)
        {
            var result = await orderRepository.GetActiveOrderByTable(tableId);
            return result;
        }
        public async Task UpdateOrderStatus(Guid orderId, OrderStatus orderStatus, Guid userId)
        {
            var toUpdate = await baseRepository.GetDataById<TblOrder>(orderId);
            toUpdate.Status = orderStatus;
            await baseRepository.SaveChanges(userId);
        }
        public async Task<TableOrderSummaryDto> GetTableOrderSummary(Guid tableId)
        {
            var result = await orderRepository.GetTableOrderSummary(tableId);
            return result;
        }
        public async Task<bool> AllOrderIsValidToComplete(List<Guid> orderIds)
        {
            if(orderIds.Count == 0) return false;
            foreach (var orderId in orderIds)
            {
                var valid = await baseRepository.IsExistAsync<TblOrder>(o => o.Id == orderId && o.Status != OrderStatus.Paid && o.Status != OrderStatus.Rejected);
                if(!valid)
                    return false;
            }
            return true;
        }
    }
}
