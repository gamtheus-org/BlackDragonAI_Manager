using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BlackDragonAI_Manager.BlbApi.Models;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using Refit;

namespace BlackDragonAI_Manager.BlbApi
{
    public class BlbApiHandler
    {
        private readonly IBlbApi _blbApi;
        private string _jwt;
        private User _user;
        private Timer _timer;
        private readonly ILocalStorageService _localStorageService;

        public BlbApiHandler(IBlbApi blbApi, ILocalStorageService storageService)
        {
            this._blbApi = blbApi;
            this._localStorageService = storageService;
        }

        public void SetupUser(User user)
        {
            this._user = user;
        }

        // Endpoints
        public async Task<AuthOutput> Authenticate()
        {
            AuthOutput authResult = null;
            authResult = await _blbApi.Authenticate(this._user);
//            catch (ApiException e)
//            {
//                var apiError = e.ToBlbApiError();
//                Console.WriteLine(apiError.Message);
//            }
            
            this._jwt = authResult.Token;
            // setup reauth timer
//            this._timer = new Timer(authResult.ExpiresInSeconds * 1000);
//            this._timer.AutoReset = false;
//            this._timer.Elapsed += (sender, args) => Authenticate().RunSynchronously();
//            this._timer.Start();
            return authResult;
        }

        public async Task<AuthOutput> Register(User user)
        {
            var authResult = await this._blbApi.Register(user);
            SetupUser(user);
            this._jwt = authResult.Token;
            return authResult;
        }

        public async Task UpdateAuthorizationLevel(string username, EAuthorizationLevel authLevel) =>
            await (await CheckAuthentication()).AuthUpdate(this._jwt, username, new AuthorizationLevelInput(){AuthorizationLevel = authLevel});

        public async Task<IEnumerable<User>> GetUsers() => 
            await (await CheckAuthentication()).GetUsers(this._jwt);

        public async Task<IEnumerable<CommandDetails>> GetAllCommands() =>
            await (await CheckAuthentication()).GetCommands(this._jwt);

        public async Task<CommandDetails> CreateCommand(CommandDetails commandDetails) =>
            await(await CheckAuthentication()).CreateCommand(this._jwt, commandDetails);

        public async Task<CommandDetails> EditCommand(CommandDetails commandDetails) =>
            await(await CheckAuthentication()).EditCommand(this._jwt, commandDetails);

        public async Task DeleteCommand(string commandName) =>
            await(await CheckAuthentication()).DeleteCommand(this._jwt, commandName.Substring(1));

        public async Task AddAlias(string commandName, string alias) =>
            await (await CheckAuthentication()).AddAlias(this._jwt, commandName.Substring(1), new AliasInput() { Alias = alias });

        public async Task DeleteAlias(string alias) =>
            await(await CheckAuthentication()).DeleteAlias(this._jwt, alias.Substring(1));

        public async Task<IEnumerable<TimedMessage>> GetTimedMessage() =>
            await (await CheckAuthentication()).GetTimedMessages(this._jwt);

        public async Task<TimedMessage> CreateTimedMessage(TimedMessage timedMessage) =>
            await (await CheckAuthentication()).CreateTimedMessage(this._jwt, timedMessage);

        public async Task<TimedMessage> UpdateTimedMessage(TimedMessage timedMessage) =>
            await (await CheckAuthentication()).UpdateTimedMessage(this._jwt, timedMessage, timedMessage.Command);

        public async Task DeleteTimedMessage(string command) =>
            await (await CheckAuthentication()).DeleteTimedMessage(this._jwt, command);

        public async Task EditTimedMessage(TimedMessage timedMessage) =>
            await (await CheckAuthentication()).EditTimedMessage(this._jwt, timedMessage.Command.Substring(1), timedMessage);

        public async Task Reconnect() =>
            await (await CheckAuthentication()).Reconnect(this._jwt);

        public async Task<StreamPlanning> CreateStreamPlanning(StreamPlanning streamPlanning) =>
            await (await CheckAuthentication()).CreateStreamPlanning(this._jwt, streamPlanning);

        public async Task<StreamPlanning> UpdateStreamPlanning(StreamPlanning streamPlanning) =>
            await (await CheckAuthentication()).UpdateStreamPlanning(this._jwt, streamPlanning.Id, streamPlanning);

        public async Task<IEnumerable<StreamPlanning>> GetStreamPlannings() =>
            await this._blbApi.GetStreamPlannings();

        public async Task<StreamPlanning> GetStreamPlanningById(long id) =>
            await (await CheckAuthentication()).GetStreamPlanningById(this._jwt, id);

        public async Task DeleteStreamPlanning(long id) =>
            await (await CheckAuthentication()).DeleteStreamPlanning(this._jwt, id);

        public async Task UpdateDiscordStreamPlannings() =>
            await (await CheckAuthentication()).UpdateDiscordStreamPlannings(this._jwt);

        public async Task LoadDiscordStreamPlannings() =>
            await (await CheckAuthentication()).LoadDiscordStreamPlannings(this._jwt);

        public async Task SharePlanningUpdate() => 
            await (await CheckAuthentication()).SharePlanningUpdate(this._jwt);

        public async Task<IBlbApi> CheckAuthentication()
        {
            const string storageKey = "user";
            if(this._jwt == null && await this._localStorageService.ContainKeyAsync(storageKey))
            {
                this._user = await this._localStorageService.GetItemAsync<User>(storageKey);
                await Authenticate();
            }
            return this._blbApi;
        }
    }

    public static class BlbApiExtensions
    {
        public static ApiError ToBlbApiError(this ApiException apiException) =>
            JsonConvert.DeserializeObject<ApiError>(apiException.Content);
    }
}
