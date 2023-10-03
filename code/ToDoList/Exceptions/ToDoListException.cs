using WebToDoList.enums;

namespace WebToDoList.Exceptions
{
    public class ToDoListException : Exception
    {
        private const string TaskNotFoundMessage = "The task with the given id could not be found.";

        private const string InternalServerErrorMessage = "An error occurred and the task could not be added correctly.";

        private const string DefaultMessage = "An error occurred.";

        private const string NotDeletedMessage = "The task could not be deleted.";

        private const string TaskNullMessage = "The task is null.";

        public ExceptionToDoList ExceptionType { get; set; }

        public ToDoListException(ExceptionToDoList exceptionType) : base(GetExceptionMessage(exceptionType))
        {
            ExceptionType = exceptionType;
        }

        private static string GetExceptionMessage(ExceptionToDoList exceptionType)
        {
            switch (exceptionType)
            {
                case ExceptionToDoList.TaskNotFound:
                    return TaskNotFoundMessage;
                case ExceptionToDoList.BadRequest:
                    return InternalServerErrorMessage;
                case ExceptionToDoList.NotDeleted:
                    return NotDeletedMessage;
                case ExceptionToDoList.TaskNull:
                    return TaskNullMessage;
                default:
                    return DefaultMessage;
            }
        }
    }
}