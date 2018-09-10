<h2 id="{{page.sections['dataservice']['secs']['releaseNotes'].anchor}}">{{page.sections['dataservice']['secs']['releaseNotes'].title}}</h2>

<p></p>

<div class="panel panel-primary">
  <div class="panel-heading">
    <span><h3 class="panel-title">1.3.0</h3></span>
    <span style="float: right;"></span>
  </div>
  <div class="panel-body">
    <b>Features</b><br/>
    * Endpoint for getting characteristics was changed: Restriction by part uuids was removed and restriction by characteristic uuids was added.
    * Until now to change catalog's valid attributes, the catalog needed to be deleted an re-created again. This can be done by calling update endpoint, now.
    * Endpoint for fetching measurements was extended by <i>mergeAttributes</i> parameter
    * New endpoint for searching mesaurement attributes
  </div>
  <div class="panel-body">
    <b>Bugfixes</b><br/>
    * Characteristics containing measurement values can be moved, now if parent part has not changed.
    * If versioning enabled all attributes were deleted if a characteristics was moved.
    * It was possible to create a catalog with an empty uuid.
    * Path names could extend maximum length of database cloumn.
    * It was not possible to delete multiple catalogs at once.
    * <i>LastModified</i> restriction on fetching measurements did not work.
  </div>
</div>