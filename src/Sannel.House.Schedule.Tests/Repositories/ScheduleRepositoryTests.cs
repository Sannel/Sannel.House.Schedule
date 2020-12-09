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

using Sannel.House.Schedule.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sannel.House.Schedule.Tests.Repositories
{
	public class ScheduleRepositoryTests : BaseTests
	{
		private void Equals(Models.Schedule expected, Models.Schedule actual)
		{
			if(expected is null)
			{
				Assert.Null(actual);
			}

			Assert.NotNull(actual);
			Assert.Equal(expected.Name, actual.Name);
			Assert.Equal(expected.ScheduleKey, actual.ScheduleKey);
			Assert.Equal(expected.DefaultMaxValue, actual.DefaultMaxValue);
			Assert.Equal(expected.DefaultMinValue, actual.DefaultMinValue);
			Assert.Equal(expected.MinimumDifference, actual.MinimumDifference);
		}

		[Fact]
		public async Task GetUnknownScheduleKeyTestAsync()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleRepository>();

			var repository = new ScheduleRepository(context, logger);

			var result = await repository.GetScheduleAsync(Guid.Empty);

			Assert.Null(result);
		}

		[Fact]
		public async Task GetKnownScheduleKeyTestAsync()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleRepository>();

			var schedule = new Models.Schedule()
			{
				ScheduleKey = Guid.NewGuid(),
				DefaultMaxValue = 100,
				DefaultMinValue = 1,
				MinimumDifference = 5,
				Name = "Test Name"
			};

			await context.Schedules.AddAsync(schedule);
			await context.SaveChangesAsync();

			var repository = new ScheduleRepository(context, logger);

			var result = await repository.GetScheduleAsync(schedule.ScheduleKey);

			Assert.NotNull(result);
			Equals(schedule, result);
		}

		[Fact]
		public async Task GetSetSchedulesKeyTestAsync()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleRepository>();


			var repository = new ScheduleRepository(context, logger);

			var result = await repository.GetSchedulesAsync(0, 10);
			Assert.NotNull(result);
			Assert.Equal(0, result.Page);
			Assert.Equal(10, result.PageSize);
			Assert.Equal(0, result.TotalCount);
			Assert.Empty(result.Data);

			var items = new List<Models.Schedule>();

			for(var i=0;i<100;i++)
			{
				var schedule = new Models.Schedule()
				{
					ScheduleKey = Guid.NewGuid(),
					DefaultMaxValue = Random.Next(50,100),
					DefaultMinValue = Random.Next(1,40),
					MinimumDifference = Random.Next(1,10),
					Name = $"Test Name {i}"
				};

				await context.AddAsync(schedule);
				items.Add(schedule);
				await context.SaveChangesAsync();
			}


			result = await repository.GetSchedulesAsync(0, 10);
			Assert.NotNull(result);
			Assert.Equal(0, result.Page);
			Assert.Equal(10, result.PageSize);
			Assert.Equal(100, result.TotalCount);
			Assert.Equal(10, result.Data.Count());

			var currentIndex = 0;
			foreach(var actual in result.Data)
			{
				var expected = items[currentIndex++];
				Equals(expected, actual);
			}

			result = await repository.GetSchedulesAsync(1, 10);
			Assert.NotNull(result);
			Assert.Equal(1, result.Page);
			Assert.Equal(10, result.PageSize);
			Assert.Equal(100, result.TotalCount);
			Assert.Equal(10, result.Data.Count());

			currentIndex = 10;
			foreach(var actual in result.Data)
			{
				var expected = items[currentIndex++];
				Equals(expected, actual);
			}

			result = await repository.GetSchedulesAsync(3, 15);
			Assert.NotNull(result);
			Assert.Equal(3, result.Page);
			Assert.Equal(15, result.PageSize);
			Assert.Equal(100, result.TotalCount);
			Assert.Equal(15, result.Data.Count());

			currentIndex = 45;
			foreach(var actual in result.Data)
			{
				var expected = items[currentIndex++];
				Equals(expected, actual);
			}
		}

		[Fact]
		public async Task ExistsAsync_Test()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleRepository>();

			var repository = new ScheduleRepository(context, logger);

			var items = new List<Models.Schedule>();

			for(var i=0;i<100;i++)
			{
				var schedule = new Models.Schedule()
				{
					ScheduleKey = Guid.NewGuid(),
					DefaultMaxValue = Random.Next(50,100),
					DefaultMinValue = Random.Next(1,40),
					MinimumDifference = Random.Next(1,10),
					Name = $"Test Name {i}"
				};

				await context.AddAsync(schedule);
				items.Add(schedule);
				await context.SaveChangesAsync();
			}

			var index = Random.Next(0, items.Count);

			var item = items[index];

			Assert.False(await repository.ExistsAsync(Guid.NewGuid()));
			Assert.True(await repository.ExistsAsync(item.ScheduleKey));

		}

		[Fact]
		public async Task AddScheduleAsync_Test()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleRepository>();

			var repository = new ScheduleRepository(context, logger);

			var schedule = new Models.Schedule()
			{
				ScheduleKey = Guid.NewGuid(),
				DefaultMaxValue = Random.Next(60, 2000),
				DefaultMinValue = Random.Next(0, 50),
				MinimumDifference = Random.Next(0, 5),
				Name = $"Random {Guid.NewGuid()}"
			};

			var result = await repository.AddScheduleAsync(schedule);
			Assert.NotNull(result);
			Assert.Single(context.Schedules);
			var first = context.Schedules.FirstOrDefault();
			Equals(schedule, first);
		}
		[Fact]
		public async Task AddScheduleAsync_NullTest()
		{
			using var context = CreateTestDB();
			var logger = CreateLogger<ScheduleRepository>();

			var repository = new ScheduleRepository(context, logger);

			var result = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddScheduleAsync(null));
			Assert.Equal("schedule", result.ParamName);
		}
	}
}
