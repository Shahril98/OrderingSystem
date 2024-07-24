using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Infrastructure;

namespace OrderingSystem.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetActiveOrderByTable(Guid tableId);
        Task<TableOrderSummaryDto> GetTableOrderSummary(Guid tableId);
    }

    public class OrderRepository(OrderingSystemDbContext context) : IOrderRepository
    {
        private IQueryable<OrderDto> GetOrders()
        {
            var result = context.TblOrder.Include(o => o.Menu).Select(o =>
            new OrderDto(o.Id, o.Menu.Name, o.Total, o.Note, o.TableId, o.Status))
                .AsQueryable();
            return result;
        }
        public async Task<List<OrderDto>> GetActiveOrderByTable(Guid tableId)
        {
            var result = await GetOrders().Where(o => o.TableId == tableId && new OrderStatus[] {OrderStatus.Preparing, OrderStatus.Placed }.Contains(o.Status))
                .ToListAsync();
            return result;
        }
        public async Task<TableOrderSummaryDto> GetTableOrderSummary(Guid tableId)
        {
            var orders = await (from table in context.TblTable
                                join order in context.TblOrder on table.Id equals order.TableId
                                join menu in context.TblMenu on order.MenuId equals menu.Id
                                where table.Id == tableId && new OrderStatus[] { OrderStatus.Completed, OrderStatus.Preparing, OrderStatus.Placed }.Contains(order.Status)
                                select new 
                                {
                                    order.TableId,
                                    TableNumber = table.Number,
                                    OrderSummary = new OrderSummaryDto(order.Id, menu.Name, order.Total, order.Note, order.Status, menu.Price)
                                })
                                .ToListAsync();
            if (orders.Count == 0)
                throw new RecordNotFoundException();
            var tableDetails = orders.Select(o => new { o.TableId, o.TableNumber }).First();
            var result = new TableOrderSummaryDto(tableDetails.TableId, tableDetails.TableNumber, orders.Select(o => o.OrderSummary).ToList());
            return result;
        }
    }
}
