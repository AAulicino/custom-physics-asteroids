using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameTests.Physics.Collisions
{
    public class ColliderCollisionDetectorTests
    {
        public ColliderCollisionDetector Model { get; private set; }

        [SetUp]
        public void Setup ()
        {
            Model = new ColliderCollisionDetector();
        }

        class IsColliding_CircleCircle : ColliderCollisionDetectorTests
        {
            [Test]
            public void Returns_True_When_Overlapping ()
            {
                ICircleColliderModel a = Substitute.For<ICircleColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Position.Returns(new Vector2(0, 0));
                a.SqrRadius.Returns(1);

                b.Position.Returns(new Vector2(0, 0));
                b.SqrRadius.Returns(1);

                Assert.IsTrue(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Do_Not_Overlapping ()
            {
                ICircleColliderModel a = Substitute.For<ICircleColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Position.Returns(new Vector2(0, 0));
                a.SqrRadius.Returns(1);

                b.Position.Returns(new Vector2(3, 0));
                b.SqrRadius.Returns(1);

                Assert.IsFalse(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Edging ()
            {
                ICircleColliderModel a = Substitute.For<ICircleColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Position.Returns(new Vector2(0, 0));
                a.SqrRadius.Returns(1);

                b.Position.Returns(new Vector2(2, 0));
                b.SqrRadius.Returns(1);

                Assert.IsFalse(Model.IsColliding(a, b));
            }
        }

        class IsColliding_SquareSquare : ColliderCollisionDetectorTests
        {
            [Test]
            public void Returns_True_When_Overlapping ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ISquareColliderModel b = Substitute.For<ISquareColliderModel>();

                a.Bounds.Returns(new Rect(0, 0, 1, 1));
                b.Bounds.Returns(new Rect(0, 0, 1, 1));

                Assert.IsTrue(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Do_Not_Overlapping ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ISquareColliderModel b = Substitute.For<ISquareColliderModel>();

                a.Bounds.Returns(new Rect(0, 0, 1, 1));
                b.Bounds.Returns(new Rect(3, 3, 1, 1));

                Assert.IsFalse(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Edging ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ISquareColliderModel b = Substitute.For<ISquareColliderModel>();

                a.Bounds.Returns(new Rect(0, 0, 1, 1));
                b.Bounds.Returns(new Rect(1, 1, 1, 1));

                Assert.IsFalse(Model.IsColliding(a, b));
            }
        }

        class IsColliding_SquareCircle : ColliderCollisionDetectorTests
        {
            [Test]
            public void Returns_True_When_Overlapping ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Position.Returns(new Vector2(0, 0));

                a.InnerRadius.Returns(1);
                a.OuterRadius.Returns(2);

                b.Position.Returns(new Vector2(0, 0));
                b.Radius.Returns(1);

                Assert.IsTrue(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Overlapping_OuterRadius_Bot_Not_InnerRadius ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Bounds.Returns(new Rect(0, 0, 1, 2));
                a.Position.Returns(new Vector2(0, 0));
                a.InnerRadius.Returns(1);
                a.OuterRadius.Returns(2);

                b.Position.Returns(new Vector2(3, 0));
                b.Radius.Returns(1);

                Assert.IsFalse(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Do_Not_Overlapping ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Position.Returns(new Vector2(0, 0));

                a.InnerRadius.Returns(1);
                a.OuterRadius.Returns(2);

                b.Position.Returns(new Vector2(4, 0));
                b.Radius.Returns(1);

                Assert.IsFalse(Model.IsColliding(a, b));
            }

            [Test]
            public void Returns_False_When_Edging_InnerRadius ()
            {
                ISquareColliderModel a = Substitute.For<ISquareColliderModel>();
                ICircleColliderModel b = Substitute.For<ICircleColliderModel>();

                a.Position.Returns(new Vector2(0, 0));

                a.InnerRadius.Returns(1);
                a.OuterRadius.Returns(2);

                b.Position.Returns(new Vector2(2, 0));
                b.Radius.Returns(1);

                Assert.IsFalse(Model.IsColliding(a, b));
            }
        }
    }
}
