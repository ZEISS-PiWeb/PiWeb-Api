#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region using

	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// <see cref="IEqualityComparer&lt;T&gt;"/> zum Vergleich zweier <see cref="RawDataTargetEntity"/>-Objekte.
	/// </summary>
	public class RawDataTargetEntityComparer : IEqualityComparer<RawDataTargetEntity>
	{
		/// <summary>
		/// Eine Instanz dieses Objekts.
		/// </summary>
		public static readonly RawDataTargetEntityComparer Default = new RawDataTargetEntityComparer();

		#region interface IEqualityComparer<RawDataTargetEntity>

		/// <summary>
		/// Bestimmt, ob die angegebenen Objekte gleich sind.
		/// </summary>
		/// <returns>
		/// true, wenn die angegebenen Objekte gleich sind, andernfalls false.
		/// </returns>
		/// <param name="x">Das erste zu vergleichende Objekt vom Typ <see cref="RawDataTargetEntity"/>.</param>
		/// <param name="y">Das zweite zu vergleichende Objekt vom Typ <see cref="RawDataTargetEntity"/>.</param>
		public bool Equals( RawDataTargetEntity x, RawDataTargetEntity y )
		{
			if( ( x == null ) && ( y == null ) || ReferenceEquals( x, y ) )
				return true;

			if( x == null || y == null )
				return false;

			return ( x.Entity == y.Entity && x.Uuid == y.Uuid );
		}

		/// <summary>
		///     Gibt einen Hashcode für das angegebene Objekt zurück.
		/// </summary>
		/// <returns>
		///     Ein Hashcode für das angegebene Objekt.
		/// </returns>
		/// <param name="obj">Das <see cref="T:RawDataTargetEntity" />, für das ein Hashcode zurückgegeben werden soll.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///     Der Typ von <paramref name="obj" /> ist ein Referenztyp, und
		///     <paramref name="obj" /> ist null.
		/// </exception>
		public int GetHashCode( RawDataTargetEntity obj )
		{
			return obj.Entity.GetHashCode() ^ ( obj.Uuid ?? "" ).GetHashCode();
		}

		#endregion
	}
}
