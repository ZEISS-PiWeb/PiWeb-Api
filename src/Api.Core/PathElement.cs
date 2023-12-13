#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Core
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// This structure represents a single part of an inspection plan path. A path part has a <see cref="Value"/> and can
	/// either specify a characteristic or a part (<see cref="Type"/>).
	/// Notice that comparision of path elements is case insensitiv per default.
	/// </summary>
	/// <remarks>This class is immutable!</remarks>
	public readonly struct PathElement : IEquatable<PathElement>
	{
		#region members

		/// <summary>
		/// Constant value for an empty part.
		/// </summary>
		public static readonly PathElement EmptyPart = Part( "" );

		/// <summary>
		/// Constant value for an empty characteristic.
		/// </summary>
		public static readonly PathElement EmptyCharacteristic = Char( "" );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PathElement"/> class.
		/// </summary>
		public PathElement( InspectionPlanEntity type = InspectionPlanEntity.Part, string value = "" )
		{
			Type = type;
			Value = value ?? string.Empty;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gibt den Type des Pfadabschnittes zurück.
		/// </summary>
		public InspectionPlanEntity Type { get; }

		/// <summary>
		/// Gibt den Namen zurück.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Gibt an, ob diese <see cref="PathElement"/>-Instanz leer ist.
		/// </summary>
		public bool IsEmpty => Value.Length == 0;

		#endregion

		#region methods

		/// <summary>
		/// Creates a new path element with type <see cref="InspectionPlanEntity.Part"/>.
		/// </summary>
		public static PathElement Part( string name )
		{
			return new PathElement( InspectionPlanEntity.Part, name );
		}

		/// <summary>
		/// Creates a new path element with type <see cref="InspectionPlanEntity.Characteristic"/>.
		/// </summary>
		public static PathElement Char( string name )
		{
			return new PathElement( InspectionPlanEntity.Characteristic, name );
		}

		/// <summary>
		/// Equality operator. Path element are compared case insensitive.
		/// </summary>
		public static bool operator ==( PathElement p1, PathElement p2 )
		{
			return p1.Equals( p2 );
		}

		/// <summary>
		/// Inequality operator. Path element are compared case insensitive.
		/// </summary>
		public static bool operator !=( PathElement p1, PathElement p2 )
		{
			return !p1.Equals( p2 );
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return obj is PathElement other && Equals( other );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return StringComparer.OrdinalIgnoreCase.GetHashCode( Value );
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return Value;
		}

		#endregion

		#region interface IEquatable<PathElementDto>

		/// <inheritdoc />
		public bool Equals( PathElement other )
		{
			return Type == other.Type && string.Equals( Value, other.Value, StringComparison.OrdinalIgnoreCase );
		}

		#endregion
	}
}