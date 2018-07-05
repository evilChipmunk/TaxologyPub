using System;
using System.Collections.Generic;

namespace Shared.Domain
{
    public static class DomainEvents
    {
        [ThreadStatic]
        private static List<Delegate> _actions;

        private static readonly List<IHandle<IDomainEvent>> Handlers;
         

        static DomainEvents()
        { 
            Handlers = new List<IHandle<IDomainEvent>>();
        }

        public static void SetHandlers<T>(IEnumerable<IHandle<IDomainEvent>> handlers) where T : IDomainEvent
        {
            Handlers.AddRange(handlers);
        }
         
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
            {
                _actions = new List<Delegate>();
            }
            _actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            _actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            foreach (var handler in Handlers)
            {
                handler.Handle(args);
            }


            //foreach (var handler in Container.GetAllInstances<IHandle<T>>())
            //{
            //    handler.Handle(args);
            //}

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }



    public static class DomainToAppEvents
    {
        [ThreadStatic]
        private static List<Delegate> _actions;

        private static readonly List<IHandle<IDomainEvent>> Handlers;


        static DomainToAppEvents()
        {
            Handlers = new List<IHandle<IDomainEvent>>();
        }

        public static void SetHandlers<T>(IEnumerable<IHandle<IDomainEvent>> handlers) where T : IDomainEvent
        {
            Handlers.AddRange(handlers);
        }

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
            {
                _actions = new List<Delegate>();
            }
            _actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            _actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            foreach (var handler in Handlers)
            {
                handler.Handle(args);
            }


            //foreach (var handler in Container.GetAllInstances<IHandle<T>>())
            //{
            //    handler.Handle(args);
            //}

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }
}