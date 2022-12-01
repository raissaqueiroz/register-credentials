using Serilog.Context;

namespace RegisterCredentials.Api.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request?.EnableBuffering();
            var reader = new StreamReader(context.Request?.Body);
            string body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            await _next(context);

            using(LogContext.PushProperty("Content", body))
            {
                _logger.LogInformation($"Request {context.Request?.Method} {context.Request?.Path.Value} returned HttpStatusCode {context.Response?.StatusCode}");
            }
        }
    }
}
