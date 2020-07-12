using BusinessLogic.Implementations;
using BusinessLogic.Interfaces;
using Domain;
using Ninject;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Web
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IUsersRepository>().To<EFUsersRepository>();
            ninjectKernel.Bind<IFriendsRepository>().To<EFFriendsRepository>();
            ninjectKernel.Bind<IFriendRequestsRepository>().To<EFFriendRequestsRepository>();
            ninjectKernel.Bind<IMessagesRepository>().To<EFMessagesRepository>();
            ninjectKernel.Bind<EFDbContext>().ToSelf().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings[0].ConnectionString);
            ninjectKernel.Inject(Membership.Provider);
        }
    }
}