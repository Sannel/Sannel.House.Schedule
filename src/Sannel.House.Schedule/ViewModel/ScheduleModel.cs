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
using System.Linq;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class ScheduleModel
	{
		/// <summary>
		/// Gets or sets the schedule key.
		/// </summary>
		/// <value>
		/// The schedule key.
		/// </value>
		public Guid ScheduleKey { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; } = string.Empty;
		/// <summary>
		/// Gets or sets the default minimum value.
		/// </summary>
		/// <value>
		/// The default minimum value.
		/// </value>
		public double DefaultMinValue { get; set; }
		/// <summary>
		/// Gets or sets the default maximum value.
		/// </summary>
		/// <value>
		/// The default maximum value.
		/// </value>
		public double? DefaultMaxValue { get; set; }
		/// <summary>
		/// Gets or sets the minimum difference.
		/// </summary>
		/// <value>
		/// The minimum difference.
		/// </value>
		public double MinimumDifference { get; set; }
	}
}
