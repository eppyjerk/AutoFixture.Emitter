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
                // Creating a new recursionBuster per object because I want to stop recursion, but I want each object to get its own unique set of objects
                Dictionary<Type, object> recursionBuster = new Dictionary<Type, object>();
                results.Add(InternalCreateObjectOfTypes(proxyGenerator, fixture, interfaceTypes, recursionBuster));
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
            Dictionary<Type, object> recursionBuster = new Dictionary<Type, object>();

            return InternalCreateObjectOfTypes(proxyGenerator, fixture, interfaceTypes, recursionBuster);
        }






        private static object InternalCreateObjectOfTypes(ProxyGenerator proxyGenerator, IFixture fixture, Type[] interfaceTypes, Dictionary<Type, object> recursionBuster)
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
                object value = null;
                string propertyName = p.Name;

                if(p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                {
                    value = specimen.Resolve(p.PropertyType);
                }
                else
                {
                    if (p.PropertyType.IsInterface)
                    {
                        if (recursionBuster.ContainsKey(p.PropertyType))
                        {
                            value = recursionBuster[p.PropertyType];
                        }
                        else
                        {
                            recursionBuster.Add(p.PropertyType, null);
                            value = InternalCreateObjectOfTypes(proxyGenerator, fixture, new Type[] { p.PropertyType }, recursionBuster);
                            recursionBuster[p.PropertyType] = value;
                        }
                    }
                }

                p.SetValue(result, value);
            }

            return result;

        }
    }
}
