
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using DataMashUp.DTO;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DataMashUp.Utilities
{
	public static class AppUtility
	{
		public static void WriteToJosn(string PhoneAbsolutePath, string content)
		{
			//var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, filePath + ".json");
			File.WriteAllText(PhoneAbsolutePath, content);

		}

		public static async Task<T> ReadJosnAsync<T> ( string PhoneAbsolutePath) where T: class
		{
			//var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"Static/{fileName}.json");
			using (var fs = System.IO.File.OpenText(PhoneAbsolutePath))
			{
				var result = JsonConvert.DeserializeObject<T>(await fs.ReadToEndAsync());
				return result;
			}
		}

		public static List<string> GetExcercise(ExerciseRecommendations ageRange, string age)
		{
			var groups = ageRange.age_groups.Select(x => x.age_range).ToList();
			var result = GetAgeGroup(ageRange: groups, age);
			var excercise = ageRange.age_groups.Find(x => x.age_range.Contains(result))?.exercises;
			if(excercise != null)
			{
				var items =  PickRandomItems(excercise, 3);
				return items;
			}
			else
			{
				return new List<string>();
			}
		}
		public static string GetAgeGroup(List<string> ageRange, string age)
		{
			var _age = int.Parse(age);
			if(_age <= 19)
			{
				return "15-19";
			}

			if(_age >= 80)
			{
				return "80-100";
			}


			List<AgeGroup> ageGroups = new List<AgeGroup>();
			foreach (var item in ageRange)
			{
				var range = item.Split('-').ToArray();

				var record = new AgeGroup
				{
					AgeText = item,
					StartAge = int.Parse(range[0]),
					EndAge = int.Parse(range[1]),
				};
				
			
				var difference = record.EndAge - record.StartAge;
				int startAge = record.StartAge;
				var memberText = new StringBuilder();
				for (int i = 0; i <= difference; i++)
				{
					 startAge = record.StartAge + i;
					record.Members.Add(startAge);
					memberText.Append(startAge.ToString() + ",");
					startAge = record.StartAge;
					ageGroups.Add(record);
				}
				record.MemberText = memberText.ToString();

			}

			var result = ageGroups.FirstOrDefault(x => x.MemberText.Contains(age));
			if(result != null)
			{
				return result.AgeText;
			}
			return "";
		}

		public static List<T> PickRandomItems<T>(List<T> sourceList, int count)
		{
			if (count >= sourceList.Count)
			{
				return sourceList;
			}

			List<T> resultList = new List<T>(sourceList);

			Random rng = new Random();

			int n = resultList.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = resultList[k];
				resultList[k] = resultList[n];
				resultList[n] = value;
			}

			return resultList.Take(count).ToList();
		}

	}
}
