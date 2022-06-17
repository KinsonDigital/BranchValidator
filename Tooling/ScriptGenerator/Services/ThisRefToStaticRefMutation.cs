namespace ScriptGenerator.Services;

public class ThisRefToStaticRefMutation : IStringMutation
{
    public string Mutate(string value) => value.Replace("this.branchName", "BranchName");
}
