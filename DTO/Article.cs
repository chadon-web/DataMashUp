namespace DataMashUp.DTO
{
	public class Article
	{
		public Source Source { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public string UrlToImage { get; set; }
		public DateTime PublishedAt { get; set; }
		public object Content { get; set; }
	}
}
