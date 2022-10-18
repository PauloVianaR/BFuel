using System;
using System.Collections.Generic;
using System.Text;

namespace BFuel.Modelos
{
    public class GoogleUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
