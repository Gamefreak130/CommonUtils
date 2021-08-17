namespace Gamefreak130.Common.Interactions
{
    using Sims3.Gameplay.Abstracts;
    using Sims3.Gameplay.Actors;
    using Sims3.Gameplay.Autonomy;
    using Sims3.Gameplay.Interactions;
    using Sims3.Gameplay.Interfaces;
    using Sims3.Gameplay.Socializing;
    using System.Collections.Generic;
    using System.Linq;

    public static class InteractionHelper
    {
        public static void InjectInteraction<TTarget>(ref InteractionDefinition singleton, InteractionDefinition newSingleton, bool requiresTuning) where TTarget : IGameObject
            => InjectInteraction<TTarget, InteractionDefinition>(ref singleton, newSingleton, requiresTuning);

        public static void InjectInteraction<TTarget>(ref ISoloInteractionDefinition singleton, ISoloInteractionDefinition newSingleton, bool requiresTuning) where TTarget : IGameObject
        {
            if (requiresTuning)
            {
                Tunings.Inject(singleton.GetType(), typeof(TTarget), newSingleton.GetType(), typeof(TTarget), true);
            }
            singleton = newSingleton;
        }

        public static void InjectInteraction<TTarget, TDefinition>(ref TDefinition singleton, TDefinition newSingleton, bool requiresTuning) where TTarget : IGameObject where TDefinition : InteractionDefinition
        {
            if (requiresTuning)
            {
                Tunings.Inject(singleton.GetType(), typeof(TTarget), newSingleton.GetType(), typeof(TTarget), true);
            }
            singleton = newSingleton;
        }

        public static void AddInteraction(GameObject gameObject, InteractionDefinition singleton)
        {
            IEnumerable<InteractionObjectPair> iops = gameObject.Interactions;
            if (gameObject.ItemComp?.InteractionsInventory is IEnumerable<InteractionObjectPair> inventoryIops)
            {
                iops = iops.Concat(inventoryIops);
            }
            if (!iops.Any(iop => iop.InteractionDefinition.GetType() == singleton.GetType()))
            {
                gameObject.AddInteraction(singleton);
                gameObject.AddInventoryInteraction(singleton);
            }
        }

        public static void ForceSocialInteraction(Sim actor, Sim target, string socialName, InteractionPriorityLevel priority, bool isCancellable)
        {
            SocialInteractionA.Definition definition = target.Interactions.Find(iop => iop.InteractionDefinition is SocialInteractionA.Definition social && social.ActionKey == socialName)?.InteractionDefinition as SocialInteractionA.Definition
                ?? new(socialName, new string[0], null, false);
            InteractionInstance instance = definition.CreateInstance(target, actor, new(priority), false, isCancellable);
            actor.InteractionQueue.Add(instance);
        }
    }
}
