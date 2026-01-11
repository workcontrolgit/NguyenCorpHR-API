using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NguyenCorpHR.Application.Behaviours;
using NguyenCorpHR.Application.Helpers;
using NguyenCorpHR.Application.Interfaces;
using NguyenCorpHR.Domain.Entities;

namespace NguyenCorpHR.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IDataShapeHelper<Position>, DataShapeHelper<Position>>();
            services.AddScoped<IDataShapeHelper<Employee>, DataShapeHelper<Employee>>();
            services.AddScoped<IDataShapeHelper<Department>, DataShapeHelper<Department>>();
            services.AddScoped<IDataShapeHelper<SalaryRange>, DataShapeHelper<SalaryRange>>();
            services.AddScoped<IModelHelper, ModelHelper>();
            // Register IDataShapeHelper implementations without relying on deprecated API surface.
            services.Scan(selector => selector
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classSelector => classSelector.AssignableTo(typeof(IDataShapeHelper<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}

