<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.3.0" %}
{% capture features %}
    <ul>
      <li>Endpoint for getting characteristics was changed: Restriction by part uuids was removed and restriction by characteristic uuids was added.</li>
      <li>Until now to change catalog's valid attributes, the catalog needed to be deleted an re-created again. This can be done by calling update endpoint, now.</li>
      <li>Endpoint for fetching measurements was extended by <i>mergeAttributes</i> parameter</li>
      <li>New endpoint for searching mesaurement attributes</li>
    </ul>
{% endcapture %}
{% capture bugfixes %}
    <ul>
      <li>Characteristics containing measurement values can be moved, now if parent part has not changed.</li>
      <li>If versioning enabled all attributes were deleted if a characteristics was moved.</li>
      <li>It was possible to create a catalog with an empty uuid.</li>
      <li>Path names colid extend maximum length of database cloumn.</li>
      <li>It was not possible to delete mlitiple catalogs at once.</li>
      <li>Restriction by <i>LastModified</i> parameter on fetching measurements did not work.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}