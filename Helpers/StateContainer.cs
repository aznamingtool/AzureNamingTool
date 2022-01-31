namespace AzNamingTool.Helpers
{
    public class StateContainer
    {
        private bool? verified;
        private bool? admin;

        public bool Verified
        {
            get => verified ?? false;
            set
            {
                verified = value;
                NotifyStateChanged();
            }
        }

        public bool Admin
        {
            get => admin ?? false;
            set
            {
                admin = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
