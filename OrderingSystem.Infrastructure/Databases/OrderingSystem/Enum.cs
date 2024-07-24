using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Databases.OrderingSystem
{
   
    public enum MenuType
    {
        Others,
        MainCourse,
        Drinks,
        Dessert
    }

    public enum PaymentType
    {
        Cash,
        QrScan,
        CreditCard
    }
    public enum OrderStatus
    {
        Placed,
        Preparing,
        Completed,
        Rejected,
        Paid
    }
    public enum MenuStatus
    {
        Available,
        NotActive,
        NotAvailable,
        SoldOut
    }
}
