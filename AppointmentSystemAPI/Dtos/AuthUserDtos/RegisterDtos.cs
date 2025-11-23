namespace AppointmentSystemAPI.Dtos.AuthUserDtos
{
    public class RegisterDtos
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ShopOwners { get; set; }

        public bool Worker { get; set; }

        public string Role { get; set; }    
    }
}
