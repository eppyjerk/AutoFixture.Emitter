using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Xunit;

namespace Eppyjerk.AutoFixture.Emitter.Tests
{
    public class FixtureExtensionsSuite
    {
        [Theory]
        [InlineData(typeof(IAppDomainSetup))]
        [InlineData(typeof(IFixture))]
        [InlineData(typeof(IFormattable))]
        [InlineData(typeof(IFormatProvider))]
        [InlineData(typeof(IComparable))]
        [InlineData(typeof(IPerson))]
        [InlineData(typeof(ICar))]
        [InlineData(typeof(IPlane))]
        public void CreateObjectOfType(Type t)
        {
            var fixture = new Fixture();
            var actualObject = fixture.CreateObjectOfType(t);

            Assert.IsAssignableFrom(t, actualObject);
        }

        [Fact]
        public void CreateObjectOfType_Generic()
        {
            var fixture = new Fixture();

            Assert.IsAssignableFrom<IAppDomainSetup>(fixture.CreateObjectOfType<IAppDomainSetup>());
            Assert.IsAssignableFrom<IFixture>(fixture.CreateObjectOfType<IFixture>());
            Assert.IsAssignableFrom<IFormattable>(fixture.CreateObjectOfType<IFormattable>());
            Assert.IsAssignableFrom<IFormatProvider>(fixture.CreateObjectOfType<IFormatProvider>());
            Assert.IsAssignableFrom<IComparable>(fixture.CreateObjectOfType<IComparable>());
            Assert.IsAssignableFrom<IPerson>(fixture.CreateObjectOfType<IPerson>());
            Assert.IsAssignableFrom<ICar>(fixture.CreateObjectOfType<ICar>());
            Assert.IsAssignableFrom<IPlane>(fixture.CreateObjectOfType<IPlane>());
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar))]
        [InlineData(typeof(IPlane), typeof(ICar))]
        [InlineData(typeof(IPerson), typeof(IPlane))]
        public void CreateObjectOfTypes_Two(Type t1, Type t2)
        {
            var fixture = new Fixture();
            var actualObject = fixture.CreateObjectOfTypes(t1, t2);

            Assert.IsAssignableFrom(t1, actualObject);
            Assert.IsAssignableFrom(t2, actualObject);
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar), typeof(IPlane))]
        public void CreateObjectOfTypes_Three(Type t1, Type t2, Type t3)
        {
            var fixture = new Fixture();
            var actualObject = fixture.CreateObjectOfTypes(t1, t2, t3);

            Assert.IsAssignableFrom(t1, actualObject);
            Assert.IsAssignableFrom(t2, actualObject);
            Assert.IsAssignableFrom(t3, actualObject);
        }


        [Theory]
        [InlineData(typeof(IAppDomainSetup))]
        [InlineData(typeof(IFixture))]
        [InlineData(typeof(IFormattable))]
        [InlineData(typeof(IFormatProvider))]
        [InlineData(typeof(IComparable))]
        [InlineData(typeof(IPerson))]
        [InlineData(typeof(ICar))]
        [InlineData(typeof(IPlane))]
        public void CreateManyObjectsOfType(Type t)
        {
            var fixture = new Fixture();
            int count = 10;
            var actualObjects = fixture.CreateManyObjectsOfType(count, t);

            Assert.Equal(count, actualObjects.Count());
        }

        [Fact]
        public void CreateManyObjectsOfType_Generic()
        {
            var fixture = new Fixture();
            int count = 10;

            var a1 = fixture.CreateManyObjectsOfType<IPerson>(count);
            var a2 = fixture.CreateManyObjectsOfType<ICar>(count);
            var a3 = fixture.CreateManyObjectsOfType<IPlane>(count);

            Assert.Equal(count, a1.Count());
            Assert.Equal(count, a2.Count());
            Assert.Equal(count, a3.Count());

            a1.ToList().ForEach(a => Assert.IsAssignableFrom<IPerson>(a));
            a2.ToList().ForEach(a => Assert.IsAssignableFrom<ICar>(a));
            a3.ToList().ForEach(a => Assert.IsAssignableFrom<IPlane>(a));
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar))]
        [InlineData(typeof(IPlane), typeof(ICar))]
        [InlineData(typeof(IPerson), typeof(IPlane))]
        public void CreateManyObjectsOfTypes_Two(Type t1, Type t2)
        {
            var fixture = new Fixture();
            int count = 10;
            var actualObjects = fixture.CreateManyObjectsOfTypes(count, t1, t2);

            Assert.Equal(count, actualObjects.Count());

            actualObjects.ToList().ForEach(a =>
            {
                Assert.IsAssignableFrom(t1, a);
                Assert.IsAssignableFrom(t2, a);
            });
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar), typeof(IPlane))]
        public void CreateManyObjectsOfTypes_Three(Type t1, Type t2, Type t3)
        {
            var fixture = new Fixture();
            int count = 10;
            var actualObjects = fixture.CreateManyObjectsOfTypes(count, t1, t2, t3);

            Assert.Equal(count, actualObjects.Count());

            actualObjects.ToList().ForEach(a =>
            {
                Assert.IsAssignableFrom(t1, a);
                Assert.IsAssignableFrom(t2, a);
                Assert.IsAssignableFrom(t3, a);
            });
        }


        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar))]
        public void CreateObjectOfTypes_DuplicateType(Type t1, Type t2)
        {
            var fixture = new Fixture();

            Assert.Throws<Exception>(() =>
            {
                var actualObject = fixture.CreateObjectOfTypes(t1, t2);
            }
            );
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar))]
        public void CreateManyObjectsOfTypes_DuplicateType(Type t1, Type t2)
        {
            var fixture = new Fixture();

            Assert.Throws<Exception>(() =>
            {
                var actualObject = fixture.CreateManyObjectsOfTypes(10, t1, t2);
            }
            );
        }

    }
}
