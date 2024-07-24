using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Dtos
{
    public class PlaceOrderDto
    {
        [Required]
        public Guid MenuId { get; set; }
        [Required]
        public int Total { get; set; }
        [Required]
        [StringLength(200)]
        public string Note { get; set; }
    }
    public record OrderDto(Guid Id, string MenuName, int Total, string Note, Guid TableId, OrderStatus Status);
    public record OrderSummaryDto(Guid Id, string Menu, int Total, string Note, OrderStatus Status, double Price)
    {
        public double TotalPrice => Price * Total;
    }
    public record TableOrderSummaryDto(Guid TableId, string TableNumber, List<OrderSummaryDto> Items)
    {
        public double TotalAll => Items.Sum(x => x.TotalPrice);
    }
}
