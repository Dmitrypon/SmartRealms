using TechTalk.SpecFlow.CommonModels;

namespace SmartRealms.API.SpaceDevices.Domain.Interfaces
{
    public interface IDevicesService
    {

        Task<Result<int>> Create(string frontSide, string backSide, Guid? userId);

        Task<Card[]> Get(Guid? userId);

        Task<Result<bool>> Delete(int cardId);

        Task<Result<bool>> Update(int cardId, string cardUpdatFrontSide, string cardUpdateBackSide);
    }
}
