### Archive lookup
<hr>

Examples in this section:
+ [Fetch information about a raw data archive](#-example--fetch-information-about-a-raw-data-archive)
+ [Fetch content of a raw data archive](#-example--fetch-content-of-a-raw-data-archive)
+ [Fetch information about multiple archives](#-example--fetch-information-about-multiple-archives)
+ [Fetch contents of multiple archives](#-example--fetch-contents-of-multiple-archives)
<hr>

The .NET SDK offers special functionality to work with archives, including the possibility to list contents of an archive which is saved as raw data on any target entity,
or retrieving only specified files of an archive instead of requesting the whole archive. This can save time and data if you only need a small portion of an archive.

>{{ site.images['info'] }} Archive lookup currently works with archives of filetype `.zip`

#### RawDataArchiveEntriesDto
{% capture table %}
Property           									 	| Description
-----------------------------------------------------|------------------------------------------------------------------
<nobr><code>RawDataInformationDto</code> ArchiveInfo </nobr> | Information about the raw data, which is an archive. Same as [RawDataInformationDto](#ba-rawData).
`string[]` Entries  | A list of all files inside the archive, including subfolders.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} Fetch information about a raw data archive

{% highlight csharp %}
var target = RawDataTargetEntityDto.CreateForPart( SamplePart.Uuid );

//Raw data with key 1 is an archive
//Fetch information and a list of archive entries
var archiveInformation = await RawDataServiceClient.ListRawDataArchiveEntries( target, 1 );
{% endhighlight %}

You will receive a `RawDataArchiveEntriesDto` containing archive information and a list of filenames.
Since folders are also entries from the point of view of the archive, they are part of the result, e.g. an entry like *Subfolder/*.
Please note that these entries do not contain data, so requesting the entry *Subfolder/* will result in an empty result.
You will not receive the folders content. For this you need to explicitly fetch the files located in the subfolder with their full name, including folder structure, as shown in the next example.

Once you know the contents of specified archive you can request single files similiar to fetching normal raw data.
You specify the raw data archive with its `RawDataTargetEntityDto` and key, the requested file with its filename.

>{{ site.images['info'] }} The filename is *case insensitive*. Archive lookup will return the first occurence of specified file if two files with the same name exist in the same folder.

{{ site.headers['example'] }} Fetch content of a raw data archive

{% highlight csharp %}
var target = RawDataTargetEntityDto.CreateForPart( SamplePart.Uuid );

//Raw data with key 1 is an archive
//Fetch the file information.txt from the archive (raw data with key 1 of SamplePart)
var file = await RawDataServiceClient.GetRawDataArchiveContent( target, 1, "information.txt" );

//Fetch a file from a subfolder inside the archive
var fileSubfolder = await RawDataServiceClient.GetRawDataArchiveContent( target, 1, "Subfolder/information.txt" );
{% endhighlight %}

This will fetch the file "information.txt", or as in the second call the file "information.txt" from a subfolder.

You can request information and file lists for more than one archive at a time using special query functionality.
#### RawDataBulkQueryDto
{% capture table %}
Property           									 	| Description
-----------------------------------------------------|------------------------------------------------------------------
<nobr><code>RawDataSelectorDto[]</code> Selectors </nobr> | List of selectors. Each selector specifies one raw data (which should be an archive). A selector contains the `RawDataTargetEntityDto` and the raw data key of the archive.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} Fetch information about multiple archives

{% highlight csharp %}
var target = RawDataTargetEntityDto.CreateForPart( SamplePart.Uuid );

//Raw data with keys 0, 1, 7 are archives
//Fetch information and a list of archive entries for each selector
var archiveEntryQuery = await RawDataServiceClient.RawDataArchiveEntryQuery(
  new RawDataBulkQueryDto( new[]
    {
      new RawDataSelectorDto( 0, target ),
      new RawDataSelectorDto( 1, target ), //more raw data from the same target entity
      new RawDataSelectorDto( 7, target_5 ) //raw data from another target entity
    }
));
{% endhighlight %}

You will receive a list of `RawDataArchiveEntriesDtos` for all archives specified by a selector.
This method offers some convenience functionality: it also accepts an array of `RawDataInformationDtos` as parameter to specify targeted raw data archives.

You can fetch multiple files of the same or different archives using the method `GetRawDataArchiveContent`.
The property `Selectors`contains the same information as the `ArchiveEntryQuery` above, with an additional list of requested files per raw data archive.
#### RawDataArchiveBulkQueryDto
{% capture table %}
Property           									 	| Description
-----------------------------------------------------|------------------------------------------------------------------
<nobr><code>RawDataArchiveSelectorDto[]</code> Selectors </nobr> | List of selectors. Each selector specifies one raw data (which should be an archive). A selector contains the `RawDataTargetEntityDto`, the raw data key of the archive and a list of requested files.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} Fetch contents of multiple archives

{% highlight csharp %}
var target = RawDataTargetEntityDto.CreateForPart( SamplePart.Uuid );

//Raw data with key 1 and 7 is an Archive
//Fetch requested archive contents
var archiveContentQuery = await RawDataServiceClient.RawDataArchiveContentQuery(
  new RawDataArchiveBulkQueryDto( new[]
    {
      new RawDataArchiveSelectorDto( 0, target, new[]
        {
          "information.txt",
          "thumbnail.jpg"
        }),
      new RawDataArchiveSelectorDto( 7, target_5, new[]
        {
          "Subfolder/someFile.txt",
          "cadFile.cad"
        })
}));
{% endhighlight %}

You will receive a list of `RawDataArchiveContentDtos`, one for each file, with no separation between files of different raw data archives.
#### RawDataArchiveContentDto
{% capture table %}
Property           									 	| Description
-----------------------------------------------------|------------------------------------------------------------------
<nobr><code>RawDataInformationDto</code> ArchiveInfo </nobr> | Information about the raw data, which is an archive. Same as [RawDataInformationDto](#ba-rawData).
`string` FileName  | The filename of the requested file.
`int` Size  | Length of data.
`byte[]` Data  | Actual data representing the file.
`Guid` MD5  | MD5 checksum of data.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

As both methods work like queries, a non existing file or archive simply wont be part of the result. If the file "cadFile.cad" in above example doesnt exist the result will simply contain only 3 files instead of 4.
You wont get an exception. This behaves differently if one specified raw data is no archive, in this case an exception is thrown.

#### Exceptions

Archive lookup only works with supported archives, currently the .zip format. If you try to list or fetch contents of raw data in any other format you will receive the following exception:
`WrappedServerErrorException: Specified RawData is no archive of supported format!`
