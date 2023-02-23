using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EntityFramework
{
    public enum ComponentListenerType
    {
        Change,
        Remove
    }
    
    public struct ComponentListener
    {
        public IComponentListener Listener;
        public ComponentListenerType ListenerType;
        [CanBeNull] public Entity FilterEntity;
    }

    
    public class ComponentListenerManager
    {
        private readonly Dictionary<Type, List<ComponentListener>> _listeners = new();

        public void AddListener(Type type, IComponentListener listener, [CanBeNull] Entity filterEntity = null, ComponentListenerType listenerType = ComponentListenerType.Change)
        {
            var list = new List<ComponentListener>();
            if (_listeners.TryGetValue(type, out var existing))
            {
                list = existing;
            }

            list.Add(new ComponentListener()
            {
                Listener = listener,
                FilterEntity = filterEntity,
                ListenerType = listenerType,
            });
            _listeners[type] = list;
        }
        
        public void RemoveListener(Type type, IComponentListener listener, [CanBeNull] Entity filterEntity, ComponentListenerType listenerType = ComponentListenerType.Change)
        {
            var list = new List<ComponentListener>();
            if (_listeners.TryGetValue(type, out var existing))
            {
                list = existing;
            }

            list.RemoveAll(l => l.Listener == listener && l.FilterEntity == filterEntity && l.ListenerType == listenerType);
            _listeners[type] = list;
        }

        public void AddChange<T>(Entity entity, T value) where T : IComponent
        {
            if (!_listeners.TryGetValue(typeof(T), out var typeListeners)) return;
            foreach (var componentListener in typeListeners)
            {
                if (componentListener.ListenerType != ComponentListenerType.Change) continue;
                if (componentListener.FilterEntity != null && componentListener.FilterEntity != entity) continue;
                
                var listener = (IComponentChangeListener<T>)componentListener.Listener;
                listener.OnComponentChange(entity, value);
            }
        }
        
        public void AddRemoval<T>(Entity entity) where T : IComponent
        {
            if (!_listeners.TryGetValue(typeof(T), out var typeListeners)) return;
            foreach (var componentListener in typeListeners)
            {
                if (componentListener.ListenerType != ComponentListenerType.Remove) continue;
                if (componentListener.FilterEntity != null && componentListener.FilterEntity != entity) continue;
                
                var listener = (IComponentRemoveListener<T>)componentListener.Listener;
                listener.OnComponentRemove(entity);
            }
        }
    }
}