<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.6.0" %}
{% capture features %}
    <ul>
        <li>Removed WSDL version from ServiceInformation</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
