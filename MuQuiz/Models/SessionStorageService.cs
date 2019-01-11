using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuQuiz.Models
{
    public class SessionStorageService
    {
        private IHttpContextAccessor httpContextAccessor;
        private ISession Session => httpContextAccessor.HttpContext.Session;

        public SessionStorageService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Name
        {
            get { return Session.GetString(nameof(Name)); }
            set { Session.SetString(nameof(Name), value); }
        }

        public string GameId
        {
            get { return Session.GetString(nameof(GameId)); }
            set { Session.SetString(nameof(GameId), value); }
        }

        public string GetQRUrl(string gameId, int pixels = 100)
        {
            var fullUrl = GetBaseUrl() + "/player?gameId=" + gameId;
            return $"https://api.qrserver.com/v1/create-qr-code/?data={fullUrl}&size={pixels}x{pixels}";
        }

        public string GetBaseUrl()
        {
            var request = httpContextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $"{request.Scheme}://{host}{pathBase}";
        }

    }
}
