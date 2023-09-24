using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DataMashUp.DTO
{
	public class RegisterDto
	{
		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string? Password { get; set; }
		[Required]
		[Compare(nameof(Password))]
		[DataType(DataType.Password)]
		public string? ConfirmPassword { get; set; }

	
	}
}
