﻿using System;
using System.Reflection;

namespace Weak_Subscriber_Manager.Subscribe_Using_Delegates
{
    /// <summary>
    /// Weak subscriber using delegates for work
    /// It is more faster than reflection weak subscriber
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TListener"></typeparam>
    /// <typeparam name="TEventArgs"></typeparam>
    public class WeakSubscribe<TSource, TListener, TEventArgs> : IDisposable
        where TEventArgs : EventArgs
        where TSource : class
        where TListener : class
    {
        private readonly WeakReference<TListener> _listenerReference;
        private readonly WeakReference<TSource> _sourceReference;
        private readonly dlgSubscribeMethod<TSource, TEventArgs> _subscribeMethod;
        private readonly dlgSubscribeMethod<TSource, TEventArgs> _unSubscribeMethod;
        private readonly dlgListenerOnEvent<TListener,TEventArgs> _onEventMethod;
        private bool _subscribed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Event source</param>
        /// <param name="subscribeMethod">Static delegate subscribe to event</param>
        /// <param name="unSubscribeMethod">Static delegate unsubscribe to event</param>
        /// <param name="listener">Listener</param>
        /// <param name="onEventMethod">Static delegate how listener work with event</param>
        public WeakSubscribe(
            TSource source,
            TListener listener,
            dlgSubscribeMethod<TSource, TEventArgs> subscribeMethod,
            dlgSubscribeMethod<TSource, TEventArgs> unSubscribeMethod,
            dlgListenerOnEvent<TListener, TEventArgs> onEventMethod)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "missing event source");
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "missing event listener");
            CheckStatical(subscribeMethod, nameof(subscribeMethod));
            CheckStatical(unSubscribeMethod, nameof(unSubscribeMethod));
            CheckStatical(onEventMethod, nameof(onEventMethod));

            _sourceReference = new WeakReference<TSource>(source);
            _listenerReference = new WeakReference<TListener>(listener);
            _subscribeMethod = subscribeMethod;
            _unSubscribeMethod = unSubscribeMethod;
            _onEventMethod = onEventMethod;

            SubscribeMethod();
        }

        private static void CheckStatical(Delegate method, string methodName)
        {
            if (!method.GetMethodInfo().IsStatic)
            {
                throw new ArgumentException($"{methodName} must be static");
            }
        }

        private void OnSourceEvent(object sender, TEventArgs e)
        {
            if (_listenerReference.TryGetTarget(out TListener _listener))
                _onEventMethod.Invoke(_listener, sender, e);
            else
                Dispose();
        }

        private void SubscribeMethod()
        {
            if (_subscribed)
                throw new Exception("Should not call _subscribed twice");

            if (_sourceReference.TryGetTarget(out TSource _source))
                _subscribeMethod.Invoke(_source, OnSourceEvent);
            else
                Dispose();

            _subscribed = true;
        }

        private void UnsubscribeMethod()
        {
            if (!_subscribed) return;
            if (_sourceReference.TryGetTarget(out TSource _source))
                _unSubscribeMethod.Invoke(_source, OnSourceEvent);
            _subscribed = false;
        }

        /// <summary>
        /// Check is weak subscriber is work
        /// </summary>
        /// <returns></returns>
        public bool CheckRelevance()
        {
            return _sourceReference.TryGetTarget(out TSource _source) &&
                   _listenerReference.TryGetTarget(out TListener _listener);
        }


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
                UnsubscribeMethod();
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