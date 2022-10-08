using Headspring;
using System.Linq;

namespace ArandaSoftLab.Core.Domain.Base
{
    public class EventTypeEnumeration : Enumeration<EventTypeEnumeration, string>
    {
        public static readonly EventTypeEnumeration Add = new EventTypeEnumeration("AGREGAR", "AGREGAR");
        public static readonly EventTypeEnumeration Update = new EventTypeEnumeration("MODIFICAR", "MODIFICAR");
        public static readonly EventTypeEnumeration Delete = new EventTypeEnumeration("ELIMINAR", "ELIMINAR");
        private EventTypeEnumeration(string value, string displayName) : base(value, displayName) { }
        public static bool Is(string Value)
        {
            return EventTypeEnumeration.GetAll().Any(rr => rr.Value == Value);
        }
        public static string GetDisplayValue(string value)
        {
            var objeto = GetAll().FirstOrDefault(rr => rr.Value == value);
            return (objeto != null ? objeto.DisplayName : value);
        }
    }
}
