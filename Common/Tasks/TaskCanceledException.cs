namespace Gamefreak130.Common.Tasks
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an exception used to communicate task cancellation.
    /// </summary>
    [Serializable]
    public class TaskCanceledException : OperationCanceledException
    {
        [NonSerialized]
        private readonly AwaitableTask _canceledTask; // The task which has been canceled.

        private const string kDefaultMessage = "A task was canceled.";

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Threading.Tasks.TaskCanceledException"/> class.
        /// </summary>
        public TaskCanceledException() : base(kDefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Threading.Tasks.TaskCanceledException"/>
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TaskCanceledException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Threading.Tasks.TaskCanceledException"/> class
        /// with a reference to the <see cref="System.Threading.Tasks.Task"/> that has been canceled.
        /// </summary>
        /// <param name="task">A task that has been canceled.</param>
        public TaskCanceledException(AwaitableTask task) :
            base(kDefaultMessage)
        {
            _canceledTask = task;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Threading.Tasks.TaskCanceledException"/>
        /// class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param>
        protected TaskCanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets the task associated with this exception.
        /// </summary>
        public AwaitableTask Task => _canceledTask;
    }
}