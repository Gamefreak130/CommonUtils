namespace Gamefreak130.Common.Interactions
{
    using Sims3.Gameplay.Abstracts;
    using Sims3.Gameplay.Actors;
    using Sims3.Gameplay.Autonomy;
    using Sims3.Gameplay.Interactions;
    using Sims3.Gameplay.Socializing;
    using System.Collections.Generic;

    public static partial class InteractionHelper
    {
        public static void AddInteraction(GameObject gameObject, InteractionDefinition singleton)
        {
            if (gameObject.Interactions.TrueForAll(iop => iop.InteractionDefinition.GetType() != singleton.GetType()) 
                && (gameObject.ItemComp?.InteractionsInventory is not List<InteractionObjectPair> inventoryIops || inventoryIops.TrueForAll(iop => iop.InteractionDefinition.GetType() != singleton.GetType())))
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
