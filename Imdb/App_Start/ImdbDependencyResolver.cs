using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace Imdb.App_Start
{
    public class ImdbDependencyResolver:IDependencyResolver 
    {
        private IUnityContainer container;

        public ImdbDependencyResolver() { }

        public ImdbDependencyResolver(IUnityContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {

            try
            {
                return container.Resolve(serviceType);

            }
            catch (Exception)
            {
                return null;
            }



        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);

            }
            catch
            {
                return new List<Object>();
            }

        }
    }
}