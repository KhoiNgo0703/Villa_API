﻿using VillaAPIProject.Model;
using VillaAPIProject.Model.Dto;

namespace VillaAPIProject.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO);
		Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
		Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
        Task RevokeRefreshToken(TokenDTO tokenDTO);
    }
}
