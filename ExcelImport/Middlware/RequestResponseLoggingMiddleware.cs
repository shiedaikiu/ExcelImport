using ExcelImport.Data;
using ExcelImport.Models;
using System.Diagnostics;

namespace ExcelImport.Middlware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Request.EnableBuffering(); // برای اینکه بتوانیم دوباره از جریان استفاده کنیم
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var log = new Log
            {
                Input = requestBody,
                Output = responseText,
                CreateDate = DateTime.UtcNow,
                DurationMs = stopwatch.ElapsedMilliseconds
            };
            db.Logs.Add(log);
            await db.SaveChangesAsync();

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
