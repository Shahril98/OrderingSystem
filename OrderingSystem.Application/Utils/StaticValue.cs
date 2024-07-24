using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Utils
{
    public enum EnvType
    {
        Development,
        Stagging,
        Production
    }
    public class StaticValue
    {
        public static EnvType envType;
    }

    public static class AppLogEvent
    {
        public static EventId RepositoryError = new EventId(5001, "Repository Error");
    }
}
