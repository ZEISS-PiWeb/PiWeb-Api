<h2 id="{{page.sections['dataservice']['secs']['events'].anchor}}">{{page.sections['dataservice']['secs']['events'].title}}</h2>

The PiWeb Server offers functionality for clients to be asynchronously notified of certain events, e.g. creation of measurements, using [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction). A C# sample application using the PiWeb Server events endpoint can be found in the [PiWeb-Training Project](https://github.com/ZEISS-PiWeb/PiWeb-Training?tab=readme-ov-file#piwebapi---events) on GitHub.<br>
Information and tutorials for different client technologies like .NET, JavaScript or Java can be found in the [SignalR client documentation](https://learn.microsoft.com/en-us/aspnet/core/signalr/client-features?view=aspnetcore-8.0).

<h3 id="{{page.sections['dataservice']['secs']['events'].anchor}}-endpoints">Endpoint</h3>

The events endpoint can be found under the route `http(s)://serverHost:port/events`. The endpoint is not part of the DataServiceRest, since it is able to send events for other parts of PiWeb, e.g. the [RawDataServiceRest](https://zeiss-piweb.github.io/PiWeb-Api/rawdataservice/v1.8/#rs-events "RawDataServiceRest events documentation").

<h3 id="{{page.sections['dataservice']['secs']['events'].anchor}}-objectstructure">Available Events</h3>

The following listing contains all events available for the entities managed using the DataServiceRest as well as the provided information in the events.

>{{ site.images['info'] }} You will only receive events for entities with corresponding permissions assigned to your user.

#### Parts

{% capture table %}
Event Type      |  Property |  Description
----------------|-----------------------------------------------------------------------------------
Created         | <nobr><code>Guid</code> PartUuid <br></nobr> <nobr><code>Guid</code> ParentPartUuid</nobr> | The unique identifier of the newly created part. <br> The unique identifier of the parent part where the new part was added to. 
Modified        | <code>Guid</code> PartUuid | The unique identifier of the modified part.
Moved           | <code>Guid</code> PartUuid <br> <code>string</code> FromPath <br><br> <code>string</code> ToPath | The unique identifier of the moved part. <br> The source path where the part was located before movement or empty string if the client is not authorized for read access. <br> The target path where the part was moved to or empty string if the client is not authorized for read access.
Deleted         | <code>Guid</code> PartUuid <br> <code>Guid</code> ParentPartUuid | The unique identifier of the deleted part. <br> The unique identifier of the parent part where the part was deleted from.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

#### Characteristics

{% capture table %}
Event Type      |  Property  |  Description
----------------|-----------------------------------------------------------------------------------
Created         | <nobr><code>Guid</code> CharacteristicUuid</nobr> <br> <code>Guid</code> ParentUuid | The unique identifier of the newly created characteristic. <br> The unique identifier of the parent inspection plan item (either a part or a characteristic) where the new characteristic was added to.
Modified        | <code>Guid</code> CharacteristicUuid <br> <code>Guid</code> ParentPartUuid | The unique identifier of the moved characteristic. <br> The unique identifier of the parent part where the characteristic was modified. The parent part may not be the direct parent in the inspection plan tree, it is possible that the direct parent is a characteristic.
Moved           | <code>Guid</code> CharacteristicUuid <br> <code>string</code> FromPath <br><br> <code>string</code> ToPath | The unique identifier of the moved characteristic. <br> The source path where the characteristic was located before movement or empty string if the client is not authorized for read access. <br> The target path where the characteristic was moved to or empty string if the client is not authorized for read access .
Deleted         | <code>Guid</code> CharacteristicUuid <br> <code>Guid</code> ParentPartUuid | The unique identifier of the deleted characteristic <br> The unique identifier of the parent part where the characteristic was deleted from.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

#### Measurements

{% capture table %}
Event Type      |  Property  |  Description
----------------|-----------------------------------------------------------------------------------
Created         | <code>Guid</code> CharacteristicUuid <br> <code>Guid</code> MeasurementUuid | The unique identifier of the part where the new measurement was created. <br> The unique identifier of the newly created measurement.
Modified        | <code>Guid</code> CharacteristicUuid <br> <code>Guid</code> MeasurementUuid | The unique identifier of the part where the measurement was modified. <br> The unique identifier of the modified measurement.
Deleted         | <code>Guid</code> CharacteristicUuid <br> <code>Guid</code> MeasurementUuid | The unique identifier of the part where the measurement was deleted. <br> The unique identifier of the deleted measurement.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}