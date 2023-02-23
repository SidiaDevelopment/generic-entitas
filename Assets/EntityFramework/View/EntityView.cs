using UnityEngine;

namespace EntityFramework
{
    public class EntityView : MonoBehaviour
    {
        protected Entity _entity;
        protected EntityManager _entityManager;

        public virtual void Link(EntityManager entityManager, Entity entity)
        {
            _entity = entity;
            _entityManager = entityManager;

            TypeTools.IterateGenericInterfaces<IComponentListener>(this, type =>
            {
                _entityManager.ListenerManager.AddListener(type, (IComponentListener)this, _entity);
            });
            
            TypeTools.IterateGenericInterfaces<IComponentRemoveListener>(this, type =>
            {
                _entityManager.ListenerManager.AddListener(type, (IComponentListener)this, _entity, ComponentListenerType.Remove);
            });
        }
        
        public virtual void Unlink()
        {
            TypeTools.IterateGenericInterfaces<IComponentListener>(this, type =>
            {
                _entityManager.ListenerManager.RemoveListener(type, (IComponentListener)this, _entity);
            });
            
            TypeTools.IterateGenericInterfaces<IComponentListener>(this, type =>
            {
                _entityManager.ListenerManager.RemoveListener(type, (IComponentListener)this, _entity, ComponentListenerType.Remove);
            });
        }
    }
}