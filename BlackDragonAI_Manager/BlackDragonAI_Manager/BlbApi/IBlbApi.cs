using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAI_Manager.BlbApi.Models;
using BlackDragonAI_Manager.Models;
using Refit;

namespace BlackDragonAI_Manager.BlbApi
{
    public interface IBlbApi
    {
        [Post("/users/login")]
        Task<AuthOutput> Authenticate([Body] User user);

        [Post("/users/register")]
        Task<AuthOutput> Register([Body] User user);

        [Put("/users/auth/{username}")]
        Task AuthUpdate([Header("X-Access-Token")] string authToken, string username, [Body] AuthorizationLevelInput authLevelInput);

        [Get("/users")]
        Task<IEnumerable<User>> GetUsers([Header("X-Access-Token")] string authToken);

        [Get("/commands")]
        Task<IEnumerable<CommandDetails>> GetCommands([Header("X-Access-Token")] string authToken);

        [Post("/commands")]
        Task<CommandDetails> CreateCommand([Header("X-Access-Token")] string authToken, [Body] CommandDetails commandDetails);

        [Put("/commands")]
        Task<CommandDetails> EditCommand([Header("X-Access-Token")] string authToken, [Body] CommandDetails commandDetails);

        [Delete("/commands/{commandName}")]
        Task DeleteCommand([Header("X-Access-Token")] string authToken, string commandName);

        [Post("/commands/alias/{commandName}")]
        Task AddAlias([Header("X-Access-Token")] string authToken, string commandName, [Body] AliasInput aliasInput);

        [Delete("/commands/alias/{alias}")]
        Task DeleteAlias([Header("X-Access-Token")] string authToken, string alias);

        [Post("/timedmessages")]
        Task<TimedMessage> CreateTimedMessage([Header("X-Access-Token")] string authToken, [Body] TimedMessage timedMessage);

        [Post("/timedmessages/{commandName}")]
        Task<TimedMessage> UpdateTimedMessage([Header("X-Access-Token")] string authToken, [Body] TimedMessage timedMessage, string commandName);

        [Get("/timedmessages")]
        Task<IEnumerable<TimedMessage>> GetTimedMessages([Header("X-Access-Token")] string authToken);

        [Delete("/timedmessages/{commandName}")]
        Task DeleteTimedMessage([Header("X-Access-Token")] string authToken, string commandName);

        [Put("/timedmessages/{commandName}")]
        Task<TimedMessage> EditTimedMessage([Header("X-Access-Token")] string authToken, string commandName,
            TimedMessage timedMessage);

        [Post("/streamplannings")]
        Task<StreamPlanning> CreateStreamPlanning([Header("X-Access-Token")] string authToken,
            [Body] StreamPlanning streamPlanning);

        [Put("/streamplannings/{id}")]
        Task<StreamPlanning> UpdateStreamPlanning([Header("X-Access-Token")] string authToken, long id,
            [Body] StreamPlanning streamPlanning);

        [Get("/streamplannings")]
        Task<IEnumerable<StreamPlanning>> GetStreamPlannings();

        [Get("/streamplannings/{id}")]
        Task<StreamPlanning> GetStreamPlanningById([Header("X-Access-Token")] string authToken, long id);

        [Delete("/streamplannings/{id}")]
        Task DeleteStreamPlanning([Header("X-Access-Token")] string authToken, long id);

        [Put("/streamplannings/discord/update")]
        Task UpdateDiscordStreamPlannings([Header("X-Access-Token")] string authToken);

        [Put("/streamplannings/discord/load")]
        Task LoadDiscordStreamPlannings([Header("X-Access-Token")] string authToken);
        
        [Post("/streamplannings/discord/share")]
        Task SharePlanningUpdate([Header("X-Access-Token")] string authToken);

        [Post("/reconnect")]
        Task Reconnect([Header("X-Access-Token")] string authToken);

        [Get("/auth")]
        Task<TwitchAuthInformation> GetAuthUrl([Header("X-Access-Token")] string authToken);
    }
}
