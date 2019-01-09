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

    }
}
