namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class AttributeDtoExtensionsTest
	{
		#region methods

		[Test]
		public void GetAttributeValue_ForExistingAttribute_ReturnsValue()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
					new AttributeDto( 2, "Test2" ),
					new AttributeDto( 3, "Test3" ),
					new AttributeDto( 4, "Test4" ),
				}
			};

			var value = sut.GetAttributeValue( 2 );
			Assert.That( value, Is.EqualTo( "Test2" ) );
		}

		[Test]
		public void GetAttributeValue_ForNonExistingAttribute_ReturnsNull()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
					new AttributeDto( 2, "Test2" ),
					new AttributeDto( 3, "Test3" ),
					new AttributeDto( 4, "Test4" ),
				}
			};

			var value = sut.GetAttributeValue( 20 );
			Assert.That( value, Is.Null );
		}

		[Test]
		public void RemoveAttribute_RemovesExistingAttribute()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
					new AttributeDto( 2, "Test2" ),
					new AttributeDto( 3, "Test3" ),
					new AttributeDto( 4, "Test4" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 3, "Test3" ),
				new AttributeDto( 4, "Test4" ),
			};

			sut.RemoveAttribute( 2 );
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void RemoveAttribute_SkipsNonExistingAttribute()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
					new AttributeDto( 2, "Test2" ),
					new AttributeDto( 3, "Test3" ),
					new AttributeDto( 4, "Test4" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 2, "Test2" ),
				new AttributeDto( 3, "Test3" ),
				new AttributeDto( 4, "Test4" ),
			};

			sut.RemoveAttribute( 22 );
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void InsertNullAttribute_InsertsAttribute_WithoutValue()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 22, null ),
			};

			sut.InsertNullAttribute( 22 );
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void SetAttribute_SetsTheAttributeValue()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 22, "New Value" ),
			};

			sut.SetAttribute( new AttributeDto( 22, "New Value" ) );
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void SetAttributeValue_SetsTheAttributeValue()
		{
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 22, "New Value" ),
			};

			sut.SetAttributeValue( 22, "New Value" );
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		#endregion
	}
}