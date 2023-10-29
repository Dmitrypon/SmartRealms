namespace SmartRealms.API.DTO
{
    public class DeviceDTO
    {
        private object item;

        public DeviceDTO(object item)
        {
            this.item = item;
        }

        public int? Id { get; set; }
        public int? LocalionId { get; set; }
        public string Serial { get; set; }

    }
}
