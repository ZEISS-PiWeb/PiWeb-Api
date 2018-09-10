<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>
<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.4.2</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    * Update attribute endpoint did return wrong status code.
  </div>
</div>

<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.4.1</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    * Filtering measurements by measurement value attributes did not work properly.
  </div>
</div>

<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.4.0</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Features</b><br/>
    * Endpoint for getting measurements was extended by parameters <i>mergeCondition</i> and <i>mergeMasterPart</i>.
    * Sever setting: Possibility to prevent a part to be deleted if there measurements belong to this part
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    * Some string attributes got lost on moving a part if versioning was enabled.
    * Moving and renaming characteristics in one call is not possible. If tried an error message is returned, now.
  </div>
</div>