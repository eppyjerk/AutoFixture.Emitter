using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Eppyjerk.AutoFixture.Emitter
{
    public static class FixtureExtensions
    {
        public static IEnumerable<object> CreateManyObjectsOfType<T>(this IFixture fixture, int count)
        {
            return fixture.CreateManyObjectsOfType(count, typeof(T));
        }
        public static IEnumerable<object> CreateManyObjectsOfType(this IFixture fixture, int count, Type interfaceTypes)
        {
            return Enumerable.Range(1, count).Select(i => fixture.CreateObjectOfTypes(interfaceTypes));
        }
        public static IEnumerable<object> CreateManyObjectsOfTypes(this IFixture fixture, int count, params Type[] interfaceTypes)
        {
            return Enumerable.Range(1, count).Select(i => fixture.CreateObjectOfTypes(interfaceTypes));
        }


        public static object CreateObjectOfType<T>(this IFixture fixture)
        {
            return fixture.CreateObjectOfTypes(typeof(T));
        }
        public static object CreateObjectOfType(this IFixture fixture, Type interfaceType)
        {
            return fixture.CreateObjectOfTypes(interfaceType);
        }
        public static object CreateObjectOfTypes(this IFixture fixture, params Type[] interfaceTypes)
        {
            if (interfaceTypes.Length == 0)
            {
                //TODO: exception
                throw new Exception("Must have interface types");
            }

            var proxyGenerator = new ProxyGenerator();

            Type firstType = interfaceTypes.First();
            Type[] otherTypes = new Type[0];
            if (interfaceTypes.Length > 1)
            {
                otherTypes = interfaceTypes.Skip(1).ToArray();
            }


            var result = proxyGenerator.CreateInterfaceProxyWithoutTarget(firstType, otherTypes, new FixtureInterceptor());


            var properties = result.GetType()
                .GetProperties()
                .Where(p => p.CanWrite)
                ;

            var specimen = new SpecimenContext(fixture);
            foreach (var p in properties)
            {
                var value = specimen.Resolve(p.PropertyType);
                p.SetValue(result, value);
            }

            return result;
        }

    }
}
