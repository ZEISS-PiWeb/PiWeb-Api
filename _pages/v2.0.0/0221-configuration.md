<h2 id="{{page.sections['basics']['secs']['configuration'].anchor}}">{{page.sections['basics']['secs']['configuration'].title}}</h2>

All types of entities can be described by several attributes. `Configuration` class includes all possible attributes for each entity identified by particular property:

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


While `AttributeDefinition` is... `CatalogAttributeDefinition` is a attribute definition based on an existing catalog.

{{ site.headers['example'] }} Create a new attribute for a part

{% highlight csharp %}
//Create the client and fetch the configuration
var client = new DataServiceRestClient( "https://piwebserver:8080" );
var configuration = await client.GetConfiguration();

//Crate a new AttributeDefinition with key 11001
AbstractAttributeDefinition attributeDefinition = new AttributeDefinition( 11001, "Description", AttributeType.AlphaNumeric, 255 );

//Check if attribute does already exist
bool attributeDoesAlreadyExist = configuration.GetDefinition( 11001 );

//Create new attribute
await client.CreateAttributeDefinition( Entity.Part, attributeDefinition );
{% endhighlight %}


Explain different AttributeDefinition classes
Explain QDAS K-Values an link to WellKnownKey class