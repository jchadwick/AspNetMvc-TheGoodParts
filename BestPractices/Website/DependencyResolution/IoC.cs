using System.Data.Entity;
using Common.DataAccess;
using StructureMap;

namespace Website {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                        scan.RegisterConcreteTypesAgainstTheFirstInterface();
                                    });
                            x.Scan(scan =>
                                    {
                                        scan.Assembly("Common");
                                        scan.WithDefaultConventions();
                                        scan.RegisterConcreteTypesAgainstTheFirstInterface();
                                    });
                            x.For<DbContext>().Use<DataContext>();
                        });

            return ObjectFactory.Container;
        }
    }
}