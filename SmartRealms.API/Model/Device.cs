﻿using SmartRealms.API.Enums;

namespace SmartRealms.API.Model
{
    public class Device
    {
        public int Id { get; set; }

        public int? ProjectId { get; set; }

        public string NodeSerial { get; set; }

        public string Serial { get; set; }

        public DeviceType Type { get; set; }

    }
}
