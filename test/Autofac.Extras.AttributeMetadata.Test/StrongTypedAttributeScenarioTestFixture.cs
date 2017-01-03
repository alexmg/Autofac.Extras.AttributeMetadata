﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Extras.AttributeMetadata.Test.ScenarioTypes;
using Autofac.Integration.Mef;
using Xunit;

namespace Autofac.Extras.AttributeMetadata.Test
{
    public class StrongTypedAttributeScenarioTestFixture
    {
        [Fact]
        public void validate_wireup_of_typed_attributes_to_strongly_typed_metadata_on_resolve()
        {
            // arrange
            var builder = new ContainerBuilder();
            builder.RegisterMetadataRegistrationSources();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As<IStrongTypedScenario>()
                .WithAttributedMetadata<IStrongTypedScenarioMetadata>();

            // act
            var items = builder.Build().Resolve<IEnumerable<Lazy<IStrongTypedScenario, IStrongTypedScenarioMetadata>>>();

            // assert
            Assert.Equal(2, items.Count());
            Assert.Equal(1, items.Where(p => p.Metadata.Name == "Hello" && p.Metadata.Age == 42).Count());
            Assert.Equal(1, items.Where(p => p.Metadata.Name == "Goodbye" && p.Metadata.Age == 24).Count());

            Assert.IsType<StrongTypedScenario>(items.Where(p => p.Metadata.Name == "Hello" && p.Metadata.Age == 42).First().Value);
            Assert.IsType<AlternateStrongTypedScenario>(items.Where(p => p.Metadata.Name == "Goodbye" && p.Metadata.Age == 24).First().Value);
        }
    }
}