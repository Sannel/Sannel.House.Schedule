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

using Microsoft.IdentityModel.Tokens;
using Moq;
using Sannel.House.Base.Models;
using Sannel.House.Base.MQTT.Interfaces;
using Sannel.House.Schedule.Interfaces;
using Sannel.House.Schedule.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sannel.House.Schedule.Tests.Services
{
	public class ScheduleServiceTests : BaseTests
	{
		[Fact]
		public async Task GetScheduleAsyncNullTest()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleService>();

			var repository = new Mock<IScheduleRepository>();

			var testId = Guid.NewGuid();
			var called = 0;
			repository.Setup(i => i.GetScheduleAsync(It.IsAny<Guid>()))
				.ReturnsAsync((Guid id) =>
				{
					called++;
					Assert.Equal(testId, id);
					return null;
				});


			var mqtt = new Mock<IMqttClientPublishService>();
			mqtt.Setup(i => i.Publish(It.IsAny<Object>()))
				.Throws(new Exception("Called to Publish"));

			mqtt.Setup(i => i.PublishAsync(It.IsAny<Object>()))
				.Throws(new Exception("Called to PublishAsync"));

			var service = new ScheduleService(repository.Object,
				mqtt.Object,
				logger);

			var result = await service.GetScheduleAsync(testId);
			Assert.Null(result);
			Assert.Equal(1, called);
		}

		[Fact]
		public async Task GetScheduleAsyncTest()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleService>();

			var repository = new Mock<IScheduleRepository>();

			var testId = Guid.NewGuid();
			var called = 0;
			var expectedSchedule = new Models.Schedule()
			{
				ScheduleId = Random.Next(),
				ScheduleKey = testId,
				Name = "Test 1",
				DefaultMaxValue = 100,
				DefaultMinValue = 1,
				MinimumDifference = 2
			};
			repository.Setup(i => i.GetScheduleAsync(It.IsAny<Guid>()))
				.ReturnsAsync((Guid id) =>
				{
					called++;
					Assert.Equal(testId, id);
					return expectedSchedule;
				});


			var mqtt = new Mock<IMqttClientPublishService>();
			mqtt.Setup(i => i.Publish(It.IsAny<Object>()))
				.Throws(new Exception("Called to Publish"));

			mqtt.Setup(i => i.PublishAsync(It.IsAny<Object>()))
				.Throws(new Exception("Called to PublishAsync"));

			var service = new ScheduleService(repository.Object,
				mqtt.Object,
				logger);

			var result = await service.GetScheduleAsync(testId);
			Assert.NotNull(result);
			Assert.Equal(1, called);
			Assert.Equal(expectedSchedule.ScheduleId, result.ScheduleId);
			Assert.Equal(expectedSchedule.ScheduleKey, result.ScheduleKey);
			Assert.Equal(expectedSchedule.Name, result.Name);
			Assert.Equal(expectedSchedule.DefaultMaxValue, result.DefaultMaxValue);
			Assert.Equal(expectedSchedule.DefaultMinValue, result.DefaultMinValue);
			Assert.Equal(expectedSchedule.MinimumDifference, result.MinimumDifference);
		}
	}
}
