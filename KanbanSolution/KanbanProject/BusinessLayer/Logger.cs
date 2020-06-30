[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace KanbanProject.BusinessLayer
{
    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); // Log

        public static log4net.ILog GetLogger()
        {
            return log;
        }
    }
}
