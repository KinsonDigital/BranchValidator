// <copyright file="GitHubActionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Exceptions;
using BranchValidator.Services.Interfaces;
using BranchValidatorTests.Helpers;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests;

/// <summary>
/// Tests the <see cref="GitHubAction"/> class.
/// </summary>
public class GitHubActionTests
{
    private readonly Mock<IGitHubConsoleService> mockConsoleService;
    private readonly Mock<IActionOutputService> mockActionOutputService;
    private readonly Mock<IExpressionExecutorService> mockExpressionExecutorService;
    private readonly Mock<IFunctionService> mockFunctionService;
    private readonly Mock<IBranchNameObservable> mockBranchNameObservable;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubActionTests"/> class.
    /// </summary>
    public GitHubActionTests()
    {
        this.mockConsoleService = new Mock<IGitHubConsoleService>();
        this.mockActionOutputService = new Mock<IActionOutputService>();
        this.mockExpressionExecutorService = new Mock<IExpressionExecutorService>();

        this.mockFunctionService = new Mock<IFunctionService>();
        this.mockFunctionService.SetupGet(p => p.FunctionSignatures)
            .Returns(() => new[] { "equalTo(value: string)" }.ToReadOnlyCollection());

        this.mockBranchNameObservable = new Mock<IBranchNameObservable>();
    }

    #region Method Tests
    [Fact]
    public async void Run_WhenInvoked_ShowsWelcomeMessage()
    {
        var inputs = CreateInputs();
        var action = CreateAction();

        // Act
        await action.Run(inputs, _ => { }, _ => { });

        // Assert
        this.mockConsoleService.VerifyOnce(m => m.WriteLine("Welcome To The BranchValidator GitHub Action!!"));
        this.mockConsoleService.VerifyOnce(m => m.BlankLine());
    }

    [Fact]
    public async void Run_WhenInvoked_PrintsFunctionList()
    {
        // Arrange
        const string functionListMsg = "equalTo(value: string)";

        var inputs = CreateInputs();
        var action = CreateAction();

        // Act
        await action.Run(inputs, _ => { }, _ => { });

        // Assert
        this.mockConsoleService.VerifyOnce(m => m.WriteGroup("Available Functions", new[] { functionListMsg }));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void Run_WithEmptyOrNullBranchNameInput_ThrowsException(string branchName)
    {
        // Arrange
        var inputs = CreateInputs(branchName: branchName);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, _ => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<InvalidActionInput>()
            .WithMessage("The 'branch-name' action input cannot be null or empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void Run_WithEmptyOrNullValidationLogicInput_ThrowsException(string validationLogic)
    {
        // Arrange
        var inputs = CreateInputs(validationLogic: validationLogic);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, _ => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<InvalidActionInput>()
            .WithMessage("The 'validation-logic' action input cannot be null or empty.");
    }

    [Theory]
    [InlineData("valid-branch", "The branch 'valid-branch' is valid.", true)]
    [InlineData("invalid-branch", "The branch 'invalid-branch' is invalid.", false)]
    public async void Run_WithValidBranch_CorrectlySetsOutput(
        string branchName,
        string expectedMsgResult,
        bool expectedValidResult)
    {
        // Arrange
        var inputs = CreateInputs(validationLogic: $"funA('{branchName}')", branchName: branchName, failWhenNotValid: false);
        this.mockExpressionExecutorService.Setup(m => m.Execute(inputs.ValidationLogic, inputs.BranchName))
            .Returns((expectedValidResult, expectedMsgResult));
        var action = CreateAction();

        // Act
        await action.Run(inputs, _ => { }, _ => { });

        // Assert
        this.mockConsoleService.Verify(m => m.BlankLine(), Times.Exactly(2));
        this.mockConsoleService.VerifyOnce(m => m.WriteLine(expectedMsgResult));
        this.mockActionOutputService.VerifyOnce(m => m.SetOutputValue("valid-branch", expectedValidResult.ToString().ToLower()));
        this.mockBranchNameObservable.VerifyOnce(m => m.PushNotification(branchName));
        this.mockBranchNameObservable.VerifyOnce(m => m.UnsubscribeAll());
    }

    [Fact]
    public async void Run_WhenFailingActionForInvalidBranchesWithInvalidBranch_FailsActionWithException()
    {
        // Arrange
        var inputs = CreateInputs();
        this.mockExpressionExecutorService.Setup(m => m.Execute(inputs.ValidationLogic, inputs.BranchName))
            .Returns((false, "failure-exception-msg"));
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, _ => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("failure-exception-msg");
    }

    [Fact]
    public async void Run_WhenNotFailingActionForInvalidBranches_DoesNotFailAction()
    {
        // Arrange
        var inputs = CreateInputs(failWhenNotValid: false);
        this.mockExpressionExecutorService.Setup(m => m.Execute(inputs.ValidationLogic, inputs.BranchName))
            .Returns((false, "branch invalid"));
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, _ => { }, e => throw e);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async void Run_WhenFailingActionForInvalidBranchesWithValidBranch_DoesNotFailAction()
    {
        // Arrange
        var inputs = CreateInputs(failWhenNotValid: true);
        this.mockExpressionExecutorService.Setup(m => m.Execute(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((true, "branch valid"));
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, _ => { }, e => throw e);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async void Run_WhenInvoked_ExecutesOnCompleted()
    {
        // Arrange
        var onCompletedExecuted = false;
        var inputs = CreateInputs(failWhenNotValid: false);
        var action = CreateAction();

        // Act
        await action.Run(inputs, _ => onCompletedExecuted = true, _ => { });

        // Assert
        onCompletedExecuted.Should().BeTrue();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ActionInputs"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ActionInputs CreateInputs(
        string branchName = "test-branch",
        string validationLogic = "equalTo('test-branch')",
        bool? failWhenNotValid = true) => new ()
    {
        BranchName = branchName,
        ValidationLogic = validationLogic,
        FailWhenNotValid = failWhenNotValid,
    };

    /// <summary>
    /// Creates a new instance of <see cref="GitHubAction"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private GitHubAction CreateAction()
        => new (this.mockConsoleService.Object,
            this.mockActionOutputService.Object,
            this.mockExpressionExecutorService.Object,
            this.mockFunctionService.Object,
            this.mockBranchNameObservable.Object);
}
