<h2 id="{{page.sections['rawdataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['rawdataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.8.0" %}
{% capture features %}
    <ul>
      <li>Internal optimizations for PiWeb clients.</li>
      <li>Added endpoint to subscribe to <a href="/PiWeb-Api/rawdataservice/v1.8/#rs-events">events</a> of RawDataService activity</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
