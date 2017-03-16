using System;
using System.IO;
using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace s3CSharpIntz
{
	class MainClass
	{
		public struct Application
		{
			[JsonProperty(PropertyName = "First name")]
			public string first_name;
			[JsonProperty(PropertyName = "Last name")]
			public string last_name;
			[JsonProperty(PropertyName = "Salary requested in $thousands/year")]
			public float salary_per_year;
			[JsonProperty(PropertyName = "I physically reside in the USA")]
			public bool reside_in_usa;
			[JsonProperty(PropertyName = "My current residence (US state code)")]
			public string state_code;
			[JsonProperty(PropertyName = "I'm seeking full time employment")]
			public bool full_time;
			[JsonProperty(PropertyName = "I'm seeking part time or contract work")]
			public bool part_time;
			[JsonProperty(PropertyName = "I am authorized to work in the U.S.")]
			public bool auth_work_in_usa;
			[JsonProperty(PropertyName = "My github URL")]
			public string github_url;
			[JsonProperty(PropertyName = "My linkedin URL")]
			public string linkedin_url;
			[JsonProperty(PropertyName = "My stackoverflow URL")]
			public string stackoverflow_url;
			[JsonProperty(PropertyName = "Other personal or professional URL 1")]
			public string other_url;
			[JsonProperty(PropertyName = "Other personal or professional URL 2")]
			public string other_url2;
			[JsonProperty(PropertyName = "Available to work date")]
			public DateTime start_date;
			[JsonProperty(PropertyName = "This survey completed date")]
			public DateTime today;
			[JsonProperty(PropertyName = "Email")]
			public string email;
			[JsonProperty(PropertyName = "USA contact phone #")]
			public string phone_number;
			[JsonProperty(PropertyName = "Other comments for us!")]
			public string other_comments;
		}

		public static void Main(string[] args)
		{
			Console.WriteLine("This app will walk you through building a json object for the Inteligenz interview process.");
			Console.WriteLine("Please enter the requested information when prompted");
			var app = new Application();
			Console.WriteLine("First name:");
			app.first_name = Console.ReadLine();
			Console.WriteLine("Last name: ");
			app.last_name = Console.ReadLine();
			Console.WriteLine("Salary per year: ");
			var sal = float.Parse(Console.ReadLine());
			app.salary_per_year = sal;
			Console.WriteLine("Reside in the USA? ");
			app.reside_in_usa = bool.Parse(Console.ReadLine());
			Console.WriteLine("State Code");
			app.state_code = Console.ReadLine();
			Console.WriteLine("Looking for FullTime work?");
			app.full_time = bool.Parse(Console.ReadLine());
			Console.WriteLine("Part Time/Contract?");
			app.part_time = bool.Parse(Console.ReadLine());
			Console.WriteLine("Authorized to work in the US?");
			app.auth_work_in_usa = bool.Parse(Console.ReadLine());
			Console.WriteLine("Github URL");
			app.github_url = Console.ReadLine();
			Console.WriteLine("Linkedin URL");
			app.linkedin_url = Console.ReadLine();
			Console.WriteLine("StackOverflow URL?");
			app.stackoverflow_url = Console.ReadLine();
			Console.WriteLine("Other professional URL");
			app.other_url = Console.ReadLine();
			Console.WriteLine("Other Professional URL 2");
			app.other_url2 = Console.ReadLine();
			Console.WriteLine("Date available to start (assuming offer today)");
			app.start_date = DateTime.Parse(Console.ReadLine());
			app.today = DateTime.Now;
			Console.WriteLine("Email?");
			app.email = Console.ReadLine();
			Console.WriteLine("Phone number");
			app.phone_number = Console.ReadLine();
			Console.WriteLine("Other comments?");
			app.other_comments = Console.ReadLine();
			var json = JsonConvert.SerializeObject(app);
			Console.WriteLine(json);
			Console.WriteLine("Look correct to you? If so hit any key else hit <ctrl> + c and rerun the app");
			Console.ReadLine();
			var keyName = "Intz17a - R1.02 - " + app.last_name + ", " + app.first_name + " - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + ".json";
			File.WriteAllText(@"./"+ keyName, json);
			if (sendToS3(@"./" + keyName , "intz-dev-responses", keyName))
			{
				Console.WriteLine("Success!");
			}
		}

		public static Boolean sendToS3(string filePath, string bucketName, string keyName)
		{
			var aws = new AmazonS3Client("Access_token_here", "Secert_here", RegionEndpoint.USWest2);
			try
			{
				var request = new PutObjectRequest()
				{
					BucketName = bucketName,
					Key = keyName,
					FilePath = filePath,
					ContentType = "text/json"
				};

				PutObjectResponse response1 = aws.PutObject(request);
			}
			catch (AmazonS3Exception ex)
			{
				if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKey") || ex.ErrorCode.Equals("InvalidSecurity")))
				{
					throw new Exception("Please verify the creditionals provided for AWS");
				}
				else
				{
					throw new Exception("Error occured: " + ex.Message);	
				}
			}
			return true;
		}
	}
}
