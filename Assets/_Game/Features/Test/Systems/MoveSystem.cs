using System.Collections.Generic;
using EntityFramework;
using UnityEngine;

public class MoveSystem : EntityFramework.System, IExecuteSystem
{
    private readonly Group _group;
    private readonly List<Entity> _buffer = new List<Entity>();

    public MoveSystem(EntityManager manager) : base(manager)
    {
        _group = _manager.GetGroup(
            typeof(PositionComponent),
            typeof(VelocityComponent)
        );
    }
    
    public void Execute()
    {
        foreach (var entity in _group.GetEntities(_buffer))
        {
            var velocity = entity.GetComponent<VelocityComponent>().Value;
            var pos = entity.GetComponent<PositionComponent>().Value;
            entity.ReplaceComponent<PositionComponent>(new PositionComponent()
            {
                Value = pos + velocity * Time.deltaTime
            });
        }
    }

    
}