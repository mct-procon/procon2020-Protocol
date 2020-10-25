using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MCTProcon31Protocol.Json
{
    public class ProconAPIClient : IDisposable
    {
        private HttpClient hc { get; set; }
        private string endPoint { get; set; }

        public ProconAPIClient(string apiToken, string endPoint)
        {
            hc = new HttpClient();
            PrepareHeader(apiToken);
            this.endPoint = endPoint[endPoint.Length - 1] == '/' ? endPoint : (endPoint + "/");
        }

        private void PrepareHeader(string apiToken)
        {
            if (hc.DefaultRequestHeaders.Contains("x-api-token"))
                hc.DefaultRequestHeaders.Remove("x-api-token");
            hc.DefaultRequestHeaders.Add("x-api-token", apiToken);
        }

#nullable enable
        public void ChangeTokenAndEndPoint(string? apiToken, string? endPoint)
        {
            if (!(apiToken is null)) PrepareHeader(apiToken);
            if (!(endPoint is null)) this.endPoint = endPoint;
        }
#nullable disable
        private async Task<APIResult<T>> Get<T>(string location) where T : class
        {
            var response = await hc.GetAsync(endPoint + location);
            return await APIResult<T>.ResponseToResult(response);
        }

        private async Task<APIResult<V>> Post<T, V>(string location, T content) where V : class
        {
            using (var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8)) {
                var response = await hc.PostAsync(endPoint + location, httpContent);
                return await APIResult<V>.ResponseToResult(response);
            }
        }
        public Task<APIResult<Teams.Me>> Me() => Get<Teams.Me>("teams/me");
        public Task<APIResult<Matches.Matches>> TeamMatches(int teamId) => Get<Matches.Matches>("teams/" + teamId.ToString() + "/matches");
        public Task<APIResult<Matches.Matches>> TeamMatches(Teams.Me teamId) => TeamMatches(teamId.Id);
        public Task<APIResult<Matches.Matches>> TeamMatches(Matches.Team teamId) => TeamMatches(teamId.Id);
        public Task<APIResult<Matches.Matches>> TeamMatches(Matches.TeamInformation teamId) => TeamMatches(teamId.Id);
        public Task<APIResult<Matches.Matches>> Matches() => Get<Matches.Matches>("matches");
        public Task<APIResult<Matches.Match>> Match(int matchId) => Get<Matches.Match>("matches/" + matchId.ToString());
        public Task<APIResult<Matches.Match>> Match(Matches.MatchInformation matchId) => Match(matchId.Id);
        public Task<APIResult<Matches.AgentActions>> SendAction(int matchId, Matches.AgentActions actions) => Post<Matches.AgentActions, Matches.AgentActions>("matches/" + matchId.ToString() + "/action", actions);

        public void Dispose()
        {
            ((IDisposable)hc).Dispose();
        }
    }
    public struct APIResult<T> where T : class
    {
        public int HTTPReturnCode { get; set; }
        public T Value { get; private set; }
        public bool IsSuccess => !(Value is null);

        internal APIResult(int httpCode, T val)
        {
            HTTPReturnCode = httpCode;
            Value = val;
        }

        internal APIResult(int httpCode)
        {
            HTTPReturnCode = httpCode;
            Value = null;
        }

        internal APIResult(T val)
        {
            HTTPReturnCode = (int)System.Net.HttpStatusCode.OK;
            Value = val;
        }

        internal static async Task<APIResult<T>> ResponseToResult(HttpResponseMessage response) =>
            (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                new APIResult<T>(JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()))
            :
                new APIResult<T>((int)response.StatusCode);
    }
}
