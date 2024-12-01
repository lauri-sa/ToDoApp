using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    class ResourceProviderService : IResourceProviderService
    {
        public Dictionary<string, T> GetResourcesWithPrefix<T>(string preFix)
        {
            if (Application.Current.Resources == null)
                return new Dictionary<string, T>();

            return Application.Current.Resources.MergedDictionaries
                .SelectMany(dict => dict.Keys)
                .OfType<string>()
                .Where(key => key.StartsWith(preFix))
                .ToDictionary(
                    key => key.Replace(preFix, string.Empty),
                    key => GetResource<T>(key)
                );
        }

        public T GetResource<T>(string resourceName)
        {
            if (Application.Current.Resources.TryGetValue(resourceName, out var resource) && resource is T typedResource)
            {
                return typedResource;
            }

            throw new InvalidOperationException($"Resource '{resourceName}' not found or is not of type {typeof(T)}.");
        }
    }
}