using TokenBasedAuthApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TokenBasedAuthApi.EndPoints
{
    public static class FunctionalEndPoints
    {
        public static RouteGroupBuilder CreateFunctionalEndpoints(this IEndpointRouteBuilder routes)
        {
            //test endpoint which should work only for authorized users
            var group = routes.MapGroup("").WithTags("TestApi");
            group.MapGet("/api/testApi/get-items",
            //protect the endpoint using the Authorize attribute or the RequireAuthorization() method
            [Authorize] //C# v10 allows to specify an attribute directly on a lambda expression
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            (AppDbContext db) =>
            {
                return Results.Ok(db.ServiceTypes.ToList());
            });
            return group;
        }
    }
}
