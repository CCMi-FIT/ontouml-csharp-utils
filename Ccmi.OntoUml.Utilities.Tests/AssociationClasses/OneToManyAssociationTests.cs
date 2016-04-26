using Ccmi.OntoUml.Utilities.AssociationClasses;
using System;
using Xunit;

namespace Ccmi.OntoUml.Utilities.Tests.AssociationClasses
{
    public class OneToManyAssociationTests
    {
        [Fact]
        public void SingletonWorks()
        {
            var instanceA = OneToManyAssociation<string, string>.Instance();
            var instanceB = OneToManyAssociation<string, string>.Instance();

            Assert.Equal(instanceA, instanceB);
        }

        [Fact]
        public void AddingAndGettingWorks()
        {
            var instance = OneToManyAssociation<string, string>.Instance();
            instance.CreateLink("a", "b");

            Assert.Contains("b", instance.GetMany("a"));
            Assert.Equal("a", instance.GetOne("b"));
        }

        [Fact]
        public void RemovingWorks()
        {
            var instance = OneToManyAssociation<string, string>.Instance();
            instance.CreateLink("c", "d");
            Assert.Equal("c", instance.GetOne("d"));
            instance.DestroyLink("c", "d");
            Assert.Null(instance.GetOne("d"));
        }

        [Fact]
        public void RemovingDoesntThrow()
        {
            var instance = OneToManyAssociation<string, string>.Instance();
            instance.DestroyLink("random", "link");
        }

        [Fact]
        public void CannotAddMultiple()
        {
            var instance = OneToManyAssociation<string, string>.Instance();
            instance.CreateLink("b", "c");

            Assert.Throws<InvalidOperationException>(() => instance.CreateLink("b", "c"));
        }
    }
}
