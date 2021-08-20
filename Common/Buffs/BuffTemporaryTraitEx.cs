namespace Gamefreak130.Common.Buffs
{
    using Sims3.Gameplay.Actors;
    using Sims3.Gameplay.ActorSystems;
    using Sims3.Gameplay.Autonomy;
    using Sims3.Gameplay.CAS;
    using Sims3.Gameplay.Skills;
    using Sims3.Gameplay.UI;
    using Sims3.Gameplay.WorldBuilderUtil;
    using Sims3.SimIFace;
    using Sims3.SimIFace.Enums;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>An extension of the BuffTemporaryTrait class which supports the addition of more than one trait. It also allows for the addition of hidden/reward traits.</summary>
    public abstract class BuffTemporaryTraitEx : Buff
    {
        public class BuffInstanceTemporaryTraitEx : BuffInstance
        {
            private SimDescription mTargetSim;

            public List<TraitNames> TraitsAdded { get; } = new();

            public List<TraitNames> TraitsRemoved { get; } = new();

            public BuffInstanceTemporaryTraitEx()
            {
            }

            public BuffInstanceTemporaryTraitEx(Buff buff, BuffNames buffGuid, int effectValue, float timeoutCount) : base(buff, buffGuid, effectValue, timeoutCount)
            {
            }

            public override BuffInstance Clone() => new BuffInstanceTemporaryTraitEx(mBuff, mBuffGuid, mEffectValue, mTimeoutCount);

            public override void SetTargetSim(SimDescription targetSim) => mTargetSim = targetSim;

            public void AddTemporaryTrait(TraitNames trait) => AddTemporaryTrait(trait, false);

            public void AddTemporaryTrait(TraitNames trait, bool hidden)
            {
                TraitManager traitManager = mTargetSim.TraitManager;
                Trait traitToAdd = TraitManager.GetTraitFromDictionary(trait);
                if (traitToAdd is null || traitManager.HasElement(trait) || !GameUtils.IsInstalled(traitToAdd.ProductVersion) || (!hidden && TraitsAdded.Where(guid => TraitManager.GetTraitFromDictionary(guid).IsVisible).Count() == traitManager.CountVisibleTraits()))
                {
                    return;
                }
                IEnumerable<Trait> conflictingTraits = traitManager.GetDictionaryConflictingTraits(traitToAdd).Where(x => mTargetSim.HasTrait(x.Guid));
                if (conflictingTraits.Count() > 0)
                {
                    foreach (Trait conflictingTrait in conflictingTraits)
                    {
                        TraitsRemoved.Add(conflictingTrait.Guid);
                        traitManager.RemoveElement(conflictingTrait.Guid);
                    }
                }
                else if (!hidden)
                {
                    TraitNames randomVisibleElement;
                    do
                    {
                        randomVisibleElement = traitManager.GetRandomVisibleElement().Guid;
                    }
                    while (TraitsAdded.Contains(randomVisibleElement));
                    TraitsRemoved.Add(randomVisibleElement);
                    traitManager.RemoveElement(randomVisibleElement);
                }
                if (traitManager.CanAddTrait(traitToAdd, true) && traitManager.AddElement(trait))
                {
                    TraitsAdded.Add(trait);
                    if (hidden && traitToAdd.IsReward)
                    {
                        traitManager.mRewardTraits.Remove(traitToAdd);
                    }
                }
                if (hidden)
                {
                    Sims3.UI.Hud.RewardTraitsPanel.Instance?.PopulateTraits();
                }
                if (mTargetSim.CreatedSim is not null)
                {
                    (Responder.Instance.HudModel as HudModel).OnSimAgeChanged(mTargetSim.CreatedSim.ObjectId);
                }
            }
        }

        public BuffTemporaryTraitEx(BuffData info) : base(info)
        {
        }

        public override BuffInstance CreateBuffInstance() => new BuffInstanceTemporaryTraitEx(this, BuffGuid, EffectValue, TimeoutSimMinutes);

        public override void OnRemoval(BuffManager bm, BuffInstance bi)
        {
            BuffInstanceTemporaryTraitEx buffInstanceTemporaryTrait = bi as BuffInstanceTemporaryTraitEx;
            TraitManager traitManager = bm.Actor.TraitManager;
            foreach (TraitNames guid in buffInstanceTemporaryTrait.TraitsAdded)
            {
                RemoveElement(traitManager, guid);
            }
            foreach (TraitNames guid in buffInstanceTemporaryTrait.TraitsRemoved)
            {
                traitManager.AddElement(guid);
            }
            buffInstanceTemporaryTrait.TraitsAdded.Clear();
            buffInstanceTemporaryTrait.TraitsRemoved.Clear();
            (Responder.Instance.HudModel as HudModel).OnSimAgeChanged(bm.Actor.ObjectId);
        }

        private static void RemoveElement(TraitManager traitManager, TraitNames guid)
        {
            Trait traitFromDictionary = TraitManager.GetTraitFromDictionary(guid);
            if (traitFromDictionary.TraitListener is not null)
            {
                traitFromDictionary.TraitListener.Remove();
            }
            if (traitFromDictionary != null && traitFromDictionary.IsReward)
            {
                traitManager.mRewardTraits.Remove(traitFromDictionary);
            }
            Sim actor = traitManager.Actor;
            if (actor is not null)
            {
                actor.UpdateSacsParametersForTraitOrBuff(typeof(TraitNames), (ulong)guid, YesOrNo.no);
                actor.SocialComponent.UpdateTraits();
                if (traitFromDictionary.Guid is TraitNames.FutureSim && !actor.BuffManager.HasElement(BuffNames.EmbracingTheFuture))
                {
                    actor.SkillManager.SubtractFromSkillGainModifier(SkillNames.Future, BuffEmbracingTheFuture.kEmbracingFutureSkillMultiplier - 1f);
                }
                if (traitFromDictionary.Guid is TraitNames.FutureSimLHR)
                {
                    actor.TraitManager.RemoveElement(TraitNames.FutureSim);
                }
            }
            if (traitManager.mValues.TryGetValue((ulong)guid, out Trait trait))
            {
                traitManager.mValues.Remove((ulong)guid);
                traitManager.OnRemoved(trait);
            }
            if (!CharacterImportOnGameLoad.InProgress)
            {
                traitManager.AddDesireAlarm();
                MetaAutonomyManager.UpdatePreferredVenuesForSim(actor);
            }
        }
    }
}
