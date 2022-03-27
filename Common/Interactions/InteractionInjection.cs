namespace Gamefreak130.Common.Interactions
{
    using Sims3.Gameplay.Autonomy;
    using Sims3.Gameplay.Interactions;
    using Sims3.Gameplay.Interfaces;
    using System;

    public static partial class InteractionHelper
    {
        public static void InjectInteraction<TTarget>(ref InteractionDefinition singleton, InteractionDefinition newSingleton, bool requiresTuning) where TTarget : IGameObject
            => InjectInteraction<TTarget, InteractionDefinition>(ref singleton, newSingleton, requiresTuning);

        public static void InjectInteraction<TTarget>(ref ISoloInteractionDefinition singleton, ISoloInteractionDefinition newSingleton, bool requiresTuning) where TTarget : IGameObject
        {
            if (requiresTuning)
            {
                CopyTuning(singleton.GetType(), typeof(TTarget), newSingleton.GetType(), typeof(TTarget), true);
            }
            singleton = newSingleton;
        }

        public static void InjectInteraction<TTarget, TDefinition>(ref TDefinition singleton, TDefinition newSingleton, bool requiresTuning) where TTarget : IGameObject where TDefinition : InteractionDefinition
        {
            if (requiresTuning)
            {
                CopyTuning(singleton.GetType(), typeof(TTarget), newSingleton.GetType(), typeof(TTarget), true);
            }
            singleton = newSingleton;
        }

        public static InteractionTuning CopyTuning(Type oldType, Type oldTarget, Type newType, Type newTarget, bool clone)
        {
            InteractionTuning interactionTuning = AutonomyTuning.GetTuning(newType.FullName, newTarget.FullName);
            if (interactionTuning is null)
            {
                interactionTuning = AutonomyTuning.GetTuning(oldType, oldType.FullName, oldTarget);
                if (interactionTuning is null)
                {
                    return null;
                }
                if (clone)
                {
                    interactionTuning = CloneTuning(interactionTuning);
                }
                AutonomyTuning.AddTuning(newType.FullName, newTarget.FullName, interactionTuning);
            }
            InteractionObjectPair.sTuningCache.Remove(new(newType, newTarget));
            return interactionTuning;
        }

        private static InteractionTuning CloneTuning(InteractionTuning oldTuning) => new()
        {
            mFlags = oldTuning.mFlags,
            ActionTopic = oldTuning.ActionTopic,
            AlwaysChooseBest = oldTuning.AlwaysChooseBest,
            Availability = CloneAvailability(oldTuning.Availability),
            CodeVersion = oldTuning.CodeVersion,
            FullInteractionName = oldTuning.FullInteractionName,
            FullObjectName = oldTuning.FullObjectName,
            mChecks = oldTuning.mChecks is null ? null : new(oldTuning.mChecks),
            mTradeoff = CloneTradeoff(oldTuning.mTradeoff),
            PosturePreconditions = oldTuning.PosturePreconditions,
            ScoringFunction = oldTuning.ScoringFunction,
            ScoringFunctionOnlyAppliesToSpecificCommodity = oldTuning.ScoringFunctionOnlyAppliesToSpecificCommodity,
            ScoringFunctionString = oldTuning.ScoringFunctionString,
            ShortInteractionName = oldTuning.ShortInteractionName,
            ShortObjectName = oldTuning.ShortObjectName
        };

        private static Tradeoff CloneTradeoff(Tradeoff old) => new()
        {
            mFlags = old.mFlags,
            mInputs = old.mInputs is null ? null : new(old.mInputs),
            mName = old.mName,
            mNumParameters = old.mNumParameters,
            mOutputs = old.mOutputs is null ? null : new(old.mOutputs),
            mVariableRestrictions = old.mVariableRestrictions,
            TimeEstimate = old.TimeEstimate
        };

        private static Availability CloneAvailability(Availability old) => new()
        {
            mFlags = old.mFlags,
            AgeSpeciesAvailabilityFlags = old.AgeSpeciesAvailabilityFlags,
            CareerThresholdType = old.CareerThresholdType,
            CareerThresholdValue = old.CareerThresholdValue,
            ExcludingBuffs = old.ExcludingBuffs is null ? null : new(old.ExcludingBuffs),
            ExcludingTraits = old.ExcludingTraits is null ? null : new(old.ExcludingTraits),
            MoodThresholdType = old.MoodThresholdType,
            MoodThresholdValue = old.MoodThresholdValue,
            MotiveThresholdType = old.MotiveThresholdType,
            MotiveThresholdValue = old.MotiveThresholdValue,
            RequiredBuffs = old.RequiredBuffs is null ? null : new(old.RequiredBuffs),
            RequiredTraits = old.RequiredTraits is null ? null : new(old.RequiredTraits),
            SkillThresholdType = old.SkillThresholdType,
            SkillThresholdValue = old.SkillThresholdValue,
            WorldRestrictionType = old.WorldRestrictionType,
            OccultRestrictions = old.OccultRestrictions,
            OccultRestrictionType = old.OccultRestrictionType,
            SnowLevelValue = old.SnowLevelValue,
            WorldRestrictionWorldNames = old.WorldRestrictionWorldNames is null ? null : new(old.WorldRestrictionWorldNames),
            WorldRestrictionWorldTypes = old.WorldRestrictionWorldTypes is null ? null : new(old.WorldRestrictionWorldTypes)
        };
    }
}
