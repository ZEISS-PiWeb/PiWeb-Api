#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using JetBrains.Annotations;

	#endregion

	public sealed class Payload
	{
		#region members

		public static readonly Payload Empty = new Payload { Value = null };

		#endregion

		#region constructors

		/// <summary>
		/// Prevents a default instance of the <see cref="Payload"/> class from being created.
		/// </summary>
		private Payload()
		{ }

		#endregion

		#region properties

		public object Value { get; private set; }

		#endregion

		#region methods

		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null" />.</exception>
		public static Payload Create( [NotNull] object value )
		{
			if( value == null ) throw new ArgumentNullException( nameof( value ) );

			return new Payload { Value = value };
		}

		#endregion
	}
}