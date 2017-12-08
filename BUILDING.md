Building PiWeb API
==================

TL;DR
-----

* do development on "develop" branch or feature branches called "feature/<cool_new_feature>"
* do NOT do development on "master" branch
* use GitHub forks and pull requests, but do NOT merge to "master" branch
    * use development or feature branches instead
* tag release with "v<version_number>", e.g. "v1.0.1"
* publish automatically built NuGet package from AppVeyor to nuget.org
* merge release tag to master branch

Comprehensive
-------------

PiWeb API is a .NET library packaged as NuGet. The NuGet packages are built automatically using
[AppVeyor](https://ci.appveyor.com/project/czjlorenz/piweb-api). There is also a build badge on
the README.md front page like this:

[![Build status](https://ci.appveyor.com/api/projects/status/q48run5x0ge40h9p?svg=true)](https://ci.appveyor.com/project/czjlorenz/piweb-api)

PiWeb API uses a branching model called
[Git Flow](http://nvie.com/posts/a-successful-git-branching-model/)
in order to provide bug fixes for stable releases. This means that the master branch
only contains stable code. Any development work is done either on a branch called "develop"
or on separate feature branches called "feature/<cool_new_feature>". Hotfixes are done
on branches called "hotfix/<bug_description>". Stabilization work towards a new
release (if required) is done on branches called "release/<target_version>". Once a
release is ready for deployment a tag called "v<version_number>", e.g. "v1.0.0", is
created. This tag is merged to the master branch. Therefore the master branch only
contains stable release code.

This branching model was first described by Vincent Driessen in
his blog entry
[A successful Git branching model](http://nvie.com/posts/a-successful-git-branching-model/).
A good overview and comparison with the simplifed
[Github flow](https://guides.github.com/introduction/flow/) can be found in the blog entry
[Git Flow vs Github flow](https://lucamezzalira.com/2014/03/10/git-flow-vs-github-flow/).

Version numbers of assembly and nuget package follow the rules set by
[semantic versioning](https://semver.org/). They are automatically created
from tags in the git repository using
[GitVersion](https://gitversion.readthedocs.io/en/latest/examples/).

Release notes are automatically created from git commit messages using
[GitReleaseNotes](https://github.com/GitTools/GitReleaseNotes) and put into
the nuget package.

The build scripting should support GitHub flow as well as long as tags with version
numbers are created. You should be aware that with GitHub flow only bugfixes for the
current release are possible.

Unstable and release nuget packages are created by [AppVeyor](https://ci.appveyor.com/project/czjlorenz/piweb-api).
The AppVeyor [project feed](https://ci.appveyor.com/nuget/piweb-api) provides
an easy way to consume any built version including unstable versions.
Stable release nuget packages are meant to be deployed to nuget.org making them
official.