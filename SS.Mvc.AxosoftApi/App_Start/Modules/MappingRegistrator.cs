using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Profile = AutoMapper.Profile;

namespace SS.Mvc.AxosoftApi
{
    public static class MappingRegistrator
    {
        public static void Register(Assembly mainAssembly, string @namespace)
        {
            if (mainAssembly == null) throw new ArgumentNullException(nameof(mainAssembly));
            if (@namespace == null) throw new ArgumentNullException(nameof(@namespace));
            Register(GetReferencedAssemblies(mainAssembly, @namespace));
        }

        public static void Register(IEnumerable<Assembly> sourceAssemblies)
        {
            if (sourceAssemblies == null) throw new ArgumentNullException(nameof(sourceAssemblies));

            var types = sourceAssemblies.SelectMany(x => x.GetExportedTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && typeof(Profile).IsAssignableFrom(t))
#if DEBUG
					.ToArray()
#endif
                ;

            Mapper.Initialize(cfg =>
            {
                foreach (var t in types)
                {
                    cfg.AddProfile(t);
                }
            });

            Mapper.AssertConfigurationIsValid();
        }

        internal static IEnumerable<Assembly> GetReferencedAssemblies(Assembly mainAssembly, string ns)
        {
            var referencedAssemblies = mainAssembly.GetReferencedAssemblies()
                .Where(x => x.Name.Contains(ns))
                .Select(Assembly.Load);

            return new[] { mainAssembly }.Concat(referencedAssemblies);
        }
    }
}
