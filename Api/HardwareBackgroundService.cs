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
        //_serialPort.PortName = "COM3";//Set your board COM
        _serialPort.PortName = "/dev/tty.Bluetooth-Incoming-Port"; //ikke merge denne
        _serialPort.BaudRate = 115200;
        _serialPort.Open();
        Stopwatch stopWatch1 = new Stopwatch();
        Stopwatch stopWatch2 = new Stopwatch();

        bool sensor1_har_startet = false;
        bool sensor2_har_startet = false;
        int teller_ball = 0;
        string tid_spiller1 = null;
        string tid_spiller2 = null;
        bool restart = false; // MÅ FIKSES
        bool time_return = false;
        TimeSpan ts;
        TimeSpan ts2;
        string elapsedTime = null;
        string elapsedTime2 = null;


        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            string a = _serialPort.ReadExisting();

            if (a == "0")
            {
                Console.WriteLine("Tid startet");
                stopWatch1.Start();
                stopWatch2.Start();
                sensor1_har_startet = true;
                _application.setStartTime();
            }

            if (a == "s")
            {
                Console.WriteLine("Sensor har registrert ball");
                ts = stopWatch1.Elapsed;
                ts2 = stopWatch2.Elapsed;
                // Format and display the TimeSpan value.'

                elapsedTime = elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                elapsedTime2 = elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts2.Hours, ts2.Minutes, ts2.Seconds,
                    ts2.Milliseconds / 10);

                teller_ball += 1;
                sensor2_har_startet = true;

            }

            if (teller_ball == 1 && sensor2_har_startet)
            {
                tid_spiller1 = elapsedTime;
                _application.setStopTime();
            }
            else if (teller_ball == 2 && sensor2_har_startet)
            {
                tid_spiller2 = elapsedTime2;
                _application.setStopTime2();
                TimeSpan tsPlayer1 = TimeSpan.Parse(tid_spiller1);
                TimeSpan tsPlayer2 = TimeSpan.Parse(tid_spiller2);
                _application.setTheTime(tsPlayer1, tsPlayer2);
                time_return = true;
                Console.WriteLine(tid_spiller1);
                Console.WriteLine(tid_spiller2);
                sensor2_har_startet = false;
            }



            if (restart == true)
            {
                Console.WriteLine("Restart");
                stopWatch1.Reset();
                stopWatch2.Reset();
                restart = false; // DOBBELTSJEKKE
                teller_ball = 0;
                tid_spiller1 = null;
                tid_spiller2 = null;
                sensor2_har_startet = false;
                elapsedTime = null;
                time_return = false;
            }
        }
    }
}


/*
     // to find what is wrong
if(a != "")
{
    Console.WriteLine(a);
}
*/