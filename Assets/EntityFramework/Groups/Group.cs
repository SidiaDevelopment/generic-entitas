using System.Collections.Generic;
using UnityEngine;

namespace EntityFramework
{
    public delegate void EntityAdded(Group group, Entity entity);
    public delegate void EntityRemoved(Group group, Entity entity);
    public delegate void EntityChanged(Group group, Entity entity);

    public class Group
    {
        public event EntityAdded OnEntityAdded;
        public event EntityRemoved OnEntityRemoved;
        public event EntityChanged OnEntityChanged;
        
        private Matcher _matcher;
        private HashSet<Entity> _entities = new();

        public Group(Matcher matcher)
        {
            _matcher = matcher;
        }

        public int Count => _entities.Count;

        public void HandleEntity(Entity entity)
        {
            OnEntityChanged?.Invoke(this, entity);
            if (_entities.Contains(entity))
            {
                if (!_matcher.Matches(entity))
                {
                    _entities.Remove(entity);
                    OnEntityRemoved?.Invoke(this, entity);
                }
            }
            else
            {
                if (_matcher.Matches(entity))
                {
                    _entities.Add(entity);
                    OnEntityAdded?.Invoke(this, entity);
                }
            }
        }

        public HashSet<Entity> GetEntities()
        {
            return _entities;
        }
        

        public List<Entity> GetEntities(List<Entity> buffer)
        {
            buffer.Clear();
            buffer.AddRange(_entities);
            return buffer;
        }
    }
}