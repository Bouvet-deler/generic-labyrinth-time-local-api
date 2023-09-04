using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace generic_high_score_local_api;

public class HardwereBackgroundService : BackgroundService
{
    private readonly ILogger<HardwereBackgroundService> _logger;
    private readonly Application _application;

    static SerialPort _serialPort;


    public HardwereBackgroundService(ILogger<HardwereBackgroundService> logger, Application application)
    {
        _logger = logger;
        _application = application;


    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _serialPort = new SerialPort();
        _serialPort.PortName = "COM3";//Set your board COM
        _serialPort.BaudRate = 9600;
        _serialPort.Open();
        Stopwatch stopWatch1 = new Stopwatch();

        bool sensor1_har_startet = false;
        int teller_ball = 0;
        string tid_spiller1 = null;
        string tid_spiller2 = null;
        bool restart = false;

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            //  _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //  await Task.Delay(1000, stoppingToken);

            //_application.setStartTime();

            string a = _serialPort.ReadExisting();

            /*
            if(a != "")
            {
                Console.WriteLine(a);
            }
            */

            if (a == "0")
            {

                Console.WriteLine("Tid startet");
                stopWatch1.Start();
                sensor1_har_startet = true;
                //  _application.setStartTime(); 
            }


            // if (a == "Sensor2" && sensor1_har_startet == true)
            if (a == "Sensor2")
            {  // tiden skal stoppe

                Console.WriteLine("Sensor har registrert ball");
                //Console.WriteLine(a);
                // stopWatch1.Stop();

                TimeSpan ts = stopWatch1.Elapsed;
                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);


                teller_ball += 1;
                if (teller_ball == 1)
                {
                    tid_spiller1 = elapsedTime;
                    //    _application.setStopTime();
                    Thread.Sleep(15);
                }
                else if (teller_ball == 2)
                {
                    tid_spiller2 = elapsedTime;
                    //      _application.setStopTime2();
                    TimeSpan tsPlayer1 = TimeSpan.Parse(tid_spiller1);
                    TimeSpan tsPlayer2 = TimeSpan.Parse(tid_spiller2);
                    _application.setTheTime(tsPlayer1, tsPlayer2);
                    Thread.Sleep(15);
                    // restart = true;
                }

                Console.WriteLine(tid_spiller1);
                Console.WriteLine(tid_spiller2);


            }
            if (a == "Restart")
            {
                Console.WriteLine("Restart");
                stopWatch1.Reset();
                //restart = false;
                teller_ball = 0;
                tid_spiller1 = null;
                tid_spiller2 = null;

            }
        }
    }
}