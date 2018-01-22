#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
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
		/// <param name="e1">Das erste zu vergleichende Objekt vom Typ <see cref="RawDataTargetEntity"/>.</param>
		/// <param name="e2">Das zweite zu vergleichende Objekt vom Typ <see cref="RawDataTargetEntity"/>.</param>
		public bool Equals( RawDataTargetEntity e1, RawDataTargetEntity e2 )
		{
			if( ( e1 == null ) && ( e2 == null ) || ReferenceEquals( e1, e2 ) )
				return true;

			if( e1 == null || e2 == null )
				return false;

			return ( e1.Entity == e2.Entity && e1.Uuid == e2.Uuid );
		}

		/// <summary>
		///     Gibt einen Hashcode für das angegebene Objekt zurück.
		/// </summary>
		/// <returns>
		///     Ein Hashcode für das angegebene Objekt.
		/// </returns>
		/// <param name="e">Das <see cref="T:RawDataTargetEntity" />, für das ein Hashcode zurückgegeben werden soll.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///     Der Typ von <paramref name="e" /> ist ein Referenztyp, und
		///     <paramref name="e" /> ist null.
		/// </exception>
		public int GetHashCode( RawDataTargetEntity e )
		{
			return e.Entity.GetHashCode() ^ ( e.Uuid ?? "" ).GetHashCode();
		}

		#endregion
	}
}
