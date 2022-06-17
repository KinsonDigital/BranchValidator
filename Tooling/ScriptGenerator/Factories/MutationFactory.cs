using ScriptGenerator.Services;

namespace ScriptGenerator.Factories;

public static class MutationFactory
{
    private static IStringMutation? staticMethodMutation;
    private static IStringMutation? thisRefToStaticRefMutation;
    private static IStringMutation? removeInheritCodeDocMutation;
    private static IStringMutation? removeExpressionAttributeMutation;

    public static IStringMutation[] CreateMutations()
    {
        var result = new List<IStringMutation>();

        staticMethodMutation ??= new StaticMethodMutation();
        thisRefToStaticRefMutation ??= new ThisRefToStaticRefMutation();
        removeInheritCodeDocMutation ??= new RemoveInheritCodeDocsMutation();
        removeExpressionAttributeMutation ??= new RemoveExpressionAttributeMutation();

        result.Add(staticMethodMutation);
        result.Add(thisRefToStaticRefMutation);
        result.Add(removeInheritCodeDocMutation);
        result.Add(removeExpressionAttributeMutation);

        return result.ToArray();
    }
}
