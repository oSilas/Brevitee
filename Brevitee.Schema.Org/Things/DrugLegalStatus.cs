using System;

namespace Brevitee.Schema.Org
{
	///<summary>The legal availability status of a medical drug.</summary>
	public class DrugLegalStatus: MedicalIntangible
	{
		///<summary>The location in which the status applies.</summary>
		public AdministrativeArea ApplicableLocation {get; set;}
	}
}
