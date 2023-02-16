namespace TokenBasedAuthApi.Extensions
{
    public static class LoggerExtensions
    {
        public static WebApplicationBuilder ConfigureLogger(this WebApplicationBuilder builder)
        {
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
            return builder;
        }
    }
}
