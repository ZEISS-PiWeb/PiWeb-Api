<h2 id="{{page.sections['rawdataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['rawdataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.5.0" %}
{% capture features %}
    <ul>
      <li>Endpoint to list content of .zip archives saved as raw data</li>
      <li>Endpoint to fetch specified files inside .zip archives</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
