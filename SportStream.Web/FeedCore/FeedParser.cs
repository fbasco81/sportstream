using SportStream.Web.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SportStream.Web.FeedCore
{
	/// <summary>
	/// A simple RSS, RDF and ATOM feed parser.
	/// </summary>
	public class FeedParser
	{
		/// <summary>
		/// Parses the given <see cref="FeedType"/> and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
		/// </summary>
		/// <returns></returns>
		public async Task< IList<Item>> Parse(string url, FeedType feedType)
		{
			switch (feedType)
			{
				case FeedType.RSS:
					return await ParseRss(url);
				case FeedType.RDF:
					return await ParseRdf(url);
				case FeedType.Atom:
					return await ParseAtom(url);
				default:
					throw new NotSupportedException(string.Format("{0} is not supported", feedType.ToString()));
			}
		}

		/// <summary>
		/// Parses an Atom feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
		/// </summary>
		public virtual async Task<IList<Item>> ParseAtom(string url)
		{
			try
			{
				var doc = await ReadResourceFromUrl(url);
				// Feed/Entry
				var entries = from item in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
											select new Item
											{
												FeedType = FeedType.Atom,
												Content = item.Elements().First(i => i.Name.LocalName == "content").Value,
												Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
												PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "published").Value),
												Title = item.Elements().First(i => i.Name.LocalName == "title").Value
											};
				return entries.ToList();
			}
			catch
			{
				return new List<Item>();
			}
		}

		/// <summary>
		/// Parses an RSS feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
		/// </summary>
		public virtual async Task<IList<Item>> ParseRss(string url)
		{
			
			try
			{
				var doc = await ReadResourceFromUrl(url);
				// RSS/Channel/item
				var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
											select new Item
											{
												FeedType = FeedType.RSS,
												Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
												Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
												PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value),
												Title = item.Elements().First(i => i.Name.LocalName == "title").Value
											};
				return entries.ToList();

			}
			catch (HttpRequestException e)
			{
				Console.WriteLine("\nException Caught!");
				Console.WriteLine("Message :{0} ", e.Message);
			}

			// Need to call dispose on the HttpClient object
			// when done using it, so the app doesn't leak resources
			return null;



		}

		private async Task<XDocument> ReadResourceFromUrl(string url)
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

		/// <summary>
		/// Parses an RDF feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
		/// </summary>
		public virtual async Task<IList<Item>> ParseRdf(string url)
		{
			try
			{
				var doc = await ReadResourceFromUrl(url);
				// <item> is under the root
				var entries = from item in doc.Root.Descendants().Where(i => i.Name.LocalName == "item")
											select new Item
											{
												FeedType = FeedType.RDF,
												Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
												Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
												PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "date").Value),
												Title = item.Elements().First(i => i.Name.LocalName == "title").Value
											};
				return entries.ToList();
			}
			catch
			{
				return new List<Item>();
			}
		}

		private DateTime ParseDate(string date)
		{
			DateTime result;
			if (DateTime.TryParse(date, out result))
				return result;
			else
				return DateTime.MinValue;
		}
	}
	/// <summary>
	/// Represents the XML format of a feed.
	/// </summary>
	public enum FeedType
	{
		/// <summary>
		/// Really Simple Syndication format.
		/// </summary>
		RSS,
		/// <summary>
		/// RDF site summary format.
		/// </summary>
		RDF,
		/// <summary>
		/// Atom Syndication format.
		/// </summary>
		Atom
	}
}
