using System;

namespace InsonusK.EventInvocator
{
    public static class EventInvocator_Extension
    {
        public static void SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> ev, object sender, TEventArgs args)
        {
            EventHandler<TEventArgs> _ev = ev;
            _ev?.Invoke(sender,args);
        }

        public static void SafeInvoke(this EventHandler ev, object sender)
        {
            EventHandler _ev = ev;
            _ev?.Invoke(sender,null);
        }

        public static void SafeInvoke<TAction>(TAction _delegate,object[] parameters = null) where TAction:Delegate
        {
            TAction _ev = _delegate;
            _ev?.DynamicInvoke(parameters);
        }
    }
}