using System;
using NUnit.Framework;

namespace Client
{

    [TestFixture]
    public class MappingFixture
    {
        private Set _set;
        private Subset _subset;

        public class Set
        {
            public object A { get; set; }

            public object B { get; set; }
        }

        public class Subset
        {
            public object A { get; set; }
        }

        public class ParentOfSet
        {
            public Set B { get; set; }
        }

        public class ParentOfSubset
        {
            public Subset B { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            _set = new Set { A = "A", B = "B" };
            _subset = new Subset { A = "A" };
        }

        [Test]
        public void WhenTargetTypeIsSet()
        {
            Assert.Throws<ApplicationException>(() => _subset.MapTo<Set>());
        }

        [Test]
        public void WhenTargetTypeIsSubset()
        {
            var result = _set.MapTo<Subset>();
            Assert.That(result.A, Is.EqualTo("A"));
        }

        [Test]
        public void WhenNestedTargetTypeIsSubSet()
        {
            var parentOfSet = new ParentOfSet {B = _set};
            parentOfSet.MapTo<ParentOfSubset>();
        }

        [Test]
        public void ShouldMaWhenNestedTargetTypeIsSet()
        {
            var parentOfSet = new ParentOfSubset { B = _subset };
            Assert.Throws<ApplicationException>(() => parentOfSet.MapTo<ParentOfSet>());
        }
    }
}