using EntityFramework;
using Unity.VisualScripting;

public class TestFeature : Feature
{
    public TestFeature(EntityManager manager) : base(manager)
    {
        Add(new MoveSystem(manager));
        Add(new ReactiveSystemTest(manager));
    }
}