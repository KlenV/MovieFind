using Ninject.Modules;

namespace DAL
{
    public class IoCCon : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}

