using Autofac.Core;
using SS.Logging;
using SS.Logging.NLog;
using System.Linq;
using System.Reflection;

namespace SS.Mvc.AxosoftApi.Modules
{
    public sealed class LogModule : Autofac.Module
    {
        public bool InjectProperties { get; }

        public LogModule()
            : this(false)
        {
        }

        public LogModule(bool injectProperties)
        {
            InjectProperties = injectProperties;
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            if (InjectProperties)
            {
                // Handle properties.
                registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
            }
        }

        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(ILogFacade) && p.CanWrite && p.GetIndexParameters().Length == 0);

            // Set the properties located.
            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, new NLogLogFacade(instanceType.FullName), null);
            }
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter((p, i) => p.ParameterType == typeof(ILogFacade),
                        (p, i) => new NLogLogFacade(t.FullName))
                });
        }
    }
}
