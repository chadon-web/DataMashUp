
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

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
