﻿using System;

namespace Weak_Subscriber_Manager.Subscribe_Using_Delegates
{
    /// <summary>
    /// Delegate of subscribe or unsubscribe to event
    /// </summary>
    /// <typeparam name="TSource">Source class type</typeparam>
    /// <typeparam name="TEventArgs">Event argument type</typeparam>
    /// <param name="eventSource">Event source</param>
    /// <param name="listenerEventHandler">Subscribe/unsubscribe method</param>
    public delegate void dlgSubscribeMethod<in TSource, TEventArgs>(
        TSource eventSource,
        EventHandler<TEventArgs> listenerEventHandler)
        where TSource : class
        where TEventArgs : EventArgs;

    /// <summary>
    /// Delegate how listener invoke event
    /// </summary>
    /// <typeparam name="TListener">Listener class type</typeparam>
    /// <typeparam name="TEventArgs">Event argument type</typeparam>
    /// <param name="eventListener">Listener</param>
    /// <param name="eventSender">event parameter sender</param>
    /// <param name="eventArgs">event parameter event args</param>
    public delegate void dlgListenerOnEvent<in TListener, in TEventArgs>(
        TListener eventListener,
        object eventSender,
        TEventArgs eventArgs)
        where TListener : class
        where TEventArgs : EventArgs;

}