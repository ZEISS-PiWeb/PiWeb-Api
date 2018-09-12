<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.1.0" %}
{% capture features %}
    <ul>
      <li>It is possible to fetch current supported interface version by calling service root uri.</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}