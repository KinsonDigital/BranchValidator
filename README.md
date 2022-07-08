<h1 align="center">

**Branch Validator**
</h1>

<div align="center">

![](https://img.shields.io/github/workflow/status/KinsonDigital/BranchValidator/%F0%9F%9A%80Preview%20Release?label=QA%20Release%20%F0%9F%9A%80&logo=GitHub&style=plastic)
![](https://img.shields.io/github/workflow/status/KinsonDigital/BranchValidator/%F0%9F%9A%80Preview%20Release?label=Preview%20Release%20%F0%9F%9A%80&logo=GitHub&style=plastic)
![](https://img.shields.io/github/workflow/status/KinsonDigital/BranchValidator/%F0%9F%9A%80Production%20Release?label=Production%20Release%20%F0%9F%9A%80&logo=GitHub&style=plastic)

[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg?style=plastic)](code_of_conduct.md)

![Twitter URL](https://img.shields.io/twitter/url?label=Follow%20@KDCoder&logo=twitter&style=plastic&url=https%3A%2F%2Ftwitter.com%2FKDCoder)
[![Join our Discord](https://img.shields.io/badge/chat%20on-discord-7289DA?style=plastic)](https://discord.gg/qewu6fNgv7)

</div>


<div align="center">

### GIT Branch Name Validator GitHub Action
</div>

<div align="center">

## **What is it?**
</div>


This **GitHub Action** can be used to check if GIT branch names are valid using a simple expression in combination of some simple globbing syntax.  These expressions are simple functions that can be used in combination with AND (&&) and OR(||) logic to perform the validation.


<div align="center">
  <h3 style="font-weight:bold">Quick Example</h3>
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
    - uses: actions/checkout@v2

    - name: Validate Branch
      uses: KinsonDigital/BranchValidator@v1.0.0-preview.1
      with:
        branch-name: "${{ github.ref }}" # The current branch the workflow is running on
        validation-logic: "equalTo('main')"
        trim-from-start: "refs/heads/"
        fail-when-not-valid: true # Optional. Default is true.
```

<div align="right">
<a href="#examples">More Examples Below!! 👇🏼</a>
</div>

---

<div align="center"><h2 style="font-weight:bold">What is the point?</h2></div>

Well, it will partially depend on your project and its needs.  What it really comes down to is enforcing branch names and its structure.  In CI/CD systems, the name of the branch can determine how the system runs.  The workflows you have might depend on the name of a branch.  Maybe your project uses the github issue number as part of the branch name so the GIT side can be easily linked
back to the project management/issue side of things.  Maybe your build behaves differently depending on which branch it is building.

When these kinds of things are setup and the branch name is incorrect, they can cause issues and confusion with your build and release
system.  This GitHub action will help enforce these project standards to help keep things running smoothly.

The added benefit is to make things easier to understand on the GIT side of things.  To have the ability to enforce an issue number to exist in your branch name makes it easier for developers to find which branch belongs to which issue.  With this kind of enforcement, you can also setup automation to rely on the issue number existing.  This means you can easily setup an automation to check GitHub to verify that the issue even exists which would prevent accidental issue numbers being used in a branch name that does not exist. 

The applications of this are endless!!

---

<div align="center">

## **Action Inputs**
</div>

| Input Name            | Description                                                 | Required | Default Value      |
|-----------------------|:------------------------------------------------------------|:--------:|:------------------:|
| `branch-name`         | The name of the GIT branch.                                 | yes      |     N/A            |
| `validation-logic`    | The validation expression used to validate the branch name. | yes      |     N/A            |
| `trim-from-start`     | Trims the test from the beginning of a branch name.         | no       |  empty string      |
| `fail-when-not-valid` | Fails the job if the branch is not valid.                   | no       |     true           |



---

<div align="center">

## **Action Outputs**
</div>

| Input Name            | Description                                                 | Values Returned  |
|-----------------------|:------------------------------------------------------------|:----------------:|
| `branch-name`         | Returns a string value of 'true' or 'false' indicating whether or not the branch is valid.  | 'true' or 'false' |

---

<div align="center" style="font-weight:bold">

## **Validation Logic Expression Functions**
</div>

Below is a small list of the available expression functions that you can use in the value of the `validation-logic` input.  These expression functions can be used in combination using the `&&` and `||` operators.  
  - Example: equalTo('feature/my-*-branch') && allLowerCase()
    - This would check to see if the branch is equal to the value and that the entire branch is lower case.

<div align="center">

### **Expression Function List**
</div>

1. `equalTo(string)` - Checks to see if the branch name given is equal to the argument value of the function.  The argument value must be a string value that is surrounded by single or double quotes.  The quotes used must be the opposite of the quotes used for the entire input value.  Standard YAML syntax rules apply.
    The function value allows the use of 2 characters that provide globbing like behavior.  The 2 characters are `#` and `*` and can be used together as many times as needed.
     - Example 1: equalTo('main')
       - This will check if the branch is equal to the value of `main`.
     - Example 2: equalTo('feature/my-*-branch')
       - This will check if the branch starts with the value `feature/my-` and that it ends with `-branch`.  Anything is acceptable between the beginning and end of the branch where the `*` character is located.  This should be a familiar concept to other systems and uses of this type of syntax.
     - Example 3: equalTo('feature/#-sample-branch')
       - This would return valid if the branch name was `feature/my-sample-branch`.  This will check if the branch starts with the value `feature/` and ends with the value `-sample-branch`.  Any text between the start and the end will be checked to see if it is a whole number of any digit size.
         - This would return as valid if the name of the branch was `feature/12-sample-branch` or `feature/12345-sample-branch`.
         - This would return as not valid if the name of the branch was `feature/10-20-sample-branch`.  This is because in this example, the branch ends with the value `-20-sample-branch`, not `-sample-branch`.
     - Example: 4: equalTo('release/v#.#.#-*.#')
       - The would return valid if the branch name was `release/v1.2.3-preview.4`.
2. `allLowerCase()` - Check if the branch name is all lower case
    - Example 1: allLowerCase()
      - This would return valid if the name of the branch was `all-lower-case`.  This would return invalid if the name of the branch was `not-all-LOWER-case`.
3. `allUpperCase()` - Check if the branch name is all upper case
    - Example 1: allUpperCase()
      - This would return valid if the name of the branch was `ALL-UPPER-CASE`.  This would return invalid if the name of the branch was `NOT-ALL-upper-CASE`.

<div align="center" style="font-weight:bold">

## **Examples**
</div>

<div align="left">

Fail the workflow if the feature branch does not start with the value `feature/`,
followed by a number and a hyphen, ends with any text, and is all lower case.

```yaml
name: Branch Validation Sample Workflow

on:
  workflow_dispatch:

jobs:
  Validate_Feature_Branch:
    name: Validate Feature Branch
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2

    - name: Validate Branch
      uses: KinsonDigital/BranchValidator@v1.0.0-preview.1
      with:
        branch-name: "feature/123-my-branch"
        validation-logic: "equalTo('feature/#-*') && allLowerCase()"
        fail-when-not-valid: true # Optional. Default is true.
```
</div>

Fail the workflow manually

``` yaml
name: Branch Validation Sample Workflow

on:
  workflow_dispatch:

jobs:
  Validate_Feature_Branch:
    name: Validate Feature Branch
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2

    - name: Validate Branch
      id: validate-branch
      uses: KinsonDigital/BranchValidator@v1.0.0-preview.1
      with:
        branch-name: "release/v1.2.3-preview" # Missing the text '.4' at the end of the branch
        validation-logic: "equalTo('release/v#.#.#-preview.#')"
        fail-when-not-valid: false

    - name: Fail Workflow
      if: ${{ steps.validate-branch.outputs.valid-branch }} = 'false'
      run: |
        Write-Host "The branch is invalid!!";
        exit 1; # Fail the workflow
```

---

<div align="center">

## **Other Info**
</div>

<div align="left">

### **License**
- [MIT License - BranchValidator](https://github.com/KinsonDigital/BranchValidator/blob/preview/v1.0.0-preview.1/LICENSE)
</div>

<div align="left">

### **Maintainer**
</div>

- [Calvin Wilkinson](https://github.com/CalvinWilkinson) (Owner and main contributor of the GitHub organization [KinsonDigital](https://github.com/KinsonDigital))
  - [Got Nuget](https://github.com/KinsonDigital/BranchValidator) is used in various projects for this organization with great success.
- Click [here](https://github.com/KinsonDigital/BranchValidator/issues/new/choose) to report any issues for this GitHub action!!

<div align="center">

## **Please help by [donating](https://github.com/sponsors/KinsonDigital)!!🙏**
</div>

Any donations for the work I do is greatly appreciated and helps me do this full time.  I work on many things such as GitHub actions, game development frameworks and libraries, and also provide C#/dotnet technology training.

<div align="right">
<a href="#what-is-it">Back to the top!👆🏼</a>
</div>
