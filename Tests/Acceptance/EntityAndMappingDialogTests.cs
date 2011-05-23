using System;
using NUnit.Framework;
using Should;
using VisualStudio;
using VisualStudio.Data;
using VisualStudio.UI.EntityAndMapping;

namespace Tests.Acceptance
{
    [TestFixture]
    public class EntityAndMappingTests
    {
        [Test]
        public void Should_Return_Ok_Result_When_Ok_Button_Is_Pressed()
        {
            var tableDefinition = new EntityAndMappingDialog(new SqlServer());
            var result = tableDefinition.Display("DefaultEntityName",
                x => Maybe.Nothing<Exception>(),
                x => Maybe.Nothing<Exception>(),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "public class Entity {}"),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "public class Mapping {}"));
            result.HasValue.ShouldBeTrue();
        }

        [Test]
        public void Should_Return_Cancel_Result_When_Cancel_Button_Is_Pressed()
        {
            var tableDefinition = new EntityAndMappingDialog(new SqlServer());
            var result = tableDefinition.Display("DefaultEntityName",
                x => Maybe.Nothing<Exception>(),
                x => Maybe.Nothing<Exception>(),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "public class Entity {}"),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "public class Mapping {}"));
            result.HasValue.ShouldBeFalse();
        }

        [Test]
        public void Should_Display_Error_Message_When_Error_Occurs()
        {
            var tableDefinition = new EntityAndMappingDialog(new SqlServer());
            var result = tableDefinition.Display("DefaultEntityName",
                x => Maybe.Nothing<Exception>(),
                x => Maybe.Just(new Exception("Something failed.")),
                x => Tuple.Create(Maybe.Nothing<Exception>(), "public class Entity {}"),
                x => Tuple.Create(Maybe.Just(new Exception("Something failed.")), "public class Mapping {}"));
            result.HasValue.ShouldBeFalse();
        }
    }
}
