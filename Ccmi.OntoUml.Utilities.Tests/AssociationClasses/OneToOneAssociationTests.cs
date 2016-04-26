using Ccmi.OntoUml.Utilities.AssociationClasses;
using System;
using Xunit;

namespace Ccmi.OntoUml.Utilities.Tests.AssociationClasses
{
    public class OneToOneAssociationTests
    {
        [Fact]
        public void SingletonWorks()
        {
            var instanceA = OneToOneAssociation<string, string>.Instance();
            var instanceB = OneToOneAssociation<string, string>.Instance();

            Assert.Equal(instanceA, instanceB);
        }

        [Fact]
        public void AddingAndGettingWorks()
        {
            var instance = OneToOneAssociation<string, string>.Instance();
            instance.CreateLink("a", "b");

            Assert.Equal("b", instance.GetRight("a"));
            Assert.Equal("a", instance.GetLeft("b"));
        }

        [Fact]
        public void RemovingWorks()
        {
            var instance = OneToOneAssociation<string, string>.Instance();
            instance.CreateLink("c", "d");
            Assert.Equal("d", instance.GetRight("c"));
            instance.DestroyLink("c", "d");
            Assert.Null(instance.GetRight("c"));
        }

        [Fact]
        public void RemovingDoesntThrow()
        {
            var instance = OneToOneAssociation<string, string>.Instance();
            instance.DestroyLink("random", "link");
        }

        [Fact]
        public void CannotAddMultiple()
        {
            var instance = OneToOneAssociation<string, string>.Instance();
            instance.CreateLink("b", "c");

            Assert.Throws<InvalidOperationException>(() => instance.CreateLink("b", "c"));
        }
    }
}
