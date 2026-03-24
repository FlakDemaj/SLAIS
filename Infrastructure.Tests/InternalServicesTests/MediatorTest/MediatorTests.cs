using Application.Utils.Logger;
using Application.Utils.Mediator.Interfaces;

using FluentAssertions;

using Infrastructure.InternalServices;
using Infrastructure.Tests.InternalServicesTests.MediatorTest.Helpers;
using Infrastructure.Transaction;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Infrastructure.Tests.InternalServicesTests.MediatorTest;

public class MediatorTests
{
    #region Setup

    private readonly ISlaisLogger<Mediator> _logger;

    public MediatorTests()
    {
        _logger = Substitute.For<ISlaisLogger<Mediator>>();
    }

    private Mediator BuildMediator(IServiceProvider serviceProvider)
    {
        return new Mediator(serviceProvider, _logger);
    }

    #endregion

    #region SendAsync

    [Fact]
    public async Task SendAsync_ShouldReturnResult_WhenHandlerExists()
    {
        // Arrange
        var request = new TestRequest();
        var expectedResult = new TestResponse { Value = "test-result" };

        var handler = Substitute.For<IRequestHandler<TestRequest, TestResponse>>();

        handler
            .HandleAsync(request, Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var pipeline = Substitute.For<IPipelineTransactionBehavior<TestRequest, TestResponse>>();

        pipeline
            .HandleAsync(
                request,
                Arg.Any<Func<Task<TestResponse>>>(),
                Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var next = callInfo.ArgAt<Func<Task<TestResponse>>>(1);
                return next();
            });

        var services = new ServiceCollection();

        services.AddSingleton(handler);
        services.AddSingleton(pipeline);
        services.AddSingleton(_logger);

        var serviceProvider = services.BuildServiceProvider();
        var mediator = BuildMediator(serviceProvider);

        // Act
        var result = await mediator.SendAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(expectedResult.Value);

        await handler
            .Received(1)
            .HandleAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendAsync_ShouldThrowException_WhenHandlerNotFound()
    {
        // Arrange
        var request = new TestRequest();

        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var mediator = BuildMediator(serviceProvider);

        // Act
        var act = async () => await mediator.SendAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SendAsync_ShouldCallPipeline_BeforeHandler()
    {
        // Arrange
        var callOrder = new List<string>();
        var request = new TestRequest();
        var expectedResult = new TestResponse { Value = "test-result" };

        var handler = Substitute.For<IRequestHandler<TestRequest, TestResponse>>();

        handler
            .HandleAsync(request, Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                callOrder.Add("Handler");
                return expectedResult;
            });

        var pipeline = Substitute.For<IPipelineTransactionBehavior<TestRequest, TestResponse>>();

        pipeline
            .HandleAsync(
                request,
                Arg.Any<Func<Task<TestResponse>>>(),
                Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                callOrder.Add("Pipeline");
                var next = callInfo.ArgAt<Func<Task<TestResponse>>>(1);
                return next();
            });

        var services = new ServiceCollection();

        services.AddSingleton(handler);
        services.AddSingleton(pipeline);
        services.AddSingleton(_logger);

        var serviceProvider = services.BuildServiceProvider();
        var mediator = BuildMediator(serviceProvider);

        // Act
        await mediator.SendAsync(request, CancellationToken.None);

        // Assert
        callOrder.Should().ContainInOrder("Pipeline", "Handler");
    }

    #endregion
}
