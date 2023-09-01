namespace MCS.HomeSite.Common
{
    public interface IUrlGenerator
    {
        string? GeneratorUrl(string prefix, string action);
    }

    public class UrlGenerator : IUrlGenerator
    {
        private readonly IHttpContextAccessor _accessor;

        public UrlGenerator(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string? GeneratorUrl(string prefix, string action)
        {
            var url = $"{_accessor?.HttpContext?.Request.Scheme}://{_accessor?.HttpContext?.Request.Host}{prefix}/{action}";
            return url;
        }
    }
}
