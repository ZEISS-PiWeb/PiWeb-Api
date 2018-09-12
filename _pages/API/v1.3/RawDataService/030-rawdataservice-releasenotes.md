<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.2.0" %}
{% capture features %}
    <ul>
      <li>New endpoints to fetch raw data objects more efficently were created.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}