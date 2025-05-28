<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.11.0" %}
{% capture features %}
    <ul>
        <li>Added endpoint to fetch the number of parts at a certain path or depth</li>
        <li>Added endpoint to fetch the number of characteristics at a certain path or depth</li>
        <li>Added endpoint to subscribe to <a href="/PiWeb-Api/dataservice/v1.11/#ds-events">events</a> of DataService activity</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
