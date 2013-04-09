using System;
using log4net;

namespace Customer.Project.Utilities
{
    public class FormatLogger : IFormatLogger
    {
        public FormatLogger(ILog log)
        {
            TheLogger = log;
        }

        public ILog TheLogger { get; set; }

        #region IFormatLogger Members

        /// <summary>
        /// Logs an exception as an info message
        /// </summary>
        public void Info(Exception ex, string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;

            string fullMessage = String.Format("A business logic error occured: {0} ({1}). {2}. At: {3}"
                                               , ex.Message, ex.Message.GetType().Name
                                               , theMessage
                                               , ex.StackTrace);
            TheLogger.Info(fullMessage);
        }

        public void Info(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Info(theMessage);
        }

        public void Warn(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Warn(theMessage);
        }

        public void Debug(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Debug(theMessage);
        }

        public void Error(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Error(theMessage);
        }

        public void Error(Exception ex, string prefix, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(prefix, formatStringParameters)
                                    : prefix;
            TheLogger.Error(theMessage, ex);
        }

        public void Fatal(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Fatal(theMessage);
        }

        public void Fatal(Exception ex, string prefix, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(prefix, formatStringParameters)
                                    : prefix;
            TheLogger.Fatal(theMessage, ex);
        }

        public void Log(LogLevel level, string messageFormatString, params object[] formatStringParameters)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Info:
                    Info(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Warn:
                    Warn(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Error:
                    Error(messageFormatString, formatStringParameters);
                    break;

                case LogLevel.Fatal:
                    Fatal(messageFormatString, formatStringParameters);
                    break;

                default:
                    throw new InvalidOperationException("Unknown LogLevel");
            }
        }

        public void Log(LogLevel level, Exception ex, string prefix, params object[] formatStringParameters)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    {
                        string theMessage = formatStringParameters.Length > 0
                                                ? String.Format(prefix, formatStringParameters)
                                                : prefix;
                        TheLogger.Debug(theMessage + ex.ToFullException(), ex);
                        break;
                    }


                case LogLevel.Info:
                    {
                        string theMessage = formatStringParameters.Length > 0
                                                ? String.Format(prefix, formatStringParameters)
                                                : prefix;
                        TheLogger.Info(theMessage + ex.ToFullException(), ex);
                        break;
                    }
                case LogLevel.Warn:
                    {
                        string theMessage = formatStringParameters.Length > 0
                                                ? String.Format(prefix, formatStringParameters)
                                                : prefix;
                        TheLogger.Warn(theMessage + ex.ToFullException(), ex);
                        break;
                    }

                case LogLevel.Error:
                    Error(ex, prefix, formatStringParameters);
                    break;

                case LogLevel.Fatal:
                    Fatal(ex, prefix, formatStringParameters);
                    break;

                default:
                    throw new InvalidOperationException("Unknown LogLevel");
            }
        }

        #endregion
    }
}