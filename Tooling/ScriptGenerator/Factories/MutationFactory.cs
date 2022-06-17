using ScriptGenerator.Services;

namespace ScriptGenerator.Factories;

public static class MutationFactory
{
    private static IStringMutation? staticMethodMutation;
    private static IStringMutation? thisRefToStaticRefMutation;
    private static IStringMutation? removeInheritCodeDocMutation;

    public static IStringMutation[] CreateMutations()
    {
        var result = new List<IStringMutation>();

        staticMethodMutation ??= new StaticMethodMutation();
        thisRefToStaticRefMutation ??= new ThisRefToStaticRefMutation();
        removeInheritCodeDocMutation ??= new RemoveInheritCodeDocs();

        result.Add(staticMethodMutation);
        result.Add(thisRefToStaticRefMutation);
        result.Add(removeInheritCodeDocMutation);

        return result.ToArray();
    }
}
