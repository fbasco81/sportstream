﻿using SportStream.Web.FeedCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStream.Web.Core
{
	/// <summary>
	/// Represents a feed item.
	/// </summary>
	public class Item
	{
		public string Link { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime PublishDate { get; set; }
		public FeedType FeedType { get; set; }

		public Item()
		{
			Link = "";
			Title = "";
			Content = "";
			PublishDate = DateTime.Today;
			FeedType = FeedType.RSS;
		}
	}
}
