using Calculator.Controllers;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Calculator.Tests.Controllers
{
    public class CalculatorControllerTests
    {
        CalculatorController _controller;
        
        [SetUp]
        public void Setup()
        {
            _controller = new CalculatorController
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Session = new MockHttpSession()
                    }
                }
            };
        }


        [Test]
        public void PressingNumberDisplaysNumber()
        {
            var data = new PressBody() { Number = "4", Operation = OperationKind.Number };
            var expected = "4";
            var actual = _controller.PostPress(data).Value.Display;

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PressingOperationDisplaysOperation()
        { 
            var data = new PressBody() { Operation = OperationKind.Multiply};
            var expected = "Multiply";
            var actual = _controller.PostPress(data).Value.Display;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Multiplying4Times2Equals8()
        {
            var operation = new PressBody() { Operation = OperationKind.Multiply };
            var num1 = new PressBody() { Number = "4", Operation = OperationKind.Number };
            var num2 = new PressBody() { Number = "2", Operation = OperationKind.Number };
            var equals = new PressBody() { Operation = OperationKind.Equals };
            var expected = "8";

            _controller.PostPress(num1);
            _controller.PostPress(operation);
            _controller.PostPress(num2);
            var actual = _controller.PostPress(equals).Value.Display;

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DisplayNegativeNumber()
        {
            var operation = new PressBody() { Operation = OperationKind.Subtract };
            var num1 = new PressBody() { Number = "4", Operation = OperationKind.Number };

            var expected = "-4";

            _controller.PostPress(operation);
            var actual = _controller.PostPress(num1).Value.Display;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DisplayNegativeNumberMinusNegativeNumber()
        {
            var operation = new PressBody() { Operation = OperationKind.Subtract };
            var num1 = new PressBody() { Number = "4", Operation = OperationKind.Number };
            var num2 = new PressBody() { Number = "5", Operation = OperationKind.Number };
            var equals = new PressBody() { Operation = OperationKind.Equals };

            var expected = "1";

            _controller.PostPress(operation);
            _controller.PostPress(num1);
            _controller.PostPress(operation);
            _controller.PostPress(operation);
            _controller.PostPress(num2);

            var actual = _controller.PostPress(equals).Value.Display;

            Assert.AreEqual(expected, actual);
        }
    }
}
