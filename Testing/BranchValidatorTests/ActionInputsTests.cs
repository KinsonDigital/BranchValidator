// <copyright file="ActionInputsTests.cs" company="KinsonDigital">
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
public class ActionInputsTests
{
    #region Contructor Tests
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

        inputs.ValidationLogic.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.ValidationLogic)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.ValidationLogic))
            .AssertOptionAttrProps("validation-logic", true, string.Empty, "The logic expression to use to validate the branch name.");

        inputs.TrimFromStart.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.TrimFromStart)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.TrimFromStart))
            .AssertOptionAttrProps("trim-from-start", false, string.Empty, "The value to trim from the start of the branch.  This is not case sensitive.");

        inputs.FailWhenNotValid.Should().BeTrue();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.FailWhenNotValid)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.FailWhenNotValid))
            .AssertOptionAttrProps("fail-when-not-valid", false, true, "If true, will fail the job if the branch name is not valid.");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void BranchName_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        inputs.BranchName = "test-branch";

        // Assert
        inputs.BranchName.Should().Be("test-branch");
    }

    [Fact]
    public void ValidationLogic_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        inputs.ValidationLogic = "test-logic";

        // Assert
        inputs.ValidationLogic.Should().Be("test-logic");
    }

    [Fact]
    public void TrimFromStart_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        inputs.TrimFromStart = "trim-test";

        // Assert
        inputs.TrimFromStart.Should().Be("trim-test");
    }

    [Fact]
    public void FailWhenNotValid_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();
        var expected = !inputs.FailWhenNotValid;

        // Act
        inputs.FailWhenNotValid = !inputs.FailWhenNotValid;

        // Assert
        inputs.FailWhenNotValid.Should().Be(expected);
    }
    #endregion
}
