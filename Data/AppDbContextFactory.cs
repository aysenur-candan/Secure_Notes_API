using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SecureNotesAPI.Data;
using DotNetEnv;
using System.Text.RegularExpressions;

namespace SecureNotesAPI.Factories
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            Env.Load();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var rawConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");

            if (string.IsNullOrWhiteSpace(rawConnectionString))
                throw new Exception("POSTGRES_CONNECTION environment variable is not set.");

            if (environment == "Development")
            {
                rawConnectionString = OverrideHostAndPort(rawConnectionString, "localhost", "5433");
            }

            optionsBuilder.UseNpgsql(rawConnectionString);
            return new AppDbContext(optionsBuilder.Options);
        }

        private string OverrideHostAndPort(string connectionString, string newHost, string newPort)
        {
            connectionString = Regex.Replace(connectionString, @"Host=\s*[^;]+", $"Host={newHost}", RegexOptions.IgnoreCase);
            connectionString = Regex.Replace(connectionString, @"Port=\s*\d+", $"Port={newPort}", RegexOptions.IgnoreCase);
            return connectionString;
        }
    }
}
