using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Dtos
{
    public class AddTableDto
    {
        [Required]
        [StringLength(10)]
        public string Number { get; set; }
    }
    public class UpdateTableDto:AddTableDto
    {

    }
    public record TableDto(Guid Id,  string Number);
}
