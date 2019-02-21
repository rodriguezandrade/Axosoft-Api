using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SS.Mvc.AxosoftApi
{
    /// <summary>
    /// Instantiates and registers all <see cref="AutoMapper.Profile" /> types found in the specified assemblies.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public sealed class MappingsModule : Autofac.Module
    {
        private readonly Assembly[] _sourceAssemblies;

        public MappingsModule(Assembly mainAssembly, string @namespace)
            : this(MappingRegistrator.GetReferencedAssemblies(mainAssembly, @namespace))
        {
        }

        public MappingsModule(IEnumerable<Assembly> sourceAssemblies)
        {
            if (sourceAssemblies == null) throw new ArgumentNullException(nameof(sourceAssemblies));
            _sourceAssemblies = sourceAssemblies.ToArray();
            if (_sourceAssemblies.Length == 0)
            {
                throw new ArgumentException("Source assemblies are empty.");
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            MappingRegistrator.Register(_sourceAssemblies);

            builder.RegisterInstance(Mapper.Instance).As<IMapper>();
        }
    }
}
