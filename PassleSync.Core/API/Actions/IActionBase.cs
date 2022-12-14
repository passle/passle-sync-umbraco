namespace PassleSync.Core.API.Actions
{
    public interface IActionBase<T> where T : IActionModel
    {
        void Execute(T model);
    }
}
