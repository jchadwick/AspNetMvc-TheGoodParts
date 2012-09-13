using System.Web.Mvc;
using Website.Filters;

namespace Website.App_Start
{
    public class ModelBindingConfig
    {
        public static void RegisterCustomModelBinding()
        {
            ValueProviderFactories.Factories.Add(new CurrentUsernameValueProvider.Factory());
        }
    }
}