using Serilog.Context;

namespace Microsoft.AspNetCore.Builder
{
    public static class SerilogApplicationBuilderExtensions
    {
        public const string CORRELATION_ID_HEADER = "x-correlation-id";
        public static IApplicationBuilder UseCorrelationIdHeader(this IApplicationBuilder builder)
        {
            builder.Use((context, next) =>
            {
                context.Response.Headers.Add(CORRELATION_ID_HEADER, context.TraceIdentifier);

                return next();
            });

            return builder;
        }

        public static IApplicationBuilder UseSerilogCorrelationIdEnricher(this IApplicationBuilder builder)
        {
            builder.Use((context, next) =>
            {
                LogContext.PushProperty("CorrelationId", context.TraceIdentifier);
                LogContext.PushProperty("ClientIpAddress", context.Connection.RemoteIpAddress);
                return next();
            });

            return builder;
        }

        public static IApplicationBuilder UseSerilogUPNEnricher(this IApplicationBuilder builder)
        {
            builder.Use((context, next) =>
            {
                LogContext.PushProperty("UPN", context.User?.Identity?.Name);
                return next();
            });

            return builder;
        }
    }
}
