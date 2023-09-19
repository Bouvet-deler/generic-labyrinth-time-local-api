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

    public IResult DeleteTopList(string topListName)
    {
        try
        {
            string path = $"{_path}{topListName}.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
                return Results.Ok($"{topListName} has been deleted.");
            }
            else
            {
                return Results.Problem($"Toplist with name {topListName} does not exist.");
            }
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
            bool reduceLapTime = false;
            bool reduceLapTime2 = false;
            bool increaseLapTime = false;
            bool increaseLapTime2 = false;

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

            // cheat codes for users with a specific name to reduce or increase their time
            // reduceLapTime = checkIfCoolPerson(reduceLapTime, upperUserName);
            // reduceLapTime2 = checkIfCoolPerson(reduceLapTime2, upperUserName2);
            // increaseLapTime = checkIfVeryCoolPerson(increaseLapTime, upperUserName);
            // increaseLapTime2 = checkIfVeryCoolPerson(increaseLapTime2, upperUserName2);

            // time_span1 = reduceTimeOfUser(time_span1, reduceLapTime);
            // time_span2 = reduceTimeOfUser(time_span2, reduceLapTime2);
            // time_span1 = increaseTimeOfUser(time_span1, increaseLapTime);
            // time_span2 = increaseTimeOfUser(time_span2, increaseLapTime2);

            user.Time = time_span1;
            user.Email = email;
            user.PhoneNumber = phoneNumber;

            user2.Time = time_span2;
            user2.Email = email2;
            user2.PhoneNumber = phoneNumber2;

            runStart = false;
            runStop = false;
            runStop2 = false;
            reduceLapTime = false;
            reduceLapTime2 = false;
            increaseLapTime = false;
            increaseLapTime2 = false;

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

    // Functions to simulate stop signal and lap time for both users
    // when not connected to the sensor/microcontroller

    // public void simulateEndTime()
    // {
    //     time_span1 = "01:12:000";
    //     runStop = true;
    // }

    // public void simulateEndTime2()
    // {
    //     time_span2 = "00:09:110";
    //     runStop2 = true;
    // }

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

    public bool checkIfCoolPerson(bool coolPerson, string username)
    {
        if (coolPersons(username))
        {
            coolPerson = true;
        }
        return coolPerson;
    }

    public bool checkIfVeryCoolPerson(bool veryCoolPerson, string username)
    {
        if (veryCoolPersons(username))
        {
            veryCoolPerson = true;
        }
        return veryCoolPerson;
    }

    public string reduceTimeOfUser(string lapTime, bool username)
    {
        if (username)
        {
            TimeSpan betterTime = TimeSpan.ParseExact(lapTime, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(5));
            lapTime = betterTime.ToString("mm':'ss':'fff");
        }
        return lapTime;
    }

    public string increaseTimeOfUser(string lapTime, bool username)
    {
        if (username)
        {
            TimeSpan betterTime = TimeSpan.ParseExact(lapTime, String.Format("mm':'ss':'fff"), culture, TimeSpanStyles.AssumeNegative).Add(TimeSpan.FromSeconds(-5));
            lapTime = betterTime.ToString("mm':'ss':'fff");
        }
        return lapTime;
    }

    public bool coolPersons(string s)
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

    public bool veryCoolPersons(string s)
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
            List<User> listWithWinners = new();

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

    public IResult pickRandomWinnersFromTopTen(int numberOfWinners)
    {
        try
        {
            Random random = new Random();
            using StreamReader reader = new($"{_path}{CurrentTopListFileName}.txt");
            var json = reader.ReadToEnd();

            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            List<User> listWithWinners = new();

            int index = 0;
            while (index < numberOfWinners)
            {
                int winner = random.Next(0, 9);
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

    public int getNumberOfParticipants()
    {
        using StreamReader reader = new($"{_path}{CurrentTopListFileName}.txt");
        var json = reader.ReadToEnd();
        List<User> users = JsonConvert.DeserializeObject<List<User>>(json);

        return users.Count();
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
