using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportStream.Web.FeedCore;

namespace SportStream.Web.Controllers
{
	[Route("api/[controller]")]
	public class StreamSourceController : Controller
	{
		// GET api/values
		[HttpGet("rss")]
		//[ProducesResponseType(typeof(TodoItem), 201)] // Created
		//[ProducesResponseType(typeof(TodoItem), 400)] // BadRequest
		public async Task<IList<string>> Get([FromQuery] string q)
		{
			try
			{
				FeedParser parser = new FeedParser();
				var items = await parser.Parse("http://cdn.livetvcdn.net/rss/upcoming_it.xml", FeedType.RSS);

				var filteredItems = items
					.Where(item => item.Title.ToLower().Contains(q.ToLower()));

				var result = new List<string>();
			
				filteredItems.ToList().ForEach(async item =>
				{
					var page = await FollowLinkContent(item.Link);
					result.Add(page.Substring(1,10));
				});
				return result;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		private async Task<string> FollowLinkContent(string url)
		{
			using (var client = new HttpClient())
			{
				var response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();

				return responseBody;

			}
		}

		// GET api/values/5
		//[HttpGet("{id}")]
		//public string Get(int id)
		//{
		//	return "value";
		//}


	}
}
