using System;

namespace InsonusK.EventInvocator
{
    public static class EventInvocator_Extension
    {
        /// <summary>
        /// Safe invoke of event
        /// </summary>
        /// <param name="ev">event</param>
        /// <param name="sender">sender</param>
        /// <param name="args">event arguments</param>
        /// <typeparam name="TEventArgs">Type of event arguments</typeparam>
        public static void SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> ev, object sender, TEventArgs args)
        {
            EventHandler<TEventArgs> _ev = ev;
            _ev?.Invoke(sender,args);
        }
        /// <summary>
        /// Safe invoke of event
        /// </summary>
        /// <param name="ev">event</param>
        /// <param name="sender">sender</param>
        public static void SafeInvoke(this EventHandler ev, object sender)
        {
            EventHandler _ev = ev;
            _ev?.Invoke(sender,null);
        }

        /// <summary>
        /// Safe invoke of delegate
        /// </summary>
        /// <param name="delegate">Delegate</param>
        /// <param name="parameters">delegate parameters</param>
        public static void SafeInvoke(this Delegate @delegate, object[] parameters = null) 
        {
            Delegate _ev = @delegate;
            _ev?.DynamicInvoke(parameters);
        }
    }
}