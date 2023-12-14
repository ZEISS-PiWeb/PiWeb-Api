#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using Zeiss.PiWeb.Api.Core;

	#endregion

	/// <summary>
	/// Extension class with various helper methods for attribute list manipulation.
	/// </summary>
	public static class AttributeItemExtensions
	{
		#region methods

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <paramref name="key"/> If the attribute consists of a catalog entry the entry
		/// is returned, otherwise the attribute's value (string, int, double or DateTime) is returned.
		/// </summary>
		public static object GetRawAttributeValue(
			this IAttributeItem item,
			ushort key,
			ConfigurationDto configuration,
			CatalogCollectionDto catalogs )
		{
			var attribute = item.GetAttribute( key );
			if( attribute == null )
				return null;

			if( attribute.Value.RawValue != null )
				return attribute.Value.RawValue;

			return configuration.ParseValue( key, attribute.Value.Value, catalogs );
		}

		#endregion
	}
}