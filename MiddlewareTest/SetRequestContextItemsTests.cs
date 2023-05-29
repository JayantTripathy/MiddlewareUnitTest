using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MiddlewareUnitTest.Middleware;
using Moq;
using System.Net.Http;

namespace MiddlewareTest
{
    public class SetRequestContextItemsTests
    {
        [Fact]
        public async Task WhenRequestHeadersContainXRequestId_IfContextItemIsSet_True()
        {
            // Create an instance of SetRequestContextItems() class
            // Call the InvokeAsync() method
            // Check if the resultant HttpContext.Items Dictionary
            // contains xRequestIdKey with some Value


            // SetRequestContextItems() class requires two dependencies
            // HttpContext and RequestDelegate
            // We need to mock HttpContext.Request and RequestDelegate for our case
            // The Business Logic runs on HttpContext.Request.Headers Dictionary and
            // not any other HttpContext attribute,
            // so we can safely mock only the Request.Headers part
            // RequestDelegate is an ActionDelegate that invokes the next Task
            // Hence we can just pass a dummy Action as the next Task to perform


            // Mocking the HttpContext.Request.Headers when xRequestIdKey Header is present
            var headers = new Dictionary<string, StringValues>() {
            { SetRequestContextItemsMiddleware.XRequestIdKey, "123456" }};


            var httpContextMoq = new Mock<HttpContext>();
            httpContextMoq.Setup(x => x.Request.Headers)
                .Returns(new HeaderDictionary(headers));
            httpContextMoq.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>());


            var httpContext = httpContextMoq.Object;


            var requestDelegate = new RequestDelegate(
                    (innerContext) => Task.FromResult(0));


            // Act
            var middleware = new SetRequestContextItemsMiddleware(
                    requestDelegate);
            await middleware.InvokeAsync(httpContext);


            // Assert


            // check if the HttpContext.Items dictionary
            // contains any Key with XRequestIdKey
            // which implies that the Middleware
            // was able to place a value inside the Items
            // dictionary which is the expectation

            Assert.True(
                httpContext.Items.ContainsKey(SetRequestContextItemsMiddleware.XRequestIdKey));
        }

        [Fact]
        public async Task WhenRequestHeadersDoesNotContainXRequestId_IfContextItemIsNotSet_True()
        {
            //var headers = new Dictionary<string, StringValues>() {
            //{ SetRequestContextItemsMiddleware.XRequestIdKey, "123456" }};

            var headers = new Dictionary<string, StringValues>();


            var httpContextMoq = new Mock<HttpContext>();
            httpContextMoq.Setup(x => x.Request.Headers)
                .Returns(new HeaderDictionary(headers));
            httpContextMoq.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>());


            var httpContext = httpContextMoq.Object;


            var requestDelegate = new RequestDelegate(
                    (innerContext) => Task.FromResult(0));


            // Act
            var middleware = new SetRequestContextItemsMiddleware(
                    requestDelegate);
            await middleware.InvokeAsync(httpContext);

            Assert.True(
                 httpContext.Items.ContainsKey(
                     SetRequestContextItemsMiddleware.XRequestIdKey));

        }
    }
}