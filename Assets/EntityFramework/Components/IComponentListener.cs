using JetBrains.Annotations;

namespace EntityFramework
{
    public interface IComponentListener { }
    
    public interface IComponentChangeListener : IComponentListener { }
    public interface IComponentChangeListener<in TComponent> : IComponentChangeListener where TComponent : IComponent
    {
        public void OnComponentChange(Entity entity, TComponent component);
    }
    
    public interface IComponentRemoveListener : IComponentListener { }
    public interface IComponentRemoveListener<in TComponent> : IComponentRemoveListener where TComponent : IComponent
    {
        public void OnComponentRemove(Entity entity);
    }
}