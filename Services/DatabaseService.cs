using System.Collections.ObjectModel;
using System.Text.Json;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    class DatabaseService : IDatabaseService
    {
        private readonly string _fileName = "tasks.json";
        private readonly string _filePath;

        public DatabaseService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, _fileName);
        }

        public async Task<bool> SaveImage(FileResult image, string imagePath)
        {
            try
            {
                using Stream sourceStream = await image.OpenReadAsync();
                using (FileStream fs = File.OpenWrite(imagePath))
                {
                    await sourceStream.CopyToAsync(fs);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTask(int? id)
        {
            var tasks = await GetAllTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task != null)
            {
                tasks.Remove(task);
                var reorganizedTasks = tasks.Select((t, index) =>
                {
                    t.Id = index + 1; 
                    return t;
                }).ToList();
                tasks = new(reorganizedTasks);

                try
                {
                    await SaveAllTasks(tasks);

                    if (!string.IsNullOrWhiteSpace(task.ImagePath) && File.Exists(task.ImagePath))
                    {
                        File.Delete(task.ImagePath);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }

            return false; // Task with given id not found
        }

        public async Task<ObservableCollection<TaskItemModel>> GetAllTasks()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return [];
                }

                using FileStream fs = File.OpenRead(_filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = await JsonSerializer.DeserializeAsync<ObservableCollection<TaskItemModel>>(fs, options);
                return result ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return [];
            }
        }

        public async Task<TaskItemModel> GetTask(int? id)
        {
            var tasks = await GetAllTasks();
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public async Task<bool> SetIsCompleted(int? id)
        {
            var tasks = await GetAllTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                try
                {
                    await SaveAllTasks(tasks);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }

            return false; // Task with given id not found
        }

        public async Task<bool> SaveTask(TaskItemModel task)
        {
            var tasks = await GetAllTasks();
            task.Id = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;
            tasks.Add(task);

            try
            {
                await SaveAllTasks(tasks);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        private async Task SaveAllTasks(ObservableCollection<TaskItemModel> tasks)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var tasksJson = JsonSerializer.Serialize(tasks, options);
            
            try
            {
                using StreamWriter sw = new(_filePath);
                await sw.WriteAsync(tasksJson);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}