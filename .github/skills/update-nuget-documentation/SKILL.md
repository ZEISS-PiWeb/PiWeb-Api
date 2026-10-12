---
name: update-nuget-documentation
description: Update the GitHub-Pages documentation for the NuGet packages.
---

# Update NuGet Documentation

Use this skill to update the GitHub-Pages documentation for the PiWeb NuGet packages, by creating a new versioned subdirectory and updating the relevant metadata.

# Source structure

The GitHub-pages are using the static site generator Jekyll (https://github.com/jekyll/jekyll).
The source files for the documentation are located in the directory `/_pages/SDK` of the repository.
In this folder, the versions of the Nuget documentation are organized into subdirectories, with each subdirectory corresponding to a specific version of the documentation.
The naming follows the schema `v{major}.{minor}`, for example, `v1.2`.
The main file containing version-related metadata inside this subfolders is `02-netsdk.md`.

# NuGet version

You can find the newest version by checking the release tags in the GitHub repository, see https://github.com/ZEISS-PiWeb/PiWeb-Api/tags.
Tag structure is `release/{major}.{minor}.{patch}`, for example, `release/10.0.0`. Please note that the patch version is not relevant for the documentation.

# Metadata structure

Metadata is at the top of the file, enclosed within triple dashes (`---`).
`version`: An integer representing the version, but used for better sorting, by converting the major and minor version numbers into a single integer, by simply removing the dot between major and minor version. For example, version `9.1` would become `91`. This works as long as the minor version is smaller than 10. Make sure that the minor version is smaller than 10, and the resulting value is unique for each version, if not, stop and ask the user for guidance.
`displayVersion`: A string representing the version in a human-readable format, typically following the `{major}.{minor}` schema. For example `9.1`.
`isCurrentVersion`: A boolean indicating whether this version is the current version of the documentation. Only `true` for the latest version of the documentation.
`permalink`: A string representing the URL path for the documentation page. Typically follows the format `/sdk/v{major}.{minor}/`. For example `/sdk/v9.1/`.

# Workflow

1. Check for the newest NuGet version by looking at the release tags in the GitHub repository, and ask the user if they want to update the documentation to this version.
2. Make sure that the worktree is clean, with no previous changes. If this is not the case, tell the user to clean the worktree before proceeding.
3. Create and checkout a new branch for your changes, following the scheme `doc/Update_documentation_for_v{major}.{minor}`, but check that the branch does not already exist. If it exists, ask the user how to proceed.
4. Navigate to the `/_pages/SDK` directory of the repository.
5. Find the highest existing versioned documentation subdirectory in `/_pages/SDK` and record its version. Use the latest documented version, even if one or more NuGet releases were skipped. If no versioned subdirectory exists, ask the user how to proceed.
6. Create a new directory named `v{major}.{minor}` corresponding to the new version of the NuGet documentation, if it does not already exist. If it exists, ask the user how to proceed.
7. Copy the contents of the versioned subdirectory recorded in step 5 into the new subdirectory.
8. Create a commit which contains this copied content, but do not push the changes yet. This makes it easier to later see metadata changes separately from copied content.
9. Edit the `02-netsdk.md` file within the new subdirectory to update the documentation metadata as needed, see Metadata structure. No content changes should be done, only metadata.
10. Edit the `02-netsdk.md` file in the versioned subdirectory recorded in step 5 to set `isCurrentVersion` to `false`. Make sure that now only one version, the newest one, has `isCurrentVersion` set to `true`.
11. Commit your changes in a second commit, but do not push the changes yet.
12. Tell the user that the version-related changes have been committed, but content changes still need to be made and pushed.