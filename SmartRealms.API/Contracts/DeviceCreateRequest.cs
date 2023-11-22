using SmartRealms.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartRealms.API.Procedure
{
    /// <summary>
    /// Procedure of adding a new device
    /// </summary>
    public class DeviceCreateRequest
    {
        public int Id { get; set; }

        public int? LocationId { get; set; }

        
        public string Serial { get; set; }

        public DeviceType Type { get; set; }
    }
}
