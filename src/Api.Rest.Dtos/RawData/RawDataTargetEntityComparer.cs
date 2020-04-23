#region Copyright

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
	/// <see cref="IEqualityComparer{T}"/> zum Vergleich zweier <see cref="RawDataTargetEntity"/>-Objekte.
	/// </summary>
	public class RawDataTargetEntityComparer : IEqualityComparer<RawDataTargetEntity>
	{
		#region members

		/// <summary>
		/// Eine Instanz dieses Objekts.
		/// </summary>
		public static readonly RawDataTargetEntityComparer Default = new RawDataTargetEntityComparer();

		#endregion

		#region interface IEqualityComparer<RawDataTargetEntity>

		/// <inheritdoc />
		public bool Equals( RawDataTargetEntity x, RawDataTargetEntity y )
		{
			if( x == null && y == null || ReferenceEquals( x, y ) )
				return true;

			if( x == null || y == null )
				return false;

			return x.Entity == y.Entity && x.Uuid == y.Uuid;
		}

		/// <inheritdoc />
		public int GetHashCode( RawDataTargetEntity obj )
		{
			return obj.Entity.GetHashCode() ^ ( obj.Uuid ?? "" ).GetHashCode();
		}

		#endregion
	}
}