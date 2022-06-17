namespace ScriptGenerator.Services;

public class StaticMethodMutation : IStringMutation
{
    public string Mutate(string value) => value.Replace("public bool", "public static bool");
}
