using FluentAssertions;
using FluentResults;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;
using SGP.Shared.Errors;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.PublicApi.Extensions
{
    [UnitTest]
    public class FluentResultExtensionsTests
    {
        [Fact]
        public void Should_AddErrorsInGraphQL_WhenResultHasError()
        {
            // Arrange
            const string errorMessage = "Requisição inválida.";
            var result = new Result<string>().WithError(new Error(errorMessage));
            var resolveFieldContext = new ResolveFieldContext { Errors = new ExecutionErrors() };

            // Act
            result.ToExecutionError(resolveFieldContext);

            // Assert
            resolveFieldContext.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.ContainEquivalentOf(new ExecutionError(errorMessage));
        }

        [Fact]
        public void Should_ReturnsBadRequestResult_WhenResultHasError()
        {
            // Act
            const int expectedStatusCode = StatusCodes.Status400BadRequest;
            const string errorMessage = "Requisição inválida.";
            var expectedApiResponse = new ApiResponse(expectedStatusCode, new ApiError(errorMessage));
            var result = new Result().WithError(new Error(errorMessage));

            // Arrange
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            act.StatusCode.Should().Be(expectedStatusCode);
            act.Value.Should().BeEquivalentTo(expectedApiResponse);
        }

        [Fact]
        public void Should_ReturnsBadRequestResult_WhenTypedResultHasError()
        {
            // Act
            const int expectedStatusCode = StatusCodes.Status400BadRequest;
            const string errorMessage = "Requisição inválida.";
            var expectedApiResponse = new ApiResponse(expectedStatusCode, new ApiError(errorMessage));
            var result = new Result<string>().WithError(new Error(errorMessage));

            // Arrange
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            act.StatusCode.Should().Be(expectedStatusCode);
            act.Value.Should().BeEquivalentTo(expectedApiResponse);
        }

        [Fact]
        public void Should_ReturnsDistinctErrorsMessage_WhenResultHasErrors()
        {
            // Arrange
            const int expectedCount = 3;
            var result = new Result().WithErrors(new[] { "Erro0", "Erro1", "Erro0", "Erro2" });

            // Act
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            var apiResponse = act.Value as ApiResponse;
            apiResponse.Should().NotBeNull();
            apiResponse.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(expectedCount);
        }

        [Fact]
        public void Should_ReturnsNotFoundRequestResult_WhenResultHasNotFoundError()
        {
            // Act
            const int expectedStatusCode = StatusCodes.Status404NotFound;
            const string errorMessage = "Nenhum registro encontrado.";
            var expectedApiResponse = new ApiResponse(expectedStatusCode, new ApiError(errorMessage));
            var result = new Result().WithError(new NotFoundError(errorMessage));

            // Arrange
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
            act.StatusCode.Should().Be(expectedStatusCode);
            act.Value.Should().BeEquivalentTo(expectedApiResponse);
        }

        [Fact]
        public void Should_ReturnsNotFoundRequestResult_WhenTypedResultHasNotFoundError()
        {
            // Act
            const int expectedStatusCode = StatusCodes.Status404NotFound;
            const string errorMessage = "Nenhum registro encontrado.";
            var expectedApiResponse = new ApiResponse(expectedStatusCode, new ApiError(errorMessage));
            var result = new Result<string>().WithError(new NotFoundError(errorMessage));

            // Arrange
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
            act.StatusCode.Should().Be(expectedStatusCode);
            act.Value.Should().BeEquivalentTo(expectedApiResponse);
        }

        [Fact]
        public void Should_ReturnsOkResult_WhenResultIsOk()
        {
            // Act
            const int expectedStatusCode = StatusCodes.Status200OK;
            var expectedApiResponse = new ApiResponse(expectedStatusCode);
            var result = new Result();

            // Arrange
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            act.StatusCode.Should().Be(expectedStatusCode);
            act.Value.Should().BeEquivalentTo(expectedApiResponse);
        }

        [Fact]
        public void Should_ReturnsOkResult_WhenTypedResultIsOk()
        {
            // Act
            const string value = "Hello World!!!";
            const int expectedStatusCode = StatusCodes.Status200OK;
            var expectedApiResponse = new ApiResponse<string>(expectedStatusCode, value);
            var result = new Result<string>().WithValue(value);

            // Arrange
            var act = result.ToHttpResult();

            // Assert
            act.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            act.StatusCode.Should().Be(expectedStatusCode);
            act.Value.Should().BeEquivalentTo(expectedApiResponse);
        }
    }
}