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

using Microsoft.AspNetCore.Mvc;
using Moq;
using Sannel.House.Base.Models;
using Sannel.House.Schedule.Controllers;
using Sannel.House.Schedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sannel.House.Schedule.Tests.Controllers
{
	public class ScheduleControllerTests : BaseTests
	{
		[Fact]
		public async Task GetScheduleAsync_BadRequestTest()
		{
			var service = new Mock<IScheduleService>();
			var logger = CreateLogger<ScheduleController>();

			var controller = new ScheduleController(service.Object, logger);
			controller.ModelState.AddModelError("scheduleKey", "Schedule Key was not provided");

			var result = await controller.Get(Guid.Empty);
			var bror = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
			var erm = Assert.IsAssignableFrom<ErrorResponseModel>(bror.Value);

			Assert.Equal(System.Net.HttpStatusCode.BadRequest, erm.StatusCode);
			Assert.Single(erm.Errors);

			var first = erm.Errors.First();
			Assert.Equal("scheduleKey", first.Key); 
			Assert.Equal("Schedule Key was not provided", string.Join(' ', first.Value));
		}

		[Fact]
		public async Task GetScheduleAsync_NotFoundTest()
		{
			var service = new Mock<IScheduleService>();

			service.Setup(i => i.GetScheduleAsync(It.IsAny<Guid>())).ReturnsAsync((Models.Schedule)null);

			var logger = CreateLogger<ScheduleController>();

			var controller = new ScheduleController(service.Object, logger);

			var result = await controller.Get(Guid.Empty);
			var nfor = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
			var sm = Assert.IsAssignableFrom<ResponseModel<ViewModel.ScheduleModel>>(nfor.Value);

			Assert.Equal(System.Net.HttpStatusCode.NotFound, sm.StatusCode);
			Assert.Equal("Not Found", sm.Title);
			Assert.Null(sm.Data);
		}

		[Fact]
		public async Task GetScheduleAsync_OkTest()
		{
			var service = new Mock<IScheduleService>();

			var expected = new Models.Schedule()
			{
				ScheduleKey = Guid.NewGuid(),
				Name = $"Test {Random.Next(0, int.MaxValue)}",
				MinimumDifference = 2,
				DefaultMaxValue = Random.Next(50, 400),
				DefaultMinValue = Random.Next(0, 40)
			};
			service.Setup(i => i.GetScheduleAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

			var logger = CreateLogger<ScheduleController>();

			var controller = new ScheduleController(service.Object, logger);

			var result = await controller.Get(expected.ScheduleKey);
			var okor = Assert.IsAssignableFrom<OkObjectResult>(result);
			var sm = Assert.IsAssignableFrom<ResponseModel<ViewModel.ScheduleModel>>(okor.Value);

			Assert.Equal(System.Net.HttpStatusCode.OK, sm.StatusCode);
			Assert.NotNull(sm.Data);
			var actual = sm.Data;
			Assert.Equal(expected.ScheduleKey, actual.ScheduleKey);
			Assert.Equal(expected.Name, actual.Name);
			Assert.Equal(expected.MinimumDifference, actual.MinimumDifference);
			Assert.Equal(expected.DefaultMaxValue, actual.DefaultMaxValue);
			Assert.Equal(expected.DefaultMinValue, actual.DefaultMinValue);
		}
	}
}
