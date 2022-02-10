using System.Collections.Generic;

namespace Columns;

public sealed class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public static List<User> LoadUsers()
    {
        return new List<User>
            {
                new User
                {
                    FirstName = "Andrea",
                    LastName = "Johansen",
                    City = "Hornbæk",
                    Country = "Denmark",
                },
                new User
                {
                    FirstName = "Phil",
                    LastName = "Scott",
                    City = "Dayton",
                    Country = "United States",
                },
                new User
                {
                    FirstName = "Patrik",
                    LastName = "Svensson",
                    City = "Stockholm",
                    Country = "Sweden",
                },
                new User
                {
                    FirstName = "Freya",
                    LastName = "Thompson",
                    City = "Rotorua",
                    Country = "New Zealand",
                },
                new User
                {
                    FirstName = "طاها",
                    LastName = "رضایی",
                    City = "اهواز",
                    Country = "Iran",
                },
                new User
                {
                    FirstName = "Yara",
                    LastName = "Simon",
                    City = "Develier",
                    Country = "Switzerland",
                },
                new User
                {
                    FirstName = "Giray",
                    LastName = "Erbay",
                    City = "Karabük",
                    Country = "Turkey",
                },
                new User
                {
                    FirstName = "Miodrag",
                    LastName = "Schaffer",
                    City = "Möckern",
                    Country = "Germany",
                },
                new User
                {
                    FirstName = "Carmela",
                    LastName = "Lo Castro",
                    City = "Firenze",
                    Country = "Italy",
                },
                new User
                {
                    FirstName = "Roberto",
                    LastName = "Sims",
                    City = "Mallow",
                    Country = "Ireland",
                },
            };
    }
}
