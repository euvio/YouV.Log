using NLog;
using NLog.LayoutRenderers;
using System.Reflection;

namespace YouV.Log
{
    public delegate void LogWrittenEventHandler(string message, string category, LogLevel logLevel);

    public static class Logger
    {
        private static readonly string _appAll = Assembly.GetEntryAssembly()!.GetName().Name + ".app.all";
        private static readonly Dictionary<string, NLog.Logger> _existingLoggers;
        private static readonly object _locker;

        static Logger()
        {
            LayoutRenderer.Register<TagLayoutRenderer>("tag");
            _locker = new object();
            _existingLoggers = new Dictionary<string, NLog.Logger>();
            AddNewCategoryLogger(_appAll);
        }

        public static event LogWrittenEventHandler? LogWritten;

        public static void WriteLog(string message, string? category = null, LogLevel logLevel = LogLevel.INFO, bool alsoIntoAll = true)
        {
            string tag;
            if (category == null)
            {
                alsoIntoAll = true;
                tag = _appAll;
            }
            else
            {
                tag = category;
            }

            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    if (category != null)
                        Debug(message, category, category);
                    if (alsoIntoAll)
                        Debug(message, _appAll, tag);

                    break;

                case LogLevel.INFO:
                    if (category != null)
                        Info(message, category, category);
                    if (alsoIntoAll)
                        Info(message, _appAll, tag);
                    break;

                case LogLevel.WARN:
                    if (category != null)
                        Warn(message, category, category);
                    if (alsoIntoAll)
                        Warn(message, _appAll, tag);
                    break;

                case LogLevel.ERROR:
                    if (category != null)
                        Error(message, category, category);
                    if (alsoIntoAll)
                        Error(message, _appAll, tag);
                    break;

                case LogLevel.FATAL:
                    if (category != null)
                        Fatal(message, category, category);
                    if (alsoIntoAll)
                        Fatal(message, _appAll, tag);
                    break;
            }

            LogWritten?.BeginInvoke(message, category!, logLevel, null, null);
        }

        private static void AddNewCategoryLogger(string category)
        {
            if (!_existingLoggers.ContainsKey(category))
            {
                lock (_locker)
                {
                    if (!_existingLoggers.ContainsKey(category))
                    {
                        _existingLoggers[category] = LogManager.GetLogger(category);
                    }
                }
            }
        }

        private static void Debug(string message, string category, string tag)
        {
            AddNewCategoryLogger(category);
            LogEventInfo entity = new LogEventInfo()
            {
                Message = message,
                LoggerName = category
            };
            entity.Properties["Tag"] = tag;

            _existingLoggers[category].Log(NLog.LogLevel.Debug, entity);

        }

        private static void Error(string message, string category, string tag)
        {
            AddNewCategoryLogger(category);
            LogEventInfo entity = new LogEventInfo()
            {
                Message = message,
                LoggerName = category
            };
            entity.Properties["Tag"] = tag;

            _existingLoggers[category].Log(NLog.LogLevel.Error, entity);
        }

        private static void Fatal(string message, string category, string tag)
        {
            AddNewCategoryLogger(category);
            LogEventInfo entity = new LogEventInfo()
            {
                Message = message,
                LoggerName = category
            };
            entity.Properties["Tag"] = tag;

            _existingLoggers[category].Log(NLog.LogLevel.Fatal, entity);
        }

        private static void Info(string message, string category, string tag)
        {
            AddNewCategoryLogger(category);
            LogEventInfo entity = new LogEventInfo()
            {
                Message = message,
                LoggerName = category
            };
            entity.Properties["Tag"] = tag;

            _existingLoggers[category].Log(NLog.LogLevel.Info, entity);
        }

        private static void Warn(string message, string category, string tag)
        {
            AddNewCategoryLogger(category);
            LogEventInfo entity = new LogEventInfo()
            {
                Message = message,
                LoggerName = category
            };
            entity.Properties["Tag"] = tag;

            _existingLoggers[category].Log(NLog.LogLevel.Warn, entity);
        }
    }
}