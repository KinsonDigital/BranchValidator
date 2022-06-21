namespace BranchValidator.Services.Interfaces;

public interface IScriptService<T>
{
    T Execute(string scriptSrc);
}
