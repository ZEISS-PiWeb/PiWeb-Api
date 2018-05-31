#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Utilities
{
	#region usings

	using System;
	using System.Net;
	using PiWebApi.Annotations;

	#endregion

	/// <summary>
	/// Credential that contains username / Password information.
	/// </summary>
	public sealed class UsernamePasswordCredential : ICredential
	{
		#region constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public UsernamePasswordCredential( [NotNull] string username, [NotNull] string password )
		{
			Username = username ?? throw new ArgumentNullException( nameof( username ) );
			Password = password ?? throw new ArgumentNullException( nameof( password ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// Return the username.
		/// </summary>
		[NotNull]
		public string Username { get; }

		/// <summary>
		/// Returns the password.
		/// </summary>
		[NotNull]
		public string Password { get; }

		#endregion

		#region methods

		public static UsernamePasswordCredential CreateFromNetworkCredential( NetworkCredential credential )
		{
			if( credential == null ) return null;

			return new UsernamePasswordCredential( credential.UserName, credential.Password );
		}

		public NetworkCredential ToNetworkCredential() => new NetworkCredential( Username, Password );

		public bool Equals( UsernamePasswordCredential other )
		{
			if( ReferenceEquals( this, other ) ) return true;

			return other != null
			       && string.Equals( Username, other.Username )
			       && string.Equals( Password, other.Password );
		}

		public override bool Equals( object other ) => Equals( other as UsernamePasswordCredential );

		public override int GetHashCode()
		{
			unchecked
			{
				return ( Username.GetHashCode() * 397 ) ^ Password.GetHashCode();
			}
		}

		#endregion

		#region interface ICredential

		/// <summary>
		/// Return a text that can be used for displaying.
		/// </summary>
		public string DisplayId => Username;

		public bool Equals( ICredential other ) => Equals( other as UsernamePasswordCredential );

		#endregion
	}
}