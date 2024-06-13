using Azure.Core;
using Azure.Identity;
using FoundationaLLM;
using FoundationaLLM.Client.Core;
using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

class Program
{
    private static TokenCredential? Credentials;
    private static string? Scope;

    static async Task Main(string[] args)
    {
        // Initialize the default authentication.
        Credentials = new AzureCliCredential();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose setup method:")
                .AddChoices(new[]
                {
                    "Manual setup with no Dependency Injection",
                    "Use Dependency Injection with appsettings.json",
                    "Use Dependency Injection with Azure App Config"
                }));

        switch (choice)
        {
            case "Manual setup with no Dependency Injection":
                await ManualSetup();
                break;
            case "Use Dependency Injection with appsettings.json":
                await SetupWithDependencyInjection();
                break;
            case "Use Dependency Injection with Azure App Config":
                await SetupWithAzureAppConfig();
                break;
            default:
                AnsiConsole.Markup("[red]Invalid choice[/]");
                break;
        }

        AnsiConsole.MarkupLine("[green]Done[/]");
    }

    private static async Task ManualSetup()
    {
        Scope = "api://FoundationaLLM-Auth/Data.Read";
        var services = new ServiceCollection();

        // ------------------------------------------------------------------------
        // 1. Configure a Core API entry within the HttpClientFactory
        // ------------------------------------------------------------------------
        services.AddHttpClient(HttpClients.CoreAPI, client =>
        {
            client.BaseAddress = new Uri("https://localhost:63279");
            client.Timeout = TimeSpan.FromSeconds(900);
        });

        var serviceProvider = services.BuildServiceProvider();

        // ------------------------------------------------------------------------
        // 2. Instantiate the CoreClient and CoreRESTClient instances
        // ------------------------------------------------------------------------
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var coreRestClient = new CoreRESTClient(httpClientFactory);
        var coreClient = new CoreClient(coreRestClient);

        AnsiConsole.MarkupLine("[green]Manual setup with no Dependency Injection[/]");

        // ------------------------------------------------------------------------
        // 3. Make CoreClient and CoreRESTClient calls
        // ------------------------------------------------------------------------
        await MakeCoreClientCalls(coreClient, coreRestClient);
    }

    private static async Task SetupWithDependencyInjection()
    {
        // ------------------------------------------------------------------------
        // 1. Set up Dependency Injection and read config from appsettings.json
        // ------------------------------------------------------------------------
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Get the scope from the appsettings.json file.
        Scope = configuration[AppConfigurationKeys.FoundationaLLM_Chat_Entra_Scopes];

        var services = new ServiceCollection();

        // ------------------------------------------------------------------------
        // 2. Use the CoreClient extension method to add the CoreClient to the service collection
        // ------------------------------------------------------------------------
        services.AddCoreClient(configuration);

        var serviceProvider = services.BuildServiceProvider();

        // ------------------------------------------------------------------------
        // 3. Retrieve the CoreClient and CoreRESTClient instances
        // ------------------------------------------------------------------------
        var coreClient = serviceProvider.GetRequiredService<ICoreClient>();
        var coreRestClient = serviceProvider.GetRequiredService<ICoreRESTClient>();

        AnsiConsole.MarkupLine("[green]Setup with Dependency Injection using appsettings.json[/]");

        // ------------------------------------------------------------------------
        // 4. Make CoreClient and CoreRESTClient calls
        // ------------------------------------------------------------------------
        await MakeCoreClientCalls(coreClient, coreRestClient);
    }

    private static async Task SetupWithAzureAppConfig()
    {
        // ------------------------------------------------------------------------
        // 1. Set up Dependency Injection and read config from Azure App Configuration
        // ------------------------------------------------------------------------
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable(EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString));
                options.ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(Credentials);
                });
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_Instance);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_APIs);
                options.Select(AppConfigurationKeyFilters.FoundationaLLM_Chat_Entra);
            })
            .Build();

        // Get the scope from the Azure App Configuration.
        Scope = configuration[AppConfigurationKeys.FoundationaLLM_Chat_Entra_Scopes];

        var services = new ServiceCollection();

        // ------------------------------------------------------------------------
        // 2. Use the CoreClient extension method to add the CoreClient to the service collection
        // ------------------------------------------------------------------------
        services.AddCoreClient(configuration);

        var serviceProvider = services.BuildServiceProvider();

        // ------------------------------------------------------------------------
        // 3. Retrieve the CoreClient and CoreRESTClient instances
        // ------------------------------------------------------------------------
        var coreClient = serviceProvider.GetRequiredService<ICoreClient>();
        var coreRestClient = serviceProvider.GetRequiredService<ICoreRESTClient>();

        AnsiConsole.MarkupLine("[green]Setup with Dependency Injection using Azure App Config[/]");

        // ------------------------------------------------------------------------
        // 4. Make CoreClient and CoreRESTClient calls
        // ------------------------------------------------------------------------
        await MakeCoreClientCalls(coreClient, coreRestClient);
    }

    private static async Task MakeCoreClientCalls(ICoreClient coreClient, ICoreRESTClient coreRestClient)
    {
        // Use the REST client directly on an anonymous method (no authentication token required):
        var status = await coreRestClient.Status.GetServiceStatusAsync();
        AnsiConsole.WriteLine(status);

        // Retrieve an authentication token:
        var token = await GetAuthToken();

        // Use the CoreClient with an authentication token:
        var results = await coreClient.GetAgentsAsync(token);
        AnsiConsole.WriteLine("\r\nAgents:");
        var agents = results.Select(x => x.Resource).ToList();

        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Description");

        foreach (var agent in agents)
        {
            table.AddRow(agent.Name, agent.Description ?? string.Empty);
        }

        AnsiConsole.Write(table);

        if (agents.Count == 0) return;
        // Take the first agent and send a completion request:
        var agentName = agents.FirstOrDefault()?.Name;
        if (string.IsNullOrWhiteSpace(agentName)) return;

        AnsiConsole.WriteLine($"Asking the {agentName} agent \"Who are you?\"");
        var completion = await coreClient.SendSessionlessCompletionAsync("Who are you?", agentName, token);
        AnsiConsole.WriteLine($"Agent completion response: {completion.Text}");
    }

    private static async Task<string> GetAuthToken()
    {
        if (Scope == null) return string.Empty;

        // The scope needs to just be the base URI, not the full URI.
        var scope = Scope[..Scope.LastIndexOf('/')];

        if (Credentials == null) return string.Empty;
        var tokenResult = await Credentials.GetTokenAsync(
            new([scope]),
            default);

        return tokenResult.Token;
    }
}
