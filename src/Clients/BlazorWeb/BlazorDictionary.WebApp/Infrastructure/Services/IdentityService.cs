using BlazorDictionary.Common.Infrastructure.Exceptions;
using BlazorDictionary.Common.Infrastructure.Results;
using BlazorDictionary.Common.Models.Queries;
using BlazorDictionary.Common.Models.RequestModels;
using BlazorDictionary.WebApp.Infrastructure.Extensions;
using BlazorDictionary.WebApp.Infrastructure.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorDictionary.WebApp.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private JsonSerializerOptions defaultJsonOpt => new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ISyncLocalStorageService _syncLocalStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;


    public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _syncLocalStorageService = syncLocalStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }


    public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

    public string GetUserToken()
    {
        return _syncLocalStorageService.GetToken();
    }

    public string GetUserName()
    {
        return _syncLocalStorageService.GetToken();
    }

    public Guid GetUserId()
    {
        return _syncLocalStorageService.GetUserId();
    }

    public async Task<bool> Login(LoginUserCommand command)
    {
        string responseStr;
        var httpResponse = await _httpClient.PostAsJsonAsync("/api/User/Login", command);

        if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                responseStr = await httpResponse.Content.ReadAsStringAsync();
                var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr, defaultJsonOpt);
                responseStr = validation.FlattenErrors;
                throw new DatabaseValidationException(responseStr);
            }

            return false;
        }


        responseStr = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<LoginUserViewModel>(responseStr);

        if (!string.IsNullOrEmpty(response.Token)) // login success
        {
            _syncLocalStorageService.SetToken(response.Token);
            _syncLocalStorageService.SetUsername(response.UserName);
            _syncLocalStorageService.SetUserId(response.Id);

           // ((AuthStateProvider)authenticationStateProvider).NotifyUserLogin(response.UserName, response.Id);

           _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.Token);

            return true;
        }

        return false;
    }

    public void Logout()
    {
        _syncLocalStorageService.RemoveItem(LocalStorageExtension.TokenName);
        _syncLocalStorageService.RemoveItem(LocalStorageExtension.UserName);
        _syncLocalStorageService.RemoveItem(LocalStorageExtension.UserId);

       // ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}