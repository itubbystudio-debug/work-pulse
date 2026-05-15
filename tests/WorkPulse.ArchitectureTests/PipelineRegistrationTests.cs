using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WorkPulse.Application;
using WorkPulse.Application.Common.Behaviors;

namespace WorkPulse.ArchitectureTests;

public sealed class PipelineRegistrationTests
{
    [Fact]
    public void Application_Should_Register_MediatR_Pipeline_In_Required_Order()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApplication();

        var behaviorTypes = services
            .Where(descriptor => descriptor.ServiceType == typeof(IPipelineBehavior<,>))
            .Select(descriptor => descriptor.ImplementationType)
            .ToArray();

        behaviorTypes.Should().Equal(
            typeof(LoggingBehavior<,>),
            typeof(UnhandledExceptionBehavior<,>),
            typeof(ValidationBehavior<,>),
            typeof(PerformanceBehavior<,>));
    }
}
