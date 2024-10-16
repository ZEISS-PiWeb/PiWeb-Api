<h2 id="{{page.sections['rawdataservice']['secs']['events'].anchor}}">{{page.sections['rawdataservice']['secs']['events'].title}}</h2>

The PiWeb Server offers functionality for clients to be asynchronously notified of certain events, e.g. creation of raw data, using [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction). A C# sample application using the PiWeb Server events endpoint can be found in the [PiWeb-Training Project](https://github.com/ZEISS-PiWeb/PiWeb-Training?tab=readme-ov-file#piwebapi---events) on GitHub.<br>
Information and tutorials for different client technologies like .NET, JavaScript or Java can be found in the [SignalR client documentation](https://learn.microsoft.com/en-us/aspnet/core/signalr/client-features?view=aspnetcore-8.0).

<h3 id="{{page.sections['rawdataservice']['secs']['events'].anchor}}-endpoints">Endpoint</h3>

The events endpoint can be found under the route `http(s)://serverHost:port/events`. The endpoint is not part of the RawDataServiceRest, since it is able to send events for other parts of PiWeb, e.g. the [DataServiceRest](https://zeiss-piweb.github.io/PiWeb-Api/dataservice/v1.11/#ds-events "DataServiceRest events documentation").

<h3 id="{{page.sections['interfaceInformation']['secs']['interfaceinformation'].anchor}}-objectstructure">Available Events</h3>

The following listing contains all events available for the entities managed using the DataServiceRest as well as the provided information in the events.

>{{ site.images['info'] }} You will only receive events for entities with corresponding permissions assigned to your user.

#### Raw Data

{% capture table %}
Event Type      |  Property  |  Description
----------------|-----------------------------------------------------------------------------------
Created         | <code>int</code> Key <br> <code>string</code> TargetUuid <br><br><br> <nobr><code>string</code> RawDataEntity</nobr> <br> <code>string</code> Filename | The raw data key <br> The unique identifier of the inspection plan entity (part, characteristic, measurement, value) where the new raw data was added to. Remarks: Raw data of values use a compound UUID in the form of MeasurementUuid\|CharacteristicUuid. <br> The type of target entity (part, characteristic, measurement, value). <br> The file name of the raw data.
Modified        | <code>int</code> Key <br> <code>string</code> TargetUuid <br><br><br> <code>string</code> RawDataEntity <br> <code>string</code> OldFilename <br> <code>string</code> NewFilename | The raw data key <br> The unique identifier of the inspection plan entity (part, characteristic, measurement, value) where the raw data was modified. Remarks: Raw data of values use a compound UUID in the form of MeasurementUuid\|CharacteristicUuid. <br> The type of target entity (part, characteristic, measurement, value). <br> The old file name of the raw data before modification. <br> The new file name of the raw data after modification.
Deleted         | <code>int</code> Key <br> <code>string</code> TargetUuid <br><br><br> <code>string</code> RawDataEntity <br> <code>string</code> Filename | The raw data key <br> The unique identifier of the inspection plan entity (part, characteristic, measurement, value) where the new raw data was removed from. Remarks: Raw data of values use a compound UUID in the form of MeasurementUuid\|CharacteristicUuid. <br> The type of target entity (part, characteristic, measurement, value). <br> The file name of the deleted data.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}