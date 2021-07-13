namespace Gamefreak130.Common.Booters
{
    using Sims3.Gameplay.DreamsAndPromises;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using System.Collections.Generic;
    using System.Linq;
    using static Sims3.SimIFace.ResourceUtils;

    public class MajorDreamBooter : DreamTreeBooter
    {
        public MajorDreamBooter(string treeFileName, string linkedDreamsFileName) : this(null, treeFileName, linkedDreamsFileName)
        {
        }

        public MajorDreamBooter(string primitivesFileName, string treeFileName, string linkedDreamsFileName) : base(primitivesFileName, treeFileName) 
            => AddResource(linkedDreamsFileName);

        protected string LinkedDreams => GetResourceAt(2);

        protected override void LoadData()
        {
            base.LoadData();
            if (DreamsAndPromisesManager.sDreamTrees.TryGetValue(HashString64(DreamTree), out DreamTree tree))
            {
                DreamsAndPromisesManager.sCasFeederTrees.Add(tree);
                foreach (DreamNodeInstance node in tree.Root.Children
                                                            .Where(nodeBase => GameUtils.IsInstalled(nodeBase.Primitive.RequiredProductVersions))
                                                            .OfType<DreamNodeInstance>())
                {
                    DreamsAndPromisesManager.sMajorWishes.Add(node.PrimitiveId, node);
                }
            }
            ParseLinkedWishes();
        }

        private void ParseLinkedWishes()
        {
            XmlDbData xmlDbData = XmlDbData.ReadData(LinkedDreams);
            XmlDbTable xmlDbTable = xmlDbData.Tables["WishLink"];
            foreach (XmlDbRow xmlDbRow in xmlDbTable.Rows)
            {
                string str = xmlDbRow.GetString("MajorDream");
                uint majorDreamId = ParserFunctions.TryParseEnum(str, out DreamNames majorDream, DreamNames.none) ? (uint)majorDream : HashString32(str);
                if (!DreamsAndPromisesManager.sLinkedWishes.TryGetValue(majorDreamId, out List<DreamsAndPromisesManager.LinkedWishInfo> list))
                {
                    list = new List<DreamsAndPromisesManager.LinkedWishInfo>();
                    DreamsAndPromisesManager.sLinkedWishes.Add(majorDreamId, list);
                }
                str = xmlDbRow.GetString("LinkedWish");
                uint primitiveId = ParserFunctions.TryParseEnum(str, out DreamNames linkedPrimitive, DreamNames.none) ? (uint)linkedPrimitive : HashString32(str);
                string subject = xmlDbRow.GetString("Subject");
                int num = xmlDbRow.GetInt("Number", int.MinValue);
                list.Add(new DreamsAndPromisesManager.LinkedWishInfo(primitiveId, subject, num));
            }
        }
    }
}
