<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.3.0</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Features</b><br/>
    <li>
      <ul>Endpoint for getting characteristics was changed: Restriction by part uuids was removed and restriction by characteristic uuids was added.</ul>
      <ul>Until now to change catalog's valid attributes, the catalog needed to be deleted an re-created again. This can be done by calling update endpoint, now.</ul>
      <ul>Endpoint for fetching measurements was extended by <i>mergeAttributes</i> parameter</ul>
      <ul>New endpoint for searching mesaurement attributes</ul>
    </li>
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    <li>
      <ul>Characteristics containing measurement values can be moved, now if parent part has not changed.</ul>
      <ul>If versioning enabled all attributes were deleted if a characteristics was moved.</ul>
      <ul>It was possible to create a catalog with an empty uuid.</ul>
      <ul>Path names could extend maximum length of database cloumn.</ul>
      <ul>It was not possible to delete multiple catalogs at once.</ul>
      <ul><i>LastModified</i> restriction on fetching measurements did not work.</ul>
    </li>
  </div>
</div>