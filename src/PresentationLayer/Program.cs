using Autofac;
using ConfigurationLayer.InfrastructureLayerConfiguration.AutoFacConfigurations;

new Startup()
    .CreateHostBuilderWithAutofacConfig<AutofacConfig>(
        args,
        new ContainerBuilder())
       .Build()
       .Run();



