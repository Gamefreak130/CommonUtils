namespace Gamefreak130.Common.Booters
{
    using Sims3.Gameplay.Socializing;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using System.Xml;

    public class SocialBooter : Booter
    {
        public SocialBooter(string xmlResource) : base(xmlResource)
        {
        }

        public override void LoadData()
        {
            XmlDocument xmlDocument = Simulator.LoadXML(mXmlResource);
            bool isEp5Installed = GameUtils.IsInstalled(ProductVersion.EP5);
            if (xmlDocument is not null)
            {
                foreach (XmlElement current in new XmlElementLookup(xmlDocument)["Action"])
                {
                    XmlElementLookup table = new(current);
                    ParserFunctions.TryParseEnum(current.GetAttribute("com"), out CommodityTypes intendedCom, CommodityTypes.Undefined);
                    ActionData data = new(current.GetAttribute("key"), intendedCom, ProductVersion.BaseGame, table, isEp5Installed);
                    ActionData.Add(data);
                }
            }
        }
    }
}
