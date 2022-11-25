#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Core;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class DataValueDtoTest
	{
		#region methods

		[Test]
		public void Test_Compare_Empty_Values()
		{
			// arrange
			var value1 = new DataValueDto();
			var value2 = new DataValueDto();

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.True );
		}

		[Test]
		public void Test_Compare_Empty_Value_And_Not_Empty_Value()
		{
			// arrange
			var value1 = new DataValueDto();
			var value2 = new DataValueDto( 1 );

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.False );
		}

		[Test]
		public void Test_Compare_Values_When_Attributes_Count_Is_Different()
		{
			// arrange
			var value1 = new DataValueDto( new[] { new Attribute( 42, "foo" ), new Attribute( 24, "bar" ) } );
			var value2 = new DataValueDto( new[] { new Attribute( 24, "foo" ) } );

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.False );
		}

		[Test]
		public void Test_Compare_Values_When_MeasuredValues_Are_Equal()
		{
			// arrange
			var value1 = new DataValueDto( 1 );
			var value2 = new DataValueDto( 1 );

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.True );
		}

		[Test]
		public void Test_Compare_Values_When_MeasuredValues_Are_Not_Equal()
		{
			// arrange
			var value1 = new DataValueDto( 1 );
			var value2 = new DataValueDto( 2 );

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.False );
		}

		[Test]
		public void Test_Compare_Values_When_Attributes_Are_Equal()
		{
			// arrange
			var value1 = new DataValueDto( new[] { new Attribute( 1, 5 ), new Attribute( 2, "foo" ) } );
			var value2 = new DataValueDto( new[] { new Attribute( 1, 5 ), new Attribute( 2, "foo" ) } );

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.True );
		}

		[Test]
		public void Test_Compare_Values_When_Attributes_Are_Not_Equal()
		{
			// arrange
			var value1 = new DataValueDto( new[] { new Attribute( 1, 5 ), new Attribute( 2, "foo" ) } );
			var value2 = new DataValueDto( new[] { new Attribute( 1, 4 ), new Attribute( 2, "bar" ) } );

			// act, assert
			Assert.That( Equals( value1, value2 ), Is.False );
		}

		[Test]
		public void Test_GetHashCode_IsSame_When_Values_Are_Empty()
		{
			// arrange
			var value1 = new DataValueDto();
			var value2 = new DataValueDto();

			// act
			var hashCode1 = value1.GetHashCode();
			var hashCode2 = value2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsSame_When_Attributes_Contains_Only_One_Attribute_Which_Are_Equal()
		{
			// arrange
			var value1 = new DataValueDto( 1 );
			var value2 = new DataValueDto( 1 );

			// act
			var hashCode1 = value1.GetHashCode();
			var hashCode2 = value2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Attributes_Contains_Only_One_Attribute_Which_Are_Not_Equal()
		{
			var value1 = new DataValueDto( 1 );
			var value2 = new DataValueDto( 2 );

			// act
			var hashCode1 = value1.GetHashCode();
			var hashCode2 = value2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Attributes_Count_Are_Different()
		{
			var value1 = new DataValueDto( new[] { new Attribute( 42, "foo" ), new Attribute( 24, "bar" ) } );
			var value2 = new DataValueDto( new[] { new Attribute( 24, "foo" ) } );

			// act
			var hashCode1 = value1.GetHashCode();
			var hashCode2 = value2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsSame_When_Attributes_Are_Equal()
		{
			// arrange
			var value1 = new DataValueDto( new[] { new Attribute( 1, 5 ), new Attribute( 2, "foo" ) } );
			var value2 = new DataValueDto( new[] { new Attribute( 1, 5 ), new Attribute( 2, "foo" ) } );

			// act
			var hashCode1 = value1.GetHashCode();
			var hashCode2 = value2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Attributes_Are_Not_Equal()
		{
			// arrange
			var value1 = new DataValueDto( new[] { new Attribute( 1, 5 ), new Attribute( 2, "foo" ) } );
			var value2 = new DataValueDto( new[] { new Attribute( 1, 4 ), new Attribute( 2, "bar" ) } );

			// act
			var hashCode1 = value1.GetHashCode();
			var hashCode2 = value2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		#endregion
	}
}