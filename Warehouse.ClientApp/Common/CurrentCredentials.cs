using System;
using System.Security;

namespace Warehouse.ClientApp.Common
{
    public class CurrentCredentials : ICurrentCredentials
    {
        private bool _isLoggedIn;
        private Guid _id;
        private string _name;
        private SecureString _password;

        public bool IsLoggedIn { get => _isLoggedIn; }
        public Guid Id { get => _id; }
        public string Name { get => _name; }
        public SecureString Password { get => _password; }

        public void Login(Guid id, string name, SecureString password)
        {
            _id = id;
            _name = name;
            _password = password;
            _isLoggedIn = true;
        }

        public void Logout()
        {
            _name = string.Empty;
            _password = null;
            _isLoggedIn = false;
        }
    }
}
