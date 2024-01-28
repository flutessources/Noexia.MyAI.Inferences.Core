namespace Noexia.MyAI.Inferences.Core.Test
{
    [TestFixture]
    public class InferenceSessionBaseTests
    {
        [Test]
        public void StartInference_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var inferenceSession = new TestInferenceSession();

            // Act
            bool result = inferenceSession.StartInference().Result;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SendInput_WithValidInput_ReturnsUser()
        {
            // Arrange
            var inferenceSession = new TestInferenceSession();
            var input = new TestInferenceInput();

            // Act
            var user = inferenceSession.SendInputTest(input);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(input, user.input);
        }

        [Test]
        public void SendInput_WithNullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var inferenceSession = new TestInferenceSession();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => inferenceSession.SendInputTest(null));
        }
    }

    public class TestInferenceSession : InferenceSessionBase<TestInferenceInput, TestInferenceOutput, TestInferenceUser>
    {
        public override Task<bool> StartInference()
        {
            // Implement logic for starting inference
            return Task.FromResult(true);
        }

        public TestInferenceUser SendInputTest(TestInferenceInput input, Action<TestInferenceUser> beforeAddToqueue = null)
        {
            return base.SendInput(input, beforeAddToqueue);
        }

        protected override void InferenceInputProcess(TestInferenceUser user)
        {
            // Implement logic for inference input process
        }
    }

    public class TestInferenceInput : IInferenceInput
    {
        // Implement properties or fields as needed
    }

    public class TestInferenceOutput : IInferenceOutput
    {
        // Implement properties or fields as needed
    }

    public class TestInferenceUser : InferenceUserBase<TestInferenceInput, TestInferenceOutput>
    {
        // Implement additional members or customization if needed
    }
}