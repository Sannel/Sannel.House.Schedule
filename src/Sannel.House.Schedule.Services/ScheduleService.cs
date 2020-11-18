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

using Microsoft.Extensions.Logging;
using Sannel.House.Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sannel.House.Base.MQTT.Interfaces;
using Sannel.House.Base.Models;

namespace Sannel.House.Schedule.Services
{
	public class ScheduleService : IScheduleService
	{
		private readonly IScheduleRepository repository;
		private readonly IMqttClientPublishService mqtt;
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduleService"/> class.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="mqtt">The MQTT.</param>
		/// <param name="logger">The logger.</param>
		/// <exception cref="ArgumentNullException">
		/// repository
		/// or
		/// mqtt
		/// or
		/// logger
		/// </exception>
		public ScheduleService(IScheduleRepository repository,
			IMqttClientPublishService mqtt,
			ILogger<ScheduleService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.mqtt = mqtt ?? throw new ArgumentNullException(nameof(mqtt));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Gets the schedule asynchronous.
		/// </summary>
		/// <param name="scheduleKey">The schedule key.</param>
		/// <returns></returns>
		public async Task<Models.Schedule?> GetScheduleAsync(Guid scheduleKey)
		{
			logger.LogDebug("Getting Schedule by ScheduleKey {scheduleKey}", scheduleKey);
			var result = await repository.GetScheduleAsync(scheduleKey);

			if(result is null)
			{
				logger.LogInformation("No Schedule found for scheduleKey {scheduleKey}", scheduleKey);
			}
			else
			{
				logger.LogInformation("Found Schedule for scheduleKey {scheduleKey}", scheduleKey);

				if(logger.IsEnabled(LogLevel.Debug))
				{
					logger.LogDebug("Schedule Information: Name {name}, ScheduleId {scheduleId}", result.Name, result.ScheduleId);
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the schedules paged asynchronous.
		/// </summary>
		/// <param name="pageIndex">The index of this page</param>
		/// <param name="pageSize">The size of the page</param>
		/// <returns></returns>
		public async Task<PagedResponseModel<Models.Schedule>> GetSchedulesAsync(int pageIndex, int pageSize)
		{
			logger.LogDebug("Getting Schedules in Page Index {pageIndex} and of Page Size {pageSize}", pageIndex, pageSize);

			var result = await repository.GetSchedulesAsync(pageIndex, pageSize);

			return result;
		}
	}
}
