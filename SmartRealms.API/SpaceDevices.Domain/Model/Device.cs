using SmartRealms.API.Enums;

namespace SmartRealms.API.SpaceDevices.Domain.Model
{
    public class Device
    {
        public int Id { get; set; }

        public int? LocationId { get; set; }

        public string NodeSerial { get; set; }

        public string Serial { get; set; }

        public DeviceTypes Type { get; set; }
    }
}
