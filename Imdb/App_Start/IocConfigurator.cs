using Imdb.Models;
using Imdb.Repository;
using Imdb.Repository.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Imdb.App_Start
{
    public class IocConfigurator
    {
        public static void ConfigureIocUnityContaner()
        {
            IUnityContainer container = new UnityContainer();
            registerServices(container);
            DependencyResolver.SetResolver(new ImdbDependencyResolver(container));
        }

        private static void registerServices(IUnityContainer container)
        {
            ImdbContext context = new ImdbContext();
            container.RegisterType<IPersonRepository, PersonRepository>(new InjectionConstructor(context));
            container.RegisterType<IProducerRepository, ProducerRepository>(new InjectionConstructor(context));
            container.RegisterType<IActorRepository, ActorRepository>(new InjectionConstructor(context));
            container.RegisterType<IMovieRepository, MovieRepository>(new InjectionConstructor(context));


        }
    }
}