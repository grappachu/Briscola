using System;
using System.Runtime.Serialization;

namespace Grappachu.Briscola.Exceptions
{
    /// <summary>
    ///     Defines che basic exception class for all exception raised by this application
    /// </summary>
    public class BriscolaException : Exception
    {
        /// <inheritdoc />
        public BriscolaException()
        {
        }

        /// <inheritdoc />
        public BriscolaException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public BriscolaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected BriscolaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}