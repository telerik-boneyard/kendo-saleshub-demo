using SalesHub.Client.Builders;
using SalesHub.Client.Converters;
using SalesHub.Client.Processors;
using SalesHub.Client.ViewModels.Client;
using SalesHub.Core.Models;
using SalesHub.Data.Repositories;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SalesHub.Client.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(SalesHub.Client.App_Start.NinjectWebCommon), "Stop")]

namespace SalesHub.Client.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using SalesHub.Client.NinjectResolver;
    using SalesHub.Core.Repositories;
    using SalesHub.Data;
    using System;
    using System.Web;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            kernel.Bind<ICurrencyTypeRepository>().To<CurrencyTypeRepository>();
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<IOrderDetailRepository>().To<OrderDetailRepository>();
            kernel.Bind<IOrderNotesRepository>().To<OrderNotesRepository>();
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<IPaymentTermRepository>().To<PaymentTermRepository>();
            kernel.Bind<IPaymentTermTypeRepository>().To<PaymentTermTypeRepository>();
            kernel.Bind<IPackageTypeRepository>().To<PackageTypeRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IOriginRepository>().To<OriginRepository>();
            kernel.Bind<IDestinationRepository>().To<DestinationRepository>();
            kernel.Bind<ISuggestedValueRepository>().To<SuggestedValueRepository>();

            kernel.Bind<ISalesHubDbContext>().To<SalesHubDbContext>().InThreadScope();
            kernel.Bind<ISellingCompanyTreeViewBuilder>().To<SellingCompanyTreeViewBuilder>();
            kernel.Bind<ICustomerPathBuilder>().To<CustomerPathBuilder>();
            kernel.Bind<ICurrencyTypeSelectListBuilder>().To<CurrencyTypeSelectListBuilder>();
            kernel.Bind<IPaymentTermTypeSelectListBuilder>().To<PaymentTermTypeSelectListBuilder>();
            kernel.Bind<ICreditTermDurationSelectListBuilder>().To<CreditTermDurationSelectListBuilder>();
            kernel.Bind<ISplitPercentagesSelectListBuilder>().To<SplitPercentagesSelectListBuilder>();
            kernel.Bind<IOrderViewModelSelectListBuilder>().To<OrderViewModelSelectListBuilder>();
            kernel.Bind<IPackageTypeSelectListBuilder>().To<PackageTypeSelectListBuilder>();
            kernel.Bind<IOrderViewDataBuilder>().To<OrderViewDataBuilder>();

            kernel.Bind<IPaymentTermProcessor>().To<PaymentTermProcessor>();

            kernel.Bind<IConverter<OrderViewModel, Order>>().To<OrderViewModelConverter>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }
    }
}
