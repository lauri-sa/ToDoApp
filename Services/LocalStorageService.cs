using System.Text.Json;
using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    class LocalStorageService : ILocalStorageService
    {
        public void Save<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            Preferences.Set(key, json);
        }

        public T Get<T>(string key)
        {
            var json = Preferences.Get(key, string.Empty);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
        }

        public void Remove(string key)
        {
            Preferences.Remove(key);
        }
    }
}