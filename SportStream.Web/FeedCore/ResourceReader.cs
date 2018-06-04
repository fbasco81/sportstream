using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SportStream.Web.FeedCore
{
	public class ResourceReader  : IResourceReader
	{
		private readonly string url;

		public ResourceReader(string url) {
			this.url = url;
		}
		public async Task<XDocument> GetAsync()
		{
			using (var client = new HttpClient())
			{
				var response = await client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();

				responseBody = responseBody.Replace("&ugrave", "u").Replace("ugrave", "");

				return XDocument.Parse(responseBody);

			}
		}
	}
}