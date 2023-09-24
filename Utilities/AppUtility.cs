
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using DataMashUp.DTO;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net;
using System.Net.Mail;

namespace DataMashUp.Utilities
{
	public static class AppUtility
	{
		public static void WriteToJosn(string PhoneAbsolutePath, string content)
		{
			//var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, filePath + ".json");
			File.WriteAllText(PhoneAbsolutePath, content);

		}


		public static async Task<bool> SendMail(EmailRequest emailRequest)
		{
			try
			{
				//var key1 = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
				var sender = Base64Decode("chidionoh2@gmail.com");
				var key = Base64Decode("U0cuaUF5OUE0QzNST09ob25UdXdMaHFLUS4wUjlENHVuUHRtbjI5Qkp0bTlENFp4M1FNMzcwSHBuS0JZdmJ4bXJvVnJV\r\n");
				using var message = new MailMessage();
				message.From = new MailAddress(sender, "chidionoh2@gmail.com");

				message.IsBodyHtml = true;
				message.To.Add(new MailAddress("", "Bounce Online"));
				message.Body = emailRequest.Body;

				message.Subject = emailRequest.Subject;
				if (emailRequest.Attachments.Count > 0)
				{
					foreach (var attachment in emailRequest.Attachments)
					{
						string fileName = Path.GetFileName(attachment.FileName);
						message.Attachments.Add(new System.Net.Mail.Attachment(attachment.OpenReadStream(), fileName));
					}
				}

				using var client = new SmtpClient(host: "smtp.sendgrid.net", port: 587);
				client.Credentials = new NetworkCredential(
					userName: "apikey",
					password: key
					);


				await client.SendMailAsync(message);
				return true;
			}
			catch (Exception ex)
			{

				var model = $"{JsonConvert.SerializeObject(emailRequest)}";
				var message = $"{"internal server occured while sending email"}{" - "}{ex}{" - "}{model}{DateTime.Now}";

				return false;
			}
		}



		public static void SendSMS(string phoneNumber, string Message)
		{


			string accountSid = "AC91a3922ce1b0014ea218ad44aea261a2";
			string authToken = "424474290bc86eaaf273a54f5d5edb4a\r\n";

			TwilioClient.Init(accountSid, authToken);

			var message = MessageResource.Create(
				body: "Testing ",
				from: new Twilio.Types.PhoneNumber("+447723487855"),
				to: new Twilio.Types.PhoneNumber(phoneNumber)
			);


		}

		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(base64EncodedBytes);

		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
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

		public static string FormatConfirmEmailTemplate(string url, string rootPath, string userName)
		{
			string templateRootPath = CombinePath(rootPath, "confirmEmailTemplate.html");
			string content = string.Empty;
			using var sr = new StreamReader(templateRootPath);
			content = sr.ReadToEnd();
			content = content.Replace("{{userName}}", userName);
			content = content.Replace("{{urlLink}}", url);
			return content;
		}
		private static string CombinePath(string rootpath, string name)
		{
			return Path.Combine(rootpath, "Static", name);
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
