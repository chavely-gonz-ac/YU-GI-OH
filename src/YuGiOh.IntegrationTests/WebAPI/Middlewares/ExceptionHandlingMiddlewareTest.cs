using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using YuGiOh.WebAPI.Middlewares;
using YuGiOh.Domain.Exceptions;

namespace YuGiOh.IntegrationTests.WebAPI.Middlewares
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_Should_Handle_APIException_Properly()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            httpContext.Response.Body = responseStream;

            var exception = APIException.NotFound("Player not found", "Missing in database");
            RequestDelegate next = _ => throw exception;

            var middleware = new ExceptionHandlingMiddleware(next);

            // Act
            await middleware.InvokeAsync(httpContext);
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var json = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(json);

            // Assert
            httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.GetProperty("title").GetString().Should().Be("Not Found");
            result.GetProperty("message").GetString().Should().Be("Player not found");
        }

        [Fact]
        public async Task InvokeAsync_Should_Handle_Unexpected_Exception()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            httpContext.Response.Body = responseStream;

            RequestDelegate next = _ => throw new Exception("Something went wrong");
            var middleware = new ExceptionHandlingMiddleware(next);

            // Act
            await middleware.InvokeAsync(httpContext);
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var json = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(json);

            // Assert
            httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.GetProperty("title").GetString().Should().Be("Internal Server Error");
            result.GetProperty("message").GetString().Should().Be("Something went wrong");
        }

        [Fact]
        public async Task InvokeAsync_Should_Pass_When_No_Exception()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            httpContext.Response.Body = responseStream;

            var next = new Mock<RequestDelegate>();
            next.Setup(n => n(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var middleware = new ExceptionHandlingMiddleware(next.Object);

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
