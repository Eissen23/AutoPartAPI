using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Validator;

internal static class Startup
{
    public static IServiceCollection AddBehaviours(this IServiceCollection services, Assembly assemblyContainingValidators)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatiomBehavior<,>));
        return services;
    }
}
