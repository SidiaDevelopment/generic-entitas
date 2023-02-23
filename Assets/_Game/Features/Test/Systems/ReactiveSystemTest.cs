using System.Collections.Generic;
using EntityFramework;
using UnityEngine;

public class ReactiveSystemTest : ReactiveSystem
{
    public ReactiveSystemTest(EntityManager manager) : base(manager)
    {
    }
    
    protected override Collector GetTrigger(EntityManager manager) => manager.GetCollector(CollectorType.Added,
        typeof(PositionComponent)
    );

    protected override void Execute(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            var pos = entity.GetComponent<PositionComponent>().Value;
            entity.ReplaceComponent(new RotationComponent()
            {
                Value = new Vector3(0, pos.x * 20, 0)
            });
        }
    }
}