namespace Osnowa.Osnowa.Core.CSharpUtilities
{
    using System;
    using System.Diagnostics;

    public static class RepeatedActionExecutor
    {
        public static TResult Execute<TResult>(Func<(bool succeeded, TResult taskResult)> taskToExecute, 
            int maxAttemps = -1, float maxMillis = -1f)
        {
            bool limitedAttempts = maxAttemps == -1;
            bool limitedTime = maxMillis == -1f;
            var stopwatch = new Stopwatch();
            if (limitedTime) stopwatch.Start();

            for (int attempt = 1; limitedAttempts || attempt <= maxAttemps; attempt++)
            {
                (bool succeeded, TResult taskResult) result = taskToExecute();
                if (result.succeeded)
                    return result.taskResult;

                if (limitedTime && stopwatch.ElapsedMilliseconds > maxMillis)
                    throw new InvalidOperationException($"Exceeded the limit of {maxMillis} milliseconds.");
            }

            throw new InvalidOperationException($"Exceeded the limit of {maxAttemps} attemps.");
        }

        public static TResult Execute<TResult>(Func<TResult> taskToExecute, 
            int maxAttemps = -1, float maxMillis = -1f)
        {
            bool limitedAttempts = maxAttemps != -1;
            bool limitedTime = maxMillis == -1f;
            var stopwatch = new Stopwatch();
            if (limitedTime) stopwatch.Start();

            for (int attempt = 1; !limitedAttempts || attempt <= maxAttemps; attempt++)
            {
                TResult result = taskToExecute();
                if (result != null)
                    return result;

                if (limitedTime && stopwatch.ElapsedMilliseconds > maxMillis)
                    throw new InvalidOperationException($"Exceeded the limit of {maxMillis} milliseconds.");
            }

            throw new InvalidOperationException($"Exceeded the limit of {maxAttemps} attemps.");
        }
    }
}