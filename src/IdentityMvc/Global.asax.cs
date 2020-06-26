using Castle.Windsor;
using Castle.Windsor.Installer;
using IdentityMvc.CoreServices.DI.Installers;
using IdentityMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdentityMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _Container;

        protected void Application_Start()
        {
            _Container = new WindsorContainer().Install(
                                new ServicesInstaller(), 
                                new ControllerInstaller());
            
            var controllerFactory = new WindsorControllerFactory(_Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            _Container.Dispose();
        }
    }
}
