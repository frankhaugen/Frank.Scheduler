using System.Collections.Generic;
using System.Xml.Serialization;

namespace scheduled_job
{
	[XmlRoot(ElementName = "rss")]
	public class Rss
	{
		[XmlElement(ElementName = "channel")]
		public Channel Channel { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "media", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Media { get; set; }
	}

	[XmlRoot(ElementName = "image")]
	public class Image
	{
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "url")]
		public string Url { get; set; }
		[XmlElement(ElementName = "link")]
		public string Link { get; set; }
	}

	[XmlRoot(ElementName = "content", Namespace = "http://search.yahoo.com/mrss/")]
	public class Content
	{
		[XmlAttribute(AttributeName = "url")]
		public string Url { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
	}

	[XmlRoot(ElementName = "item")]
	public class Item
	{
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "link")]
		public string Link { get; set; }
		[XmlElement(ElementName = "description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "pubDate")]
		public string PubDate { get; set; }
		[XmlElement(ElementName = "guid")]
		public string Guid { get; set; }
		[XmlElement(ElementName = "content", Namespace = "http://search.yahoo.com/mrss/")]
		public Content Content { get; set; }
	}

	[XmlRoot(ElementName = "channel")]
	public class Channel
	{
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "link")]
		public string Link { get; set; }
		[XmlElement(ElementName = "description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "image")]
		public Image Image { get; set; }
		[XmlElement(ElementName = "item")]
		public List<Item> Items { get; set; }
	}
}