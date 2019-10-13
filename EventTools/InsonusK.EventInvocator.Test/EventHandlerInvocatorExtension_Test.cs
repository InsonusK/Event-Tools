using System;
using NUnit.Framework;

namespace InsonusK.EventInvocator
{
    
    public class EventHandlerInvocatorExtension_Test
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestWithParameters(bool add)
        {
            TestClass _testClass = new TestClass();
            EventArgs _args = new EventArgs();
            bool _checked = false;
            if (add)
            {
                _testClass.evWithParameters += (sender, args) =>
                {
                    Assert.IsTrue(_args == args);
                    Assert.IsTrue(sender == _testClass);
                    _checked = true;
                };
            }

            _testClass.Invoke(_args);
            Assert.IsTrue(_checked == add);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestWithOutParameters(bool add)
        {
            TestClass _testClass = new TestClass();
            bool _checked = false;
            if (add)
            {
                _testClass.evWithOutParameters += (sender, args) =>
            {
                Assert.IsNull(args);
                Assert.IsTrue(sender == _testClass);
                _checked = true;
            };
            }

            _testClass.Invoke();
            Assert.IsTrue(_checked == add);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestDelegateWithParameters(bool add)
        {
            TestClass _testClass = new TestClass();
            EventArgs _args = new EventArgs();
            bool _checked = false;
            if (add)
            {
                _testClass.evDelegateWithParameters += (sender, args) =>
                {
                    Assert.IsTrue(_args == args);
                    Assert.IsTrue(sender == _testClass);
                    _checked = true;
                };
            }

            _testClass.Invoke(_args);
            Assert.IsTrue(_checked == add);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestDelegateWithOutParameters(bool add)
        {
            TestClass _testClass = new TestClass();
            bool _checked = false;
            if (add)
            {
                _testClass.evDelegateWithOutParameters += () =>
                {
                    _checked = true;
                };
            }

            _testClass.Invoke();
            Assert.IsTrue(_checked == add);
        }
    }
}
