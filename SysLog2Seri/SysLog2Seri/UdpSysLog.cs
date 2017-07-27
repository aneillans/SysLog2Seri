using Serilog;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace SysLog2Seri
{
    internal static class UdpSysLog
    {
        private static bool stopProcessing = false;
        private static Thread processingThread;
        public static void Start()
        {
            processingThread = new Thread(Process);
            processingThread.Start();
        }

        public static void Stop()
        {
            stopProcessing = true;
            if (processingThread.Join(4000))
            {
                processingThread.Abort();
            }
        }
        private static void Process()
        {
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpListener = new UdpClient(514);

            while (!stopProcessing)
            {
                try
                {
                    var bReceive = udpListener.Receive(ref anyIP);
                    var sReceive = Encoding.ASCII.GetString(bReceive);
                    var sourceIP = anyIP.Address.ToString();

                    if (bool.Parse(ConfigurationManager.AppSettings["ParseAsObject"]))
                    {
                        var parts = Regex.Matches(sReceive, @"[^\s""]+(?:""[^""]*""[^\s""]*)*|(?:""[^""]*""[^\s""]*)+")
                                        .Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToList();
                        
                        string logString = string.Empty;
                        List<object> logObjects = new List<object>();
                        logString += "{@source_device_ip} ";
                        logObjects.Add(sourceIP);
                        foreach (var part in parts)
                        {
                            if (part.Contains('='))
                            {
                                logString += @"{@" + part.Split('=')[0] + "} ";
                                logObjects.Add(part.Split('=')[1]);
                            }
                            else
                            {
                                logString += part + " ";
                            }
                        }
                        
                        Log.Information(logString, logObjects.ToArray()); 
                    }
                    else
                    {
                        Log.Information(sReceive, sourceIP);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error while receiving UDP Data");
                }
            }
        }
    }
}
