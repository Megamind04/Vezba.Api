using Ninject;
using Ninject.Extensions.ChildKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Vezba.Api.Infrastructure;
using Vezba.Api.Interfaces;
using Vezba.Entity;

namespace Vezba.Api.App_Start
{
    public class NinjectResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectResolver() : this(new StandardKernel())
        {

        }

        public NinjectResolver(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
            {
                AddBindings(kernel);
            }
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectResolver(AddRequestBindings(new ChildKernel(kernel)), true);
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void Dispose()
        {

        }

        private void AddBindings(IKernel kernel)
        {
            // singleton and transient bindings go here
        }

        private IKernel AddRequestBindings(IKernel kernel)
        {
            //kernel.Bind(typeof(ITheRingWhoGonnaRuleThemAll<>)).To(typeof(BaseRepository<>)).
            kernel.Bind<ITheRingWhoGonnaRuleThemAll<Employee>>().To<EmployeeRepository>().InSingletonScope().Named("Employee");
            kernel.Bind<ITheRingWhoGonnaRuleThemAll<Customer>>().To<CustomerRepository>().InSingletonScope().Named("Customer");
            kernel.Bind<ITheRingWhoGonnaRuleThemAll<Shipper>>().To<ShipperRepository>().InSingletonScope().Named("Shipper");
            kernel.Bind<ITheRingWhoGonnaRuleThemAll<Category>>().To<CategoryRepository>().InSingletonScope().Named("Category");
            kernel.Bind<ITheRingWhoGonnaRuleThemAll<Supplier>>().To<SupplierRepository>().InSingletonScope().Named("Supplier");
            return kernel;
        }
    }
}