using System.Text.Json.Serialization;

namespace TokenBasedAuthApi.Extensions
{
    public static class JsonSerializationExtensions
    {
        public static WebApplicationBuilder ConfigureJsonSerialization(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.IgnoreReadOnlyProperties = true;
            });
            return builder;
        }
    }
}
