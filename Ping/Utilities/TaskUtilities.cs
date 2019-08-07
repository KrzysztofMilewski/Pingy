using System;
using System.Threading.Tasks;

namespace Ping.Utilities
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }

    public static class TaskUtilities
    {
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler errorHandler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorHandler?.HandleError(ex);
            }
        }
    }
}
