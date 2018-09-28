<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.2.0" %}
{% capture features %}
    <ul>
      <li>Endpoint for deleting measurements was extended by parameter <i>deep</i> and <i>aggregation.</i></li>
      <li>Endpoint for fetching service information returns last modified timestamp for catalogs, now.</li>
      <li>New endpoint <i>attributes/{key}/{value}</i> was added. It checks if a certain attribute with the given value does exist.</li>
    </ul>
{% endcapture %}
{% capture bugfixes %}
    <ul>
      <li>Possible exception on deleting measurements.</li>
      <li>Moving or renaming an inspection plan item did not lead to an version entry.</li>
      <li>On creating parts or characteristics it was possible to create inspection plan version items including comments, now.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}