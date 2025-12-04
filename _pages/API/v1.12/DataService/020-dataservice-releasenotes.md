<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

{% assign version="1.12.0" %}
{% capture features %}
    <ul>
        <li>Added endpoint <code>/InspectionPlanItems</code> to fetch parts and characteristics in one request, e.g. to fetch complete sub-trees of the inspection plan</li>
        <li>Added parameter <code>searchCondition</code> to part and characteristic endpoints, which allows filtering the result based on part or characteristic attributes</li>
    </ul>
{% endcapture %}

{% include releaseNotes.html %}
