<div align="center">

  ![BranchingDiagram](https://raw.githubusercontent.com/KinsonDigital/BranchValidator/preview/Images/branchvalidator-logo-darkmode.png#gh-dark-mode-only)
  ![BranchingDiagram](https://raw.githubusercontent.com/KinsonDigital/BranchValidator/preview/Images/branchvalidator-logo-lightmode.png#gh-light-mode-only)  
  <br />
</div>

<h1 align="center">

**Branch Validator**
</h1>

<div align="center">

[![Build PR Status Check](https://img.shields.io/github/actions/workflow/status/KinsonDigital/BranchValidator/build-status-check.yml?label=%E2%9A%99%EF%B8%8FBuild)](https://github.com/KinsonDigital/BranchValidator/actions/workflows/build-status-check.yml)
[![Unit Test PR Status Check](https://img.shields.io/github/actions/workflow/status/KinsonDigital/BranchValidator/unit-test-status-check.yml?label=%F0%9F%A7%AATests)](https://github.com/KinsonDigital/BranchValidator/actions/workflows/unit-test-status-check.yml)

![Codecov](https://img.shields.io/codecov/c/github/KinsonDigital/BranchValidator?color=2F8840&label=Code%20Coverage&logo=codecov)

[![Good First GitHub Issues](https://img.shields.io/github/issues/kinsondigital/BranchValidator/good%20first%20issue?color=7057ff&label=Good%20First%20Issues)](https://github.com/KinsonDigital/BranchValidator/issues?q=is%3Aissue+is%3Aopen+label%3A%22good+first+issue%22)
[![Discord](https://img.shields.io/discord/481597721199902720?color=%23575CCB&label=chat%20on%20discord&logo=discord&logoColor=white)](https://discord.gg/qewu6fNgv7)

</div>

<div align="center">

## **🤷🏼‍♂️ What is it? 🤷🏼‍♂️**
</div>


This **GitHub Action** can be used to check whether or not GIT branch names are valid using an expression in combination with glob syntax. 
These expressions are functions that can be used in combination with **&&** and **||** logic to perform the validation.

<details closed><summary><b>TLDR</b> - Additional Information</summary>

What it really comes down to is enforcing branch 
names and structure. In CI/CD systems, the name of the branch can determine how the system runs. The workflows 
you have might depend on the name of a branch. For example, if the branch uses the GitHub issue number as part 
of the branch name, it will refer back to the GitHub issue. Your build may behave differently depending on which
branch it is building.

When incorrect branch names are setup, they can cause issues with your build and release systems and confusion with your team. This GitHub action will help enforce project standards to help keep things running smoothly. For example, a branch name of `my-branch` does not express the purpose of the branch. Without enforcing naming conventions of branches, how is the team supposed to know the purpose of it? In addition, the ability to 
enforce an issue number to exist in the name of a branch, makes it easier for developers to find which branch 
belongs to which issue. With this kind of enforcement, you can setup automation to trust that the branch name 
contains a number. It also allows you to create automation to check if a GitHub issue exists, which would prevent
incorrect issue numbers from being used in a branch name.

The applications of this GitHub action are endless!!

</details>

<div align="center"><h2 style="font-weight:bold"></h2></div>

>**Note** This GitHub action is built using C#/NET and runs in a docker container. If the job step for running this action is contained in a job that runs on **Windows**, you will need to move the step to a job that runs on **Ubuntu**. You can split up your jobs to fulfill `runs-on` requirements of the GitHub action. This can be accomplished by moving the step into its own job. You can then route the action step outputs to the job outputs and use them throughout the rest of your workflow.  
> For more information on step and job outputs, refer to the GitHub documentation links below:
> - [Defining outputs for jobs](https://docs.github.com/en/actions/using-jobs/defining-outputs-for-jobs)
> - [Setting a step action output parameter](https://docs.github.com/en/actions/using-workflows/workflow-commands-for-github-actions#setting-an-output-parameter)

<div align="center">
  <h2 style="font-weight:bold">🪧 Example 🪧</h2>
</div>


```yaml
name: Branch Validation Sample Workflow

on:
  workflow_dispatch:

jobs:
  Validate_Branch:
    name: Validator Branch
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3

    - name: Validate Branch
      uses: KinsonDigital/BranchValidator@v1.0.0-preview.4 # Make sure this is the latest version
      with:
        branch-name: "${{ github.ref }}" # The current branch where the workflow is running.
        validation-logic: "equalTo('main')"
        trim-from-start: "refs/heads/"
        fail-when-not-valid: true # Optional. Default is true.
```

<div align="center">

## **➡️ Action Inputs ⬅️**
</div>

<table align="center">
    <tr>
        <th>Input Name</th>
        <th>Description</th>
        <th>Required</th>
        <th>Default Value</th>
    </tr>
    <tr align="left">
        <td>branch-name</td>
        <td align="left">The name of the GIT branch.</td>
        <td align="center">yes</td>
        <td align="center">N/A</td>
    </tr>
    <tr align="left">
        <td>validation-logic</td>
        <td align="left">The expression used to validate the branch name.</td>
        <td align="center">yes</td>
        <td align="center">N/A</td>
    </tr>
    <tr align="left">
        <td>trim-from-start</td>
        <td align="left">Trims the text from the beginning of a branch name.</td>
        <td align="center">no</td>
        <td align="center">empty</td>
    </tr>
    <tr align="left">
        <td>fail-when-not-valid</td>
        <td align="left">Fails the job if the branch is not valid.</td>
        <td align="center">no</td>
        <td align="center">true</td>
    </tr>    
</table>

---

<div align="center">

## **⬅️ Action Outputs ➡️**
</div>

The name of the output is `valid-branch` and it returns a boolean of true or false. Click <a href="#manual-fail">here</a> to see an example of how to use the output of the action.

---

<div align="center" style="font-weight:bold">

## **Validation Logic Expression Functions**
</div>

Below is a small list of the available expression functions that you can use in the value of the `validation-logic` input. 
These expression functions can be used in combination with the `&&` and `||` operators:  
  - Example: equalTo('feature/my-*-branch') && allLowerCase()
    - This checks to see whether or not the branch is equal to the value and that the entire branch is lower case.

<details closed><summary><b>TLDR</b> - Expression Function List</summary>

1. `equalTo(string)` - Checks to see whether or not the given branch name is equal to the argument value of the function. The argument value must be a string value that is surrounded by single or double quotes. The quotes used must be the opposite of the quotes used for the entire input value. Standard YAML syntax rules apply. The function value allows the use of 2 characters that provide glob-like behavior. The 2 characters are `#` and `*` and can be used together as many times as needed.
     - _Example 1:_ `equalTo('main')`
       - Checks whether or not the branch is equal to the value of `main`.
     - _Example 2:_ `equalTo('feature/my-*-branch')`
       - Checks whether or not the branch starts with the value `feature/my-` and that it ends with `-branch`. Anything is acceptable between the beginning and end of the branch where the `*` character is located. This should be a familiar concept to other systems and uses of this type of syntax.
     - _Example 3:_ `equalTo('feature/#-sample-branch')`
       - Returns valid if the branch name was `feature/my-sample-branch`. This checks whether or not the branch starts with the value `feature/` and ends with the value `-sample-branch`. Any text between the start and the end will be checked to see if it is a whole number of any digit size.
         - Returns valid if the name of the branch was `feature/12-sample-branch` or `feature/12345-sample-branch`.
         - Return as not valid if the name of the branch was `feature/10-20-sample-branch`. In this example, the branch ends with the value `-20-sample-branch`, not `-sample-branch`.
     - _Example 4:_ `equalTo('release/v#.#.#-*.#')`
       - Returns valid if the branch name was `release/v1.2.3-preview.4`.
2. `allLowerCase()` - Checks whether or not the branch name is all lower case.
    - _Example 1:_ `allLowerCase()`
      - Returns valid if the name of the branch was _**all-lower-case**_. This would return invalid if the name of the branch was _**not-all-LOWER-case**_. 
3. `allUpperCase()` - Checks whether or not if the branch name is all upper case.
    - _Example 1:_ `allUpperCase()`
      - Returns valid if the name of the branch was _**ALL-UPPER-CASE**_. This would return invalid if the name of the branch was _**NOT-ALL-upper-CASE**_.
</details>

<div align="center" style="font-weight:bold">

<br>

## **🪧 More Examples 🪧**
</div>

<div align="left">

The workflow fails when the feature branch does not start with the value `feature/`,
followed by a number and a hyphen, ends with any text, and is all lower case:

```yaml
name: Branch Validation Sample Workflow

on:
  workflow_dispatch:

jobs:
  Validate_Feature_Branch:
    name: Validate Feature Branch
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3

    - name: Validate Branch
      uses: KinsonDigital/BranchValidator@v1.0.0-preview.1
      with:
        branch-name: "feature/123-my-branch"
        validation-logic: "equalTo('feature/#-*') && allLowerCase()"
        fail-when-not-valid: true # Optional. Default is true.
```
</div>

<h4 id="manual-fail">Failing the workflow manually:</h4>

``` yaml
name: Branch Validation Sample Workflow

on:
  workflow_dispatch:

jobs:
  Validate_Feature_Branch:
    name: Validate Feature Branch
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3

    - name: Validate Branch
      id: validate-branch
      uses: KinsonDigital/BranchValidator@v1.0.0-preview.1
      with:
        branch-name: "release/v1.2.3-preview" # Missing the text '.4' at the end of the branch
        validation-logic: "equalTo('release/v#.#.#-preview.#')"
        fail-when-not-valid: false # This step will not fail the workflow

    - name: Fail Workflow # But this step will fail the workflow instead.
      if: ${{ steps.validate-branch.outputs.valid-branch }} = 'false'
      run: |
        Write-Host "::error::The branch is invalid!!";
        exit 1; # Fail the workflow
```

---

<h2 style="font-weight:bold;" align="center">🙏🏼 Contributing 🙏🏼</h2>

Interested in contributing? If so, click [here](https://github.com/KinsonDigital/.github/blob/main/docs/CONTRIBUTING.md) to learn how to contribute your time or [here](https://github.com/sponsors/KinsonDigital) if you are interested in contributing your funds via one-time or recurring donation.

<div align="center">

## **🔧 Maintainers 🔧**
</div>

![x-logo-dark-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-dark-mode.svg#gh-dark-mode-only)
![x-logo-light-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-light-mode.svg#gh-light-mode-only)
[Calvin Wilkinson](https://twitter.com/KDCoder) (KinsonDigital GitHub Organization - Owner)


![x-logo-dark-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-dark-mode.svg#gh-dark-mode-only)
![x-logo-light-mode](https://raw.githubusercontent.com/KinsonDigital/.github/main/Images/x-logo-16x16-light-mode.svg#gh-light-mode-only)
[Kristen Wilkinson](https://twitter.com/kswilky) (KinsonDigital GitHub Organization - Project Management, Documentation, Tester)
 
<br>

<h2 style="font-weight:bold;" align="center">🚔 Licensing And Governance 🚔</h2>

<div align="center">

[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg?style=flat)](https://github.com/KinsonDigital/.github/blob/main/docs/code_of_conduct.md)
[![GitHub](https://img.shields.io/github/license/kinsondigital/branchvalidator)](https://github.com/KinsonDigital/BranchValidator/blob/preview/LICENSE.md)  
</div>


This software is distributed under the very permissive MIT license and all dependencies are distributed under MIT-compatible licenses.
This project has adopted the code of conduct defined by the **Contributor Covenant** to clarify expected behavior in our community.
