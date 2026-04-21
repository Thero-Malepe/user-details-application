namespace UserDetailsApi.Models.RequestModels.UserRequestModels
{
    public class LoginUserRequestModel
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
