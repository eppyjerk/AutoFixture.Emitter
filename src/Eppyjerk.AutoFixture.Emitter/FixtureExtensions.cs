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
            return fixture.CreateManyObjectsOfTypes(count, typeof(T));
        }
        public static IEnumerable<object> CreateManyObjectsOfTypes(this IFixture fixture, int count, params Type[] interfaceTypes)
        {
            // TODO: Why doesn't this call CreateObjectOfTypes when interfaceTypes.Length == 0 ???
            //return Enumerable.Range(1, count).Select(i => fixture.CreateObjectOfTypes(interfaceTypes));


            List<object> results = new List<object>();

            ValidateTypes(interfaceTypes);

            var proxyGenerator = new ProxyGenerator();

            for (int i = 0; i < count; i++)
            {
                results.Add(CreateObjectOfTypes(proxyGenerator, fixture, interfaceTypes));
            }

            return results;
        }

        private static void ValidateTypes(Type[] interfaceTypes)
        {
            if (interfaceTypes.Length == 0)
            {
                //TODO: exception
                throw new Exception("Must have interface types");
            }

            var grouped = from t in interfaceTypes
                          group t by t.FullName into grp
                          where grp.Count() > 1
                          select grp;

            if (grouped.Count() > 0)
            {
                //TODO: exception
                throw new Exception("Cannot define type more than once");
            }

        }

        public static object CreateObjectOfType<T>(this IFixture fixture)
        {
            return fixture.CreateObjectOfTypes(typeof(T));
        }
        public static object CreateObjectOfTypes(this IFixture fixture, params Type[] interfaceTypes)
        {
            ValidateTypes(interfaceTypes);

            var proxyGenerator = new ProxyGenerator();

            return CreateObjectOfTypes(proxyGenerator, fixture, interfaceTypes);
        }

        private static object CreateObjectOfTypes(ProxyGenerator proxyGenerator, IFixture fixture, Type[] interfaceTypes)
        {
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
