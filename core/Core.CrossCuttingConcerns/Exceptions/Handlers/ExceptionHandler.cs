using Core.CrossCuttingConcerns.Exceptions.Types;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers
{
    /// <summary>
    /// Base abstract class for handling different types of exceptions.
    /// </summary>
    public abstract class ExceptionHandler
    {
        /// <summary>
        /// Handles exceptions of various types and calls appropriate methods based on the exception type.
        /// </summary>
        /// <param name="exception">The exception to be handled.</param>
        /// <returns>A Task representing the asynchronous exception handling operation.</returns>
        public Task HandleExceptionAsync(Exception exception) =>
            exception switch
            {
                BusinessException businessException => HandleException(businessException),
                ValidationException validationException => HandleException(validationException),
                AuthorizationException authorizationException => HandleException(authorizationException),
                NotFoundException notFoundException => HandleException(notFoundException),
                _ => HandleException(exception)
            };

        /// <summary>
        /// Handles the specific type of BusinessException.
        /// </summary>
        /// <param name="businessException">The BusinessException to be handled.</param>
        /// <returns>A Task representing the asynchronous handling of the BusinessException.</returns>
        protected abstract Task HandleException(BusinessException businessException);

        /// <summary>
        /// Handles the specific type of ValidationException.
        /// </summary>
        /// <param name="validationException">The ValidationException to be handled.</param>
        /// <returns>A Task representing the asynchronous handling of the ValidationException.</returns>
        protected abstract Task HandleException(ValidationException validationException);

        /// <summary>
        /// Handles the specific type of AuthorizationException.
        /// </summary>
        /// <param name="authorizationException">The AuthorizationException to be handled.</param>
        /// <returns>A Task representing the asynchronous handling of the AuthorizationException.</returns>
        protected abstract Task HandleException(AuthorizationException authorizationException);

        /// <summary>
        /// Handles the specific type of NotFoundException.
        /// </summary>
        /// <param name="notFoundException">The NotFoundException to be handled.</param>
        /// <returns>A Task representing the asynchronous handling of the NotFoundException.</returns>
        protected abstract Task HandleException(NotFoundException notFoundException);

        /// <summary>
        /// Handles generic exceptions not matching any specific types.
        /// </summary>
        /// <param name="exception">The generic Exception to be handled.</param>
        /// <returns>A Task representing the asynchronous handling of the generic Exception.</returns>
        protected abstract Task HandleException(Exception exception);
    }
}
