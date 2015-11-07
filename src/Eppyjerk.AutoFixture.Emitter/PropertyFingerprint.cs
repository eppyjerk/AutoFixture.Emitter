using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eppyjerk.AutoFixture.Emitter
{
    internal class PropertyFingerprint : IEqualityComparer<PropertyFingerprint>, IComparable<PropertyFingerprint>
    {
        public string Fingerprint { get; private set; }
        public bool IsSetAccessor { get; private set; }
        public bool IsGetAccessor { get; private set; }
        public PropertyFingerprint(MethodInfo method)
        {
            var properties = method.DeclaringType.GetProperties();

            var setter = properties.FirstOrDefault(p => p.GetSetMethod() == method);
            var getter = properties.FirstOrDefault(p => p.GetGetMethod() == method);

            this.IsSetAccessor = setter != null;
            this.IsGetAccessor = getter != null;

            string propertyName = this.IsSetAccessor
                ? setter.Name
                : this.IsGetAccessor ? getter.Name : "Not-a-property"
                ;

            this.Fingerprint = string.Format("{0}; {1};"
                , method.DeclaringType.FullName
                , propertyName
                );
        }

        public override string ToString()
        {
            return this.Fingerprint;
        }

        public int CompareTo(PropertyFingerprint other)
        {
            return this.Fingerprint.CompareTo(other.Fingerprint);
        }

        public bool Equals(PropertyFingerprint x, PropertyFingerprint y)
        {
            return x.Fingerprint.Equals(y.Fingerprint);
        }

        public int GetHashCode(PropertyFingerprint obj)
        {
            return obj.Fingerprint.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.Fingerprint.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return this.Fingerprint.Equals((obj as PropertyFingerprint).Fingerprint);
        }
    }

}
