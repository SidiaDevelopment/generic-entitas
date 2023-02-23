using UnityEngine;
using EntityFramework;

public class TestView : AutoEntityView
    , IComponentChangeListener<PositionComponent>
    , IComponentChangeListener<RotationComponent>
{
    public override void Link(EntityManager entityManager, Entity entity)
    {
        base.Link(entityManager, entity);
        _entity.AddComponent<PositionComponent>(new PositionComponent()
        {
            Value = Vector3.zero
        });
        
        _entity.AddComponent<RotationComponent>(new RotationComponent()
        {
            Value = Vector3.zero
        });
    }
    
    public void OnComponentChange(Entity entity, PositionComponent component)
    {
        transform.position = component.Value;
    }
    
    public void OnComponentChange(Entity entity, RotationComponent component)
    {
        transform.rotation = Quaternion.Euler(component.Value);
    }
    
    public void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _entity.ReplaceComponent(new VelocityComponent()
            {
                Value = Vector3.left * 2f
            });
        } else if (Input.GetKey(KeyCode.D))
        {
            _entity.ReplaceComponent(new VelocityComponent()
            {
                Value = Vector3.right * 2f
            });
        }
        else if (_entity.HasComponent<VelocityComponent>())
        {
            _entity.RemoveComponent<VelocityComponent>();
        }
    }
}