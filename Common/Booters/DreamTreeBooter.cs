namespace Gamefreak130.Common.Booters
{
    using Sims3.Gameplay.DreamsAndPromises;
    using Sims3.Gameplay.EventSystem;
    using Sims3.Gameplay.Objects.DreamsAndPromises;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using Sims3.SimIFace.CAS;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using static Sims3.SimIFace.ResourceUtils;

    public class DreamTreeBooter : Booter
    {
        public DreamTreeBooter(string treeFileName) : this(null, treeFileName)
        {
        }

        public DreamTreeBooter(string primitivesFileName, string treeFileName) : base(primitivesFileName, treeFileName)
        {
        }

        protected string DreamNodePrimitives => GetResourceAt(0);

        protected string DreamTree => GetResourceAt(1);

        protected override void LoadData()
        {
            XmlDbData primitivesData = XmlDbData.ReadData(DreamNodePrimitives);
            if (primitivesData is not null)
            {
                List<DreamNodePrimitive> primitivesToCache = new(DreamsAndPromisesManager.sNodePrimitves.Values);
                ParseNodePrimitives(primitivesData, ref primitivesToCache);
                if (CacheManager.IsCachingEnabled)
                {
                    CacheManager.SaveTuningData("DreamsAndPromisesPrimitives", primitivesToCache);
                }
                DreamsAndPromisesManager.MergeEquivalentNodesWithStore();
            }

            // All DMTR resources (key 0x0604ABDA) are automatically parsed during GameplayInitialize,
            // But since there's no way to inject new primitives then, custom trees will break during parsing and be left empty
            // To delay parsing until the booter can set the primitives up, we store the tree as a generic XML resource instead
            ResourceKey dreamTreeKey = ResourceKey.CreateXMLKey(DreamTree, 0);
            List<DreamTree> dreamTreesToCache = new(DreamsAndPromisesManager.sDreamTrees.Values);
            bool flag = DreamsAndPromisesManager.ParseDreamTreeByKey(dreamTreeKey, DreamsAndPromisesManager.ParseDefaults(), ref dreamTreesToCache);
            if (CacheManager.IsCachingEnabled && flag)
            {
                CacheManager.SaveTuningData("DreamsAndPromisesTrees", dreamTreesToCache);
            }
        }

        private static void ParseNodePrimitives(XmlDbData primitivesData, ref List<DreamNodePrimitive> primitivesToCache)
        {
            Type defaultDelegateTarget = typeof(DreamsAndPromisesDelegateFunctions);
            foreach (XmlDbRow xmlDbRow in primitivesData.Tables["Primitives"].Rows)
            {
                uint id = xmlDbRow.GetUInt("Id");
                string name = xmlDbRow.GetString("Name");
                xmlDbRow.TryGetEnum("RequiredProductVersions", out ProductVersion productVersion, ProductVersion.Undefined);
                if (GameUtils.IsInstalled(productVersion))
                {
                    string category = xmlDbRow.GetString("Category");
                    bool isSocialPrimitive = xmlDbRow.GetBool("IsSocialPrimitive");
                    bool acceptsNumber = xmlDbRow.GetBool("AcceptsNumber");
                    double dnpversion = xmlDbRow.GetFloat("Version");
                    string groupname = xmlDbRow.GetString("GroupName");
                    EventTypeId eventId = isSocialPrimitive ? EventTypeId.kSocialInteraction : EventTypeId.kEventNone;
                    string triggerEvent = xmlDbRow.GetString("TriggerEvent");
                    if (!string.IsNullOrEmpty(triggerEvent))
                    {
                        eventId = ParserFunctions.TryParseEnum(triggerEvent, out EventTypeId triggerEventId, EventTypeId.kEventNone) ? triggerEventId : (EventTypeId)HashString32(triggerEvent);
                    }
                    CASAGSAvailabilityFlags availabilityFlags = CASAGSAvailabilityFlags.None;
                    string speciesAvailability = xmlDbRow.GetString("SpeciesAvailability");
                    if (!string.IsNullOrEmpty(speciesAvailability))
                    {
                        availabilityFlags = ParserFunctions.ParseAllowableAgeSpecies(speciesAvailability);
                    }
                    bool satisfiableByThirdParty = xmlDbRow.GetBool("SatisfiableByThirdParty");
                    bool validInVacationWorld = xmlDbRow.GetBool("IsValidInVacationWorld");
                    xmlDbRow.TryGetEnum("SubjectType", out DreamNodePrimitive.InputSubjectType subjectType, DreamNodePrimitive.InputSubjectType.None);
                    string primaryIcon = xmlDbRow.GetString("PrimaryIcon");
                    string secondaryIcon = xmlDbRow.GetString("SecondaryIcon");

                    string potentialStartFunction = xmlDbRow.GetString("PotentialStartFunction");
                    DreamsAndPromisesPotentialStartCheckFunctionDelegate potentialStartDelegate = null;
                    Type delegateType = typeof(DreamsAndPromisesPotentialStartCheckFunctionDelegate);
                    if (!string.IsNullOrEmpty(potentialStartFunction))
                    {
                        MethodInfo method = FindMethod(potentialStartFunction, defaultDelegateTarget);
                        potentialStartDelegate = Delegate.CreateDelegate(delegateType, method) as DreamsAndPromisesPotentialStartCheckFunctionDelegate;
                    }
                    string countTextFunction = xmlDbRow.GetString("CountTextFunction");
                    DreamsAndPromisesCountTextFunctionDelegate countTextDelegate = null;
                    delegateType = typeof(DreamsAndPromisesCountTextFunctionDelegate);
                    if (!string.IsNullOrEmpty(countTextFunction))
                    {
                        MethodInfo method = FindMethod(countTextFunction, defaultDelegateTarget);
                        countTextDelegate = Delegate.CreateDelegate(delegateType, method) as DreamsAndPromisesCountTextFunctionDelegate;
                    }
                    string feedbackFunction = xmlDbRow.GetString("FeedbackFunction");
                    DreamsAndPromisesFeedbackFunctionDelegate feedbackDelegate = null;
                    delegateType = typeof(DreamsAndPromisesFeedbackFunctionDelegate);
                    if (!string.IsNullOrEmpty(feedbackFunction))
                    {
                        MethodInfo method = FindMethod(feedbackFunction, defaultDelegateTarget);
                        feedbackDelegate = Delegate.CreateDelegate(delegateType, method) as DreamsAndPromisesFeedbackFunctionDelegate;
                    }
                    feedbackDelegate ??= Delegate.CreateDelegate(delegateType, defaultDelegateTarget, isSocialPrimitive ? "SocialFeedBackFunction" : "DefaultFeedbackFunction", false, true) as DreamsAndPromisesFeedbackFunctionDelegate;
                    string checkFunction = xmlDbRow.GetString("CheckFunction");
                    DreamsAndPromisesCheckFunctionDelegate checkDelegate = null;
                    delegateType = typeof(DreamsAndPromisesCheckFunctionDelegate);
                    if (!string.IsNullOrEmpty(checkFunction))
                    {
                        MethodInfo method = FindMethod(checkFunction, defaultDelegateTarget);
                        checkDelegate = Delegate.CreateDelegate(delegateType, method) as DreamsAndPromisesCheckFunctionDelegate;
                    }
                    checkDelegate ??= Delegate.CreateDelegate(delegateType, defaultDelegateTarget, isSocialPrimitive ? "SocialCheckFunction" : "DefaultCheckFunction", false, true) as DreamsAndPromisesCheckFunctionDelegate;

                    DreamNodePrimitive dreamNodePrimitive = new(id, eventId, subjectType, acceptsNumber, feedbackDelegate, checkDelegate,
                        potentialStartDelegate, countTextDelegate, name, category, primaryIcon, secondaryIcon, isSocialPrimitive,
                        satisfiableByThirdParty, validInVacationWorld, availabilityFlags, productVersion, dnpversion, groupname);

                    if (DreamsAndPromisesManager.sNodePrimitves.ContainsKey(id))
                    {
                        if (DreamsAndPromisesManager.sNodePrimitves[id].DnpVersion < dreamNodePrimitive.DnpVersion)
                        {
                            DreamsAndPromisesManager.sNodePrimitves[id] = dreamNodePrimitive;
                            int indexToRemove = primitivesToCache.FindIndex(primitive => primitive.Id == dreamNodePrimitive.Id);
                            if (indexToRemove >= 0)
                            {
                                primitivesToCache.RemoveAt(indexToRemove);
                            }
                            primitivesToCache.Add(dreamNodePrimitive);
                        }
                    }
                    else
                    {
                        DreamsAndPromisesManager.sNodePrimitves.Add(id, dreamNodePrimitive);
                        primitivesToCache.Add(dreamNodePrimitive);
                    }
                }
            }
        }
    }
}
