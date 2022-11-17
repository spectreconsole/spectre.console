using Spectre.Console.Extensions;

namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/Align")]
public sealed class AlignTests
{
    [UsesVerify]
    public sealed class Left
    {
        [Fact]
        [Expectation("Left_Top")]
        public Task Should_Render_Panel_Left_Aligned_At_Top()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Left(new Panel("Hello World!"), VerticalAlignment.Top).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Left_Middle")]
        public Task Should_Render_Panel_Left_Aligned_At_Middle()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Left(new Panel("Hello World!"), VerticalAlignment.Middle).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Left_Bottom")]
        public Task Should_Render_Panel_Left_Aligned_At_Bottom()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Left(new Panel("Hello World!"), VerticalAlignment.Bottom).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    [UsesVerify]
    public sealed class Center
    {
        [Fact]
        [Expectation("Center_Top")]
        public Task Should_Render_Panel_Center_Aligned_At_Top()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Center(new Panel("Hello World!"), VerticalAlignment.Top).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Center_Middle")]
        public Task Should_Render_Panel_Center_Aligned_At_Middle()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Center(new Panel("Hello World!"), VerticalAlignment.Middle).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Center_Bottom")]
        public Task Should_Render_Panel_Center_Aligned_At_Bottom()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Center(new Panel("Hello World!"), VerticalAlignment.Bottom).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    [UsesVerify]
    public sealed class Right
    {
        [Fact]
        [Expectation("Right_Top")]
        public Task Should_Render_Panel_Right_Aligned_At_Top()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Right(new Panel("Hello World!"), VerticalAlignment.Top).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Right_Middle")]
        public Task Should_Render_Panel_Right_Aligned_At_Middle()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Right(new Panel("Hello World!"), VerticalAlignment.Middle).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Right_Bottom")]
        public Task Should_Render_Panel_Right_Aligned_At_Bottom()
        {
            // Given
            var console = new TestConsole().Size(new Size(40, 15));
            var align = Align.Right(new Panel("Hello World!"), VerticalAlignment.Bottom).Height(15);

            // When
            console.Write(align);

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
