using Application.Utils.Interfaces.Mediator;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using SLAIS.Presentation.Server;

namespace Presentation.Tests.Common;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public readonly IMediator MediatorMock = Substitute.For<IMediator>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var mediatorDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IMediator));

            if (mediatorDescriptor != null)
            {
                services.Remove(mediatorDescriptor);
            }

            services.AddSingleton(MediatorMock);
        });

        builder.UseEnvironment("Testing");
    }
}
