namespace ToDoApp.Interfaces
{
    public interface ILocalStorageService
    {
        void Save<T>(string key, T value);
        T Get<T>(string key);
        void Remove(string key);
    }
}