namespace VillaAPIProject.Model.Dto
{
	public class TokenDTO
	{
		//public LocalUser User { get; set; } before using Identity
		//using Identity
		//public UserDTO User { get; set; }

		//public string Role { get; set; } we can retrieve roles from token
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
