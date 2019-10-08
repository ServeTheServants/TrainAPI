namespace TrainAPI.Models
{
    public class User
    {
        public int UserId {get; set;}
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
