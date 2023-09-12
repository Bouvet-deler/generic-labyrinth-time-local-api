using System.Diagnostics;
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
        await _application.LoadTopListAsync("deafult toplist, create a new one with swagger!");
        _serialPort = new SerialPort();
        _serialPort.PortName = "COM5"; //Set your COM
        //_serialPort.PortName = "/dev/tty.Bluetooth-Incoming-Port"; //mac (bluetooth port)
        _serialPort.BaudRate = 115200;
        _serialPort.Open();

        Stopwatch stopWatch1 = new Stopwatch();
        TimeSpan ts;
        bool reset_from_application; ;
        string state = "waiting";

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            string output_from_arduino = _serialPort.ReadExisting();

            switch (state)
            {
                case "waiting":
                    if (output_from_arduino == "0") // the button is pushed down
                    {
                        SoundPlayer startSound = new SoundPlayer(".\\start_sound.wav");
                        startSound.Play();
                        Thread.Sleep(5000);
                        Console.WriteLine("Tid startet");
                        stopWatch1.Start();
                        _application.setStartTime();
                        state = "tracking player 1";
                    }
                    break;
                case "tracking player 1":

                    reset_from_application = _application.sendArduinoReset(); // if needing to reset from the API, the game is going to finished state
                    if (reset_from_application)
                    {
                        state = "finished";
                    }

                    if (output_from_arduino == "s")  // signals that the first player has finished
                    {
                        Console.WriteLine("Sensor har registrert ball"); // the sensor has registered the ball
                        ts = stopWatch1.Elapsed;

                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", // Format and display the TimeSpan value.'
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);

                        TimeSpan tsPlayer1 = TimeSpan.Parse(elapsedTime);
                        _application.setStopTime(tsPlayer1);
                        state = "tracking player 2";
                    }
                    break;
                case "tracking player 2":
                    reset_from_application = _application.sendArduinoReset();  // if needing to reset from the API, the game is going to finished state
                    if (reset_from_application)
                    {
                        state = "finished";
                    }

                    if (output_from_arduino == "s") // signals that the second player has finished
                    {
                        Console.WriteLine("Sensor har registrert ball"); // the sensor has registered the ball
                        ts = stopWatch1.Elapsed;

                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", // Format and display the TimeSpan value.'
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);

                        TimeSpan tsPlayer2 = TimeSpan.Parse(elapsedTime);
                        _application.setStopTime2(tsPlayer2);
                        state = "finished";
                    }
                    break;
                case "finished":
                    stopWatch1.Reset();
                    state = "waiting";
                    break;
                default:
                    break;
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