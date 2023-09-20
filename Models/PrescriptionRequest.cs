using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataMashUp.Models
{
	public class PrescriptionRequest
	{
		[Key]
		public long Id { get; set; }
		public long? UserId { get; set; }

        public string Name { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;

		public int Age { get; set; }

		public double Weight { get; set; }

		public double Height { get; set; }

		public double BMI { get; set; }

        public string Prescription { get; set; } = string.Empty;
		public string WhatToCosume { get; set; } = string.Empty;
		public string WhatToAvoid { get; set; } = string.Empty;

		public DateTime Date { get; set; } = DateTime.Now;
        public string Location { get; set; } = string.Empty;
	}
}
