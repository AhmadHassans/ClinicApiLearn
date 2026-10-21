namespace ClinicApiLearn.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteError(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteError(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteError(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
            await WriteError(context, StatusCodes.Status500InternalServerError, "Something went wrong.");
        }
    }

    private static async Task WriteError(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { message });
    }
}