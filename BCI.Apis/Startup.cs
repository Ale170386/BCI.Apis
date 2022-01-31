using AutoMapper;
using BCI.Api.Business.BusinessRequest;
using BCI.Api.Business.Utilities;
using BCI.Api.Data;
using BCI.Api.Data.DataRequest;
using BCI.Api.Data.DataTrace;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using BCI.Api.Business.BusinessTrace;
using BCI.Api.Business.BusinessProcessLog;
using BCI.Api.Data.DataProcessLog;

namespace BCI.Apis
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            //{
            //    builder.WithOrigins("http://40.87.62.201/", "http://40.87.62.201:80", "http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            //}));

            services.AddCors();

            services.AddControllers();            

            string connectionString = configuration.GetConnectionString("defaultConnectionString");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BCI.Apis", Version = "v1" });

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-Api-Key: ApiKey",
                    In = ParameterLocation.Header,
                    Name = "ApiKey",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "ApiKey",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            },
                         },
                         new string[] {}
                     }
                });
            });

            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AutoMapperProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<ITraceDAL, TraceDAL>();
            services.AddScoped<ITraceBL, TraceBL>();
            services.AddScoped<IActivationRequestDAL, ActivationRequestDAL>();
            services.AddScoped<IActivationRequestBL, ActivationRequestBL>();  
            services.AddScoped<IFTP, FTP>();
            services.AddScoped<ICredentials, Credentials>();
            services.AddScoped<IProcessLogBL, ProcessLogBL>();
            services.AddScoped<IProcessLogDAL, ProcessLogDAL>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors("ApiCorsPolicy");
            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            ); //This needs to set everything allowed

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BCI.Apis"));
            

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
