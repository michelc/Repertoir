using System.Collections.Specialized;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Helpers;

namespace Repertoir.Tests.Helpers
{
    [TestClass]
    public class BinderHelperTest
    {
        // Unit testing custom model binder in ASP.NET MVC 2
        // http://stackoverflow.com/a/2310954/17316

        [TestMethod]
        public void StringModelBinder_supprime_espaces_avant_et_apres()
        {
            // Arrange
            var collection = new NameValueCollection { { "foo", " bar " } };
            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = new NameValueCollectionValueProvider(collection, null)
            };
            var binder = new StringModelBinder();

            // Act
            var result = (string)binder.BindModel(new ControllerContext(), bindingContext);

            // Assert
            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void StringModelBinder_renvoie_null_si_recoit_null()
        {
            // Arrange
            var collection = new NameValueCollection { { "foo", null } };
            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = new NameValueCollectionValueProvider(collection, null)
            };
            var binder = new StringModelBinder();

            // Act
            var result = (string)binder.BindModel(new ControllerContext(), bindingContext);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void StringModelBinder_renvoie_null_si_ne_recoit_que_des_espaces()
        {
            // Arrange
            var collection = new NameValueCollection { { "foo", "     " } };
            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = new NameValueCollectionValueProvider(collection, null)
            };
            var binder = new StringModelBinder();

            // Act
            var result = (string)binder.BindModel(new ControllerContext(), bindingContext);

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}
