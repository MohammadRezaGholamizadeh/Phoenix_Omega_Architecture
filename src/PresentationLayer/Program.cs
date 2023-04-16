using ConfigurationLayer.InfrastructureLayerConfiguration.AutoFacConfigurations;
using PresentationLayer;

new Startup()
    .CreateHostBuilderWithAutofacConfig(
        args,
        new AutofacConfig())
       .Run();



