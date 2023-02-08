<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.9.0" %}
{% capture features %}
    <ul>
        <li>Endpoint for fetching measurements and values was extended by parameter <i>limitResultPerPart</i></li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
