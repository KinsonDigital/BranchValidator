// <copyright file="SampleTestClass.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace BranchValidatorTests;

[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must stay public for reflection to pick it up.")]
public class SampleTestClass
{
    public bool MethodThatReturnsBool() => true;

    public string MethodThatDoesNotReturnBool() => string.Empty;

    public bool MethodWithSingleParam(string p1) => true;

    public bool MethodWith2ParamsDiffTypes(string p1, int p2) => true;

    public bool MethodWith3Params(string p1, int p2, float p3) => true;

    public bool MethodOverload(int p1, int p2) => true;

    public bool MethodOverload(int p1, string p2) => true;
}
