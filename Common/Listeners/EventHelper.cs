namespace Gamefreak130.Common.Listeners
{
    using Sims3.Gameplay.Abstracts;
    using Sims3.Gameplay.Actors;
    using Sims3.Gameplay.CAS;
    using Sims3.Gameplay.EventSystem;

    public static class EventHelper
    {
        public static EventListener AddDelegateListener(EventTypeId id, ProcessEventDelegate @delegate, ListenerAction exceptionAction = ListenerAction.Remove)
            => AddDelegateListenerInternal(id, @delegate, null, null, exceptionAction);

        public static EventListener AddDelegateListener(EventTypeId id, ProcessEventDelegate @delegate, GameObject target, ListenerAction exceptionAction = ListenerAction.Remove)
            => AddDelegateListenerInternal(id, @delegate, null, target, exceptionAction);

        public static EventListener AddDelegateListener(EventTypeId id, ProcessEventDelegate @delegate, Sim actor, ListenerAction exceptionAction = ListenerAction.Remove)
            => AddDelegateListenerInternal(id, @delegate, actor, null, exceptionAction);

        public static EventListener AddDelegateListener(EventTypeId id, ProcessEventDelegate @delegate, Household actorHousehold, ListenerAction exceptionAction = ListenerAction.Remove)
            => AddDelegateListenerInternal(id, @delegate, actorHousehold, null, exceptionAction);

        public static EventListener AddDelegateListener(EventTypeId id, ProcessEventDelegate @delegate, Sim actor, GameObject target, ListenerAction exceptionAction = ListenerAction.Remove)
            => AddDelegateListenerInternal(id, @delegate, actor, target, exceptionAction);

        public static EventListener AddDelegateListener(EventTypeId id, ProcessEventDelegate @delegate, Household actorHousehold, GameObject target, ListenerAction exceptionAction = ListenerAction.Remove)
            => AddDelegateListenerInternal(id, @delegate, actorHousehold, target, exceptionAction);

        private static EventListener AddDelegateListenerInternal(EventTypeId id, ProcessEventDelegate @delegate, ScriptObject s, GameObject target, ListenerAction exceptionAction) 
            => EventTracker.Instance is not null
            ? EventTracker.AddListener(new DelegateListener(id, SafeProcessEventDelegate.Create(@delegate, exceptionAction), s, target))
            : null;
    }
}
