using TokenBasedAuthApi.Extensions;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureLogger()
    .ConfigureDatabase()
    //Add JWT support to Swagger
    .ConfigureSwagger()
    .ConfigureJsonSerialization()
    //add authentication and authorization services to the service provider
    .ConfigureAuthentication();
builder.Services.AddApplicationServices()
    .AddAuthorization();
var app = builder.Build();
//app.UseAuthentication(); from .NET 7 call to add authentication middleware to the pipeline. i.e. UseAuthenication() is not required. It is implicit
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
        "TokenBasedAuthentication API v1");
    });
}
app.UseHttpsRedirection();
app.RegisterEndpoints();
app.Run();

