using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;

namespace Repertoir.Tests
{
    [TestClass]
    public class RoutesTest
    {
        [TestMethod]
        public void TestIncomingRoutes()
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Assert
            RouteAssert.HasRoute(routes, "/", new { controller = "Contacts", action = "Index" });
            RouteAssert.HasRoute(routes, "/Contacts", new { controller = "Contacts", action = "Index" });
            RouteAssert.HasRoute(routes, "/Foo", new { controller = "Foo", action = "Index" });
            RouteAssert.HasRoute(routes, "/Foo/Index", new { controller = "Foo", action = "Index" });
            RouteAssert.HasRoute(routes, "/Foo/Bar", new { controller = "Foo", action = "Bar" });
            RouteAssert.HasRoute(routes, "/Foo/Bar/5", new { controller = "Foo", action = "Bar", id = 5 });
            RouteAssert.HasRoute(routes, "/Foo/Bar/5/Baz", new { controller = "Foo", action = "Bar", id = 5, slug = "Baz" });
            RouteAssert.NoRoute(routes, "/Foo/Bar/5/Baz/5");
            RouteAssert.IsIgnoredRoute(routes, "/handler.axd/pathInfo");
        }

        [TestMethod]
        public void TestOutgoingRoutes()
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Assert
            RouteAssert.GeneratesActionUrl(routes, "/", new { controller = "Contacts", action = "Index" });
            RouteAssert.GeneratesActionUrl(routes, "/Foo", new { controller = "Foo", action = "Index" });
            RouteAssert.GeneratesActionUrl(routes, "/Foo/Bar", new { controller = "Foo", action = "Bar" });
            RouteAssert.GeneratesActionUrl(routes, "/Foo/Bar/5", new { controller = "Foo", action = "Bar", id = 5 });
            RouteAssert.GeneratesActionUrl(routes, "/Foo/Bar/5/Baz", new { controller = "Foo", action = "Bar", id = 5, slug = "Baz" });
            RouteAssert.GeneratesActionUrl(routes, "/Foo/Bar/5/Baz?someKey=someValue", new { controller = "Foo", action = "Bar", id = 5, slug = "Baz", someKey = "someValue" });
        }
    }
}
