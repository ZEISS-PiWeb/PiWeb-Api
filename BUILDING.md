Building PiWeb API
==================

TL;DR
-----

* do development on feature branches called "feature/<cool_new_feature>"
* use GitHub forks and pull requests, pushing directly to master or develop is not possible
* put release notes into WHATSNEW.txt
* NuGet packages are automatically built by GitHub
* merging a PR automatically publishes the beta-NuGets to nuget.org

Comprehensive
-------------

PiWeb API is a .NET library packaged as NuGet. The NuGet packages are built automatically using
[GitHub Actions](https://github.com/ZEISS-PiWeb/PiWeb-Api/actions). There is also a build badge on
the README.md front page like this:

[![Build on develop](https://github.com/ZEISS-PiWeb/PiWeb-Api/actions/workflows/develop.yml/badge.svg?branch=develop&event=push)](https://github.com/ZEISS-PiWeb/PiWeb-Api/actions/workflows/develop.yml)

PiWeb API uses a branching model called
[Git Flow](http://nvie.com/posts/a-successful-git-branching-model/)
in order to provide bug fixes for stable releases. This means that the master branch
only contains stable code. Any development work is done either on a branch called "develop"
or on separate feature branches called "feature/<cool_new_feature>". Fixes are done
on branches called "fix/<bug_description>". Stabilization work towards a new
release (if required) is done on branches called "release/<target_version>". Once a
release is ready for deployment, the "Create-Release" workflow is triggered manually.
This creates a release branch (if not already present), and a tag called "v<version_number>", e.g. "v1.0.0".
The tag and branch is then merged to the master branch.
Therefore the master branch only contains stable release code.

This branching model was first described by Vincent Driessen in
his blog entry
[A successful Git branching model](http://nvie.com/posts/a-successful-git-branching-model/).
A good overview and comparison with the simplified
[Github flow](https://guides.github.com/introduction/flow/) can be found in the blog entry
[Git Flow vs Github flow](https://lucamezzalira.com/2014/03/10/git-flow-vs-github-flow/).

Version numbers of assembly and nuget package follow the rules set by
[semantic versioning](https://semver.org/). They are automatically created
from tags, commits and commit messages in the git repository using
[GitVersion](https://gitversion.net/docs/).

The contents of the file WHATSNEW.txt is put into the release notes section
of the main NuGet (Zeiss.PiWeb.Api.Rest). Sub-packages like Dtos and Definitions both have their own
WHATSNEW.txt file for their respective changes. So in order to tell the users about the great new features
in the new version put a description into the correct file.
