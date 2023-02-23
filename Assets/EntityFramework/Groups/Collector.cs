using System.Collections.Generic;

namespace EntityFramework
{
    public enum CollectorType
    {
        Added,
        Removed,
        AddedOrRemoved
    }
    
    public class Collector
    {
        private HashSet<Entity> _collected = new HashSet<Entity>();
        private CollectorType _collectorType;

        public int Count => _collected.Count;
        
        public Collector(Group group, CollectorType collectorType = CollectorType.Added)
        {
            _collected.Clear();
            _collectorType = collectorType;
            group.OnEntityAdded += GroupOnOnEntityAdded;
            group.OnEntityRemoved += GroupOnOnEntityRemoved;
            group.OnEntityChanged += GroupOnOnEntityChanged;
        }

        private void GroupOnOnEntityChanged(Group group, Entity entity)
        {
            if (_collectorType == CollectorType.Removed) return;
            _collected.Add(entity);

        }

        private void GroupOnOnEntityRemoved(Group group, Entity entity)
        {
            if (_collectorType == CollectorType.Added) return;
            _collected.Add(entity);
        }

        private void GroupOnOnEntityAdded(Group group, Entity entity)
        {
            if (_collectorType == CollectorType.Removed) return;
            _collected.Add(entity);
        }

        public HashSet<Entity> GetEntities()
        {
            return _collected;
        }

        public List<Entity> GetEntities(List<Entity> buffer)
        {
            buffer.Clear();
            buffer.AddRange(_collected);
            return buffer;
        }
        
        public void Clear()
        {
            _collected.Clear();
        }
    }
}