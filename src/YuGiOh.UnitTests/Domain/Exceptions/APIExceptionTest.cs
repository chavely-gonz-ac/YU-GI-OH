using System.Net;
using YuGiOh.Domain.Exceptions;

namespace YuGiOh.UnitTests.Domain.Exceptions
{
    public class APIExceptionTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            var ex = new APIException(
                HttpStatusCode.BadRequest,
                "Invalid request",
                "Bad Request",
                "Extra details"
            );

            ex.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            ex.Message.Should().Be("Invalid request");
            ex.Title.Should().Be("Bad Request");
            ex.Detail.Should().Be("Extra details");
        }

        [Fact]
        public void Default_Constructor_Should_Use_Default_Values()
        {
            var ex = new APIException();

            ex.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            ex.Message.Should().Be("An unexpected error occurred.");
            ex.Title.Should().Be("API Error");
            ex.Detail.Should().BeNull();
        }

        [Fact]
        public void ToProblemDetails_Should_Return_Valid_Object()
        {
            var ex = new APIException(
                HttpStatusCode.NotFound,
                "Deck not found",
                "Not Found",
                "The requested deck does not exist."
            );

            var result = ex.ToProblemDetails();

            result.Should().BeEquivalentTo(new
            {
                status = (int)HttpStatusCode.NotFound,
                title = "Not Found",
                message = "Deck not found",
                detail = "The requested deck does not exist."
            }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void Static_Factories_Should_Create_Correct_Instances()
        {
            var badRequest = APIException.BadRequest("Invalid input");
            badRequest.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            badRequest.Title.Should().Be("Bad Request");

            var notFound = APIException.NotFound("Player not found");
            notFound.StatusCode.Should().Be(HttpStatusCode.NotFound);
            notFound.Title.Should().Be("Not Found");

            var unauthorized = APIException.Unauthorized();
            unauthorized.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            unauthorized.Title.Should().Be("Unauthorized");

            var forbidden = APIException.Forbidden();
            forbidden.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            forbidden.Title.Should().Be("Forbidden");

            var internalError = APIException.Internal();
            internalError.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            internalError.Title.Should().Be("Internal Server Error");
        }
    }
}
