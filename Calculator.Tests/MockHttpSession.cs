using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Calculator.Tests.Controllers
{
    // copied from https://stackoverflow.com/a/59344713
    public class MockHttpSession : ISession
    {
        readonly Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();
        string ISession.Id => throw new NotImplementedException();
        bool ISession.IsAvailable => throw new NotImplementedException();
        IEnumerable<string> ISession.Keys => _sessionStorage.Keys;
        void ISession.Clear()
        {
            _sessionStorage.Clear();
        }
        Task ISession.CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        Task ISession.LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        void ISession.Remove(string key)
        {
            _sessionStorage.Remove(key);
        }
        void ISession.Set(string key, byte[] value)
        {
            _sessionStorage[key] = value;
        }
        bool ISession.TryGetValue(string key, out byte[] value)
        {
            return _sessionStorage.TryGetValue(key, out value);
        }
    }
}