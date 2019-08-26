using Autofac.Extras.AttributeMetadata.Test.ScenarioTypes;
using Autofac.Features.Metadata;
using Xunit;

namespace Autofac.Extras.AttributeMetadata.Test
{
    public class AttributeMetadataModuleTestFixture
    {
        [Fact]
        public void does_not_throw_in_nested_lifetimeScope_builders()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AttributedMetadataModule>();
            builder.RegisterType<NestedLifetimeScopeRegistrationInstance>().As<ILifetimeScopeRegistrationInstance>();
            var container = builder.Build();

            using (var lifetimeScope = container.BeginLifetimeScope(x => builder.RegisterType<int>()))
            {
                var ex = Record.Exception(() =>
                    lifetimeScope
                        .Resolve<Meta<ILifetimeScopeRegistrationInstance,
                            NestedLifetimeScopeRegistrationMetadataAttribute>>());
                Assert.Null(ex);
            }
        }
    }
}