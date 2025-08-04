namespace TimeDepositAPI.DTOs
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
    }
    
    public class RegisterResponseDto
    {
        public required string Token { get; set; }
    }

    public class UpdateUserResponseDto
    {
        public required string Message { get; set; }
    }
}
