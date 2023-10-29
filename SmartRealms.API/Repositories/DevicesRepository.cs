namespace SmartRealms.API.Repositories
{
    public class DevicesRepository
    {
        private readonly DevicesContext _context;

        public DevicesRepository(DevicesContext context)
        {
            _context = context;
        }

        internal async Task SetDeviceState(string serial, DeviceState state, DateTimeOffset eventTime)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Serial == serial && d.NodeSerial == null);
            if (device != null)
            {
                if (state == DeviceState.Offline)
                {
                    var children = await _context.Devices.Where(d => d.NodeSerial == serial).ToListAsync();
                    children.ForEach(c => c.State = state);
                    children.ForEach(c => c.Updated = eventTime);
                }
                device.State = state;
                device.Updated = eventTime;
                await _context.SaveChangesAsync();
            }
        }

        internal async Task<List<string>> GetNodesAsync()
        {
            return await _context.Devices.Where(d => string.IsNullOrWhiteSpace(d.NodeSerial)).Select(d => d.Serial).ToListAsync();
        }

        internal async Task SetDeviceState(string nodeSerial, string serial, DeviceState state, DateTimeOffset eventTime)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.NodeSerial == nodeSerial && d.Serial == serial);
            if (device != null)
            {
                device.State = state;
                device.Updated = eventTime;
                await _context.SaveChangesAsync();
            }
        }

        internal async Task UpdateData(string serial, DeviceData deviceData)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Serial == serial);
            if (device != null)
            {
                device.Data = deviceData;
                device.Updated = deviceData.Updated;
                await _context.SaveChangesAsync();
            }
        }

        internal async Task UpdateData(string nodeSerial, string serial, DeviceData deviceData)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.NodeSerial == nodeSerial && d.Serial == serial);
            if (device != null)
            {
                device.Data = deviceData;
                device.Updated = deviceData.Updated;
                await _context.SaveChangesAsync();
            }
        }

        internal async Task<long> CountInProject(int projectId)
        {
            return await _context.Devices.Where(d => d.ProjectId == projectId)
                .LongCountAsync();
        }

        internal async Task<ICollection<DeviceDTO>> GetPage(int projectId, int pageSize, int pageIndex)
        {
            return await _context.Devices
                .Where(d => d.ProjectId == projectId)
                .Include(d => d.DeviceInGroups)
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .Select(d => new DeviceDTO(d))
                .ToListAsync();
        }

        internal async Task<bool> Exists(string serial)
        {
            return await _context.Devices.AnyAsync(d => d.Serial.Equals(serial) && d.NodeSerial == null);
        }

        internal async Task<bool> Exists(string nodeSerial, string serial)
        {
            return await _context.Devices.AnyAsync(d => d.NodeSerial.Equals(nodeSerial) && d.Serial.Equals(serial));
        }

        internal async Task Add(List<Device> devices)
        {
            await _context.Devices.AddRangeAsync(devices);
        }

        internal async Task<List<Device>> GetItemsByIdsAsync(int projectId, IEnumerable<int> idsToSelect)
        {
            return await _context.Devices.Where(d => d.ProjectId == projectId && idsToSelect.Contains(d.Id)).Include(d => d.DeviceInGroups).ToListAsync();
        }

        internal async Task<Device> GetAsync(int id)
        {
            return await _context.Devices.Include(d => d.DeviceInGroups).SingleOrDefaultAsync(ci => ci.Id == id);
        }

        internal async Task<List<Device>> GetByNodeAsync(string nodeSerial)
        {
            return await _context.Devices.Where(d => d.NodeSerial.Equals(nodeSerial)).ToListAsync();
        }

        /// <summary>
        /// Find node by its serial number
        /// </summary>
        /// <param name="serial">device serial number</param>
        /// <returns>Device or null</returns>
        internal async Task<Device> GetNodeAsync(string serial)
        {
            return await _context.Devices.SingleOrDefaultAsync(ci => ci.Serial == serial && ci.NodeSerial == null);
        }

        /// <summary>
        /// Find device by its serial number
        /// </summary>
        /// <param name="serial">device serial number</param>
        /// <returns>Device or null</returns>
        internal async Task<Device> GetDeviceAsync(string serial, string nodeSerial)
        {
            return await _context.Devices.SingleOrDefaultAsync(ci => ci.Serial == serial && ci.NodeSerial.Equals(nodeSerial));
        }

        internal async Task Update(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        internal async Task Add(Device item)
        {
            _context.Devices.Add(item);

            await _context.SaveChangesAsync();
        }

        internal async Task Delete(Device device)
        {
            _context.Devices.Remove(device);

            await _context.SaveChangesAsync();
        }
    }

}
}
