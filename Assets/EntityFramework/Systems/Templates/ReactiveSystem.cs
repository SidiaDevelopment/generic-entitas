using System.Collections.Generic;

namespace EntityFramework
{
    public abstract class ReactiveSystem : System, IExecuteSystem
    {
        private List<Entity> _buffer = new();
        private Collector _collector;
        
        protected  ReactiveSystem(EntityManager manager) : base(manager)
        {
            _collector = GetTrigger(manager);
        }

        public void Execute()
        {
            Execute(_collector.GetEntities(_buffer));
        }

        protected abstract Collector GetTrigger(EntityManager manager); 
        protected abstract void Execute(List<Entity> entities);
        
    }
}