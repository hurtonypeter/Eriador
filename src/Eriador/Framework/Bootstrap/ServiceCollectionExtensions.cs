
using Microsoft.Framework.DependencyInjection;
using System;

namespace Eriador.Framework.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEriador(this IServiceCollection service)
        {
            var bootstrap = new Bootstrap();
            bootstrap.Initialize();
        }
    }
}