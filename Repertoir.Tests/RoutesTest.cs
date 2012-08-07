using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteUnitTester;

namespace Repertoir.Tests
{
    [TestClass]
    public class RoutesTest
    {
        [TestMethod]
        public void TestIncomingRoutes()
        {
            // Arrange
            var tester = new RouteTester<MvcApplication>();

            // Assert
            tester.WithIncomingRequest("/").ShouldMatchRoute("Contacts", "Index");
            tester.WithIncomingRequest("/Foo").ShouldMatchRoute("Foo", "Index");
            tester.WithIncomingRequest("/Foo/Index").ShouldMatchRoute("Foo", "Index");
            tester.WithIncomingRequest("/Foo/Bar").ShouldMatchRoute("Foo", "Bar");
            tester.WithIncomingRequest("/Foo/Bar/5").ShouldMatchRoute("Foo", "Bar", new { id = 5 });
            tester.WithIncomingRequest("/Foo/Bar/5/Baz").ShouldMatchRoute("Foo", "Bar", new { id = 5, slug = "Baz" });
            tester.WithIncomingRequest("/Foo/Bar/5/Baz/5").ShouldMatchNoRoute();
            tester.WithIncomingRequest("/handler.axd/pathInfo").ShouldBeIgnored();
        }

        [TestMethod]
        public void TestOutgoingRoutes()
        {
            // Arrange
            var tester = new RouteTester<MvcApplication>();

            // Assert
            tester.WithRouteInfo("Contacts", "Index").ShouldGenerateUrl("/");
            tester.WithRouteInfo("Foo", "Bar").ShouldGenerateUrl("/foo/bar");
            tester.WithRouteInfo("Foo", "Bar", new { id = 5 }).ShouldGenerateUrl("/foo/bar/5");
            tester.WithRouteInfo("Foo", "Bar", new { id = 5, slug = "Baz" }).ShouldGenerateUrl("/foo/bar/5/baz");
            tester.WithRouteInfo("Foo", "Bar", new { id = 5, slug = "Baz", someKey = "someValue" }).ShouldGenerateUrl("/foo/bar/5/baz?someKey=someValue");
        }
    }
}
