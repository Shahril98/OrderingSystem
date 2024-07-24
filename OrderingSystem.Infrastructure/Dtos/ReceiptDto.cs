using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Dtos
{
    public class AddReceiptDto
    {
        [Required]
        public double Total { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }
        [Required]
        [StringLength(50)]
        public string TransactionId { get; set; }
        [Required]
        public List<Guid> OrderIds { get; set; }
    }
    public record ReceiptDto (Guid Id, double Total, PaymentType PaymentType, string TransactionId);
    public record ReceiptFullDetailsDto(Guid Id, double Total, PaymentType PaymentType, string TransactionId, List<OrderDto> Orders);
}
