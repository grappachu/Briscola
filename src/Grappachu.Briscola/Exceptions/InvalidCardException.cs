using System;
using System.Runtime.Serialization;

namespace Grappachu.Briscola.Exceptions
{
    /// <summary>
    ///     Represents errors that occur when an invalid card has been choosed
    /// </summary>
    public class InvalidCardException : BriscolaException
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="InvalidCardException" />
        /// </summary>
        public InvalidCardException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="InvalidCardException" />
        /// </summary>
        /// <param name="message"></param>
        public InvalidCardException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="InvalidCardException" />
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidCardException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="InvalidCardException" />
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidCardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}