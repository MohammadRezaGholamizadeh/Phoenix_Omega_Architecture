using ApplicationLayer.AppliactionServices.ColorsAppService;
using ApplicationLayer.ApplicationServiceInterface;
using ApplicationLayer.InfraInterfaces.UnitOfWorks;
using ApplicationLayer.ServiceInterface;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataAccessLayer.EFTech.EFDataContexts;
using DataAccessLayer.EFTech.EFRepositories.Colors;
using DataAccessLayer.EFTech.UnitOfWorks;
using ServiceLayer.RepositoryInterface;
using ServiceLayer.Services.ColorService;
using ServiceLayer.Services.ColorService.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var autofacContainerBuilder =
    new ContainerBuilder();
autofacContainerBuilder.RegisterModule(new AutofacModule());
builder.Services.AddSingleton(autofacContainerBuilder);
builder.Services.AddAutofac();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(ColorAppService).Assembly)
               .AssignableTo<IApplicationService>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(ColorService).Assembly)
            .AssignableTo<IService>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(EFColorRepository).Assembly)
            .AssignableTo<IRepository>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        builder.RegisterType<EFDataContext>()
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterType<EFUnitOfWork>()
               .As<UnitOfWork>()
               .InstancePerLifetimeScope();

        builder.Build();
    }
}