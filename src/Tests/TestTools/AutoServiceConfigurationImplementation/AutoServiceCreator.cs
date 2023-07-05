﻿using ApplicationLayer.AppliactionServices.ColorsAppService;
using ApplicationLayer.ApplicationServiceInterface;
using ApplicationLayer.InfraInterfaces.UnitOfWorks;
using ApplicationLayer.ServiceInterface;
using Autofac;
using AutoServiceContainer;
using DataAccessLayer.EFTech.EFDataContexts;
using DataAccessLayer.EFTech.EFRepositories.Colors;
using DataAccessLayer.EFTech.UnitOfWorks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Services.ColorService;
using ServiceLayer.Setups.RepositoryInterface;
using System;
using System.Collections.Generic;

namespace TestTools.AutoServiceConfigurationImplementation
{
    public class AutoServiceCreator<T> : AutoServiceConfiguration
        where T : class
    {
        public override void ServicesConfiguration(
            ContainerBuilder container,
            Dictionary<Type, object> mockedServiceParameters,
            DbContext context)
        {

            container.RegisterAssemblyTypes(
                typeof(EFColorRepository).Assembly)
                     .AssignableTo<IRepository>()
                     .AsImplementedInterfaces()
                     .WithDbContext(context as EFDataContext)
                     .WithConstructorParameters(mockedServiceParameters)
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(
                typeof(ColorService).Assembly)
                     .AssignableTo<IService>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(
                typeof(ColorAppService).Assembly)
                     .AssignableTo<IApplicationService>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .InstancePerLifetimeScope();

            container.RegisterType<EFUnitOfWork>()
                     .As<UnitOfWork>()
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();
        }

        public override DbContext SqlLiteConfiguration(
            SqliteConnection sqliteConnection)
        {
            var constructorParameters =
                AutoServiceTools.MockObjectListCreator();
            return new InMemoryDataBase()
                .CreateInMemoryDataContext<EFDataContext>(
                sqliteConnection,
                constructorParameters);
        }

        public override DbContext SqlServerConfiguration()
        {
            return new EFDataContext(GetConnectionString());
        }
    }
}