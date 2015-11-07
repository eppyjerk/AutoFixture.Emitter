using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Eppyjerk.AutoFixture.Emitter
{
    internal class FixtureInterceptor : IInterceptor
    {
        private Dictionary<PropertyFingerprint, object> _propertyValues = new Dictionary<PropertyFingerprint, object>();
        public void Intercept(IInvocation invocation)
        {
            var fingerprint = new PropertyFingerprint(invocation.Method);

            if (fingerprint.IsSetAccessor)
            {
                object value = invocation.Arguments[0];
                if (_propertyValues.ContainsKey(fingerprint))
                {
                    _propertyValues[fingerprint] = value;
                }
                else
                {
                    _propertyValues.Add(fingerprint, value);
                }
            }
            else if (fingerprint.IsGetAccessor)
            {
                if (_propertyValues.ContainsKey(fingerprint))
                {
                    invocation.ReturnValue = _propertyValues[fingerprint];
                }
            }

        }
    }

}
