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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sannel.House.Schedule.Interfaces;
using Sannel.House.Schedule.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sannel.House.Schedule.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ScheduleController : ControllerBase
	{
		private readonly IScheduleService service;
		private readonly ILogger logger;

		public ScheduleController(IScheduleService service,
			ILogger<ScheduleController> logger)
		{
			this.service = service ?? throw new ArgumentNullException(nameof(service));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet]
		[Authorize(Roles = "ScheduleRead,Admin")]
		[ProducesResponseType(200, Type = typeof(Sannel.House.Base.Models.ResponseModel<ScheduleModel>))]
		[ProducesResponseType(400, Type = typeof(Sannel.House.Base.Models.ErrorResponseModel))]
		public IActionResult Get([Required]Guid scheduleKey)
		{
			return null;
		}
	}
}
