namespace Gamefreak130.Common.Booters
{
    using Sims3.Gameplay.Objects;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using System;

    public class ObjectCategoryBooter : Booter
    {
        public ObjectCategoryBooter(string xmlName) : base(xmlName)
        {
        }

        protected string CategoryInfoData => GetResourceAt(0);

        protected override void LoadData()
        {
            XmlDbData xmlDbData = XmlDbData.ReadData(CategoryInfoData);
            XmlDbTable xmlDbTable = xmlDbData.Tables["Categories"];
            foreach (XmlDbRow xmlDbRow in xmlDbTable.Rows)
            {
                string key = xmlDbRow.GetString("Key");
                if (!ObjectCategoryInfo.sObjectCategories.TryGetValue(key, out ObjectCategoryInfo objectCategoryInfo))
                {
                    string iconName = xmlDbRow.GetString("DreamsAndPromisesIcon");
                    bool allowOnVacation = xmlDbRow.GetBool("AllowOnVacation");
                    ProductVersion productVersion = ObjectCategoryInfo.ParseProductVersions(xmlDbRow);
                    string requiredMedatorInstance = xmlDbRow.GetString("RequiredMedatorInstance");
                    if (string.IsNullOrEmpty(requiredMedatorInstance) || NameGuidMap.GetGuidByName(requiredMedatorInstance) != 0UL)
                    {
                        int num = MathUtils.CountBits((uint)productVersion);
                        ResourceKey iconKey = (num == 1 && iconName != "placeholder")
                            ? ResourceKey.CreatePNGKey(iconName, ResourceUtils.ProductVersionToGroupId(productVersion))
                            : ResourceKey.CreatePNGKey(iconName, ResourceUtils.ProductVersionToGroupId(ProductVersion.BaseGame));
                        objectCategoryInfo = new ObjectCategoryInfo(key, iconKey, allowOnVacation);
                        Type t = ObjectCategoryInfo.ParseType(xmlDbRow);
                        objectCategoryInfo.AddType(t);
                        objectCategoryInfo.AddProductVersion(productVersion);
                        ObjectCategoryInfo.sObjectCategories.Add(key, objectCategoryInfo);
                    }
                }
                else
                {
                    ObjectCategoryInfo.AddRowToEntry(objectCategoryInfo, xmlDbRow);
                }
            }
            ObjectCategoryInfo.RemoveInvalidCategories();
        }
    }
}
