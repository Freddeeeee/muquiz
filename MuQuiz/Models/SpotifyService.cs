using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class SpotifyService
    {
        private SpotifyToken token;
        public SpotifyToken Token
        {
            get {
                if (DateTime.Now > token.Expires)
                    token = RequestToken(token.RefreshToken);

                return token;
            }
            set { token = value; }
        }

        IConfiguration configuration;

        public SpotifyService(IConfiguration configuration)
        {
            this.configuration = configuration;
            Token = RequestToken(configuration["Spotify:RefreshToken"]);
        }

        private SpotifyToken RequestToken(string code)
        {
            string ClientId = configuration["Spotify:ClientId"];
            string ClientSecret = configuration["Spotify:ClientSecret"];
            string ReturnURL = configuration["Spotify:ReturnURL"];

            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", code),
                new KeyValuePair<string, string>("redirect_uri", ReturnURL),
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
                var content = new FormUrlEncodedContent(args);

                var response = client.PostAsync("https://accounts.spotify.com/api/token", content).Result;
                var responseContent = response.Content;
                var responseString = responseContent.ReadAsStringAsync().Result;

                var token = JsonConvert.DeserializeObject<SpotifyToken>(responseString);
                token.Expires = DateTime.Now.AddSeconds(3600);

                if (string.IsNullOrEmpty(token.RefreshToken))
                    token.RefreshToken = code;

                return token;
            }
        }
    }
}