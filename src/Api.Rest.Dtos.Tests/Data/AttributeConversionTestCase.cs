#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System;
	using Zeiss.PiWeb.Api.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Zeiss.PiWeb.Api.Contracts.Attribute;

	#endregion

	public class AttributeConversionTestCase
	{
		#region properties

		public string DisplayText { get; set; }

		public Attribute Attribute { get; set; }

		public int? ExpectedIntValue { get; set; }
		public double? ExpectedDoubleValue { get; set; }
		public string ExpectedStringValue { get; set; }
		public DateTime? ExpectedDateValue { get; set; }

		public object ExpectedRawValue { get; set; }

		#endregion

		#region methods

		public override string ToString()
		{
			return DisplayText;
		}

		#endregion
	}
}