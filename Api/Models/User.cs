public class User 
{
    public User(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
    public string Time { get; set; }
    public string Email { get; set; }

    //String to handle international phone numbers for NDC
    public string PhoneNumber { get; set; }

}