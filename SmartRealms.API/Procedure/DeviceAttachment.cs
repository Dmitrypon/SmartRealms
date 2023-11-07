using SmartRealms.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartRealms.API.Procedure
{
    /// <summary>
    /// Procedure of adding a device
    /// </summary>
    public class DeviceAttachment
    {
        public int Id { get; set; }

        public int? ProjectId { get; set; }

        public string NodeSerial { get; set; }

        public string Serial { get; set; }

        public DeviceType Type { get; set; }
    }
}
