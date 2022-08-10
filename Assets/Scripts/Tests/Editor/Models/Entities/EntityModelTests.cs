using NSubstitute;
using NUnit.Framework;

namespace GameTests.Entities.Entity
{
    public class EntityModelTests
    {
        public EntityModel Model { get; private set; }

        public IRigidBodyModel RigidBody { get; private set; }
        public IColliderModel Collider { get; private set; }

        [SetUp]
        public void Setup ()
        {
            RigidBody = Substitute.For<IRigidBodyModel>();
            Collider = Substitute.For<IColliderModel>();

            Model = Substitute.ForPartsOf<EntityModel>(RigidBody, Collider);
        }

        class PublicProperties : EntityModelTests
        {
            [Test]
            public void RigidBody_Equals ()
            {
                Assert.AreEqual(RigidBody, Model.RigidBody);
            }

            [Test]
            public void Collider_Equals ()
            {
                Assert.AreEqual(Collider, Model.Collider);
            }

            [Test]
            public void IsAlive_True ()
            {
                Assert.IsTrue(Model.IsAlive);
            }

            [Test]
            public void IsAlive_False ()
            {
                Model.Destroy();
                Assert.IsFalse(Model.IsAlive);
            }
        }

        class OnPrePhysicsStep : EntityModelTests
        {
            [Test]
            public void Raises_OnReadyToReceiveInputs ()
            {
                bool raised = false;
                Model.OnReadyToReceiveInputs += () => raised = true;

                Model.OnPrePhysicsStep();

                Assert.IsTrue(raised);
            }
        }

        class OnPhysicsStep : EntityModelTests
        {
            [Test]
            public void Calls_RigidBody_Step ()
            {
                Model.OnPhysicsStep(1);

                RigidBody.Received(1).Step(1);
            }
        }

        class OnPostPhysicsStep : EntityModelTests
        {
            [Test]
            public void Calls_RigidBody_SyncComponents ()
            {
                Model.OnPostPhysicsStep();

                RigidBody.Received(1).SyncComponents();
            }

            [Test]
            public void Raises_OnDestroy_If_Destroyed ()
            {
                IEntityModel result = null;
                Model.OnDestroy += x => result = x;
                Model.Destroy();

                Model.OnPostPhysicsStep();

                Assert.AreEqual(Model, result);
            }
        }

        class OnCollide : EntityModelTests
        {
            [Test]
            public void Destroys_On_Collision ()
            {
                Model.OnCollide(new Collision(Model, default));
                Assert.IsFalse(Model.IsAlive);
            }
        }
    }
}
