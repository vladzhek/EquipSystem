using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, IGameService> _services = new Dictionary<Type, IGameService>();
        
        public void Register<T>(T service) where T : IGameService
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"Overwriting.");
                _services[type].Cleanup();
            }
        
            _services[type] = service;
            service.Initialize();
        }
        
        public T Get<T>() where T : IGameService
        {
            var type = typeof(T);
            if (!_services.TryGetValue(type, out IGameService service))
            {
                throw new InvalidOperationException($"Not found");
            }
            return (T)service;
        }
        
        public void Unregister<T>() where T : IGameService
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out IGameService service))
            {
                service.Cleanup();
                _services.Remove(type);
            }
        }
        
        public void ClearAll()
        {
            foreach (var service in _services.Values)
            {
                service.Cleanup();
            }
            _services.Clear();
        }
    }
}