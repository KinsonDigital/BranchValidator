// <copyright file="FunctionDefinitionsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidatorTests.Helpers;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests;

/// <summary>
/// Tests the <see cref="FunctionDefinitions"/> class.
/// </summary>
public class FunctionDefinitionsTests
{
    private readonly Mock<IDisposable> unsubscriber;
    private readonly Mock<IBranchNameObservable> mockBranchNameObservable;
    private IStringObserver observer = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionDefinitionsTests"/> class.
    /// </summary>
    public FunctionDefinitionsTests()
    {
        this.unsubscriber = new Mock<IDisposable>();
        this.mockBranchNameObservable = new Mock<IBranchNameObservable>();
        this.mockBranchNameObservable.Setup(m => m.Subscribe(It.IsAny<IStringObserver>()))
            .Returns(() => this.unsubscriber.Object)
            .Callback<IStringObserver>(observerValue =>
            {
                observerValue.Should().NotBeNull();

                this.observer = observerValue;
            });
    }

    #region Constructor Tests
    [Fact]
    public void Observer_WhenOnCompletedIsInvoked_DisposesUnsubscriber()
    {
        // Arrange
        var unused = CreateDefinitions();

        // Act
        this.observer.OnCompleted();
        this.observer.OnCompleted();

        // Assert
        this.unsubscriber.VerifyOnce(m => m.Dispose());
    }

    [Fact]
    public void Observer_WhenOnOnErrorIsInvoked_DisposesUnsubscriber()
    {
        // Arrange
        var unused = CreateDefinitions();

        // Act
        this.observer.OnError(new Exception("test-exception"));
        this.observer.OnError(new Exception("test-exception"));

        // Assert
        this.unsubscriber.VerifyOnce(m => m.Dispose());
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Dispose_WhenInvoked_DisposesOfObject()
    {
        // Arrange
        var definitions = CreateDefinitions();

        // Act
        definitions.Dispose();
        definitions.Dispose();

        // Assert
        this.unsubscriber.VerifyOnce(m => m.Dispose());
    }

    [Theory]
    [InlineData("my-branch", "my-branch", true)]
    [InlineData(null, "", true)]
    [InlineData("", null, true)]
    [InlineData("my-branch", "other-branch", false)]
    [InlineData(null, "my-branch", false)]
    [InlineData("", "my-branch", false)]
    [InlineData("my-branch", null, false)]
    [InlineData("my-branch", "", false)]
    public void EqualTo_WhenInvoked_ReturnsCorrectResult(
        string value,
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.EqualTo(value);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(8, "", false)]
    [InlineData(4, "feature/123-my-branch", false)]
    [InlineData(400, "feature/123-my-branch", false)]
    [InlineData(8, "feature/123-my-branch", true)]
    public void IsCharNum_WhenInvoked_ReturnsCorrectResult(
        uint charPos,
        string branchName,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.IsCharNum(charPos);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 0, 10, false)]
    [InlineData("", 0, 10, false)]
    [InlineData("feature/123-test-branch", 8, 12, false)]
    [InlineData("feature/test-branch", 12, 3000, false)]
    [InlineData("feature/123-test-branch", 8, 11, false)]
    [InlineData("feature/123-test-branch", 8, 10, true)]
    [InlineData("feature/0123456789", 12, 3000, true)]
    public void IsSectionNum_WhenInvokedWithStartAndEndPosParams_ReturnsCorrectResult(
        string branchName,
        uint startPos,
        uint endPos,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.IsSectionNum(startPos, endPos);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 0, "-", false)]
    [InlineData("", 0, "-", false)]
    [InlineData("feature/123-test-branch", 8, null, false)]
    [InlineData("feature/123-test-branch", 8, "", false)]
    [InlineData("feature/123testbranch", 8, "-", false)]
    [InlineData("feature/123-testbranch", 11, "-", false)]
    [InlineData("feature/123test-branch", 8, "-", false)]
    [InlineData("feature/123-test-branch", 3000, "-", false)]
    [InlineData("feature/123-test-branch", 8, "-", true)]
    [InlineData("feature/123-test-branch", 8, "-other-characters", true)]
    [InlineData("0123456789-", 0, "-", true)]
    public void IsSectionNum_WhenInvokedWithStartPosAndUpToCharParams_ReturnsCorrectResult(
        string branchName,
        uint startPos,
        string upToChar,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.IsSectionNum(startPos, upToChar);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("feature/123-test-branch", "123-test", true)]
    [InlineData("feature/123-test-branch", "is-not-contained", false)]
    public void Contains_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.Contains(value);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("feature/123-test-branch", "123-test", false)]
    [InlineData("feature/123-test-branch", "is-not-contained", true)]
    public void NotContains_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.NotContains(value);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("feature/123-branch", "test", 1, false)]
    [InlineData("feature/123-branch", "a", 2, true)]
    [InlineData("feature/123-test-branch-test", "test", 4, false)]
    [InlineData("feature/123-test-branch", "test", 1, true)]
    [InlineData("feature/123-test-branch-test", "test", 2, true)]
    public void ExistTotal_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        uint total,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.ExistTotal(value, total);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("feature/123-test-branch", "123", 2, true)]
    [InlineData("feature/123-branch", "test", 200, true)]
    [InlineData("feature/123-test-branch", "123", 1, false)]
    public void ExistsLessThan_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        uint total,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.ExistsLessThan(value, total);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("feature/123-test-123-branch-123", "123", 2, true)]
    [InlineData("feature/123-test-branch", "123", 1, false)]
    public void ExistsGreaterThan_WhenInvoked_ReturnsCorrectResult(
        string branchName,
        string value,
        uint total,
        bool expected)
    {
        // Arrange
        var definitions = CreateDefinitions();
        this.observer.OnNext(branchName);

        // Act
        var actual = definitions.ExistsGreaterThan(value, total);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FunctionDefinitions"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FunctionDefinitions CreateDefinitions() => new (this.mockBranchNameObservable.Object);
}
