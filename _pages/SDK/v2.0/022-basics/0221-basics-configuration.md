<h2 id="{{page.sections['basics']['secs']['configuration'].anchor}}">{{page.sections['basics']['secs']['configuration'].title}}</h2>

All types of entities can be described by several attributes.
Every attribute has an unique key. This key is important, as it has the purpose of an identifier for PiWeb to identify different attributes. PiWeb knows different important attributes and their keys, changing them is not advised.

>{{ site.headers['bestPractice'] }} Use the `WellKnownKeys` class
The .NET SDK provides the `WellKnownKeys` class where you can find important standardized attribute keys.

{% highlight csharp %}
//Get a standardized key for the part description
var partDescriptionKey = WellKnownKeys.Part.Description;
{% endhighlight %}

The class `Configuration` includes all possible attributes for each entity identified by particular property:

<img src="/PiWeb-Api/images/configuration-schema.png" class="img-responsive center-block">

{% capture table %}
Property                                          | Description
--------------------------------------------------|--------------------------------------------------------------------
<nobr><code>AbstractAttributeDefinition</code> AllAttributes</nobr>  | Returns a list of all attribute definitions in this configuration.
<nobr><code>AttributeDefinition</code> CatalogAttributes</nobr>  | Returns a list of all attribute catalog definitions in this configuration.
<nobr><code>AbstractAttributeDefinition</code> CharacteristicAttributes</nobr>  | Returns a list of all characteristic attribute definitions in this configuration.
<nobr><code>AbstractAttributeDefinition</code> MeasurementAttributes</nobr>  | Returns a list of all measurement attribute definitions in this configuration.
<nobr><code>AbstractAttributeDefinition</code> PartAttributes</nobr>  | Returns a list of all part attribute definitions in this configuration.
<nobr><code>AbstractAttributeDefinition</code> ValueAttributes</nobr>  | Returns a list of all value attribute definitions in this configuration.
<nobr><code>VersioningType</code> VersioningType</nobr> | Specifies how the server is performing inspection plan versioning.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

As you can see in above class diagram `Configuration` consists of several methods to easily handle entities' attribute definitions.

`AbstractAttributeDefinition` has two implementations: `AttributeDefinition`and `CatalogAttributeDefinition`:

<img src="/PiWeb-Api/images/attributedefinition-schema.png" class="img-responsive center-block">

#### AbstractAttributeDefinition
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`string` Description | The description of the attribute.
`ushort` Key | The unique key/identifier.
`bool` QueryEfficient | Indicates if the attribute is efficient for filtering operations.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### AttributeDefinition
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`ushort` Length | The maximal lenght of an attribute. Only valid if the type is AlphaNumeric.
`bool` LengthSpecified | Indicates if the length is specified.
`AttributeType` Type | The attribute type, i.e. `AlphaNumeric`, `Float`, `Integer` or `DateTime`.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### CatalogAttributeDefinition
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`Guid` Catalog | The Guid of the catalog that should be usable as an attribute value.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
<hr>

While `AttributeDefinition` describes an attribute for parts, characteristics, measurements, measured values and catalogs, `CatalogAttributeDefinition` is an attribute definition based on an existing catalog. This means that a `CatalogAttributeDefinition` doesn't define an attribute that can be used *in a catalog*, but the definition of an attributes value *as a catalog* entry. The following example will help to understand the difference.

{{ site.headers['example'] }} Create a new `AttributeDefinition`;

{% highlight csharp %}
//Create an AttributeDefinition;
AbstractAttributeDefinition AttributeDefinition = new AttributeDefinition( 11001, "Description", AttributeType.AlphaNumeric, 255 );
{% endhighlight %}
Here we create an attribute *Description*, which can be used in parts, catalogs etc.

{{ site.headers['example'] }} Create a new `CatalogAttributeDefinition`;

{% highlight csharp %}
Catalog TestCatalog = new Catalog
{
  Name = "TestCatalog",
  Uuid = Guid.NewGuid(),
  ValidAttributes = new[]{...},
  CatalogEntries = new[]{...}
};

AbstractAttributeDefinition CatalogAttributeDefinition = new CatalogAttributeDefinition
{
  Catalog = TestCatalog.Uuid,
  Description = "Row_with_catalog_as_value",
  Key = 11002
};
{% endhighlight %}

Here we create a new catalog *TestCatalog*. The next step is to create a `CatalogAttributeDefinition` that defines a new attribute `Row_with_catalog_as_value`, and to assign the `TestCatalog` to this attribute. Now the catalog entries can be used as values for this attribute.

>{{ site.headers['bestPractice'] }} Check if a key already exists in the configuration
Keys are unique, so creating an attribute with the same key will result in an exception. You should always check if an attribute already exists, see example below.

{{ site.headers['example'] }} Create a new attribute for a part

{% highlight csharp %}
//Create the client and fetch the configuration
var client = new DataServiceRestClient( "https://piwebserver:8080" );
var configuration = await client.GetConfiguration();

//Create a new AttributeDefinition with key 11001
AbstractAttributeDefinition attributeDefinition = new AttributeDefinition( 11001, "Description", AttributeType.AlphaNumeric, 255 );

//Check if attribute does already exist
bool attributeDoesAlreadyExist = configuration.GetDefinition( 11001 );

//Create new attribute if not existing
if(!attributeDoesAlreadyExist)
{
await client.CreateAttributeDefinition( Entity.Part, attributeDefinition );
}
{% endhighlight %}
