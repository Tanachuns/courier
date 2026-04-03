using System.Text;

namespace courier.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Enable buffering so the body can be read multiple times
        context.Request.EnableBuffering();

        // Read the body as string
        string bodyAsText;
        using (var reader = new StreamReader(
                   context.Request.Body,
                   encoding: Encoding.UTF8,
                   detectEncodingFromByteOrderMarks: false,
                   leaveOpen: true))
        {
            bodyAsText = await reader.ReadToEndAsync();
            // Reset position so downstream can read it
            context.Request.Body.Position = 0;
        }

        if (!string.IsNullOrWhiteSpace(bodyAsText))
        {
            try
            {
                // Optional: parse and format JSON nicely
                var json = System.Text.Json.JsonSerializer.Deserialize<object>(bodyAsText);
                var formattedJson = System.Text.Json.JsonSerializer.Serialize(json, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("Request Body:\n{Body}", formattedJson);
            }
            catch
            {
                // Fallback if body is not valid JSON
                _logger.LogInformation("Request Body (raw):\n{Body}", bodyAsText);
            }
        }

        // Continue the pipeline
        await _next(context);
    }
}