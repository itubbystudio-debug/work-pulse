using FluentAssertions;
using NetArchTest.Rules;
using WorkPulse.Domain.Common;
using ApplicationDependencyInjection = WorkPulse.Application.DependencyInjection;
using InfrastructureDependencyInjection = WorkPulse.Infrastructure.DependencyInjection;

namespace WorkPulse.ArchitectureTests;

public sealed class ArchitectureDependencyTests
{
    [Fact]
    public void Domain_Should_Not_Reference_Infrastructure_Or_Framework_Packages()
    {
        var forbiddenAssemblies = new[]
        {
            "WorkPulse.Application",
            "WorkPulse.Infrastructure",
            "WorkPulse.Api",
            "MediatR",
            "Microsoft.EntityFrameworkCore",
            "Microsoft.AspNetCore",
        };

        var referencedAssemblies = typeof(BaseEntity).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name)
            .ToArray();

        referencedAssemblies.Should().NotContain(forbiddenAssemblies);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure_Or_Api()
    {
        var result = Types.InAssembly(typeof(ApplicationDependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("WorkPulse.Infrastructure", "WorkPulse.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Depend_On_Application()
    {
        var referencedAssemblies = typeof(InfrastructureDependencyInjection).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name)
            .ToArray();

        referencedAssemblies.Should().Contain(typeof(ApplicationDependencyInjection).Assembly.GetName().Name);
    }
}
