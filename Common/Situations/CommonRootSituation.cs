namespace Gamefreak130.Common.Situations
{
    using Sims3.Gameplay.Autonomy;
    using Sims3.Gameplay.Core;

    public abstract class CommonRootSituation : RootSituation
    {
        public CommonRootSituation()
        {
        }

        public CommonRootSituation(Lot lot) : base(lot) 
            => Initialize(null);

        public override void Initialize(Situation _)
        {
            try
            {
                Init();
            }
            catch
            {
                Exit();
                throw;
            }
        }

        protected abstract void Init();
    }
}
