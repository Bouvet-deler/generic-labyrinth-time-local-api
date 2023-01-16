public class User 
{
    public User(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
    public int Points { get; set; }
    public List<string> Codes { get; set; } = new List<string>();
}