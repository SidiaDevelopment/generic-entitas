using System;
using System.Collections.Generic;

namespace EntityFramework
{
    public class EntityManager : Singleton<EntityManager>
    {
        public ComponentListenerManager ListenerManager => _componentListenerManager;
        
        private HashSet<Entity> _entities = new();
        private Dictionary<Matcher, Group> _groups = new(new MatcherComparer());
        private Dictionary<Matcher, Collector> _collectors = new(new MatcherComparer());
        private Feature _systems;

        private readonly Stack<Entity> _entityPool = new();
        
        private readonly ComponentListenerManager _componentListenerManager;

        public EntityManager()
        {
            _componentListenerManager = new ComponentListenerManager();
        }

        public Entity CreateEntity()
        {
            var entity = _entityPool.Count > 0 ? _entityPool.Pop() : new Entity();
            entity.Initialize();
            
            entity.OnComponentRemoved += EntityOnComponentRemoved;
            entity.OnComponentAdded += EntityOnComponentAdded;
            
            _entities.Add(entity);
            return entity;
        }

        private void EntityOnComponentRemoved(Entity entity)
        {
            UpdateGroups(entity);
        }

        private void EntityOnComponentAdded(Entity entity, IComponent component)
        {
            UpdateGroups(entity);
        }
        
        private void UpdateGroups(Entity entity)
        {
            foreach (var (key, value) in _groups)
            {
                value.HandleEntity(entity);
            }
        }
        
        public Group GetGroup(params Type[] filter)
        {
            var matcher = new Matcher(filter);
            return GetGroup(matcher);
        }

        public Group GetGroup(Matcher matcher)
        {
            if (_groups.TryGetValue(matcher, out var cachedGroup))
                return cachedGroup;
            
            var group = new Group(matcher);
            
            foreach (var entity in _entities)
            {
                group.HandleEntity(entity);
            }
            _groups.Add(matcher, group);
            return group;
        }

        public Collector GetCollector(CollectorType collectorType, params Type[] filter)
        {
            var matcher = new Matcher(filter);
            return GetCollector(matcher, collectorType);
        }
        
        public Collector GetCollector(Matcher matcher, CollectorType collectorType)
        {
            var group = GetGroup(matcher);
            if (_collectors.TryGetValue(matcher, out var cachedGroup))
                return cachedGroup;

            var collector = new Collector(group, collectorType);
            _collectors.Add(matcher, collector);
            return collector;
        }
        
        public void DisposeEntity(Entity entity)
        {
            entity.Dispose();
            _entityPool.Push(entity);
            _entities.Remove(entity);
        }

        public void Initialize(Feature systems)
        {
            _systems = systems;
            _systems.Initialize();
        }
        
        public void Execute()
        {
            _systems.Execute();
            
            foreach (var (key, value) in _collectors)
            {
                value.Clear();
            }
        }

        public void Teardown()
        {
            _systems.Teardown();
            foreach (var entity in _entities)
            {
                entity.Dispose();
            }
            _entities.Clear();
            _entityPool.Clear();
            _groups.Clear();
            _collectors.Clear();
        }
    }
}