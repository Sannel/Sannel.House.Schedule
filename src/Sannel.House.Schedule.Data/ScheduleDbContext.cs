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
using Sannel.House.Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.Data
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
	/// <seealso cref="Sannel.House.Schedule.Interfaces.IScheduleContext" />
	public class ScheduleDbContext : DbContext, IScheduleContext
	{
		/// <summary>
		/// Gets the schedules.
		/// </summary>
		/// <value>
		/// The schedules.
		/// </value>
		public DbSet<Models.Schedule> Schedules => Set<Models.Schedule>();

		/// <summary>
		/// Gets the schedule properties.
		/// </summary>
		/// <value>
		/// The schedule properties.
		/// </value>
		public DbSet<Models.ScheduleProperty> ScheduleProperties => Set<Models.ScheduleProperty>();

		/// <summary>
		/// Gets the schedule starts.
		/// </summary>
		/// <value>
		/// The schedule starts.
		/// </value>
		public DbSet<Models.ScheduleStart> ScheduleStarts => Set<Models.ScheduleStart>();

		public ScheduleDbContext([NotNull] DbContextOptions options) : base(options)
		{
		}

		protected ScheduleDbContext()
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var s = builder.Entity<Models.Schedule>();
			s.HasIndex(i => i.ScheduleKey).IsUnique();

			var sp = builder.Entity<Models.ScheduleProperty>();
			sp.HasIndex(i => i.Name);
			sp.HasIndex(nameof(Models.ScheduleProperty.ScheduleId), nameof(Models.ScheduleProperty.Name)).IsUnique();

			var ss = builder.Entity<Models.ScheduleStart>();
			ss.HasIndex(nameof(Models.ScheduleStart.Start), nameof(Models.ScheduleStart.Type));
		}
	}
}
