using System;
using log4net;

namespace Customer.Project.Utilities
{
    public class FormatLogger : IFormatLogger
    {
        public ILog TheLogger { get; set; }
        public FormatLogger(ILog log)
        {
            TheLogger = log;
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
            TheLogger.Warn(String.Format(messageFormatString, formatStringParameters));
        }
        public void Debug(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Debug(String.Format(messageFormatString, formatStringParameters));
        }
        public void Error(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                    ? String.Format(messageFormatString, formatStringParameters)
                                    : messageFormatString;
            TheLogger.Error(String.Format(messageFormatString, formatStringParameters));
        }
        public void Error(Exception ex, string prefix, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                       ? String.Format(prefix, formatStringParameters)
                                       : prefix;
            TheLogger.Error(String.Format(prefix, formatStringParameters) + ex.ToFullException(), ex);
        }
        public void Fatal(string messageFormatString, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                       ? String.Format(messageFormatString, formatStringParameters)
                                       : messageFormatString;
            TheLogger.Fatal(String.Format(messageFormatString, formatStringParameters));
        }
        public void Fatal(Exception ex, string prefix, params object[] formatStringParameters)
        {
            string theMessage = formatStringParameters.Length > 0
                                       ? String.Format(prefix, formatStringParameters)
                                       : prefix;
            TheLogger.Fatal(String.Format(prefix, formatStringParameters) + ex.ToFullException(), ex);
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
    }
}
