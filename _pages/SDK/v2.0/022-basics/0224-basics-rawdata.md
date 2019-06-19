<h2 id="{{page.sections['basics']['secs']['rawData'].anchor}}">{{page.sections['basics']['secs']['rawData'].title}}</h2>

Examples in this section:
+ [Creating additional data](#-example--creating-additional-data)
+ [Fetching information about additional data](#-example--fetching-information-about-additional-data)
+ [Fetching additional data](#-example--fetching-additional-data)
+ [Updating additional data](#-example--updating-additional-data)
+ [Deleting additional data](#-example--deleting-additional-data)
<hr>

Additional data are attachments that can be added to any entity in the inspection plan. This can be text, images, log files, CAD models or any binary file. There is no limit to the number of files and you can edit or remove additional data as desired.
Every additional data is linked to a `RawDataInformation` object:

<img src="/PiWeb-Api/images/rawDataInformation.png" class="img-responsive center-block">

#### RawDataInformation
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`DateTime` Created | The time of creation, *set by server*
`string` FileName | The filename of the additional data, which does not have to be unique (unlike in a real filesystem).
`int` Key | A unique key that identifies this specific additional data for a corresponding entity. Entities can have multiple additional data objects that are distinct by this key.
`LastModified` |  The time of the last modification, *set by server*
`Guid` MD5 | A Uuid using the MD5-Hash of the additional data object (the file).
`string` MimeType | The <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types">MIME-Type</a> of the additional data. <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Complete_list_of_MIME_types">List of MIME-Types</a>.
`int` Size | The size of the additional data in bytes.
<nobr><code>RawDataTargetEntity</code> Target</nobr> | The target object this additional data object belongs to.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

The `RawDataTargetEntity` Target links the additional data to the associated entity by its Uuid.
>{{ site.headers['bestPractice'] }} Use the helper methods of `RawDataTargetEntity`
This class offers several helper methods to create the link to the associated entity.

#### RawDataTargetEntity
{% capture table %}
Method                                          | Description
------------------------------------------------|------------------------------------------------------------------
<nobr><code>RawDataTargetEntity CreateForCharacteristic(Guid uuid)</code></nobr> | The target is a characteristic.
<nobr><code>RawDataTargetEntity CreateForPart(Guid uuid)</code></nobr> | The target is a part.
<nobr><code>RawDataTargetEntity CreateForMeasurement(Guid uuid)</code></nobr> | The target is a measurement.
<nobr><code>RawDataTargetEntity CreateForValue(Guid measurementUuid, Guid characteristicUuid)</code></nobr> | The target is a measured value of a specific measurement.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} Creating additional data

{% highlight csharp %}
//SamplePart is the part (either new or fetched) where additional data should be added
//Create/choose additional data
var additionalData = Encoding.UTF8.GetBytes( "More important information" );
var target = RawDataTargetEntity.CreateForPart( SamplePart.Uuid );

//Create RawDataInformation
var information = new RawDataInformation
{
	FileName = "SampleFile.txt",
	MimeType = "text/plain",
	Key = -1,
	MD5 = new Guid( MD5.Create().ComputeHash( additionalData ) ),
	Size = additionalData.Length,
	Target = target
};

//Create on server
await RawDataServiceClient.CreateRawData(information, additionalData);
{% endhighlight %}

>{{ site.images['info'] }} When using -1 as the key the server will generate a new unique key.

{{ site.headers['example'] }} Fetching information about additional data

{% highlight csharp %}
//SamplePart is the part (either new or fetched) where additional data should be fetched
var target = RawDataTargetEntity.CreateForPart( SamplePart.Uuid );

//Fetch a list of additional data of our target (the SamplePart)
var additionalDataInformation = await RawDataServiceClient.ListRawData( new[] { target } );
{% endhighlight %}

This will result in an array of all `RawDataInformation` objects linked to the specified part, in this case the SamplePart with only our SampleFile.txt as additional data. This means that we only get information about the files associated with our part, but not the files itself. We don't want to transfer data we are not interested in, and produce unnecessary network traffic. <br>
With the overview of the available additional data we can fetch the files of interest:

{{ site.headers['example'] }} Fetching additional data

{% highlight csharp %}
//var additionalDataInformation as above

//Get the RawDataInformation object (the first entry in our case)
var informationAboutSampleFile = additionalDataInformation.First();

//Fetch the file using the correct RawDataInformation
var sampleFile = await RawDataServiceClient.GetRawData( informationAboutSampleFile );
{% endhighlight %}

With this you will retrieve the actual additional data in its byte representation. Now you can edit the file as desired and later update the additional data:

{{ site.headers['example'] }} Updating additional data

{% highlight csharp %}
//var informationAboutSampleFile as above

//Fetch the file using the correct RawDataInformation
var sampleFile = await RawDataServiceClient.GetRawData( informationAboutSampleFile );

//Edit the file
sampleFile = Encoding.UTF8.GetBytes( "Important information changed!" );

//Update RawDataInformation
informationAboutSampleFile.MD5 = new Guid(MD5.Create().ComputeHash(sampleFile)); //recompute hash
informationAboutSampleFile.length = sampleFile.length;

//Update file on server
await RawDataServiceClient.UpdateRawData(informationAboutSampleFile, sampleFile);
{% endhighlight %}

It is important to update the `RawDataInformation` as well, so it matches the new file. The hash and length need to be updated, while the key and target stays the same. Changing the filename or MIME-Type is also possible. The server automatically updates the property `LastModified`, the date of creation is instead not changed because we only updated our data.

{{ site.headers['example'] }} Deleting additional data

{% highlight csharp %}
//var informationAboutSampleFile and Part as in above examples

//Delete the specific file of our SamplePart
await RawDataServiceClient.DeleteRawDataForPart( Part.Uuid, informationAboutSampleFile.Key );

//Or simply delete all additional data of our SamplePart
await RawDataServiceClient.DeleteRawDataForPart( Part.Uuid );
{% endhighlight %}

>{{ site.headers['bestPractice'] }} Use the specific delete method per entity
The `DataServiceRestClient` offers a delete method for each type of entity which can contain additional data. They need the entity uuid and the additional data key to delete a specific file, or only the entity uuid to delete all additional data of the entity.
