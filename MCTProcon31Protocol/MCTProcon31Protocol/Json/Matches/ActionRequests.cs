using Newtonsoft.Json;

namespace MCTProcon31Protocol.Json.Matches
{
    [JsonObject]
    public class ActionRequests
    {
        [JsonProperty("actions")]
        public ActionRequest[] _data;

        [JsonIgnore]
        public ActionRequest this[int i] => _data[i];

        public ActionRequests() { }

        public ActionRequests(ActionRequest[] requests)
        {
            _data = requests;
        }
    }
}