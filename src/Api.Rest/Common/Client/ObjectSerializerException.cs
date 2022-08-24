#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	using System;

	/// <summary>
	/// Represents errors that occur during de-/serialization.
	/// </summary>
	public sealed class ObjectSerializerException : Exception
	{
		/// <inheritdoc />
		public ObjectSerializerException( string message, Exception exception )
			: base( message, exception )
		{
		}
	}
}