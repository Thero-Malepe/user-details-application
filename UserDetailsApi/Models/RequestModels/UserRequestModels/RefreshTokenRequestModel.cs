namespace UserDetailsApi.Models.RequestModels.UserRequestModels
{
    public class RefreshTokenRequestModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
