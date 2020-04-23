﻿#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region using

	using System;

	#endregion

	/// <summary>
	/// This class represents a single part of an inspection plan path. A path part has a <see cref="Value"/> and can either specify a characteristic or a part (<see cref="Type"/>).
	/// Notice that comparision of path elements is case insensitiv per default.
	/// </summary>
	/// <remarks>This class is immutable!</remarks>
	public sealed class PathElement : IEquatable<PathElement>
	{
		#region constants

		/// <summary>
		/// Constant value for an empty part.
		/// </summary>
		public static readonly PathElement EmptyPart = PathElement.Part( "" );

		/// <summary>
		/// Constant value for an empty characteristic.
		/// </summary>
		public static readonly PathElement EmptyCharacteristic = PathElement.Char( "" );

		#endregion

		#region members

		private int _HashCode = -1;

		#endregion

		#region constructors

		/// <summary>
		/// Constructor.
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
			return Equals( p1, p2 );
		}

		/// <summary>
		/// Inequality operator. Path element are compared case insensitive.
		/// </summary>
		public static bool operator !=( PathElement p1, PathElement p2 )
		{
			return !Equals( p1, p2 );
		}

		/// <summary>
		/// Overridden <see cref="System.Object.Equals(object)"/> method. Path element are compared case insensitive.
		/// </summary>
		public override bool Equals( object obj )
		{
			return Equals( obj as PathElement );
		}

		/// <summary>
		/// Overrridden <see cref="System.Object.GetHashCode"/> method.
		/// </summary>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			if( _HashCode == -1 )
				_HashCode = StringComparer.OrdinalIgnoreCase.GetHashCode( Value );
			return _HashCode;
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}

		/// <summary>
		/// Overrridden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return Value;
		}

		#endregion

		#region interface IEquatable<PathElement>

		/// <inheritdoc />
		public bool Equals( PathElement other )
		{
			return !ReferenceEquals( other, null ) &&
					Type == other.Type &&
					GetHashCode() == other.GetHashCode() &&
					string.Equals( Value, other.Value, StringComparison.OrdinalIgnoreCase );
		}

		#endregion
	}
}