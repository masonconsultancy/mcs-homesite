using System.Reflection;
using System.Text.Json;
using mcs_homesite.Areas.DataTables.Data;

namespace mcs_homesite.Extensions
{
    public static class Extensions
    {
        public static async Task<T?> ReadAsync<T>(string filePath)
        {
            await using var stream = ReadManifestData<mcs_homesiteContext>(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        public static Stream ReadManifestData<TSource>(string embeddedFileName) where TSource : class
        {
            var assembly = typeof(TSource).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));
            var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("Could not load manifest resource stream.");
            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
