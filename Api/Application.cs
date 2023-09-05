using generic_high_score_local_api;
using System;
using System.Diagnostics;
using System.Text.Json;

public class Application
{
    public string CurrentTopListFileName { get; private set; } = "deafult toplist, create a new one with swagger!";
    public List<User> CurrentToplist { get; private set; } = new();

    bool runStart = false;

    bool runStop = false;

    bool runStop2 = false;

    bool resetArduino = false;

    string time_span1;

    string time_span2;

    private string _path = "../Toplists/";

    public IResult CreateNewTopList(string toplistName)
    {
        string path = $"{_path}{toplistName}.txt";
        if (File.Exists(path)) return Results.BadRequest($"Toplist {toplistName} allready exists.");
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
        if (!File.Exists(path)) return Results.BadRequest($"Toplist {toplistName} does not exists.");
        try
        {
            // Create the file, or overwrite if the file exists.
            string fileContent = await File.ReadAllTextAsync(path);
            CurrentToplist = JsonSerializer.Deserialize<List<User>>(fileContent)!;
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
            string upperUserName = userName.ToUpper();
            string upperUserName2 = userName2.ToUpper();

            // check if user exists 
            User? user = CurrentToplist.Find(u => u.Name.Equals(upperUserName));
            User? user2 = CurrentToplist.Find(u => u.Name.Equals(upperUserName2));

            // create user if it dosen't exists
            if (user == null)
            {
                if (userName.Length < 2 || userName.Length > 10) return Results.BadRequest("Username must be between 2 and 10 letters.");
                user = new User(upperUserName);
                CurrentToplist.Add(user);
            }
            if (user2 == null)
            {
                if (userName2.Length < 2 || userName2.Length > 10) return Results.BadRequest("Username must be between 2 and 10 letters.");
                user2 = new User(upperUserName2);
                CurrentToplist.Add(user2);
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

    public void setStartTime()
    {
        runStart = true;
    }

    public void setStopTime()
    {
        runStop = true;
    }

    public void setStopTime2()
    {
        runStop2 = true;
    }
    public void simulateStartTime()
    {
        runStart = true;
    }



    public void setTheTime(TimeSpan timePlayer1, TimeSpan timePlayer2)
    {
        time_span1 = timePlayer1.ToString("mm':'ss':'fff");
        time_span2 = timePlayer2.ToString("mm':'ss':'fff");
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

    public string sendTimePlayer1()
    {
        return time_span1;
    }

    public string sendTimePlayer2()
    {
        return time_span2;
    }

    private async Task<IResult> SaveState()
    {
        try
        {
            string path = $"{_path}{CurrentTopListFileName}.txt";

            JsonSerializerOptions options = new() { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(CurrentToplist.OrderBy(x => x.Time).ToList());
            await File.WriteAllTextAsync(path, jsonString);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
