<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.2.0" %}
{% capture features %}
    <ul>
      <li>Extended fetching raw data by several filter possibilities.</li>
      <li>Endpoint for detching raw data and fetching thumbnails was extended by <i>expectedMd5</i> parameter to enable caching.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}