<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>
{% assign version="1.4.2" %}
{% capture bugfixes %}
    <ul><li>Update attribute endpoint did return wrong status code.</li></ul>
{% endcapture %}

{% include releaseNotes.html %}

{% assign version="1.4.1" %}
{% capture bugfixes %}
    <ul><li>Filtering measurements by measurement value attributes did not work properly.</li></ul>
{% endcapture %}

{% include releaseNotes.html %}

{% assign version="1.4.0" %}
{% capture features %}
    <ul>
    <li>Endpoint for getting measurements was extended by parameters <i>mergeCondition</i> and <i>mergeMasterPart</i>.</li>
    <li>Sever setting: Possibility to prevent a part to be deleted if there are still measurements belonging to this part.</li>
    </ul>
{% endcapture %}
{% capture bugfixes %}
    <ul>
    <li>Some string attributes got lost on moving a part if versioning was enabled.</li>
    <li>Moving and renaming characteristics in one call is not possible. If tried an error message is returned, now.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}