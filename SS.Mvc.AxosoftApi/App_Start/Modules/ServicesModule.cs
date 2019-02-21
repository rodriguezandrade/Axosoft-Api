using Autofac;
using SS.Logging;
using SS.Logging.NLog;
using SS.Net;
using SS.Templating;
using SS.Xml;

namespace SS.Mvc.AxosoftApi.Modules
{
    public sealed class ServicesModule : Module
    {
        private readonly string _emailTemplateDirectory;
        private readonly string _dumpDirectory;
        private readonly object[] _lifetimeScopeTags;

        public ServicesModule(string emailTemplateDirectory, string dumpDirectory, params object[] lifetimeScopeTags)
        {
            _emailTemplateDirectory = emailTemplateDirectory;
            _dumpDirectory = dumpDirectory;
            _lifetimeScopeTags = lifetimeScopeTags;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ServicesModule).Assembly; // Replace with reference to services assembly

            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerMatchingLifetimeScope(_lifetimeScopeTags);

            if (_dumpDirectory != null)
            {
                builder.Register(c => new DummyEmailSender { Folder = _dumpDirectory }).As<IEmailSender>();
                //builder.Register(x => new DummySmsSender { Folder = _dumpDirectory }).As<ISmsSender>();

            }
            else
            {
                builder.RegisterType<EmailSender>().As<IEmailSender>();
                //builder.RegisterType<SmsSender>().As<ISmsSender>();
            }

            builder.RegisterType<TemplateCompiler>().As<ITemplateCompiler>();
            builder.RegisterType<CustomXmlSerializer>().As<IXmlSerializer>();
            builder.RegisterType<TemplateEmailSender>().As<ITemplateEmailSender>().OnActivated(e =>
            {
                e.Instance.TemplateDirectory = _emailTemplateDirectory;
            });

            //builder.RegisterInstance(new HttpClientFactory())
            //	.As<IHttpClientFactory>();
        }
    }
}
