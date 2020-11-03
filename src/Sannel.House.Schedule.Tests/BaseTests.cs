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
using Sannel.House.Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.Tests
{
	public abstract class BaseTests : Sannel.House.Base.Tests.BaseTests<ScheduleDbContext>
	{
		/// <summary>
		/// Creates the database context.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns></returns>
		public override ScheduleDbContext CreateDbContext(DbContextOptions options)
			=> new ScheduleDbContext(options);

		/// <summary>
		/// Gets the type of the migration assembly.
		/// </summary>
		/// <value>
		/// The type of the migration assembly.
		/// </value>
		public override Type MigrationAssemblyType
			=> typeof(Sannel.House.Schedule.Data.Migrations.Sqlite.DevicesDesignTimeFactory);
	}
}
