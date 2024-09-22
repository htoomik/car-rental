using CarRental.Domain.Abstractions;
using CarRental.Domain.CommandValidators;
using CarRental.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Domain.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddCarRental(this IServiceCollection services)
    {
        RegisterCommandHandlers(services);
        RegisterQueryHandlers(services);
        RegisterPricingStrategies(services);

        services.AddTransient<IRentalPriceCalculator, RentalPriceCalculator>();

        services.AddValidatorsFromAssemblyContaining<EndRentalCommandValidator>();

        return services;
    }

    private static void RegisterCommandHandlers(IServiceCollection services)
    {
        RegisterAllImplementationsAsTransient(services, typeof(ICommandHandler));
    }

    private static void RegisterQueryHandlers(IServiceCollection services)
    {
        RegisterAllImplementationsAsTransient(services, typeof(IQueryHandler));
    }

    private static void RegisterPricingStrategies(IServiceCollection services)
    {
        RegisterAllImplementationsAsTransient(services, typeof(IPricingStrategy));
    }

    private static void RegisterAllImplementationsAsTransient(IServiceCollection services, Type interfaceType)
    {
        var implementations = typeof(ServiceRegistration)
            .Assembly.GetTypes()
            .Where(x => x.GetInterface(interfaceType.Name) == interfaceType && !x.IsAbstract && x.IsClass);

        foreach (var implType in implementations)
        {
            services.Add(ServiceDescriptor.Transient(interfaceType, implType));
            services.Add(ServiceDescriptor.Transient(implType, implType));
        }
    }
}