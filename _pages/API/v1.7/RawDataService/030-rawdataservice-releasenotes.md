<h2 id="{{page.sections['rawdataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['rawdataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.7.0" %}
{% capture features %}
    <ul>
      <li>Add new endpoint to modify raw data information</li>
      <li>Change URL to get raw data information</li>
      <li>Add response describing the created raw data when posting a file.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
