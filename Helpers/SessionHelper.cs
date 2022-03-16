using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace AzNamingTool.Helpers
{
    public class SessionHelper
    {
        private ProtectedSessionStorage storage;

        public SessionHelper(ProtectedSessionStorage storage)
        {
            this.storage = storage;
        }

        public async Task<object> GetSessionValue(string name)
        {
            return await storage.GetAsync<object>(name);
        }

        public async Task SetSessionValue(string name, object value)
        {
            await storage.SetAsync(name, value);
        }

    }
}
