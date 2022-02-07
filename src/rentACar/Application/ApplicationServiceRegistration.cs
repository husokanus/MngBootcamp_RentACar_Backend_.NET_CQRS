﻿using Application.Features.Brands.Rules;
using Application.Features.Cars.Rules;
using Application.Features.Colors.Rules;
using Application.Features.CorporateCustomers.Rules;
using Application.Features.Fuels.Rules;
using Application.Features.IndividualCustomers.Rules;
using Application.Features.Maintenenaces.Rules;
using Application.Features.Models.Rules;
using Application.Features.Rentals.Rules;
using Application.Services.Managers;
using Application.Services.Managers.Abstract;
using Application.Services.Managers.Concrete;
using Core.Application.Pipelines.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly() );
            services.AddMediatR(Assembly.GetExecutingAssembly() );
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly() );

            services.AddScoped<BrandBusinessRules>();
            services.AddScoped<ModelBusinessRules>();
            services.AddScoped<CarBusinessRules>();
            services.AddScoped<ColorBusinessRules>();
            services.AddScoped<FuelBusinessRules>();
            services.AddScoped<RentalBusinessRules>();
            services.AddScoped<MaintenanceBusinessRules>();
            services.AddScoped<IndividualCustomerBusinessRules>();
            services.AddScoped<CorporateCustomerBusinessRules>();

            services.AddScoped<IFindexScoreService, FakeFindexScoreServiceAdapter>();
            services.AddScoped<IIndividualCustomerService, IndividualCustomerManager>();
            services.AddScoped<ICorporateCustomerService, CorporateCustomerManager>();
            services.AddScoped<ICarService, CarManager>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}
