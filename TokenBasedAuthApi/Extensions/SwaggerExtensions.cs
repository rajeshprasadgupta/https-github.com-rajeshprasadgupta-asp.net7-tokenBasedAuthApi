using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace TokenBasedAuthApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                //Using the AddSecurityDefinition() method, describe how the API is protected
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    //We will use an API key, which is the bearer token, in the header with the name Authorization
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Description = "Insert the token with the 'Bearer ' prefix"
                });
                //with AddSecurityRequirement(), we specified that we have a security requirement for our endpoints, which means that the security information must be sent for every request
                c.AddSecurityRequirement(new
                  OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = HeaderNames.Authorization,
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer",
                                BearerFormat = "JWT",
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id =
                                     JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                c.SwaggerDoc("v1", new()
                {
                    Title = builder.Environment.ApplicationName,
                    Version = "v1",
                    Contact = new()
                    {
                        Name = "anonymous",
                        Email = "authors@localhost.com",
                        Url = new Uri("https://www.localhost.com/")
                    },
                    Description = "This API has TokenBasedAuthentication related operations",
                    License = new Microsoft.OpenApi.Models.
                        OpenApiLicense(),
                    TermsOfService = new("https://www.localhost.com/")
                });
            });
            return builder;
        }
    }
}
