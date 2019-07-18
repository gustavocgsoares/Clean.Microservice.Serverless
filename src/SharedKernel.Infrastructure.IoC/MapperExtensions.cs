using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class MapperExtensions
    {
        public static void AddMappers<TProfileAssembly>(this IServiceCollection services)
            where TProfileAssembly : Profile
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddAutoMapper();

            Mapper.Initialize(x => x.AddProfiles(typeof(TProfileAssembly).Assembly));
        }
    }
}
