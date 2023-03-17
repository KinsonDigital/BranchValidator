<h1 align="center" style='color:mediumseagreen;font-weight:bold'>
    BranchValidator Preview Release Notes - v1.0.0-preview.3
</h1>

<h2 align="center" style='font-weight:bold'>Quick Reminder</h2>

<div align="center">

As with all software, there is always a chance for issues and bugs, especially for preview releases, which is why your input is greatly appreciated. 🙏🏼
</div>

---

<h2 style="font-weight:bold" align="center">New Features ✨</h2>

1. [#12](https://github.com/KinsonDigital/BranchValidator/issues/12) - Refactored code to use the types from the library [KDActionUtils](https://github.com/KinsonDigital/KDActionUtils).  The following list of types have been removed and replaced with the same types from the library.
    - `IAction`
    - `IArgParsingService`
    - `NullOrEmptyStringArgumentException`
    - `IActionOutputService`
    - `ActionOutputService`
    - `GitHubConsoleService`
    - `AppService`
    - `ArgParsingService`
    - `ConsoleService`
    - `IAppService`
    - `IConsoleService`
2. [#10](https://github.com/KinsonDigital/BranchValidator/issues/10) - Added URL in the GitHub console output.
    - This URL is a link to the project's create issue page.

---

<h2 style="font-weight:bold" align="center">Nuget/Library Updates 📦</h2>

1. [#12](https://github.com/KinsonDigital/BranchValidator/issues/12) - Added nuget package **KinsonDigital.KDActionUtils** version **v1.0.0-preview.1**.

---

<h2 style="font-weight:bold" align="center">Other 🪧</h2>
<h5 align="center">(Includes anything that does not fit into the categories above)</h5>

1. [#5](https://github.com/KinsonDigital/BranchValidator/issues/5) - Create logo for project.
    - Created light and dark mode versions of the project logo.
    - Added logo to the top of the project README file.
2. [#18](https://github.com/KinsonDigital/BranchValidator/issues/18) - Updated and added issue and PR templates to the project.
3. [#16](https://github.com/KinsonDigital/BranchValidator/issues/16) - Added code of conduct file to the project.
4. [#23](https://github.com/KinsonDigital/BranchValidator/issues/23) - Fixed `release-todo-issue-template.yml` by removing character artifacts that were preventing the template from working properly.
5. [#9](https://github.com/KinsonDigital/BranchValidator/issues/9) - Updated the name of the GitHub action from `BranchValidator` to `Branch Validator`.
    - This will make it a little more intuitive when searching for actions in the marketplace.
6. [#28](https://github.com/KinsonDigital/CASL/issues/28) - Updated the `release-todo-issue-template.yml` file.
    - Added a new item to the **Perform Release ToDo List** section to check that the PR is approved and merged.
    - Bolded a section of the last item in the **Perform Release ToDo List** section.
7. [#27](https://github.com/KinsonDigital/CASL/issues/27) - Added branching documentation with branching documents.
8. [#30](https://github.com/KinsonDigital/CASL/issues/30) - Updated project readme.
    - Added a note explaining why the action must be run in **Ubuntu** with some links to some GitHub documentation resources.
9. [#35](https://github.com/KinsonDigital/CASL/issues/35) - Updated the `preview-feature-pr-template.md` pull request template file.
    - Rearranged the order of 2 tasks items.
10. [#38](https://github.com/KinsonDigital/BranchValidator/issues/38) - Improved project README.
11. [#41](https://github.com/KinsonDigital/BranchValidator/issues/41) - Made various improvements to the pull request templates by adding 2 new tasks as well as rearranging the order of of the tasks.  List of pull request templates updated below:
    - `feature-pr-template.md`
    - `preview-feature-pr-template.md`
    - `preview-release-pr-template.md`
    - `production-release-pr-template.md`
