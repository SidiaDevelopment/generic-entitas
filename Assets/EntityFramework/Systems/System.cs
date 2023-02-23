namespace EntityFramework
{
    public abstract class System
    {
        protected EntityManager _manager;
        
        protected System(EntityManager manager)
        {
            _manager = manager;
        }
    }
}