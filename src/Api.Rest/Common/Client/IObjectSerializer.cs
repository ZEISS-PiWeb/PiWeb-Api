#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	using System.IO;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods to support de-/serialization of objects.
	/// </summary>
	public interface IObjectSerializer
	{
		/// <summary>
		/// Writes the provided value to the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <exception cref="ObjectSerializerException">Error during deserialization.</exception>
		void Serialize<T>( [NotNull] Stream stream, T value );

		/// <summary>
		/// Reads the value from the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to read from.</param>
		/// <exception cref="ObjectSerializerException">Error during serialization.</exception>
		T Deserialize<T>( [NotNull] Stream stream );
	}
}