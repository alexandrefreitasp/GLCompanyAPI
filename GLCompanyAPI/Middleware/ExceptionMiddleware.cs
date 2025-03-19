using GLCompanyAPI.Enums;
using GLCompanyAPI.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace GLCompanyAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, ErrorCode.NotFound, HttpStatusCode.NotFound);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, ErrorCode.Unauthorized, HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, ErrorCode.UnexpectedError, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ErrorCode errorCode, HttpStatusCode statusCode)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse(errorCode, exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);

            return response.WriteAsync(result);
        }
    }
}