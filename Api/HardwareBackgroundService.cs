﻿using System.Diagnostics;
using System.IO.Ports;
using System.Media;

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
        _serialPort.PortName = "COM3"; //Set your COM
        _serialPort.BaudRate = 115200;
        _serialPort.Open();

        Stopwatch stopWatch1 = new Stopwatch();
        Stopwatch stopWatch2 = new Stopwatch();
        TimeSpan ts;
        TimeSpan ts2;

        bool sensor1_har_startet = false;
        bool sensor2_har_startet = false;
        bool restart = false;
        bool time_return = false;
        string? tid_spiller1 = null;
        string? tid_spiller2 = null;
        string elapsedTime = null;
        string elapsedTime2 = null;
        int teller_ball = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            string output_from_arduino = _serialPort.ReadExisting();

            if (output_from_arduino == "0") // the button is pushed down
            {
                SoundPlayer startSound = new SoundPlayer(".\\start_sound.wav");
                startSound.Play();
                Thread.Sleep(5000);
                Console.WriteLine("Tid startet");
                stopWatch1.Start();
                stopWatch2.Start();
                sensor1_har_startet = true;
                _application.setStartTime();
            }

            if (output_from_arduino == "s")
            {
                Console.WriteLine("Sensor har registrert ball"); // the sensor has registered the ball
                ts = stopWatch1.Elapsed;
                ts2 = stopWatch2.Elapsed;

                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", // Format and display the TimeSpan value.'
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
                TimeSpan tsPlayer1 = TimeSpan.Parse(tid_spiller1);
                _application.setStopTime(tsPlayer1);
            }
            else if (teller_ball == 2 && sensor2_har_startet)
            {
                tid_spiller2 = elapsedTime2;
                TimeSpan tsPlayer2 = TimeSpan.Parse(tid_spiller2);
                _application.setStopTime2(tsPlayer2);
                time_return = true;
                Console.WriteLine(tid_spiller1);
                Console.WriteLine(tid_spiller2);
                sensor2_har_startet = false;
            }

            restart = _application.sendArduinoReset();

            if (restart == true)
            {
                stopWatch1.Reset();
                stopWatch2.Reset();
                restart = false;
                teller_ball = 0;
                tid_spiller1 = null;
                tid_spiller2 = null;
                sensor2_har_startet = false;
                elapsedTime = null;
                elapsedTime2 = null;
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