using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using Unity.Services.Core.Environments;
using UnityEngine.Events;
using UnityEngine;
using Unity.Services.Analytics;

public class AnalyticsClass : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    async void Awake()
    {

        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        // Add a listener to apply settings when successfully retrieved:
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;


        // Fetch configuration settings from the remote service:
        RemoteConfigService.Instance.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        analyticsEnabled = false;
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                SetAnalytics();
                break;
        }
    }

    public static bool analyticsEnabled = false;
    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // options can be passed in the initializer, e.g if you want to set analytics-user-id or an environment-name use the lines from below:
        // var options = new InitializationOptions()
        //   .SetOption("com.unity.services.core.analytics-user-id", "my-user-id-1234")
        //   .SetOption("com.unity.services.core.environment-name", "production");
        // await UnityServices.InitializeAsync(options);

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async void SetAnalytics()
    {
        analyticsEnabled = RemoteConfigService.Instance.appConfig.GetBool("AnalyticsEnabled");
        List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
    }

    public static void SendCustomEvent(string name, Dictionary<string, object> parameters)
    {
        if (!analyticsEnabled || AbstractRoomLogic.instance.simulationRunning) return;

        /*Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "fabulousString", "hello there" },
            { "sparklingInt", 1337 },
            { "spectacularFloat", 0.451f },
            { "peculiarBool", true },
        };*/
        // The ‘myEvent’ event will get queued up and sent every minute
        AnalyticsService.Instance.CustomData(name, parameters);

        // Optional - You can call Events.Flush() to send the event immediately
        AnalyticsService.Instance.Flush();
    }
}
