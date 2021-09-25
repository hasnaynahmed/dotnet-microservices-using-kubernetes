using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();        

        //Platform related items
        IEnumerable<Platform> GetAllPlatfroms();
        void CreatePlatform(Platform plat);
        bool PlaformExits(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        //Command related items
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commnadId);
        void CreateCommand(int platformId, Command command);


    }
}