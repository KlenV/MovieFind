using Ninject.Modules;

namespace BLL
{
    public class IoCCon_BLL : NinjectModule
    {
        public override void Load()
        {
            Bind<IBLogic>().To<BusinessLogic>();
        }
    }
}