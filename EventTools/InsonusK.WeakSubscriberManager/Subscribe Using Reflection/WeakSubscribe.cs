﻿using System;
using System.Reflection;

 namespace Weak_Subscriber_Manager.Subscribe_Using_Reflection
{
    /// <summary>
    /// Weak subscriber using reflection for work
    /// work slower than delegate version 
    /// </summary>
    /// <typeparam name="TSource">Event Source</typeparam>
    /// <typeparam name="TEventArgs">Event argument</typeparam>
    public class WeakSubscribe<TSource, TEventArgs> : IDisposable
        where TEventArgs : EventArgs
        where TSource : class
    {
        private readonly WeakReference _listenerReference;
        private readonly WeakReference _sourceReference;
        private readonly MethodInfo _listenerEventHandlerMethodInfo;
        private readonly EventInfo _sourceEventInfo;
        private readonly Delegate _ourEventHandler;
        private bool _subscribed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Event source</param>
        /// <param name="sourceEventName">Event name</param>
        /// <param name="listenerEventHandler">Lister event handler method</param>
        public WeakSubscribe(
            TSource source,
            string sourceEventName,
            EventHandler<TEventArgs> listenerEventHandler)
            : this(source, typeof(TSource).GetEvent(sourceEventName), listenerEventHandler)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Event source</param>
        /// <param name="sourceEventInfo">Event name</param>
        /// <param name="listenerEventHandler">Lister event handler method</param>
        public WeakSubscribe(
            TSource source,
            EventInfo sourceEventInfo,
            EventHandler<TEventArgs> listenerEventHandler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "missing event source");
            if (sourceEventInfo == null)
                throw new ArgumentNullException(nameof(sourceEventInfo), "missing source event info");
            if (listenerEventHandler == null)
                throw new ArgumentNullException(nameof(sourceEventInfo), "missing subscriber EventHandler");

            _sourceReference = new WeakReference(source);
            _sourceEventInfo = sourceEventInfo;

            _listenerReference = new WeakReference(listenerEventHandler.Target);
            _listenerEventHandlerMethodInfo = listenerEventHandler.GetMethodInfo();

            _ourEventHandler = CreateEventHandler();
            AddEventHandler();
        }

        protected virtual Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }

        protected void OnSourceEvent(object sender, TEventArgs e)
        {
            object _listener = _listenerReference.Target;
            if (_listener != null)
                _listenerEventHandlerMethodInfo.Invoke(_listener, new object[2]
                {
                    sender,
                    e
                });
            else
                Dispose();
        }

        private void RemoveEventHandler()
        {
            if (!_subscribed)
                return;
            object _source = _sourceReference.Target;
            if (_source == null)
                return;
            _sourceEventInfo.GetRemoveMethod(false).Invoke(_source, new object[1]
            {
                _ourEventHandler
            });
            _subscribed = false;
        }

        private void AddEventHandler()
        {
            if (_subscribed)
                throw new Exception("Should not call _subscribed twice");
            object _source = _sourceReference.Target;
            if (_source == null) return;
            _sourceEventInfo.GetAddMethod(false).Invoke(_source, new object[1]
            {
                _ourEventHandler
            });
            _subscribed = true;
        }

        #region Implementation of IUnmanagedObject

        /// <summary>
        /// Check is weak subscriber is work
        /// </summary>
        /// <returns></returns>
        public bool CheckRelevance()
        {
            return _sourceReference.IsAlive &&
                   _listenerReference.IsAlive;
        }

        #endregion

        #region Implementation of IDisposable
        private bool _disposed = false;
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                RemoveEventHandler();
                _disposed = true;
            }
        }
        ~WeakSubscribe()
        {
            Dispose(false);
        }
        #endregion
    }
}