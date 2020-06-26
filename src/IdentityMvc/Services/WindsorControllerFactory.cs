using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentityMvc.Services
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _Kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
			_Kernel = kernel;
        }

		public override void ReleaseController(IController controller)
		{
			_Kernel.ReleaseComponent(controller);
		}

		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			if (controllerType == null)
			{
				throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
			}
			return (IController)_Kernel.Resolve(controllerType);
		}
	}



}
