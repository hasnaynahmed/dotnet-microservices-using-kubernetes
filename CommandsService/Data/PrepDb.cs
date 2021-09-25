using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using(var servicescope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = servicescope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                SeedData(servicescope.ServiceProvider.GetService<ICommandRepo>(),platforms );
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
                Console.WriteLine("--> Seeding new platforms...");
                foreach(var plat in platforms)
                {
                    if(!repo.ExternalPlatformExists(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();

                        Console.WriteLine($"--> Added new platforms. {plat.Name}");
                    }
                }
        }
    }

}