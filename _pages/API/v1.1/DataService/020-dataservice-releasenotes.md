<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.2.0" %}
{% capture features %}
    <ul>
      <li>It is possible to fetch current supported interface version by calling service root uri.</li>
    </ul>
{% endcapture %}
{% capture bugfixes %}
    <ul>
      <li>Catalog search returned non valid attributes, too.</li>
      <li>On creating or updating measurement values wihtout any attributes were written to the database, too.</li>
      <li>Deleting measurements by attribute conditions did not work properly.</li>
      <li>On inspection plan an measurement search only attributes existing in current configuration are returned.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}