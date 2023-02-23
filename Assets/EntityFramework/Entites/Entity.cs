using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityFramework
{
    public delegate void EntityComponentChanged(Entity entity, IComponent component);
    public delegate void EntityComponentRemoved(Entity entity);
    public delegate void EntityComponentAdded(Entity entity, IComponent component);
    
    public class Entity
    {
        private Dictionary<Type, IComponent> _components = new();
        private EntityManager _manager;

        public event EntityComponentAdded OnComponentAdded;
        public event EntityComponentRemoved OnComponentRemoved;

        public void Initialize()
        {
            _manager = EntityManager.Instance;
        }

        public void Dispose()
        {
            _components.Clear();
        }
        
        public void AddComponent<TComponent>(TComponent component) where TComponent : IComponent
        {
            var type = typeof(TComponent);
            if (_components.ContainsKey(type))
                throw new Exception("Entity already has component");
            
            _components.Add(type, component);
            
            _manager.ListenerManager.AddChange<TComponent>(this, component);
            OnComponentAdded?.Invoke(this, component);
        }
        
        public void ReplaceComponent<TComponent>(TComponent component) where TComponent : IComponent
        {
            var type = typeof(TComponent);
            _components[type] = component;
            
            _manager.ListenerManager.AddChange<TComponent>(this, component);
            OnComponentAdded?.Invoke(this, component);
        }
        
        public void RemoveComponent<TComponent>() where TComponent : IComponent
        {
            var type = typeof(TComponent);
            _components.Remove(type);
            
            _manager.ListenerManager.AddRemoval<TComponent>(this);
            OnComponentRemoved?.Invoke(this);

        }

        public TComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            var type = typeof(TComponent);
            if (!_components.TryGetValue(type, out var component))
                throw new Exception($"Entity does not have component {type.Name}");

            return (TComponent)component;
        }

        public bool HasComponent<TComponent>() where TComponent : IComponent
        {
            var type = typeof(TComponent);
            return HasComponent(type);
        }
        
        public bool HasComponent(Type type)
        {
            return _components.ContainsKey(type);
        }

        public bool HasComponents(Type[] types)
        {
            return types.All(HasComponent);
        }
    }
}