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
using Sannel.House.Schedule.Models;

namespace Sannel.House.Schedule.Interfaces
{
	public interface IScheduleContext
	{
		/// <summary>
		/// Gets the schedule properties.
		/// </summary>
		/// <value>
		/// The schedule properties.
		/// </value>
		DbSet<ScheduleProperty> ScheduleProperties { get; }
		/// <summary>
		/// Gets the schedules.
		/// </summary>
		/// <value>
		/// The schedules.
		/// </value>
		DbSet<Models.Schedule> Schedules { get; }
		/// <summary>
		/// Gets the schedule starts.
		/// </summary>
		/// <value>
		/// The schedule starts.
		/// </value>
		DbSet<ScheduleStart> ScheduleStarts { get; }
	}
}