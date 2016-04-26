using Ccmi.OntoUml.Utilities.AssociationClasses;
using System;
using Xunit;

namespace Ccmi.OntoUml.Utilities.Tests.AssociationClasses
{
    public class ManyToManyAssociationTests
    {
        [Fact]
        public void SingletonWorks()
        {
            var instanceA = ManyToManyAssociation<string, string>.Instance();
            var instanceB = ManyToManyAssociation<string, string>.Instance();

            Assert.Equal(instanceA, instanceB);
        }

        [Fact]
        public void AddingAndGettingWorks()
        {
            var instance = ManyToManyAssociation<string, string>.Instance();
            instance.CreateLink("a", "b", "c");

            Assert.Contains("b", instance.GetNs("a"));
            Assert.Contains("c", instance.GetNs("a"));
            Assert.Contains("a", instance.GetMs("b"));
        }

        [Fact]
        public void RemovingWorks()
        {
            var instance = ManyToManyAssociation<string, string>.Instance();
            instance.CreateLink("c", "d");
            Assert.Contains("c", instance.GetMs("d"));
            instance.DestroyLink("c", "d");
            Assert.Empty(instance.GetMs("d"));
        }

        [Fact]
        public void RemovingDoesntThrow()
        {
            var instance = ManyToManyAssociation<string, string>.Instance();
            instance.DestroyLink("random", "link");
        }

        [Fact]
        public void CannotAddMultiple()
        {
            var instance = ManyToManyAssociation<string, string>.Instance();
            instance.CreateLink("b", "c");

            Assert.Throws<InvalidOperationException>(() => instance.CreateLink("b", "c"));
        }
    }
}
