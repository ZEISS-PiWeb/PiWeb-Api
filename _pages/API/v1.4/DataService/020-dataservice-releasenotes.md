<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>
<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.4.2</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    <ul><li>Update attribute endpoint did return wrong status code.</li></ul>
  </div>
</div>

<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.4.1</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    <ul><li>Filtering measurements by measurement value attributes did not work properly.</li></ul>
  </div>
</div>

<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.4.0</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Features</b><br/>
    <ul>
    <li>Endpoint for getting measurements was extended by parameters <i>mergeCondition</i> and <i>mergeMasterPart</i>.</li>
    <li>Sever setting: Possibility to prevent a part to be deleted if there measurements belong to this part.</li>
    </ul>
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    <ul>
    <li>Some string attributes got lost on moving a part if versioning was enabled.</li>
    <li>Moving and renaming characteristics in one call is not possible. If tried an error message is returned, now.</li>
    </ul>
  </div>
</div>