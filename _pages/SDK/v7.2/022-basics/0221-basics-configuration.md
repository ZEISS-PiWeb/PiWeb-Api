<h2 id="{{page.sections['basics']['secs']['configuration'].anchor}}">{{page.sections['basics']['secs']['configuration'].title}}</h2>

Examples in this section:
+ [Creating a new AttributeDefinitionDto](#-example--creating-a-new-attributedefinition)
+ [Creating a new CatalogAttributeDefinitionDto](#-example--creating-a-new-catalogattributedefinition)
+ [Creating a new attribute for a part](#-example--creating-a-new-attribute-for-a-part)
<hr>

All types of entities can be described by several attributes.
Every attribute is identified by a unique key. PiWeb's standard configuration consists of well known attributes, changing standardized keys is not advised. When customizing the configuration a best practice is to use keys outside of PiWeb's standard range, for example between 14000 and 20000 should be enough space for your personal keys. <br>
To mark an attribute as a key in PiWeb it is often displayed with the letter *K* prior to the actual value, e.g. *K1234*. In code you only use the value, so *1234*. Leading zeros are ignored, a key *0024* is the same as *24*.

>{{ site.headers['bestPractice'] }} Use `WellKnownKeys` class
Our .NET SDK provides the `WellKnownKeys` class including important standardized attribute keys.

{% highlight csharp %}
//Get standardized key for the part description
var partDescriptionKey = WellKnownKeys.Part.Description;
{% endhighlight %}

`ConfigurationDto` class includes all possible attributes for each entity in a particular property:

<img src="/PiWeb-Api/images/v6/configuration-schema.png" class="img-responsive center-block">

{% capture table %}
Property                                          | Description
--------------------------------------------------|--------------------------------------------------------------------
<nobr><code>AbstractAttributeDefinitionDto[]</code> AllAttributes</nobr>              | Returns a list of all attribute definitions in this configuration.
<nobr><code>AttributeDefinitionDto[]</code> CatalogAttributes</nobr>                  | Returns a list of all attribute definitions for a catalog entry in this configuration.
<nobr><code>AbstractAttributeDefinitionDto[]</code> CharacteristicAttributes</nobr>   | Returns a list of all characteristic attribute definitions in this configuration.
<nobr><code>AbstractAttributeDefinitionDto[]</code> MeasurementAttributes</nobr>      | Returns a list of all measurement attribute definitions in this configuration.
<nobr><code>AbstractAttributeDefinitionDto[]</code> PartAttributes</nobr>             | Returns a list of all part attribute definitions in this configuration.
<nobr><code>AbstractAttributeDefinitionDto[]</code> ValueAttributes</nobr>            | Returns a list of all value attribute definitions in this configuration.
<nobr><code>VersioningType</code> VersioningTypeDto</nobr>                            | Specifies how the server is performing inspection plan versioning.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

As you can see in above class diagram `ConfigurationDto` consists of several methods to easily handle entities' attribute definitions.

`AbstractAttributeDefinitionDto` has two implementations: `AttributeDefinitionDto` and `CatalogAttributeDefinitionDto`:

<img src="/PiWeb-Api/images/v6/attributedefinition-schema.png" class="img-responsive center-block">

#### AbstractAttributeDefinitionDto
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`string` Description | The description of the attribute.
`ushort` Key | The unique key/identifier.
`bool` QueryEfficient | Indicates if the attribute is efficient for filtering operations. This flag is currently unused but may be used in future web service versions.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### AttributeDefinitionDto
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`ushort?` Length | The maximal lenght of an attribute. Only valid if the type is AlphaNumeric.
`AttributeTypeDto` Type | The attribute type, i.e. `AlphaNumeric`, `Float`, `Integer` or `DateTime`.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### CatalogAttributeDefinitionDto
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`Guid` Catalog | The Guid of the catalog that should be usable as an attribute value.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
<hr>

While `AttributeDefinitionDto` describes an attribute for parts, characteristics, measurements, measured values and catalogs, `CatalogAttributeDefinitionDto` is an attribute definition based on an existing catalog. This means that a `CatalogAttributeDefinitionDto` doesn't define an attribute that can be used *in a catalog*, but the definition of an attributes value *as a catalog* entry. The following example will help to understand the difference.

{{ site.headers['example'] }} Creating a new `AttributeDefinitionDto`

{% highlight csharp %}
//Create an AttributeDefinitionDto;
var attributeDef = new AttributeDefinitionDto( 11001, "Description", AttributeTypeDto.AlphaNumeric, 255 );
{% endhighlight %}
Here we create an attribute *Description*, which can be used in parts, catalogs etc.

{{ site.headers['example'] }} Creating a new `CatalogAttributeDefinitionDto`

{% highlight csharp %}
//Create a catalog using recently created attributeDef as catalog's valid attribute
var testCatalog = new CatalogDto
{
  Name = "TestCatalog",
  Uuid = Guid.NewGuid(),
  ValidAttributes = new[]{ attributeDef.Key, ... },
  CatalogEntries = new[]{ ... }
};

//Create a CatalogAttributeDefinition
var catalogAttributeDefinition = new CatalogAttributeDefinitionDto
{
  Catalog = testCatalog.Uuid,
  Description = "Catalog Based Attribute",
  Key = 11002
};
{% endhighlight %}

Here we create a new catalog *TestCatalog*. The next step is to create a `CatalogAttributeDefinitionDto` that defines a new attribute `Catalog Based Attribute`, and to assign the `TestCatalog` to this attribute. Now the catalog entries can be used as values for this attribute.

>{{ site.headers['bestPractice'] }} Check if a key already exists in the configuration
Keys are unique, so creating an attribute with the same key will result in an exception. You should always check if an attribute already exists, see example below.

{{ site.headers['example'] }} Creating a new attribute for a part

{% highlight csharp %}
//Create the client and fetch the configuration
var configuration = await DataServiceClient.GetConfiguration();

//Create a new AttributeDefinitionDto with key 11001
var attributeDefinition = new AttributeDefinitionDto( 11001, "Description", AttributeTypeDto.AlphaNumeric, 255 );

//Check if attribute does already exist
var attributeDoesAlreadyExist = configuration.GetDefinition( 11001 );

//Create new attribute if not existing
if(attributeDoesAlreadyExist == null)
{
  await DataServiceClient.CreateAttributeDefinition( EntityDto.Part, attributeDefinition );
}
{% endhighlight %}
