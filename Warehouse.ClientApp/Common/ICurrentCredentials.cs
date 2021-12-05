using System;
using System.Security;

namespace Warehouse.ClientApp.Common
{
    public interface ICurrentCredentials
    {
        bool IsLoggedIn { get; }
        Guid Id { get; }
        string Name { get; }
        SecureString Password { get; }
        void Login(Guid id, string name, SecureString password);
        void Logout();
    }
}
