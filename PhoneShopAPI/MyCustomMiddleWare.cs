namespace PhoneShopAPI
{
    public class MyCustomMiddleWare
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }


        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("HACKBY", "MR QUAN");
            //return httpContext.Response.WriteAsync("Hello world!");
            return _next(httpContext);
            //return _next(httpContext);
        }
    }
}
