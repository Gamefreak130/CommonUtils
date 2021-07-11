namespace Gamefreak130.Common.Booters
{
    using Sims3.Gameplay.ActorSystems;
    using Sims3.Gameplay.Utilities;

    public class BuffBooter : Booter
    {
        public BuffBooter(string xmlName) : base(xmlName)
        {
        }

        protected string BuffData => GetResourceAt(0);

        protected override void LoadData()
        {
            if (XmlDbData.ReadData(BuffData, true) is XmlDbData xmlDbData)
            {
                BuffManager.ParseBuffData(xmlDbData, true);
            }
        }
    }
}
