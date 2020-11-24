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
using System.Collections;
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

		[Fact]
		public async Task GetSchedulesAsync_BadRequestTest()
		{
			var service = new Mock<IScheduleService>();
			var logger = CreateLogger<ScheduleController>();

			var controller = new ScheduleController(service.Object, logger);
			controller.ModelState.AddModelError("pageIndex", "pageIndex was not provided");

			var result = await controller.GetPaged(0, 25);
			var bror = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
			var erm = Assert.IsAssignableFrom<ErrorResponseModel>(bror.Value);

			Assert.Equal(System.Net.HttpStatusCode.BadRequest, erm.StatusCode);
			Assert.Single(erm.Errors);

			var first = erm.Errors.First();
			Assert.Equal("pageIndex", first.Key); 
			Assert.Equal("pageIndex was not provided", string.Join(' ', first.Value));
		}

		[Fact]
		public async Task GetSchedulesAsync_NoResultsTest()
		{
			var service = new Mock<IScheduleService>();
			var logger = CreateLogger<ScheduleController>();

			var called = 0;
			var pIndex = 0;
			var pSize = 25;

			service.Setup(i => i.GetSchedulesAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync((int pageIndex, int pageSize) =>
				{
					called++;
					Assert.Equal(pIndex, pageIndex);
					Assert.Equal(pSize, pageSize);
					return new PagedResponseModel<Models.Schedule>("Paged Data", new List<Models.Schedule>(), 0, pageIndex, pageSize);
				});

			var controller = new ScheduleController(service.Object, logger);

			var result = await controller.GetPaged(pIndex, pSize);
			var okor = Assert.IsAssignableFrom<OkObjectResult>(result);
			var prm = Assert.IsAssignableFrom<PagedResponseModel<ViewModel.ScheduleModel>>(okor.Value);

			Assert.NotNull(prm);
			Assert.Equal(1, called);
			Assert.Empty(prm.Data);
			Assert.Equal(pIndex, prm.Page);
			Assert.Equal(pSize, prm.PageSize);
			Assert.Equal("Paged Results", prm.Title);
		}

		[Fact]
		public async Task GetSchedulesAsync_NoResults2Test()
		{
			var service = new Mock<IScheduleService>();
			var logger = CreateLogger<ScheduleController>();

			var called = 0;
			var pIndex = 0;
			var pSize = 25;

			service.Setup(i => i.GetSchedulesAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync((int pageIndex, int pageSize) =>
				{
					called++;
					Assert.Equal(pIndex, pageIndex);
					Assert.Equal(pSize, pageSize);
					return null;
				});

			var controller = new ScheduleController(service.Object, logger);

			var result = await controller.GetPaged(pIndex, pSize);
			var okor = Assert.IsAssignableFrom<OkObjectResult>(result);
			var prm = Assert.IsAssignableFrom<PagedResponseModel<ViewModel.ScheduleModel>>(okor.Value);

			Assert.NotNull(prm);
			Assert.Equal(1, called);
			Assert.Empty(prm.Data);
			Assert.Equal(pIndex, prm.Page);
			Assert.Equal(pSize, prm.PageSize);
			Assert.Equal("Paged Results", prm.Title);
		}


		[Fact]
		public async Task GetSchedulesAsync_OnePageTest()
		{
			var service = new Mock<IScheduleService>();
			var logger = CreateLogger<ScheduleController>();

			var list = new List<Models.Schedule>()
			{
				GenerateSchedule(1),
				GenerateSchedule(2),
				GenerateSchedule(3)
			};

			var called = 0;
			var pIndex = 0;
			var pSize = 25;

			service.Setup(i => i.GetSchedulesAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync((int pageIndex, int pageSize) =>
				{
					called++;
					Assert.Equal(pIndex, pageIndex);
					Assert.Equal(pSize, pageSize);
					return new PagedResponseModel<Models.Schedule>("test title", list, list.Count, pageIndex, pageSize);
				});

			var controller = new ScheduleController(service.Object, logger);

			var result = await controller.GetPaged(pIndex, pSize);
			var okor = Assert.IsAssignableFrom<OkObjectResult>(result);
			var prm = Assert.IsAssignableFrom<PagedResponseModel<ViewModel.ScheduleModel>>(okor.Value);

			Assert.NotNull(prm);
			Assert.Equal(1, called);
			Assert.NotEmpty(prm.Data);
			Assert.Equal(list.Count, prm.Data.Count());
			Assert.Equal(list.Count, prm.TotalCount);
			Assert.Equal(pIndex, prm.Page);
			Assert.Equal(pSize, prm.PageSize);
			Assert.Equal("Paged Results", prm.Title);

			for(var i = 0;i<list.Count;i++)
			{
				var expected = list[i];
				var actual = prm.Data.ElementAt(i);
				AssertEqual(expected, actual);
			}
		}

		[Fact]
		public async Task GetSchedulesAsync_TwoPageTest()
		{
			var service = new Mock<IScheduleService>();
			var logger = CreateLogger<ScheduleController>();

			var list = new List<Models.Schedule>();
			var pSize = 25;

			for(var i=0;i<pSize * 2;i++)
			{
				list.Add(GenerateSchedule(i));
			}

			var called = 0;
			var pIndex = 0;

			service.Setup(i => i.GetSchedulesAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync((int pageIndex, int pageSize) =>
				{
					called++;
					Assert.Equal(pIndex, pageIndex);
					Assert.Equal(pSize, pageSize);
					return new PagedResponseModel<Models.Schedule>("test title", list.Take(pageSize), list.Count, pageIndex, pageSize);
				});

			var controller = new ScheduleController(service.Object, logger);

			var result = await controller.GetPaged(pIndex, pSize);
			var okor = Assert.IsAssignableFrom<OkObjectResult>(result);
			var prm = Assert.IsAssignableFrom<PagedResponseModel<ViewModel.ScheduleModel>>(okor.Value);

			Assert.NotNull(prm);
			Assert.Equal(1, called);
			Assert.NotEmpty(prm.Data);
			Assert.Equal(list.Count/2, prm.Data.Count());
			Assert.Equal(list.Count, prm.TotalCount);
			Assert.Equal(pIndex, prm.Page);
			Assert.Equal(pSize, prm.PageSize);
			Assert.Equal("Paged Results", prm.Title);

			for(var i = 0;i<list.Count/2;i++)
			{
				var expected = list[i];
				var actual = prm.Data.ElementAt(i);
				AssertEqual(expected, actual);
			}
		}
	}
}
