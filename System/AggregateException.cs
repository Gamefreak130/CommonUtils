namespace System
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>Represents one or more errors that occur during application execution.</summary>
    /// <remarks>
    /// <see cref="AggregateException"/> is used to consolidate multiple failures into a single, throwable
    /// exception object.
    /// </remarks>
    [Serializable]
    public class AggregateException : Exception
    {
        private readonly Exception[] _innerExceptions; // Complete set of exceptions.
        private ReadOnlyCollection<Exception> _rocView; // separate from _innerExceptions to enable trimming if InnerExceptions isn't used

        private const string kAggregateExceptionDefaultMessage = "One or more errors occurred.";
        private const string kAggregateExceptionInnerExceptionNull = "An element of innerExceptions was null.";

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class.
        /// </summary>
        public AggregateException()
            : this(kAggregateExceptionDefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AggregateException(string message)
            : base(message)
        {
            _innerExceptions = List<Exception>.EmptyArray;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="innerException"/> argument
        /// is null.</exception>
        public AggregateException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }

            _innerExceptions = new[] { innerException };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with
        /// references to the inner exceptions that are the cause of this exception.
        /// </summary>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="innerExceptions"/> argument
        /// is null.</exception>
        /// <exception cref="System.ArgumentException">An element of <paramref name="innerExceptions"/> is
        /// null.</exception>
        public AggregateException(IEnumerable<Exception> innerExceptions) :
            this(kAggregateExceptionDefaultMessage, innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with
        /// references to the inner exceptions that are the cause of this exception.
        /// </summary>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="innerExceptions"/> argument
        /// is null.</exception>
        /// <exception cref="System.ArgumentException">An element of <paramref name="innerExceptions"/> is
        /// null.</exception>
        public AggregateException(params Exception[] innerExceptions) :
            this(kAggregateExceptionDefaultMessage, innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with a specified error
        /// message and references to the inner exceptions that are the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="innerExceptions"/> argument
        /// is null.</exception>
        /// <exception cref="System.ArgumentException">An element of <paramref name="innerExceptions"/> is
        /// null.</exception>
        public AggregateException(string message, IEnumerable<Exception> innerExceptions)
            : this(message, innerExceptions == null ? null : new List<Exception>(innerExceptions).ToArray(), cloneExceptions: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with a specified error
        /// message and references to the inner exceptions that are the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="innerExceptions"/> argument
        /// is null.</exception>
        /// <exception cref="System.ArgumentException">An element of <paramref name="innerExceptions"/> is
        /// null.</exception>
        public AggregateException(string message, params Exception[] innerExceptions) :
            this(message, innerExceptions, cloneExceptions: true)
        {
        }

        private AggregateException(string message, Exception[] innerExceptions, bool cloneExceptions) :
            base(message, innerExceptions?.Length > 0 ? innerExceptions[0] : null)
        {
            if (innerExceptions == null)
            {
                throw new ArgumentNullException(nameof(innerExceptions));
            }

            _innerExceptions = cloneExceptions ? new Exception[innerExceptions.Length] : innerExceptions;

            for (int i = 0; i < _innerExceptions.Length; i++)
            {
                _innerExceptions[i] = innerExceptions[i];

                if (innerExceptions[i] == null)
                {
                    throw new ArgumentException(kAggregateExceptionInnerExceptionNull);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination. </param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="info"/> argument is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The exception could not be deserialized correctly.</exception>
        protected AggregateException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            Exception[] innerExceptions = info.GetValue("InnerExceptions", typeof(Exception[])) as Exception[]; // Do not rename (binary serialization)
            if (innerExceptions is null)
            {
                throw new SerializationException("The serialization stream contains no inner exceptions.");
            }

            _innerExceptions = innerExceptions;
        }

        /// <summary>
        /// Sets the <see cref="System.Runtime.Serialization.SerializationInfo"/> with information about
        /// the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination. </param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="info"/> argument is null.</exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("InnerExceptions", _innerExceptions, typeof(Exception[])); // Do not rename (binary serialization)
        }

        /// <summary>
        /// Returns the <see cref="System.AggregateException"/> that is the root cause of this exception.
        /// </summary>
        public override Exception GetBaseException()
        {
            // Returns the first inner AggregateException that contains more or less than one inner exception

            // Recursively traverse the inner exceptions as long as the inner exception of type AggregateException and has only one inner exception
            Exception back = this;
            AggregateException backAsAggregate = this;
            while (backAsAggregate != null && backAsAggregate.InnerExceptions.Count == 1)
            {
                back = back!.InnerException;
                backAsAggregate = back as AggregateException;
            }
            return back!;
        }

        /// <summary>
        /// Gets a read-only collection of the <see cref="System.Exception"/> instances that caused the
        /// current exception.
        /// </summary>
        public ReadOnlyCollection<Exception> InnerExceptions => _rocView ??= new ReadOnlyCollection<Exception>(_innerExceptions);


        /// <summary>
        /// Invokes a handler on each <see cref="System.Exception"/> contained by this <see
        /// cref="AggregateException"/>.
        /// </summary>
        /// <param name="predicate">The predicate to execute for each exception. The predicate accepts as an
        /// argument the <see cref="System.Exception"/> to be processed and returns a Boolean to indicate
        /// whether the exception was handled.</param>
        /// <remarks>
        /// Each invocation of the <paramref name="predicate"/> returns true or false to indicate whether the
        /// <see cref="System.Exception"/> was handled. After all invocations, if any exceptions went
        /// unhandled, all unhandled exceptions will be put into a new <see cref="AggregateException"/>
        /// which will be thrown. Otherwise, the <see cref="Handle"/> method simply returns. If any
        /// invocations of the <paramref name="predicate"/> throws an exception, it will halt the processing
        /// of any more exceptions and immediately propagate the thrown exception as-is.
        /// </remarks>
        /// <exception cref="AggregateException">An exception contained by this <see
        /// cref="AggregateException"/> was not handled.</exception>
        /// <exception cref="System.ArgumentNullException">The <paramref name="predicate"/> argument is
        /// null.</exception>
        public void Handle(Func<Exception, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            List<Exception> unhandledExceptions = null;
            for (int i = 0; i < _innerExceptions.Length; i++)
            {
                // If the exception was not handled, lazily allocate a list of unhandled
                // exceptions (to be rethrown later) and add it.
                if (!predicate(_innerExceptions[i]))
                {
                    unhandledExceptions ??= new List<Exception>();
                    unhandledExceptions.Add(_innerExceptions[i]);
                }
            }

            // If there are unhandled exceptions remaining, throw them.
            if (unhandledExceptions != null)
            {
                throw new AggregateException(Message, unhandledExceptions.ToArray(), cloneExceptions: false);
            }
        }


        /// <summary>
        /// Flattens the inner instances of <see cref="AggregateException"/> by expanding its contained <see cref="Exception"/> instances
        /// into a new <see cref="AggregateException"/>
        /// </summary>
        /// <returns>A new, flattened <see cref="AggregateException"/>.</returns>
        /// <remarks>
        /// If any inner exceptions are themselves instances of
        /// <see cref="AggregateException"/>, this method will recursively flatten all of them. The
        /// inner exceptions returned in the new <see cref="AggregateException"/>
        /// will be the union of all of the inner exceptions from exception tree rooted at the provided
        /// <see cref="AggregateException"/> instance.
        /// </remarks>
        public AggregateException Flatten()
        {
            // Initialize a collection to contain the flattened exceptions.
            List<Exception> flattenedExceptions = new();

            // Create a list to remember all aggregates to be flattened, this will be accessed like a FIFO queue
            var exceptionsToFlatten = new List<AggregateException> { this };
            int nDequeueIndex = 0;

            // Continue removing and recursively flattening exceptions, until there are no more.
            while (exceptionsToFlatten.Count > nDequeueIndex)
            {
                // dequeue one from exceptionsToFlatten
                ReadOnlyCollection<Exception> currentInnerExceptions = exceptionsToFlatten[nDequeueIndex++].InnerExceptions;

                for (int i = 0; i < currentInnerExceptions.Count; i++)
                {
                    Exception currentInnerException = currentInnerExceptions[i];

                    if (currentInnerException == null)
                    {
                        continue;
                    }

                    // If this exception is an aggregate, keep it around for later.  Otherwise,
                    // simply add it to the list of flattened exceptions to be returned.
                    if (currentInnerException is AggregateException currentInnerAsAggregate)
                    {
                        exceptionsToFlatten.Add(currentInnerAsAggregate);
                    }
                    else
                    {
                        flattenedExceptions.Add(currentInnerException);
                    }
                }
            }

            return new AggregateException(GetType() == typeof(AggregateException) ? base.Message : Message, flattenedExceptions.ToArray(), cloneExceptions: false);
        }

        /// <summary>Gets a message that describes the exception.</summary>
        public override string Message
        {
            get
            {
                if (_innerExceptions.Length == 0)
                {
                    return base.Message;
                }

                StringBuilder sb = new();
                sb.Append(base.Message);
                sb.Append(' ');
                for (int i = 0; i < _innerExceptions.Length; i++)
                {
                    sb.Append('(');
                    sb.Append(_innerExceptions[i].Message);
                    sb.Append(") ");
                }
                sb.Length--;
                return sb.ToString();
            }
        }

        /// <summary>
        /// Creates and returns a string representation of the current <see cref="AggregateException"/>.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            StringBuilder text = new(GetType().FullName);
            text.Append(": ").Append(Message);

            for (int i = 0; i < _innerExceptions.Length; i++)
            {
                text.Append(Environment.NewLine).Append("--- ");
                text.AppendFormat(CultureInfo.InvariantCulture, "(Inner Exception #{0})", i);
                text.Append(_innerExceptions[i].ToString());
                text.Append(Environment.NewLine).Append("--- End of inner exception stack trace ---");
                text.AppendLine();
            }

            if (StackTrace != null)
            {
                text.Append(Environment.NewLine).Append(StackTrace);
            }

            return text.ToString();
        }
    }
}
