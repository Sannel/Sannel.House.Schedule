/* Copyright 2020-2020 Sannel Software, L.L.C.
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
      http://www.apache.org/licenses/LICENSE-2.0
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/

using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sannel.House.Base.Data;
using Sannel.House.Base.Web;
using Sannel.House.Schedule.Data;
using Sannel.House.Schedule.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sannel.House.Schedule
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
			services.AddControllers();

			var connectionString = Configuration.GetWithReplacement("Db:ConnectionString");
			if (string.IsNullOrWhiteSpace(connectionString))
			{
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
				throw new ArgumentNullException("Db:ConnectionString", "Db:ConnectionString is required");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
			}

			services.AddDbContextPool<ScheduleDbContext>(o =>
			{
				switch (Configuration["Db:Provider"]?.ToLowerInvariant())
				{
					case "mysql":
						throw new NotSupportedException("We are currently not supporting mysql as a database provider");

					case "sqlserver":
						o.ConfigureSqlServer(connectionString);
						break;

					case "postgresql":
						o.ConfigurePostgreSQL(connectionString);
						break;

					case "sqlite":
					default:
						o.ConfigureSqlite(connectionString);
						break;
				}
			});


			services.AddAuthentication(Configuration["Authentication:Schema"])
				.AddIdentityServerAuthentication(Configuration["Authentication:Schema"], o =>
					{
						o.Authority = this.Configuration["Authentication:AuthorityUrl"];
						o.ApiName = this.Configuration["Authentication:ApiName"];

						var apiSecret = this.Configuration["Authentication:ApiSecret"];
						if (!string.IsNullOrWhiteSpace(apiSecret))
						{
							o.ApiSecret = apiSecret;
						}

						o.SupportedTokens = SupportedTokens.Both;

						if (Configuration.GetValue<bool?>("Authentication:DisableRequireHttpsMetadata") == true)
						{
							o.RequireHttpsMetadata = false;
						}
					});


			services.AddOpenApiDocument((s, p) =>
			{
				s.Title = "Scheduling";
				s.Description = "Project for storing and generating schedule data";
			});

			services.AddMqttService(Configuration);

			services.AddHealthChecks()
				.AddDbHealthCheck<ScheduleDbContext>("DbHealthCheck", async (context) =>
				{
					await context.Schedules.AnyAsync();
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,
			IWebHostEnvironment env,
			IServiceProvider provider,
			ILogger<Startup> logger)
		{
			provider.CheckAndInstallTrustedCertificate();

			var p = Configuration["Db:Provider"];
			var db = provider.GetService<ScheduleDbContext>() ?? throw new Exception("ScheduleDbContext is not set in service provider");

			if (string.Compare(p, "mysql", true, CultureInfo.InvariantCulture) == 0
					|| string.Compare(p, "postgresql", true, CultureInfo.InvariantCulture) == 0
					|| string.Compare(p, "sqlserver", true, CultureInfo.InvariantCulture) == 0)
			{
				logger.LogDebug("DB Provider is a Server waiting for it to be up");

				if (!db.WaitForServer(logger))
				{
					throw new Exception("Shutting Down");
				}
			}

			db.Database.Migrate();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseOpenApi();
			app.UseReDoc();

			app.UseEndpoints(i =>
			{
				i.MapControllers();
				i.MapHouseHealthChecks("/health");
				i.MapHouseRobotsTxt();
			});

		}
	}
}
