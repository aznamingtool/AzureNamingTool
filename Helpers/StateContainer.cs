using AzNamingTool.Models;

namespace AzNamingTool.Helpers
{
    public class StateContainer
    {
        private bool? _verified;
        private bool? _admin;
        private bool? _password;

        public bool Verified
        {
            get => _verified ?? false;
            set
            {
                _verified = value;
                NotifyStateChanged();
            }
        }

        public void SetVerified(bool verified)
        {
            _verified = verified;
            NotifyStateChanged();
        }

        public bool Admin
        {
            get => _admin ?? false;
            set
            {
                _admin = value;
                NotifyStateChanged();
            }
        }

        public void SetAdmin(bool admin)
        {
            _admin = admin;
            NotifyStateChanged();
        }

        public bool Password
        {
            get => _password ?? false;
            set
            {
                _password = value;
                NotifyStateChanged();
            }
        }
        
        public void SetPassword(bool password)
        {
            _password = password;
            NotifyStateChanged();
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
