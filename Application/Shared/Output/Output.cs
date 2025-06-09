namespace Application.Shared.Output
{
    public class Output<T>
    {
        public bool IsValid { get; set; }
        private T Result { get; set; }
        private List<string> Messages { get; set; }
        private List<string> ErrorMessages { get; set; }

        public Output()
        {
            IsValid = true;
        }

        public void AddResult(T result)
        {
            Result = result;
        }

        public T GetResult()
        {
            return Result;
        }

        public void AddErrorMessage(string errorMessage)
        {
            IsValid = false;
            ErrorMessages ??= new List<string>();
            ErrorMessages.Add(errorMessage);
        }

        public void AddErrorMessages(IEnumerable<string> errorMessages)
        {
            IsValid = false;
            ErrorMessages ??= new List<string>();
            ErrorMessages.AddRange(errorMessages);
        }

        public void AddMessage(string message)
        {
            if(!IsValid)
                return;

            Messages ??= new List<string>();
            Messages.Add(message);
        }

        public List<string> GetMessages()
        {
            return Messages ?? new List<string>();
        }

        public List<string> GetErrorMessages()
        {
            return ErrorMessages ?? new List<string>();
        }
    }
}
