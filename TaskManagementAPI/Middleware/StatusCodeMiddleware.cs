namespace TaskManagementAPI.Middleware
{
    public class StatusCodeMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusCodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 403)
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    StatusCode = 403,
                    Message = "Forbidden: You do not have permission to access this resource."
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            else if (context.Response.StatusCode == 404)
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    StatusCode = 404,
                    Message = "Resource not found."
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
