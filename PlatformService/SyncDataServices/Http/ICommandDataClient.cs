using System.Threading.Tasks;
using PlatformService.Dtos;

namespace SyncDataService.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}