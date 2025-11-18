using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Simpl.Mobile.Mechanic.Core.Mediator;

public interface IRequest<TResponse>
{

}

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  Task<TResponse> Handle(TRequest request, CancellationToken ct = default);
}



public interface ISender
{
  Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}

public class Sender(IServiceProvider provider) : ISender
{
  public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
  {
    var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
    dynamic handler = provider.GetRequiredService(handlerType);
    return handler.Handle((dynamic)request, ct);
  }
}



public static class Mediator
{
  public static IServiceCollection AddMediator(this IServiceCollection services, Assembly? assembly = null)
  {
    // if assembly is null, use the assembly that is calling into this function
    assembly ??= Assembly.GetCallingAssembly();

    // register the sender class
    services.AddScoped<ISender, Sender>();

    // get the type of the generic request handler
    var handlerIntType = typeof(IRequestHandler<,>);

    // get a list of all classes that implements the IRequestHandler generic interface
    var handlerTypes = assembly
      .GetTypes()
      .Where(t => !t.IsAbstract && !t.IsInterface)
      .SelectMany(t => t.GetInterfaces()
        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerIntType)
        .Select(i => new { Interface = i, Implementation = t }));
    
    // register the handlers
    foreach(var handler in handlerTypes)
    {
      services.AddScoped(handler.Interface, handler.Implementation);
    }

    return services;
  }
}