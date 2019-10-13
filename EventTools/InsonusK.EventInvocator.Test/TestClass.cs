using System;

namespace InsonusK.EventInvocator
{
    public delegate void dlgEventHandler(object sender,EventArgs e);
    public delegate void dlgEventHandlerNoParameters();

    public class TestClass
    {
        public event EventHandler<EventArgs> evWithParameters;
        public event EventHandler evWithOutParameters;

        public event dlgEventHandler evDelegateWithParameters;
        public event dlgEventHandlerNoParameters evDelegateWithOutParameters;

        public void Invoke(EventArgs args = null)
        {
            if (args == null)
            {
                evWithOutParameters.SafeInvoke(this);
                EventInvocator_Extension.SafeInvoke(evDelegateWithOutParameters);
            }
            else
            {
                evWithParameters.SafeInvoke(this,args);
                EventInvocator_Extension.SafeInvoke(evDelegateWithParameters, new object[]{this, args});
            }
        }
    }
}