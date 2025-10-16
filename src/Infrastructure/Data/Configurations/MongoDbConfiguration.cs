using System.Reflection;
using SchoolTripApi.Infrastructure.Data.Abstractions;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

public static class MongoDbConfiguration
{
    private static bool _isConfigured = false;
    private static readonly object Lock = new object();
    
    public static void ConfigureEntities()
    {
        lock (Lock)
        {
            if (_isConfigured) return;
            
            var configurationType = typeof(IMongoEntityConfiguration);
            var configurations = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => configurationType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IMongoEntityConfiguration>();

            foreach (var configuration in configurations) configuration.Configure();
            _isConfigured = true;
        }
    }
}