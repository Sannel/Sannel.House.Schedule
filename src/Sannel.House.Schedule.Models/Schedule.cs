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
	/// The Schedule Model
	/// </summary>
	public class Schedule
	{
		/// <summary>
		/// The id of the schedule
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ScheduleId { get; set; }

		/// <summary>
		/// The Unique Key representing this Schedule
		/// </summary>
		[Required]
		public Guid ScheduleKey { get; set; }

		/// <summary>
		/// The Name to display for this schedule
		/// </summary>
		[Required]
		[MaxLength(255)]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The Default Minimum Value for this schedule
		/// </summary>
		[Required]
		public double DefaultMinValue { get; set; }

		/// <summary>
		/// The Default Maximum Value for this schedule
		/// </summary>
		public double? DefaultMaxValue { get; set; }

		/// <summary>
		/// The Minimum Difference that MinValue and MaxValue must be apart from each other
		/// </summary>
		[Required]
		[Range(0,double.MaxValue)]
		public double MinimumDifference { get; set; }

	}
}
