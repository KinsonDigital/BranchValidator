// <copyright file="SampleTestClass.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidatorTests;

public class SampleTestClass
{
    public bool MethodThatReturnsBool() => true;

    public string MethodThatDoesNotReturnBool() => string.Empty;

    public bool MethodWithSingleParam(string p1) => true;

    public bool MethodWith2ParamsDiffTypes(string p1, int p2) => true;

    public bool MethodWith2ParamsSameTypes(string p1, string p2) => true;

    public bool MethodWith3Params(string p1, int p2, float p3) => true;
}
