using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// A static class responsible for logging messages of various levels (Info, Warning, Error) to the Unity console.
    /// It provides options for controlling the logging behavior such as enabling/disabling logs and setting the minimum log level.
    /// </summary>
    public static class Logger
    {
        #region Enum Definitions

        /// <summary>
        /// Defines different log levels that control the verbosity of logs.
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Information messages that provide general details about program execution.
            /// </summary>
            Info,

            /// <summary>
            /// Warnings that indicate potential issues but do not stop program execution.
            /// </summary>
            Warning,

            /// <summary>
            /// Errors that indicate critical issues which may affect functionality.
            /// </summary>
            Error
        }

        #endregion

        #region Private Variables

        private static bool enableLogging = true;  // Flag to enable or disable logging
        private static LogLevel minimumLogLevel = LogLevel.Info;  // Defines the minimum log level that should be logged

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs a message with the specified log level. Only logs messages that meet or exceed the minimum log level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The log level (Info, Warning, Error). Defaults to Info.</param>
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            if (!enableLogging || level < minimumLogLevel) return;

            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log($"[INFO] {message}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[WARNING] {message}");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"[ERROR] {message}");
                    break;
            }
        }

        /// <summary>
        /// Logs an exception message along with its stack trace.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(System.Exception exception)
        {
            if (!enableLogging) return;

            Debug.LogError($"[EXCEPTION] {exception.Message}\n{exception.StackTrace}");
        }

        /// <summary>
        /// Enables or disables logging dynamically.
        /// </summary>
        /// <param name="isEnabled">True to enable logging, false to disable.</param>
        public static void SetLoggingEnabled(bool isEnabled)
        {
            enableLogging = isEnabled;
        }

        /// <summary>
        /// Sets the minimum log level for messages to be displayed. Logs with a level lower than the minimum will be ignored.
        /// </summary>
        /// <param name="level">The minimum log level.</param>
        public static void SetMinimumLogLevel(LogLevel level)
        {
            minimumLogLevel = level;
        }

        #endregion
    }
}
