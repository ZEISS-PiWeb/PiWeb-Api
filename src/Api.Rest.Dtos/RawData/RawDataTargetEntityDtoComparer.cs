#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// <see cref="IEqualityComparer{T}"/> zum Vergleich zweier <see cref="RawDataTargetEntityDto"/>-Objekte.
	/// </summary>
	public class RawDataTargetEntityDtoComparer : IEqualityComparer<RawDataTargetEntityDto>
	{
		#region members

		/// <summary>
		/// Eine Instanz dieses Objekts.
		/// </summary>
		public static readonly RawDataTargetEntityDtoComparer Default = new RawDataTargetEntityDtoComparer();

		#endregion

		#region interface IEqualityComparer<RawDataTargetEntityDto>

		/// <inheritdoc />
		public bool Equals( RawDataTargetEntityDto x, RawDataTargetEntityDto y )
		{
			if( x == null && y == null || ReferenceEquals( x, y ) )
				return true;

			if( x == null || y == null )
				return false;

			return x.Entity == y.Entity && x.Uuid == y.Uuid;
		}

		/// <inheritdoc />
		public int GetHashCode( RawDataTargetEntityDto obj )
		{
			return obj.Entity.GetHashCode() ^ ( obj.Uuid ?? "" ).GetHashCode();
		}

		#endregion
	}
}