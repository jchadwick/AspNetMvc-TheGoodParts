using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Website.Filters
{
    public class CurrentUsernameValueProvider : IValueProvider
    {
        private readonly CultureInfo _culture;

        public CurrentUsernameValueProvider(CultureInfo culture)
        {
            _culture = culture;
        }

        public bool ContainsPrefix(string prefix)
        {
            return "CurrentUsername".Equals(prefix, StringComparison.OrdinalIgnoreCase);
        }

        public ValueProviderResult GetValue(string key)
        {
            var username = HttpContext.Current.User.Identity.Name;
            return new ValueProviderResult(username, username, _culture);
        }


        public class Factory : ValueProviderFactory
        {
            public override IValueProvider GetValueProvider(ControllerContext controllerContext)
            {
                return new CurrentUsernameValueProvider(Thread.CurrentThread.CurrentCulture);
            }
        }
    }

}