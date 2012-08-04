using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Repertoir.Tests.Helpers
{
    public class Moq
    {
        // How do you get unit tests to use routes in ASP.NET MVC?
        // http://stackoverflow.com/a/1856934/17316

        public static ViewContext GetViewContext()
        {
            var mock = new Mock<ViewContext>();

            mock.SetupGet(v => v.HttpContext).Returns(GetHttpContext());
            mock.SetupGet(v => v.Controller).Returns(new Mock<ControllerBase>().Object);
            mock.SetupGet(v => v.View).Returns(new Mock<IView>().Object);
            mock.SetupGet(v => v.ViewData).Returns(new ViewDataDictionary());
            mock.SetupGet(v => v.TempData).Returns(new TempDataDictionary());
            mock.SetupGet(v => v.RouteData).Returns(new RouteData());

            return mock.Object;
        }

        public static HttpContextBase GetHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            // These next two lines are required for the routing to generate valid URLs, apparently:
            request.SetupGet(r => r.ApplicationPath).Returns("/");
            response.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns((string r) => r);

            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Session).Returns(session.Object);
            context.SetupGet(c => c.Server).Returns(server.Object);

            return context.Object;
        }

        public static IViewDataContainer GetViewDataContainer()
        {
            return new Mock<IViewDataContainer>().Object;
        }
    }
}
