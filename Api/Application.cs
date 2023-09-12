using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;

public class Application
{
    public string CurrentTopListFileName { get; private set; } = "deafult toplist, create a new one with swagger!";

    public List<User> CurrentToplist { get; private set; } = new();

    CultureInfo culture = null;

    bool runStart = false;

    bool runStop = false;

    bool runStop2 = false;

    bool resetArduino = false;

    string? time_span1 = null;

    string? time_span2 = null;

    private string _path = "../Toplists/";

    public IResult CreateNewTopList(string toplistName)
    {
        string path = $"{_path}{toplistName}.txt";
        if (File.Exists(path)) return Results.BadRequest($"Toplist {toplistName} already exists.");
        try
        {
            File.Create(path).Close();
            CurrentTopListFileName = toplistName;
            CurrentToplist = new();
            return Results.Ok($"{toplistName} created and is now the active toplist.");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public IResult ListAllTopLists()
    {
        try
        {
            List<string> files = Directory.GetFiles(_path).ToList();
            if (files.Count < 1) return Results.Problem("There are no Toplist files in the directory.");

            return Results.Ok(files);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public async Task<IResult> LoadTopListAsync(string toplistName)
    {
        string path = $"{_path}{toplistName}.txt";
        if (!File.Exists(path))
        {
            return Results.BadRequest($"Toplist {toplistName} does not exists.");
        }

        try
        {
            // Create the file, or overwrite if the file exists.
            string fileContent = await File.ReadAllTextAsync(path);
            CurrentToplist = System.Text.Json.JsonSerializer.Deserialize<List<User>>(fileContent)!;
            CurrentTopListFileName = toplistName;
            return Results.Ok($"\"{toplistName}\" loaded and is now the active toplist.");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public async Task<IResult> NewTimeEntry(string userName, string email, int phoneNumber, string userName2, string email2, int phoneNumber2)
    {
        try
        {
            string upperUserName = userName.ToUpper().TrimEnd();
            string upperUserName2 = userName2.ToUpper().TrimEnd();
            bool coolPerson = false;
            bool coolPerson2 = false;
            bool veryCoolPerson = false;
            bool veryCoolPerson2 = false;

            // check if user exists
            User? user = CurrentToplist.Find(u => u.Email.Equals(email));
            User? user2 = CurrentToplist.Find(u => u.Email.Equals(email2));

            // create user if it dosen't exists
            if (user == null)
            {
                user = new User(upperUserName);
                CurrentToplist.Add(user);
            }

            if (user2 == null)
            {
                user2 = new User(upperUserName2);
                CurrentToplist.Add(user2);
            }

            // cheat codes for users with a specific name
            if (checkIfCoolPerson(upperUserName))
            {
                coolPerson = true;
            }

            if (checkIfCoolPerson(upperUserName2))
            {
                coolPerson2 = true;
            }

            if (checkIfVeryCoolPerson(upperUserName))
            {
                veryCoolPerson = true;
            }

            if (checkIfVeryCoolPerson(upperUserName2))
            {
                veryCoolPerson2 = true;
            }

            if (coolPerson)
            {
                // Add 10 seconds to final time
                //TimeSpan betterTime = TimeSpan.ParseExact(time_span1, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(-10));

                // Subtract 10 seconds from final time
                TimeSpan betterTime = TimeSpan.ParseExact(time_span1, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(5));
                time_span1 = betterTime.ToString("mm':'ss':'fff");
            }

            if (coolPerson2)
            {
                // Add 10 seconds to final time
                //TimeSpan betterTime = TimeSpan.ParseExact(time_span2, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(-10));

                // Subtract 10 seconds from final time
                TimeSpan betterTime = TimeSpan.ParseExact(time_span2, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(5));
                time_span2 = betterTime.ToString("mm':'ss':'fff");
            }

            if (veryCoolPerson)
            {
                // Add 10 seconds to final time
                TimeSpan betterTime = TimeSpan.ParseExact(time_span1, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(-5));

                // Subtract 10 seconds from final time
                // TimeSpan betterTime = TimeSpan.ParseExact(time_span1, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(10));
                time_span1 = betterTime.ToString("mm':'ss':'fff");
            }

            if (veryCoolPerson2)
            {
                // Add 10 seconds to final time
                TimeSpan betterTime = TimeSpan.ParseExact(time_span2, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(-5));

                // Subtract 10 seconds from final time
                // TimeSpan betterTime = TimeSpan.ParseExact(time_span2, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(10));
                time_span2 = betterTime.ToString("mm':'ss':'fff");
            }

            user.Time = time_span1;
            user.Email = email;
            user.PhoneNumber = phoneNumber;

            user2.Time = time_span2;
            user2.Email = email2;
            user2.PhoneNumber = phoneNumber2;

            runStart = false;
            runStop = false;
            runStop2 = false;
            coolPerson = false;
            coolPerson2 = false;
            veryCoolPerson = false;
            veryCoolPerson2 = false;

            // save state
            return await SaveState();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public bool StartTime()
    {
        resetArduino = false;
        return runStart;
    }

    public bool EndTime()
    {
        return runStop;
    }

    public bool EndTime2()
    {
        return runStop2;
    }

    public void simulateStartTime()
    {
        runStart = true;
    }

    public void setStartTime()
    {
        runStart = true;
    }

    public void setStopTime(TimeSpan tsPlayer1)
    {
        time_span1 = tsPlayer1.ToString("mm':'ss':'fff");
        runStop = true;
    }

    public void setStopTime2(TimeSpan tsPlayer2)
    {
        time_span2 = tsPlayer2.ToString("mm':'ss':'fff");
        runStop2 = true;
    }

    // Functions to simulate stop signal and lap time for both users
    // when not connected to the sensor/microcontroller

    //public void setStopTime()
    //{
    //    time_span1 = "01:12:000";
    //    runStop = true;
    //}

    //public void setStopTime2()
    //{
    //    time_span2 = "00:09:110";
    //    runStop2 = true;
    //}

    public void resetTime()
    {
        runStart = false;
        runStop = false;
        runStop2 = false;
        resetArduino = true;
    }

    public bool sendArduinoReset()
    {
        return resetArduino;
    }

    public string sendTimePlayer1()
    {
        if (time_span1 != null)
        {
            return time_span1;
        }
        return "";
    }

    public string sendTimePlayer2()
    {
        if (time_span2 != null)
        {
            return time_span2;
        }
        return "";
    }

    public bool checkIfCoolPerson(string s)
    {
        string[] arr = { "VETLE", "WILLIAM", "BURHAN", "CORNELIA", "JOSEFINE", "JULIE" };
        string searchElement = s;
        bool exists = Array.Exists(arr, element => element == searchElement);
        if (exists)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkIfVeryCoolPerson(string s)
    {
        string[] arr = { "VEBJØRN", "JOHAN" };
        string searchElement = s;
        bool exists = Array.Exists(arr, element => element == searchElement);
        if (exists)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IResult pickRandomWinnersFromParticipants(int numberOfWinners)
    {
        try
        {
            Random random = new Random();
            using StreamReader reader = new($"{_path}{CurrentTopListFileName}.txt");
            var json = reader.ReadToEnd();

            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            List<User> listWithWinners = new List<User>();

            int index = 0;
            while (index < numberOfWinners)
            {
                int winner = random.Next(0, users.Count);
                listWithWinners.Add(users[winner]);
                users.RemoveAt(winner);
                index++;
            }
            return Results.Ok(listWithWinners);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private async Task<IResult> SaveState()
    {
        try
        {
            string path = $"{_path}{CurrentTopListFileName}.txt";

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            string jsonString = System.Text.Json.JsonSerializer.Serialize(CurrentToplist.OrderBy(x => x.Time).ToList(), options);
            await File.WriteAllTextAsync(path, jsonString);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
