<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.7.0" %}
{% capture features %}
    <ul>
        <li>Endpoint for deleting measurements was extended by parameter <i>runAsync</i></li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
