using System;
using System.Collections;
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
        public void CreateObjectOfTypes_One(Type t)
        {
            var fixture = new Fixture();
            var actualObject = fixture.CreateObjectOfTypes(t);

            VerifyObjectOfType(t, actualObject);
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

            VerifyObjectOfType(t1, actualObject);
            VerifyObjectOfType(t2, actualObject);
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar), typeof(IPlane))]
        public void CreateObjectOfTypes_Three(Type t1, Type t2, Type t3)
        {
            var fixture = new Fixture();
            var actualObject = fixture.CreateObjectOfTypes(t1, t2, t3);

            VerifyObjectOfType(t1, actualObject);
            VerifyObjectOfType(t2, actualObject);
            VerifyObjectOfType(t3, actualObject);
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
        public void CreateManyObjectsOfTypes_One(Type t)
        {
            var fixture = new Fixture();
            int count = 10;
            var actualObjects = fixture.CreateManyObjectsOfTypes(count, t);

            Assert.Equal(count, actualObjects.Count());
            VerifyObjectsOfType(t, actualObjects);
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

            VerifyObjectsOfType(typeof(IPerson), a1);
            VerifyObjectsOfType(typeof(ICar), a2);
            VerifyObjectsOfType(typeof(IPlane), a3);
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

            VerifyObjectsOfType(t1, actualObjects);
            VerifyObjectsOfType(t2, actualObjects);
        }

        [Theory]
        [InlineData(typeof(IPerson), typeof(ICar), typeof(IPlane))]
        public void CreateManyObjectsOfTypes_Three(Type t1, Type t2, Type t3)
        {
            var fixture = new Fixture();
            int count = 10;
            var actualObjects = fixture.CreateManyObjectsOfTypes(count, t1, t2, t3);

            Assert.Equal(count, actualObjects.Count());

            VerifyObjectsOfType(t1, actualObjects);
            VerifyObjectsOfType(t2, actualObjects);
            VerifyObjectsOfType(t3, actualObjects);
        }


        [Theory]
        [InlineData(typeof(IPerson), typeof(IPerson))]
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
        [InlineData(typeof(IPerson), typeof(IPerson))]
        public void CreateManyObjectsOfTypes_DuplicateType(Type t1, Type t2)
        {
            var fixture = new Fixture();

            Assert.Throws<Exception>(() =>
            {
                var actualObject = fixture.CreateManyObjectsOfTypes(10, t1, t2);
            }
            );
        }

        [Fact]
        public void CreateObjectOfTypes_NoTypes()
        {
            var fixture = new Fixture();

            Assert.Throws<Exception>(() =>
            {
                var actualObject = fixture.CreateObjectOfTypes();
            }
            );
        }

        [Fact]
        public void CreateManyObjectsOfTypes_NoTypes()
        {
            var fixture = new Fixture();

            Assert.Throws<Exception>(() =>
            {
                var actualObjects = fixture.CreateManyObjectsOfTypes(10);
            }
            );
        }

        [Fact]
        public void CreateManyObjectsOfTypes_MultipleCombinations()
        {
            int count = 10;
            var fixture = new Fixture();

            var results1 = fixture.CreateManyObjectsOfTypes(count, typeof(IPerson), typeof(ICar));
            var results2 = fixture.CreateManyObjectsOfTypes(count, typeof(IPerson), typeof(IPlane));
            var results3 = fixture.CreateManyObjectsOfTypes(count, typeof(IPlane), typeof(ICar));
            var results4 = fixture.CreateManyObjectsOfTypes(count, typeof(ICar), typeof(IPerson));

            VerifyObjectsOfType(typeof(IPerson), results1);
            VerifyObjectsOfType(typeof(ICar), results1);

            VerifyObjectsOfType(typeof(IPerson), results2);
            VerifyObjectsOfType(typeof(IPlane), results2);

            VerifyObjectsOfType(typeof(IPlane), results3);
            VerifyObjectsOfType(typeof(ICar), results3);

            VerifyObjectsOfType(typeof(ICar), results4);
            VerifyObjectsOfType(typeof(IPerson), results4);

        }

        [Fact]
        public void CreateManyObjectsOfTypes_Performance()
        {
            int count = 1000;
            var fixture = new Fixture();

            var actualResults = fixture.CreateManyObjectsOfTypes(count, typeof(IPerson), typeof(ICar));

            VerifyObjectsOfType<IPerson>(actualResults.Cast<IPerson>());
            VerifyObjectsOfType<ICar>(actualResults.Cast<ICar>());

            Assert.Equal(count, actualResults.Count());
        }


        private void VerifyObjectsOfType<T>(IEnumerable<T> objects)
        {
            foreach (object obj in objects)
            {
                VerifyObjectOfType<T>(obj);
            }
        }

        private void VerifyObjectsOfType(Type t, IEnumerable objects)
        {
            foreach (object obj in objects)
            {
                VerifyObjectOfType(t, obj);
            }
        }

        private void VerifyObjectOfType<T>(object @object)
        {
            VerifyObjectOfType(typeof(T), @object);
        }

        private void VerifyObjectOfType(Type t, object @object)
        {
            Assert.IsAssignableFrom(t, @object);

            var properties = t.GetProperties()
                .Where(p => p.CanRead)
                ;

            foreach (var p in properties)
            {
                // No Assert, just try to read the values
                object actualValue = p.GetValue(@object);
            }
        }
    }
}
