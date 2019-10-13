﻿using System;
 using NUnit.Framework;
 using Weak_Subscriber_Manager.Subscribe_Using_Delegates;
using Weak_Subscriber_Manager.Test.TestClasses;

namespace Weak_Subscriber_Manager.Test.Subscribe_Using_Delegates
{
    
    public class WeekSubscribe_Test
    {
        [Test]
        public void Construct_RaiseEvent()
        {
            TestEventArgs eventArgs = new TestEventArgs();
            EventSourceClass source = new EventSourceClass();
            EventListnerClass listner = new EventListnerClass(source, eventArgs);
            WeakReference<WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>> weakSubscriber =
                new WeakReference<WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>>(
                    new WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>(
                        source, listner, 
                        SubscribeMethod, UnSubscribeMethod,
                        OnEventMethod)
                    );
            GC.Collect();
            source.RaiseEvent(eventArgs);
            Assert.IsTrue(listner.EventInvoked);
            Assert.IsTrue(weakSubscriber.TryGetTarget(out WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs> _subscribe));
            Assert.IsTrue(_subscribe.CheckRelevance());

            _subscribe.Dispose();
            _subscribe = null;
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out _subscribe));
        }

        private static void OnEventMethod(EventListnerClass eventlistner, object eventsender, TestEventArgs eventargs)
        {
            eventlistner.EventHandlerMethod(eventsender,eventargs);
        }

        private static void UnSubscribeMethod(EventSourceClass eventsource, EventHandler<TestEventArgs> listnereventhandler)
        {
            eventsource.evTestEvent -= listnereventhandler;
        }

        private static void SubscribeMethod(EventSourceClass eventsource, EventHandler<TestEventArgs> listnereventhandler)
        {
            eventsource.evTestEvent += listnereventhandler;
        }

        [Test]
        public void Free_Source()
        {
            TestEventArgs eventArgs = new TestEventArgs();
            EventSourceClass source = new EventSourceClass();
            EventListnerClass listner = new EventListnerClass(source, eventArgs);
            WeakReference<WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>> weakSubscriber =
                new WeakReference<WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>>(
                    new WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>(
                        source, listner,
                        SubscribeMethod, UnSubscribeMethod,
                        OnEventMethod)
                );

            source = null;
            GC.Collect();
            Assert.IsFalse(weakSubscriber.TryGetTarget(out WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs> _subscribe));
        }

        [Test]
        public void Free_Listner()
        {
            TestEventArgs eventArgs = new TestEventArgs();
            EventSourceClass source = new EventSourceClass();
            EventListnerClass listner = new EventListnerClass(source, eventArgs);
            WeakReference<WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>> weakSubscriber =
                new WeakReference<WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>>(
                    new WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs>(
                        source, listner,
                        SubscribeMethod, UnSubscribeMethod,
                        OnEventMethod)
                );

            source.RaiseEvent(eventArgs);
            Assert.IsTrue(listner.EventInvoked);
            listner = null;

            source.RaiseEvent(eventArgs);
            GC.WaitForFullGCComplete();
            Assert.IsTrue(weakSubscriber.TryGetTarget(out WeakSubscribe<EventSourceClass, EventListnerClass, TestEventArgs> _subscribe));
            Assert.IsFalse(_subscribe.CheckRelevance());
        }
    }
}