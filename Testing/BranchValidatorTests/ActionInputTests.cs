// <copyright file="ActionInputTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidatorTests.Helpers;
using CommandLine;
using FluentAssertions;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable PossibleMultipleEnumeration
namespace BranchValidatorTests;

/// <summary>
/// Tests the <see cref="ActionInputs"/> class.
/// </summary>
public class ActionInputTests
{
    #region Prop Tests
    [Fact]
    public void Ctor_WhenConstructed_PropsHaveCorrectDefaultValuesAndDecoratedWithAttributes()
    {
        // Arrange & Act
        var inputs = new ActionInputs();

        // Assert
        inputs.BranchName.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.BranchName)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.BranchName))
            .AssertOptionAttrProps("branch-name", true, string.Empty, "The name of the GIT branch.");

        inputs.BranchName.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.ValidationLogic)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.ValidationLogic))
            .AssertOptionAttrProps("validation-logic", true, string.Empty, "The logic expression to use to validate the branch name.");

        inputs.BranchName.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.FailWhenNotValid)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.FailWhenNotValid))
            .AssertOptionAttrProps("fail-when-not-valid", false, true, "If true, will fail the job if the branch name is not valid.");
    }
    #endregion
}
