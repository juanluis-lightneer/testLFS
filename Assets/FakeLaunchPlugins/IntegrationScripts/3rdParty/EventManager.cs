#if TEST_PROJECT
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
Simple EventManager class to implement one-to-many event passing. Features:
* Simple enum and payload events.
* Delayed event sending (send later in the frame in one place).
* Detects adding duplicates/removing non-existent listeners in Unity editor mode.

Usage for simple events:
Add USE_SIMPLEEVENTS in Unity Player Settings and define enum SimpleEvent { } somewhere other than this file (Events.cs?)
This feature is implemented like this so that EventManager.cs can be kept clean out of game specific events 
and simply copy pasted from project to project while still making sure that the project compiles after 
bringing the EventManager.cs in.

Add listener:     EventManager.Connect(SimpleEvent.StartGame, OnStartGame);
Remove listener:  EventManager.Disconnect(SimpleEvent.StartGame, OnStartGame);
Send event:       EventManager.Send(SimpleEvent.StartGame);
                  EventManager.SendDelayed(SimpleEvent.StartGame);

Usage for payload events:
    
Put the payload somewhere in your project (Events.cs?)
    public class ExampleEvent : Event
    {
        public readonly string message;
        public ExampleEvent(string message) { this.message = message; }        
    }

Add listener:     EventMananger.Connect<ExampleEvent>(OnExampleEvent);
Remove listener:  EventMananger.Disconnect<ExampleEvent>(OnExampleEvent);
Send event:       EventManager.Send(new ExampleEvent("Hola amigos!"));    
                  EventManager.SendDelayed(new ExampleEvent("Hola amigos!"));    

For delayed events, you need to call EventManager.FlushDelayedEvents() from some Update or other 
convenient place where events can be delivered.

Created by Sampsa Lehtonen in 2016/2017 - Twitter @snlehton - sampsa.lehtonen[AT]iki.fi
*/
namespace GameEventSystem
{
    public enum SimpleEvent
    { }

    public static class EventManager
    {
    public delegate void OnSimpleEvent();

    public delegate void OnSimpleEventAll(SimpleEvent evt);

    private static OnSimpleEventAll onSimpleEventAll;
    
    // Note: This won't compile by default - Please add SimpleEvent enum somewhere in your project
    private static Dictionary<SimpleEvent, OnSimpleEvent> simpleEventMap = new Dictionary<SimpleEvent, OnSimpleEvent>();
    private static List<SimpleEvent> delayedSimpleEvents = new List<SimpleEvent>();

    public static void Connect(SimpleEvent simpleEvent, OnSimpleEvent handler)
    {
        OnSimpleEvent del;

        if (simpleEventMap.TryGetValue(simpleEvent, out del))
        {
#if UNITY_EDITOR
            
            var list = del.GetInvocationList();
            foreach (var existingDel in list)
            {
                if (existingDel == (Delegate)handler)
                {
                    Debug.LogError("Delegate " + del + " for event " + simpleEvent + " already connected to EventManager!");
                }
            }
            
#endif
            del += handler;
            simpleEventMap[simpleEvent] = del;
        }
        else
        {
            simpleEventMap.Add(simpleEvent, handler);
        }
    }

    public static void Disconnect(SimpleEvent simpleEvent, OnSimpleEvent handler)
    {
        OnSimpleEvent del;

        if (simpleEventMap.TryGetValue(simpleEvent, out del))
        {
#if UNITY_EDITOR
            bool found = false;

            if (del != null)
            {
                var list = del.GetInvocationList();
                foreach (var existingDel in list)
                {
                    if (existingDel == (Delegate)handler)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                Debug.LogError("Trying to remove delegate " + del + " for event " + simpleEvent +
                              " which was not connected to EventManager!");
            }
#endif

            if (del != null)
            {
                del -= handler;
                simpleEventMap[simpleEvent] = del;
            }

            if (del != null)
                simpleEventMap[simpleEvent] = del;
            else
                simpleEventMap.Remove(simpleEvent);
        }
        else
        {
            Debug.LogError("trying to disconnect from nonexistent simple event " + simpleEvent);
        }
    }

    public static void Send(SimpleEvent simpleEvent)
    {
        OnSimpleEvent del;

        if (simpleEventMap.TryGetValue(simpleEvent, out del))
        {
            del();
        }

        if (onSimpleEventAll != null)
            onSimpleEventAll(simpleEvent);
    }

    public static void SendDelayed(SimpleEvent simpleEvent)
    {
        delayedSimpleEvents.Add(simpleEvent);
    }

    public static void ConnectAll(OnSimpleEventAll onSimpleEvent)
    {
        onSimpleEventAll += onSimpleEvent;

    }

    public static void DisconnectAll(OnSimpleEventAll onSimpleEvent)
    {
        onSimpleEventAll -= onSimpleEvent;
    }


        // PAYLOAD EVENT INTERFACE

        public delegate void OnEvent<T>(T data) where T : Event;

        public interface Event
        {

        }



        private class EventTypeHandler<T> where T : Event
        {
            public static OnEvent<T> handler;
            public static List<T> delayedEvents;

            private static DelayedEventProcessor delayedEventProcessor;

            public static DelayedEventProcessor AddDelayedEventProcessor(T data)
            {
                if (delayedEvents == null)
                {
                    delayedEvents = new List<T>();
                }

                delayedEvents.Add(data);

                if (delayedEventProcessor == null)
                {
                    delayedEventProcessor = delegate
                    {
                        if (handler != null)
                        {
                            for (int i = 0; i < delayedEvents.Count; i++)
                            {

                                handler(delayedEvents[i]);
                            }
                        }
                        delayedEvents.Clear();
                    };
                }

                return delayedEventProcessor;
            }
        }


        public delegate void DelayedEventProcessor();

        private static readonly HashSet<DelayedEventProcessor> delayedEventProcessors = new HashSet<DelayedEventProcessor>();


        public static void Connect<T>(OnEvent<T> handler) where T : Event
        {
            if (EventTypeHandler<T>.handler != null)
            {
                var list = EventTypeHandler<T>.handler.GetInvocationList();
                foreach (var existingDel in list)
                {
                    if (existingDel == (Delegate)handler)
                    {
                        Debug.LogWarning("Delegate " + handler + " for event " + typeof(T) +
                                         " already connected to EventManager!");
                    }
                }
            }

            EventTypeHandler<T>.handler += handler;
        }

        public static void Disconnect<T>(OnEvent<T> handler) where T : Event
        {
            bool found = false;

            if (EventTypeHandler<T>.handler != null)
            {
                var list = EventTypeHandler<T>.handler.GetInvocationList();
                foreach (var existingDel in list)
                {
                    if (existingDel == (Delegate)handler)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                Debug.LogWarning("Trying to remove delegate " + handler + " for event " + typeof(T) +
                              " which was not connected to EventManager!");
            }

            if (EventTypeHandler<T>.handler != null)
            {
                EventTypeHandler<T>.handler -= handler;
            }
        }

        public static void Send<T>(T data) where T : Event
        {
            OnEvent<T> handler = EventTypeHandler<T>.handler;
            if (handler != null)
            {
                handler(data);
            }
        }

        public static void SendDelayed<T>(T data) where T : Event
        {
            var delayedEventProcessor = EventTypeHandler<T>.AddDelayedEventProcessor(data);
            delayedEventProcessors.Add(delayedEventProcessor);
        }

        public static void FlushDelayedEvents()
        {
#if USE_SIMPLEEVENTS
        // send simple events
        for (int i = 0; i < delayedSimpleEvents.Count; i++)
            Send(delayedSimpleEvents[i]);
        delayedSimpleEvents.Clear();
#endif

            // send payload events
            foreach (var delayedEventProcessor in delayedEventProcessors)
            {
                delayedEventProcessor();
            }
            delayedEventProcessors.Clear();
        }
    }
}
#endif
