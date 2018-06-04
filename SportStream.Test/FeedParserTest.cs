using SportStream.Web.FeedCore;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace SportStream.Test
{
	public class UnitTest1
	{
		[Fact]
		public async void GivenAValidFeedUrl_ReturnListOfFeed()
		{
			var parser = new FeedParser();
			var resourceReader = new Moq.Mock<IResourceReader>();
			resourceReader.Setup((rr) => rr.GetAsync())
				.Returns(Task.FromResult<XDocument>(ValidFeedXml()));

			var items = await parser.Parse(resourceReader.Object, FeedType.RSS);

			Assert.Equal(3, items.Count);
		}

		private XDocument ValidFeedXml()
		{
			return XDocument.Parse(@"<?xml version=""1.0"" encoding=""windows-1252""?>
<rss version=""2.0"">
  <channel>
    <title>Sample Feed - Favorite RSS Related Software &amp; Resources</title>
    <description>Take a look at some of FeedForAll&apos;s favorite software and resources for learning more about RSS.</description>
    <link>http://www.feedforall.com</link>
    <category domain=""www.dmoz.com"">Computers/Software/Internet/Site Management/Content Management</category>
    <copyright>Copyright 2004 NotePage, Inc.</copyright>
    <docs>http://blogs.law.harvard.edu/tech/rss</docs>
    <language>en-us</language>
    <lastBuildDate>Mon, 1 Nov 2004 13:17:17 -0500</lastBuildDate>
    <managingEditor>marketing@feedforall.com</managingEditor>
    <pubDate>Tue, 26 Oct 2004 14:06:44 -0500</pubDate>
    <webMaster>webmaster@feedforall.com</webMaster>
    <generator>FeedForAll Beta1 (0.0.1.8)</generator>
    <image>
      <url>http://www.feedforall.com/feedforall-temp.gif</url>
      <title>FeedForAll Sample Feed</title>
      <link>http://www.feedforall.com/industry-solutions.htm</link>
      <description>FeedForAll Sample Feed</description>
      <width>144</width>
      <height>117</height>
    </image>
    <item>
      <title>RSS Resources</title>
      <description>Be sure to take a look at some of our favorite RSS Resources&lt;br&gt;
&lt;a href=&quot;http://www.rss-specifications.com&quot;&gt;RSS Specifications&lt;/a&gt;&lt;br&gt;
&lt;a href=&quot;http://www.blog-connection.com&quot;&gt;Blog Connection&lt;/a&gt;&lt;br&gt;
&lt;br&gt;</description>
      <link>http://www.feedforall.com</link>
      <pubDate>Tue, 26 Oct 2004 14:01:01 -0500</pubDate>
    </item>
    <item>
      <title>Recommended Desktop Feed Reader Software</title>
      <description>&lt;b&gt;FeedDemon&lt;/b&gt; enables you to quickly read and gather information from hundreds of web sites - without having to visit them. Don&apos;t waste any more time checking your favorite web sites for updates. Instead, use FeedDemon and make them come to you. &lt;br&gt;
More &lt;a href=&quot;http://store.esellerate.net/a.asp?c=1_SKU5139890208_AFL403073819&quot;&gt;FeedDemon Information&lt;/a&gt;</description>
      <link>http://www.feedforall.com/feedforall-partners.htm</link>
      <pubDate>Tue, 26 Oct 2004 14:03:25 -0500</pubDate>
    </item>
    <item>
      <title>Recommended Web Based Feed Reader Software</title>
      <description>&lt;b&gt;FeedScout&lt;/b&gt; enables you to view RSS/ATOM/RDF feeds from different sites directly in Internet Explorer. You can even set your Home Page to show favorite feeds. Feed Scout is a plug-in for Internet Explorer, so you won&apos;t have to learn anything except for how to press 2 new buttons on Internet Explorer toolbar. &lt;br&gt;
More &lt;a href=&quot;http://www.bytescout.com/feedscout.html&quot;&gt;Information on FeedScout&lt;/a&gt;&lt;br&gt;
&lt;br&gt;
&lt;br&gt;
&lt;b&gt;SurfPack&lt;/b&gt; can feature search tools, horoscopes, current weather conditions, LiveJournal diaries, humor, web modules and other dynamically updated content. &lt;br&gt;
More &lt;a href=&quot;http://www.surfpack.com/&quot;&gt;Information on SurfPack&lt;/a&gt;&lt;br&gt;</description>
      <link>http://www.feedforall.com/feedforall-partners.htm</link>
      <pubDate>Tue, 26 Oct 2004 14:06:44 -0500</pubDate>
    </item>
  </channel>
</rss>
			");
		}
	}
}
