﻿using System;
 using NUnit.Framework;
 using Weak_Subscriber_Manager.Subscribe_Using_Reflection;
using Weak_Subscriber_Manager.Test.TestClasses;

namespace Weak_Subscriber_Manager.Test.Subscribe_Using_Reflection
{
    
    public class WeekSubscribe_Test
    {
        [Test]
        public void Construct_RaiseEvent()
        {
            TestEventArgs eventArgs = new TestEventArgs();
            EventSourceClass source = new EventSourceClass();
            EventListnerClass listner = new EventListnerClass(source, eventArgs);
            WeakReference<WeakSubscribe<EventSourceClass, TestEventArgs>> weakSubscriber =
                new WeakReference<WeakSubscribe<EventSourceClass, TestEventArgs>>(
                    new WeakSubscribe<EventSourceClass, TestEventArgs>(
                        source,
                        nameof(source.evTestEvent),
                        listner.EventHandlerMethod));
            GC.Collect();
            source.RaiseEvent(eventArgs);
            Assert.IsTrue(listner.EventInvoked);
            Assert.IsTrue(weakSubscriber.TryGetTarget(out WeakSubscribe<EventSourceClass, TestEventArgs> _subscribe));
            Assert.IsTrue(_subscribe.CheckRelevance());

            _subscribe.Dispose();
            _subscribe = null;
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _subscribe));
        }

        [Test]
        public void Free_Source()
        {
            TestEventArgs eventArgs = new TestEventArgs();
            EventSourceClass source = new EventSourceClass();
            EventListnerClass listner = new EventListnerClass(source, eventArgs);
            WeakReference<WeakSubscribe<EventSourceClass, TestEventArgs>> weakSubscriber =
                new WeakReference<WeakSubscribe<EventSourceClass, TestEventArgs>>(
                    new WeakSubscribe<EventSourceClass, TestEventArgs>(
                        source,
                        nameof(source.evTestEvent),
                        listner.EventHandlerMethod));

            source = null;
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out WeakSubscribe<EventSourceClass, TestEventArgs> _subscribe));
        }

        [Test]
        public void Free_Listner()
        {
            TestEventArgs eventArgs = new TestEventArgs();
            EventSourceClass source = new EventSourceClass();
            EventListnerClass listner = new EventListnerClass(source, eventArgs);
            WeakReference<WeakSubscribe<EventSourceClass, TestEventArgs>> weakSubscriber =
                new WeakReference<WeakSubscribe<EventSourceClass, TestEventArgs>>(
                    new WeakSubscribe<EventSourceClass, TestEventArgs>(
                        source,
                        nameof(source.evTestEvent),
                        listner.EventHandlerMethod));

            source.RaiseEvent(eventArgs);
            Assert.IsTrue(listner.EventInvoked);
            listner = null;
            GC.Collect();
            Assert.IsTrue(weakSubscriber.TryGetTarget(out WeakSubscribe<EventSourceClass, TestEventArgs> _subscribe));
            Assert.IsFalse(_subscribe.CheckRelevance());
        }
    }
}