using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using G_API.Clients;
using G_API.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace G_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SongDBClient>();
            services.AddSingleton<SongsterClient>();
            services.AddSingleton<ChordsClient>();
            services.AddSingleton<DBClient>();

            services.AddScoped<SpotifyClient>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "G_API", Version = "v1" });
            });

            var credentials = new BasicAWSCredentials(Constants.DB_ID, Constants.DB_Secret);
            var config = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };
            var client = new AmazonDynamoDBClient(credentials, config);
            services.AddSingleton<IAmazonDynamoDB>(client);
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "G_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
