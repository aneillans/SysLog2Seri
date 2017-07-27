using Serilog;

namespace SysLog2Seri
{
    public class SysLogService
    {
        public void Start()
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.AppSettings()
               .CreateLogger();
            
            UdpSysLog.Start();
            
        }
        public void Stop()
        {
            UdpSysLog.Stop();
            Log.CloseAndFlush();
        }
    }
}
