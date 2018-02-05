using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyServices
{
    static class Program
    {
        static int Count = 0;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*       ServiceBase[] ServicesToRun; 

                   ServicesToRun = new GetAltcointraderData[]
                   {
                       new GetAltcointraderData()
                   };


                   ServiceBase.Run(ServicesToRun);

                   ServicesToRun = new GetKrakenData[]
                  {
                       new GetKrakenData()
                  };


                   ServiceBase.Run(ServicesToRun);

                   ServicesToRun = new GetLunoData[]
                  {
                       new GetLunoData()
                  };

                   ServiceBase.Run(ServicesToRun);*/


            //var stopwatch = new Stopwatch();
            //stopwatch.Start();

            // while (stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            //{
            // Execute your loop here...

            //GetAltcointraderData process1 = new GetAltcointraderData();
            //process1.DebugMe();
            //}

            //while (Count < 1000)
            //   Main();

            // GetAltcointraderData process1 = new GetAltcointraderData();
            // process1.DebugMe();
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun;


                ServicesToRun = new ServiceBase[]
               {
                 new GetAltcointraderData(),
                 new GetKrakenData()
               };

                ServiceBase.Run(ServicesToRun);

            }
            else
            {
                if (args[0] == "-testprocess")
                {
                    //AxiaProcess testing
                    GetAltcointraderData process = new GetAltcointraderData();
                    process.DebugMe();

                    GetKrakenData process2 = new GetKrakenData();
                    process2.DebugMe();

                    System.Threading.Thread.Sleep(15000000);
                    return;
                }
            }
        }
    }
}