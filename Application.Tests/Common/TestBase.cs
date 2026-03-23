using AutoMapper;

namespace Application.Tests.Common;

public class TestBase
{
    protected readonly IMapper _mapper;

    protected TestBase()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddMaps(typeof(IApplicationAssemblyMarker).Assembly);
        });

        _mapper = configuration.CreateMapper();
    }
}
