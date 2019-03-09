using CronQuery.Mvc.Jobs;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace scheduled_job
{
	public class ScheduledJob : IJob
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly string _feed = "http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master";

		public ScheduledJob(IHttpClientFactory httpClientFactory)
		{
			_clientFactory = httpClientFactory;
		}

		public async Task RunAsync()
		{
			using (var result = await _clientFactory.CreateClient().GetAsync(_feed))
			{
				var xmlSerializer = new XmlSerializer(typeof(Rss));

				var rssFeedObject = xmlSerializer.Deserialize(await result.Content.ReadAsStreamAsync()) as Rss;

				Console.WriteLine($"This is the podcast {rssFeedObject.Channel.Title}.\n\tThey have {rssFeedObject.Channel.Items.Count} episodes.\n\tThe latest episode is: {rssFeedObject.Channel.Items[0].Title}");
			}
		}
	}
}
