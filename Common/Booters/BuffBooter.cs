namespace Gamefreak130.Common.Booters
{
    using Sims3.Gameplay.ActorSystems;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using Sims3.UI;

    public class BuffBooter : Booter
    {
        public BuffBooter(string xmlResource) : base(xmlResource)
        {
        }

        public override void LoadData()
        {
            AddBuffs(null);
            UIManager.NewHotInstallStoreBuffData += AddBuffs;
        }

        public void AddBuffs(ResourceKey[] resourceKeys)
        {
            if (XmlDbData.ReadData(mXmlResource) is XmlDbData xmlDbData)
            {
                BuffManager.ParseBuffData(xmlDbData, true);
            }
        }
    }
}
