using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameTests.Entities.RigidBody
{
    public class RigidBodyModelTests
    {
        public RigidBodyModel Model { get; private set; }

        public IEntitySettings Settings { get; private set; }
        public IStageBoundsModel StageInfo { get; private set; }
        public IColliderModel Collider { get; private set; }

        [SetUp]
        public void Setup ()
        {
            Settings = Substitute.For<IEntitySettings>();
            StageInfo = Substitute.For<IStageBoundsModel>();
            Collider = Substitute.For<IColliderModel>();

            Settings.MaxSpeed.Returns(float.PositiveInfinity);

            Model = Substitute.ForPartsOf<RigidBodyModel>(Settings, StageInfo, Collider);
        }

        class PublicProperties : RigidBodyModelTests
        {
            [Test]
            public void Collider_Equals ()
            {
                Assert.AreEqual(Collider, Model.Collider);
            }

            [Test]
            public void MaxSpeed_Equals_Settings ()
            {
                Settings.MaxSpeed.Returns(10);
                Assert.AreEqual(10, Model.MaxSpeed);
            }

            [Test]
            public void Drag_Equals_Settings ()
            {
                Settings.Drag.Returns(10);
                Assert.AreEqual(10, Model.Drag);
            }

            [Test]
            public void WrapOnScreenEdge_Equals_Settings ()
            {
                Settings.WrapOnScreenEdge.Returns(true);
                Assert.IsTrue(Model.WrapOnScreenEdge);
            }
        }

        class Step : RigidBodyModelTests
        {
            [Test]
            public void Velocity_Increments_By_Acceleration ()
            {
                Settings.Drag.Returns(1);
                Model.Acceleration = Vector2.one;

                Model.Step(default);

                Assert.AreEqual(Vector2.one, Model.Velocity);
            }

            [Test]
            public void Velocity_Limited_To_Max_Speed ()
            {
                Settings.Drag.Returns(1);
                Settings.MaxSpeed.Returns(1);
                Model.Acceleration = Vector2.right * 2;
                Model.Step(1);
                Assert.AreEqual(Vector2.right, Model.Velocity);
            }

            [Test]
            public void Position_Increments_By_Velocity_Times_DeltaTime ()
            {
                Model.Velocity = Vector2.one;
                Model.Step(2);
                Assert.AreEqual(Vector2.one * 2, Model.Position);
            }

            [Test]
            public void Rotation_Increments_By_AngularVelocity_Times_DeltaTime ()
            {
                Model.AngularVelocity = 1;
                Model.Step(2);
                Assert.AreEqual(1 * 2, Model.Rotation);
            }

            [Test]
            public void Velocity_Multiplies_By_Drag ()
            {
                Settings.Drag.Returns(0.5f);
                Model.Velocity = Vector2.one;
                Model.Step(default);
                Assert.AreEqual(Vector2.one * 0.5f, Model.Velocity);
            }

            [Test]
            public void WrapOnScreenEdge_True_Wraps_Position_From_Top_To_Bottom ()
            {
                Settings.WrapOnScreenEdge.Returns(true);
                StageInfo.Rect.Returns(new Rect(-1, -1, 2, 2));

                Model.Position = Vector2.up * 2;

                Model.Step(1);

                Assert.AreEqual(Vector2.down, Model.Position);
            }

            [Test]
            public void WrapOnScreenEdge_True_Wraps_Position_From_Bottom_To_Top ()
            {
                Settings.WrapOnScreenEdge.Returns(true);
                StageInfo.Rect.Returns(new Rect(-1, -1, 2, 2));

                Model.Position = Vector2.down * 2;

                Model.Step(1);

                Assert.AreEqual(Vector2.up, Model.Position);
            }

            [Test]
            public void WrapOnScreenEdge_True_Wraps_Position_From_Left_To_Right ()
            {
                Settings.WrapOnScreenEdge.Returns(true);
                StageInfo.Rect.Returns(new Rect(-1, -1, 2, 2));

                Model.Position = Vector2.left * 2;

                Model.Step(1);

                Assert.AreEqual(Vector2.right, Model.Position);
            }

            [Test]
            public void WrapOnScreenEdge_True_Wraps_Position_From_Right_To_Left ()
            {
                Settings.WrapOnScreenEdge.Returns(true);
                StageInfo.Rect.Returns(new Rect(-1, -1, 2, 2));

                Model.Position = Vector2.right * 2;
                Model.Step(1);
                Assert.AreEqual(Vector2.left, Model.Position);
            }

            [Test]
            public void WrapOnScreenEdge_False_Does_Not_Wrap_Position ()
            {
                Settings.WrapOnScreenEdge.Returns(false);
                StageInfo.Rect.Returns(new Rect(-1, -1, 2, 2));

                Model.Position = Vector2.up * 2;
                Model.Step(1);
                Assert.AreEqual(Vector2.up * 2, Model.Position);
            }

            [Test]
            public void WrapOnScreenEdge_False_Raises_OnOutOfBounds ()
            {
                Settings.WrapOnScreenEdge.Returns(false);
                StageInfo.Rect.Returns(new Rect(-1, -1, 2, 2));
                Model.Position = Vector2.up * 2;

                bool raised = false;
                Model.OnOutOfBounds += () => raised = true;
                Model.Step(1);

                Assert.IsTrue(raised);
            }
        }

        class SyncComponents : RigidBodyModelTests
        {
            [Test]
            public void Sets_Collider_Position_To_Current_Position ()
            {
                Model.Position = Vector2.one;

                Model.SyncComponents();

                Collider.Received().Position = Vector2.one;
            }
        }
    }
}
