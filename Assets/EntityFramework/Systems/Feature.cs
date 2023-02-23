using System.Collections.Generic;

namespace EntityFramework
{
    public class Feature : System, IExecuteSystem, IInitializeSystem, ITeardownSystem
    {
        List<IExecuteSystem> _executeSystems = new();
        List<IInitializeSystem> _initializeSystems = new();
        List<ITeardownSystem> _teardownSystems = new();
        
        public Feature(EntityManager manager) : base(manager)
        {
        }

        public void Add(System system)
        {
            if (system is IExecuteSystem executeSystem)
                _executeSystems.Add(executeSystem);
            
            if (system is IInitializeSystem initializeSystem)
                _initializeSystems.Add(initializeSystem);
            
            if (system is ITeardownSystem teardownSystem)
                _teardownSystems.Add(teardownSystem);
        }

        public void Execute()
        {
            foreach (var system in _executeSystems)
            {
                system.Execute();
            }
        }


        public void Initialize()
        {
            foreach (var system in _initializeSystems)
            {
                system.Initialize();
            }
        }

        public void Teardown()
        {
            foreach (var system in _teardownSystems)
            {
                system.Teardown();
            }
        }
    }
}