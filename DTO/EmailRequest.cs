namespace DataMashUp.DTO
{
	public class EmailRequest
	{

		public EmailRequest()
		{
			Attachments = new List<IFormFile>();
		}
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public List<IFormFile> Attachments { get; set; }
	}

}
