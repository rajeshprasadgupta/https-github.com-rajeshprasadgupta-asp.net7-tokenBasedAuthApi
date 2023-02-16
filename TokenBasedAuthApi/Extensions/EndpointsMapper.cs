using TokenBasedAuthApi.EndPoints;

namespace TokenBasedAuthApi.Extensions
{
    public static class EndpointsMapper
    {
        public static RouteGroupBuilder RegisterEndpoints(this IEndpointRouteBuilder routes)
        {
            return routes.CreateAuthEndpoints()
                .CreateIdentityEndpoints()
                .CreateFunctionalEndpoints();
        }
    }
}
