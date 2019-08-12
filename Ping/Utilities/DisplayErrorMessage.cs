using System;

namespace Ping.Utilities
{
    public class DisplayErrorMessage : IErrorHandler
    {
        private readonly Action<Exception> _exceptionAction;

        public DisplayErrorMessage(Action<Exception> exceptionAction)
        {
            _exceptionAction = exceptionAction;
        }

        public void HandleError(Exception ex)
        {
            _exceptionAction(ex);
        }
    }
}
