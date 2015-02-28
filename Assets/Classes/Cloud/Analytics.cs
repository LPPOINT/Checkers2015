using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Core;
using Parse;
using UnityEngine.Cloud.Analytics;

namespace Assets.Classes.Cloud
{
    public class Analytics : UniqueGameSystem<Analytics>
    {

      

        public override void Load()
        {
            UnityAnalytics.StartSDK(GameCore.Instance.UnityAnalyticsProjectId);
            ParseAnalytics.TrackAppOpenedAsync();
        }


        public  void SendEvent(string name, IDictionary<string, object> data, AnalyticsServices services)
        {
            if (services == AnalyticsServices.ParseDotCom || services == AnalyticsServices.Both)
            {
                var specificData = data.ToDictionary(o => o.Key, o => o.Value.ToString());

                ParseAnalytics.TrackEventAsync(name, specificData);
            }
            if (services == AnalyticsServices.UnityAnalytics || services == AnalyticsServices.Both)
            {
                UnityAnalytics.CustomEvent(name, data);
            }
        }
        public  void SendEvent(string name, IDictionary<string, object> data)
        {
            SendEvent(name, data, AnalyticsServices.Both);
        }
        public  void SendEvent(string name)
        {
            SendEvent(name, new Dictionary<string, object>());
        }

        public  void SendEvent(IAnalyticsEventProvider provider, AnalyticsServices services)
        {
            SendEvent(provider.GetAnalyticsEventName(), provider.GetAnalyticsEventData(), services);
        }
        public  void SendEvent(IAnalyticsEventProvider providers)
        {
            SendEvent(providers, AnalyticsServices.Both);
        }

    }
}
