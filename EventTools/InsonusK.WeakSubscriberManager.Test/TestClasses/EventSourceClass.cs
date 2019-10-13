﻿using System;

namespace Weak_Subscriber_Manager.Test.TestClasses
{
    public class EventSourceClass
    {
        public event EventHandler<TestEventArgs> evTestEvent;

        public void RaiseEvent(TestEventArgs args)
        {
            evTestEvent?.Invoke(this,args);
        }
    }
}