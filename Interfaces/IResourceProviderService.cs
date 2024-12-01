namespace ToDoApp.Interfaces
{
    public interface IResourceProviderService
    {
        T GetResource<T>(string resourceName);

        Dictionary<string, T> GetResourcesWithPrefix<T>(string preFix);
    }
}