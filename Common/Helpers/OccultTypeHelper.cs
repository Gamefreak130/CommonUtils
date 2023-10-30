namespace Gamefreak130.Common.Helpers
{
    using Sims3.Gameplay.ActorSystems;
    using Sims3.SimIFace.Enums;
    using Sims3.UI.CAS;
    using Sims3.UI.Hud;

    public static class OccultTypeHelper
    {
        public static OccultTypesAll ToOccultTypesAll(this OccultTypes occultTypes)
            => occultTypes switch
            { 
                OccultTypes.Mummy            => OccultTypesAll.Mummy,
                OccultTypes.Frankenstein     => OccultTypesAll.Frankenstein,
                OccultTypes.Vampire          => OccultTypesAll.Vampire,
                OccultTypes.ImaginaryFriend  => OccultTypesAll.ImaginaryFriend,
                OccultTypes.Unicorn          => OccultTypesAll.Unicorn,
                OccultTypes.Genie            => OccultTypesAll.Genie,
                OccultTypes.Werewolf         => OccultTypesAll.Werewolf,
                OccultTypes.Ghost            => OccultTypesAll.Ghost,
                OccultTypes.Fairy            => OccultTypesAll.Fairy,
                OccultTypes.Witch            => OccultTypesAll.Witch,
                OccultTypes.PlantSim         => OccultTypesAll.PlantSim,
                OccultTypes.Mermaid          => OccultTypesAll.Mermaid,
                OccultTypes.Robot            => OccultTypesAll.Robot,
                OccultTypes.TimeTraveler     => OccultTypesAll.TimeTraveler,
                _                            => OccultTypesAll.Human
            };

        public static OccultTypes ToOccultTypes(this OccultTypesAll occultTypesAll)
            => occultTypesAll switch
            {
                OccultTypesAll.Ghost            => OccultTypes.Ghost,
                OccultTypesAll.ImaginaryFriend  => OccultTypes.ImaginaryFriend,
                OccultTypesAll.ImaginaryDoll    => OccultTypes.ImaginaryFriend,
                OccultTypesAll.Vampire          => OccultTypes.Vampire,
                OccultTypesAll.Mummy            => OccultTypes.Mummy,
                OccultTypesAll.Frankenstein     => OccultTypes.Frankenstein,
                OccultTypesAll.Unicorn          => OccultTypes.Unicorn,
                OccultTypesAll.Genie            => OccultTypes.Genie,
                OccultTypesAll.Werewolf         => OccultTypes.Werewolf,
                OccultTypesAll.Fairy            => OccultTypes.Fairy,
                OccultTypesAll.Witch            => OccultTypes.Witch,
                OccultTypesAll.PlantSim         => OccultTypes.PlantSim,
                OccultTypesAll.Mermaid          => OccultTypes.Mermaid,
                OccultTypesAll.Robot            => OccultTypes.Robot,
                OccultTypesAll.TimeTraveler     => OccultTypes.TimeTraveler,
                _                               => OccultTypes.None
            };

        public static OccultTypes OccultFromMiniSimDescription(IMiniSimDescription miniSimDescription) 
            => miniSimDescription switch
            {
                { IsDead: true } or { IsPlayableGhost: true }  => OccultTypes.Ghost,
                { IsMummy: true }                              => OccultTypes.Mummy,
                { IsFrankenstein: true }                       => OccultTypes.Frankenstein,
                { IsVampire: true }                            => OccultTypes.Vampire,
                { IsUnicorn: true }                            => OccultTypes.Unicorn,
                { IsGenie: true }                              => OccultTypes.Genie,
                { IsWerewolf: true }                           => OccultTypes.Werewolf,
                { IsFairy: true }                              => OccultTypes.Fairy,
                { IsWitch: true }                              => OccultTypes.Witch,
                { IsMermaid: true }                            => OccultTypes.Mermaid,
                { IsEP11Bot: true }                            => OccultTypes.Robot,
                _                                              => OccultTypes.None
            };

        public static string GetLocalizedName(OccultTypes occultTypes) => GetLocalizedName(occultTypes.ToOccultTypesAll());

        public static string GetLocalizedName(OccultTypesAll occultTypes)
        {
            if (occultTypes is OccultTypesAll.Human or OccultTypesAll.All)
            {
                return CASBasics.LocalizeEP07String("Human");
            }
            if (occultTypes is OccultTypesAll.ImaginaryDoll)
            {
                occultTypes = OccultTypesAll.ImaginaryFriend;
            }
            return OccultManager.GetSingularOccultName(occultTypes);
        }
    }
}
