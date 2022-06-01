<h1 align="center">

**Branch Validator**
</h1>

<div align="center">

### GitHub Action for checking if GIT branch names are valid using simple expressions.

<div hidden>TODO: ADD BADGES HERE</div>

</div>


<div align="center">

## **What is it?**
</div>


This **GitHub Action** can be used to check if a GIT branch name is valid using expressions.  These expressions use simple functions
with AND (&&) and OR(||) logic to perform syntax validation.


<div align="center"><h3 style="font-weight:bold">Quick Example</h3></div>


```yaml
# TODO: Add sample here
```

<div align="left">
<a href="#examples">More Examples Below!! 👇🏼</a>
</div>

---

<div align="center"><h2 style="font-weight:bold">What does it do?</h2></div>

It is simple!  It goes out to [nuget.org](https://www.nuget.org) and checks to see if a NuGet package of a particular version exists.  If it does, it returns and output value of `"true"`, if not, then it returns `"false"`.
Thats it!!

---

<div align="center">

## **Action Inputs**
</div>

TODO: Show action inputs in table

| Input Name            | Description                                                 | Required | Default Value |
|-----------------------|:------------------------------------------------------------|:---:|:-------------:|
| `branch-name`         | The name of the GIT branch.                                 | yes |      N/A      |
| `validation-logic`    | The validation expression used to validate the branch name. | yes |      N/A      |
| `fail-when-not-valid` | Fails the job if the branch is not valid.                   | no |     true      |

---

<div align="center" style="font-weight:bold">

## **Examples**
</div>

<div align="center">

### **Fail the job if the package is not found**
</div>

``` yaml
# TODO: Add sample here
```

---

<div align="center">

## **Other Info**
</div>

<div align="left">

### License
- [MIT License - BranchValidator](https://github.com/KinsonDigital/BranchValidator/blob/preview/v1.0.0-preview.1/LICENSE)
</div>

<div align="left">

### Maintainer
</div>

- [Calvin Wilkinson](https://github.com/CalvinWilkinson) (Owner and main contributor of the GitHub organization [KinsonDigital](https://github.com/KinsonDigital))
  - [Got Nuget](https://github.com/KinsonDigital/BranchValidator) is used in various projects for this organization with great success.
- Click [here](https://github.com/KinsonDigital/BranchValidator/issues/new/choose) to report any issues for this GitHub action!!

<div align="right">
<a href="#what-is-it">Back to the top!👆🏼</a>
</div>
