using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameTests.Physics.Collisions
{
    public class CollisionHandlerTests
    {
        public CollisionHandler Model { get; private set; }

        public IQuadTree<IEntityModel> QuadTree { get; private set; }
        public IPhysicsSettings PhysicsSettings { get; private set; }
        public IDebugSettings DebugSettings { get; private set; }
        public IStageBounds StageBounds { get; private set; }
        public IColliderCollisionDetector ColliderCollisionsDetector { get; private set; }

        [SetUp]
        public void Setup ()
        {
            QuadTree = Substitute.For<IQuadTree<IEntityModel>>();
            PhysicsSettings = Substitute.For<IPhysicsSettings>();
            DebugSettings = Substitute.For<IDebugSettings>();
            StageBounds = Substitute.For<IStageBounds>();
            ColliderCollisionsDetector = Substitute.For<IColliderCollisionDetector>();

            Model = new CollisionHandler(
                QuadTree,
                PhysicsSettings,
                DebugSettings,
                StageBounds,
                ColliderCollisionsDetector
            );
        }

        public IEntityModel CreateEntity<TCollider> (CollisionLayer layer)
            where TCollider : class, IColliderModel
        {
            return CreateEntity<TCollider>(layer, out _);
        }

        public IEntityModel CreateEntity<TCollider> (CollisionLayer layer, out TCollider collider)
            where TCollider : class, IColliderModel
        {
            collider = Substitute.For<TCollider>();
            collider.Layer.Returns(layer);

            IEntityModel entity = Substitute.For<IEntityModel>();
            entity.Collider.Returns(collider);
            return entity;
        }

        class DetectCollisions : CollisionHandlerTests
        {
            public readonly List<Collision> collisions = new List<Collision>();

            class QuadTreeTests : DetectCollisions
            {
                [Test]
                public void Clears_QuadTree ()
                {
                    IEntityModel[] entities = { };

                    Model.DetectCollisions(entities, collisions);

                    QuadTree.ReceivedWithAnyArgs(1).ClearAndUpdateMainRect(default);
                }

                [Test]
                public void Sets_QuadTree_MainRect_To_Stage_Bounds ()
                {
                    IEntityModel[] entities = { };

                    Rect expected = new Rect(1, 1, 1, 1);
                    StageBounds.Rect.Returns(expected);

                    Model.DetectCollisions(entities, collisions);

                    QuadTree.ReceivedWithAnyArgs(1).ClearAndUpdateMainRect(expected);
                }

                [Test]
                public void Adds_Entities_To_QuadTree ()
                {
                    IEntityModel[] entities = { };
                    Model.DetectCollisions(entities, collisions);

                    QuadTree.Received(1).InsertRange(entities);
                }

                [Test]
                public void Calls_Quad_Tree_GetNearest_Objects ()
                {
                    IEntityModel expected = Substitute.For<IEntityModel>();
                    IEntityModel[] entities = { expected };

                    Model.DetectCollisions(entities, collisions);

                    QuadTree.Received(1).GetNearestObjects(expected);
                }
            }

            class ColliderCollisionsDetectorTests : DetectCollisions
            {
                [Test]
                public void Calls_LayerCollidesWith_With_Collider_Layers ()
                {
                    IEntityModel a = CreateEntity<IColliderModel>(CollisionLayer.Player);
                    IEntityModel b = CreateEntity<IColliderModel>(CollisionLayer.Asteroid);

                    IEntityModel[] entities = { a, b };
                    QuadTree.GetNearestObjects(a).Returns(new[] { b });

                    Model.DetectCollisions(entities, collisions);

                    ColliderCollisionsDetector.Received(1).LayerCollidesWith(
                        CollisionLayer.Player,
                        CollisionLayer.Asteroid
                    );
                }

                [Test]
                public void Doesnt_Call_LayerCollidesWith_When_Colliding_With_Self ()
                {
                    IEntityModel a = Substitute.For<IEntityModel>();

                    IEntityModel[] entities = { a };
                    QuadTree.GetNearestObjects(a).Returns(new[] { a });

                    Model.DetectCollisions(entities, collisions);

                    ColliderCollisionsDetector.DidNotReceive().LayerCollidesWith(
                        CollisionLayer.Player,
                        CollisionLayer.Player
                    );
                }

                [Test]
                public void Calls_IsColliding_Circle_Circle ()
                {
                    IEntityModel a = CreateEntity(default, out ICircleColliderModel colliderA);
                    IEntityModel b = CreateEntity(default, out ICircleColliderModel colliderB);

                    IEntityModel[] entities = { a, b };
                    QuadTree.GetNearestObjects(a).Returns(new[] { a, b });

                    ColliderCollisionsDetector.LayerCollidesWith(
                        default,
                        default
                    ).ReturnsForAnyArgs(true);

                    Model.DetectCollisions(entities, collisions);

                    ColliderCollisionsDetector.Received(1).IsColliding(colliderA, colliderB);
                }

                [Test]
                public void Calls_IsColliding_Square_Square ()
                {
                    IEntityModel a = CreateEntity(default, out ISquareColliderModel colliderA);
                    IEntityModel b = CreateEntity(default, out ISquareColliderModel colliderB);

                    IEntityModel[] entities = { a, b };
                    QuadTree.GetNearestObjects(a).Returns(new[] { a, b });

                    ColliderCollisionsDetector.LayerCollidesWith(
                        default,
                        default
                    ).ReturnsForAnyArgs(true);

                    Model.DetectCollisions(entities, collisions);

                    ColliderCollisionsDetector.Received(1).IsColliding(colliderA, colliderB);
                }

                [Test]
                public void Calls_IsColliding_Square_Circle ()
                {
                    IEntityModel a = CreateEntity(default, out ISquareColliderModel colliderA);
                    IEntityModel b = CreateEntity(default, out ICircleColliderModel colliderB);

                    IEntityModel[] entities = { a, b };
                    QuadTree.GetNearestObjects(a).Returns(new[] { a, b });

                    ColliderCollisionsDetector.LayerCollidesWith(
                        default,
                        default
                    ).ReturnsForAnyArgs(true);

                    Model.DetectCollisions(entities, collisions);

                    ColliderCollisionsDetector.Received(1).IsColliding(colliderA, colliderB);
                }

                [Test]
                public void Fills_Buffer_With_Collision ()
                {
                    IEntityModel a = CreateEntity<ICircleColliderModel>(default);
                    IEntityModel b = CreateEntity<ICircleColliderModel>(default);

                    IEntityModel[] entities = { a, b };
                    QuadTree.GetNearestObjects(a).Returns(new[] { a, b });

                    ColliderCollisionsDetector.LayerCollidesWith(
                        default,
                        default
                    ).ReturnsForAnyArgs(true);

                    ColliderCollisionsDetector.IsColliding(
                        Arg.Any<ICircleColliderModel>(),
                        Arg.Any<ICircleColliderModel>()
                    ).Returns(true);

                    Model.DetectCollisions(entities, collisions);

                    Assert.AreEqual(new Collision(a, b.Collider), collisions[0]);
                }
            }
        }
    }
}
