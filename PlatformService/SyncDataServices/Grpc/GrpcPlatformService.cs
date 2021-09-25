using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.AsyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response =  new PlatformResponse();
            var platfroms = _repository.GetAllPlatforms();

            foreach(var plat in platfroms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }
            return Task.FromResult(response);

        }
    }
}