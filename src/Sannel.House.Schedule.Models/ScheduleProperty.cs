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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.Models
{
	/// <summary>
	/// Extra data related to a schedule
	/// </summary>
	public class ScheduleProperty
	{
		/// <summary>
		/// Gets or sets the schedule property identifier.
		/// </summary>
		/// <value>
		/// The schedule property identifier.
		/// </value>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid SchedulePropertyId { get; set; }

		/// <summary>
		/// Gets or sets the schedule identifier.
		/// </summary>
		/// <value>
		/// The schedule identifier.
		/// </value>
		[Required]
		public long ScheduleId { get; set; }

		/// <summary>
		/// Gets or sets the schedule.
		/// </summary>
		/// <value>
		/// The schedule.
		/// </value>
		public Schedule? Schedule { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
		[MaxLength(512)]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		[Required]
		[MaxLength(1024)]
		public string Value { get; set; } = string.Empty;
	}
}
