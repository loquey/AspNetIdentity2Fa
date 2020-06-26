using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IdentityMvc.CoreServices.Identity;
using IdentityMvc.CoreServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.DI.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITemplateFormatter>().ImplementedBy<TemplateFormatter>());
            container.Register(Component.For<IGoogleAuthenticatorService>().ImplementedBy<GoogleAuthenticatorService>());
        }
    }
}
