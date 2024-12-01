using System.ComponentModel;
using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    class ValidationService : IValidationService
    {
        private System.Timers.Timer? _messageTimer;

        private string _validationMessage = string.Empty;
        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                _validationMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationMessage)));
                ClearValidationMessage();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ClearValidationMessage()
        {
            _messageTimer?.Stop();
            _messageTimer = new System.Timers.Timer(5000) { AutoReset = false };
            _messageTimer.Elapsed += (sender, e) =>
            {
                ValidationMessage = string.Empty;
                _messageTimer?.Dispose();
                _messageTimer = null;
            };
            _messageTimer.Start();
        }

        public (bool IsInputValid, string Message) ValidateInputs(string title, string description, DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return (false, "Task title is required");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return (false, "Task description is required");
            }

            if (dueDate < DateTime.Today)
            {
                return (false, "Due date must be greater than or equal to today");
            }

            return (true, string.Empty);
        }
    }
}
