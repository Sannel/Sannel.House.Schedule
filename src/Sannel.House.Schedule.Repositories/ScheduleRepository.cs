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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sannel.House.Base.Models;
using Sannel.House.Schedule.Data;
using Sannel.House.Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.Repositories
{
	public class ScheduleRepository : IScheduleRepository
	{
		private readonly ScheduleDbContext context;
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScheduleRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="logger">The logger.</param>
		/// <exception cref="ArgumentNullException">
		/// context
		/// or
		/// logger
		/// </exception>
		public ScheduleRepository(ScheduleDbContext context,
			ILogger<ScheduleRepository> logger)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Adds the schedule asynchronous.
		/// </summary>
		/// <param name="schedule">The schedule.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">schedule</exception>
		public async Task<Guid?> AddScheduleAsync([NotNull] Models.Schedule schedule)
		{
			if(schedule is null)
			{
				throw new ArgumentNullException(nameof(schedule));
			}

			if(await context.Schedules.AnyAsync(i => i.ScheduleKey == schedule.ScheduleKey))
			{
				logger.LogWarning("Duplicate ScheduleKey attempted to be added {ScheduleKey}", schedule.ScheduleKey);

				return null;
			}

			var result = await context.Schedules.AddAsync(schedule);
			await context.SaveChangesAsync();

			result.State = EntityState.Detached;

			return result.Entity.ScheduleKey;
		}

		/// <summary>
		/// Gets the schedule asynchronous.
		/// </summary>
		/// <param name="scheduleKey">The schedule key.</param>
		/// <returns></returns>
		public async Task<Models.Schedule?> GetScheduleAsync(Guid scheduleKey)
		{
			if (logger.IsEnabled(LogLevel.Debug))
			{
				logger.LogDebug("Attempting to get Schedule with ScheduleKey {ScheduleKey}", scheduleKey);
			}

			var result = await context.Schedules.AsNoTracking().FirstOrDefaultAsync(i => i.ScheduleKey == scheduleKey);

			if (logger.IsEnabled(LogLevel.Debug))
			{
				if(result is null)
				{
					logger.LogDebug("Unable to get a Schedule with ScheduleKey {ScheduleKey}", scheduleKey);
				}
				else
				{
					logger.LogDebug("Got Schedule for ScheduleKey {ScheduleKey}. Id {ScheduleId}", scheduleKey, result.ScheduleId);
				}
			}

			return result;
		}

		public async Task<PagedResponseModel<Models.Schedule>> GetSchedulesAsync(int pageIndex, int pageSize)
		{
			var response = new PagedResponseModel<Models.Schedule>("", Enumerable.Empty<Models.Schedule>(), 0, pageIndex, pageSize);
			response.TotalCount = await context.Schedules.LongCountAsync();
			response.Data = await context.Schedules.AsNoTracking()
				.OrderBy(i => i.ScheduleId)
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return response;
		}

		/// <summary>
		/// Does a schedule with <paramref name="schedulekey" /> exist
		/// </summary>
		/// <param name="schedulekey">The schedulekey.</param>
		/// <returns></returns>
		public Task<bool> ExistsAsync([NotNull] Guid schedulekey)
			=> context.Schedules.AnyAsync(i => i.ScheduleKey == schedulekey);
	}
}
