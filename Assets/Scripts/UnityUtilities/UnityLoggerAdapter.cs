using UnityEngine;

namespace UnityUtilities
{
    public class UnityLoggerAdapter : Osnowa.Osnowa.Core.ILogger
    {
        public void Info(string message)
        {
            Debug.Log(message);
        }

        public void Warning(string message)
        {
            Debug.LogWarning(message);
        }

        public void Error(string message)
        {
            Debug.LogError(message);
        }
    }
}