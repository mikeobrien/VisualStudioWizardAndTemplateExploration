using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Should;
using VisualStudio.Workflows.EntityAndMapping;

namespace Tests.Unit
{
    [TestFixture]
    public class EntityDefinitionTests
    {
        [Test]
        public void Properly_Convert_To_Dictionary()
        {
            var entityDefinition = new EntityDefinition()
            {
                EntityName = "SomeEntity",
                Namespace = "Yada.Stuff",
            };

            var parameters = entityDefinition.ToDictionary("Default.Namespace");

            parameters.Count.ShouldEqual(4);

            parameters["entityName"].ShouldEqual("SomeEntity");
            parameters["mappingName"].ShouldEqual("SomeEntityMap");
            parameters["entityNamespace"].ShouldEqual("Default.Namespace.Domain.Yada.Stuff");
            parameters["mappingNamespace"].ShouldEqual("Default.Namespace.Infrastructure.Persistence.Mapping.Yada.Stuff");
        }
    }
}
