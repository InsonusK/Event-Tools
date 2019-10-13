﻿using System;
 using NUnit.Framework;

 namespace Weak_Subscriber_Manager.Test.TestClasses
{
    public class EventListnerClass
    {
        private WeakReference<EventSourceClass> _eventSourceClass;
        private WeakReference<TestEventArgs> _eventArgs;
        public bool EventInvoked { get; private set; } = false;
        public EventListnerClass(EventSourceClass sourceAssertClass, TestEventArgs eventArgs)
        {
            _eventSourceClass = new WeakReference<EventSourceClass>(sourceAssertClass);
            _eventArgs = new WeakReference<TestEventArgs>(eventArgs);
        }

        public void DropCheck()
        {
            EventInvoked = false;
        }

        public void EventHandlerMethod(object sender, TestEventArgs eventArgs)
        {
            Assert.IsTrue(_eventSourceClass.TryGetTarget(out EventSourceClass sourceClass));
            Assert.IsTrue(sourceClass == sender);
            Assert.IsTrue(_eventArgs.TryGetTarget(out var args));
            Assert.IsTrue(eventArgs == args);
            EventInvoked = true;
        }

    }
}