using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Autofac;
using SS.Data;
using SS.Data.EntityFramework;
using SS.Data.PetaPoco;
using SS.Mvc.AxosoftApi.Data;
using Module = Autofac.Module;

namespace SS.Mvc.AxosoftApi.Modules
{
    public sealed class DataModule : Module
    {
        private readonly string _connectionString;
        private readonly object[] _lifetimeScopeTags;

        public DataModule(string connectionString, params object[] lifetimeScopeTags)
        {
            _connectionString = connectionString;
            _lifetimeScopeTags = lifetimeScopeTags;
        }


        protected override void Load(ContainerBuilder builder)
        {
            //Database.SetInitializer(new TestInitializer());

            builder.Register(d => new ModelContext(_connectionString)).As<DbContext>()
                .InstancePerMatchingLifetimeScope(_lifetimeScopeTags)
#if DEBUG
					.OnActivated(d =>
					{
						if (Debugger.IsAttached)
						{
							d.Instance.Database.Log = message => Trace.WriteLine(message);
						}
					})
#endif
                    ;

            var assembly = typeof(DataModule).Assembly; // Replace with reference to data repositories assembly

            builder.RegisterType<EfWorkspace>().As<IWorkspace>();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(EfRepositoryBase<>)).As(typeof(IEfRepository<>)).As(typeof(IRepository<>));

            // Register Peta poco database
            builder.Register((d, p) =>
            {
                var connectionStringName = p.OfType<NamedParameter>()
                    .Where(x => x.Name == "connectionStringName")
                    .Select(x => x.Value as string).SingleOrDefault()
                    ?? _connectionString;

                return new SS.Data.PetaPoco.Database(connectionStringName);
            }).ExternallyOwned()
            .As<IDatabase>();
        }
    }
}
