using Topshelf;

namespace SysLog2Seri
{
    internal static class TopShelfConfig
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<SysLogService>(service =>
                {
                    service.ConstructUsing(s => new SysLogService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsNetworkService();
                configure.SetServiceName("SysLog2Seri");
                configure.SetDisplayName("SysLog 2 Seri Proxy");
                configure.SetDescription("Pipe SysLog entries to Serilog");
            });
        }
    }
}
