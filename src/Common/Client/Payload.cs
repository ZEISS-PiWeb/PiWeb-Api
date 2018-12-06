#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;

	#endregion

	public sealed class Payload
	{
		#region members

		public static readonly Payload Empty = new Payload { Value = null };

		#endregion

		#region constructors

		private Payload()
		{
		}

		#endregion

		#region properties

		public object Value { get; private set; }

		#endregion

		#region methods

		public static Payload Create( object value )
		{
			if( value == null ) throw new ArgumentNullException( nameof(value) );

			return new Payload { Value = value };
		}

		#endregion
	}
}