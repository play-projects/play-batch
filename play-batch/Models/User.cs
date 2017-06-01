namespace platch.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Uid { get; set; }
        public string Token { get; set; }

        public static User NotFound = new User
        {
            Username = string.Empty,
            Password = string.Empty,
            Uid = 0,
            Token = string.Empty
        };
    }
}
