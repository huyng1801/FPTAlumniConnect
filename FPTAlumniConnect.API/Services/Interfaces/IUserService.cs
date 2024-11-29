﻿using FirebaseAdmin.Auth;
using FPTAlumniConnect.BusinessTier.Payload;
using FPTAlumniConnect.BusinessTier.Payload.User;
using FPTAlumniConnect.DataTier.Paginate;

namespace FPTAlumniConnect.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateNewUser(UserInfo request);
        Task<IPaginate<GetUserResponse>> ViewAllUser(UserFilter filter, PagingModel pagingModel);
        //Task<LoginResponse> LoginUser(LoginFirebaseRequest request);
        Task<bool> UpdateUserInfo(int id, UserInfo request);
        Task<GetUserResponse> GetUserById(int id);
        Task<LoginResponse> LoginUser(LoginFirebaseRequest request);
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<RegisterResponse> Register(RegisterRequest request);
    }
}
