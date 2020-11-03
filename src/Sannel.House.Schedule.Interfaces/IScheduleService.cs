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

using Sannel.House.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.Interfaces
{
	public interface IScheduleService
	{
		/// <summary>
		/// Gets the schedule asynchronous.
		/// </summary>
		/// <param name="scheduleKey">The schedule key.</param>
		/// <returns></returns>
		Task<Models.Schedule?> GetScheduleAsync(Guid scheduleKey);

		/// <summary>
		/// Gets the schedules paged asynchronous.
		/// </summary>
		/// <param name="pageIndex">The index of this page</param>
		/// <param name="pageSize">The size of the page</param>
		/// <returns></returns>
		Task<PagedResponseModel<Models.Schedule>> GetSchedulesAsync(int pageIndex, int pageSize);
	}
}
