using ChatAppController.Models;
using System.Collections.Concurrent;

namespace ChatAppController.Data
{
    public class ShareDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new ConcurrentDictionary<string, UserConnection>();
        public ConcurrentDictionary<string, UserConnection> connections => _connections;
    }
}
