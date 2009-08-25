using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Controllers;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Interfaces;
using FarseerGames.FarseerPhysics.Mathematics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace MarioPhysicsTest
{
    class GameObject
    {
        public string ResourceName;
        public Geom Geom;
        public Body Body;



        public Vector2 Position
        {
            get
            {
                return Body.Position;
            }
            set
            {
                Body.Position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return Body.Rotation;
            }
            set
            {
                Body.Rotation = value;
            }
            
        }

        public bool IsStatic
        {
            get
            {
                return Body.IsStatic;
            }
            set
            {
                Body.IsStatic = value;
            }
        }

        public virtual void Update()
        {
        }
    }

    class GameObjectFactory
    {
        public static GameObject CreateCircleGameObject(PhysicsSimulator simulator, string resourceName, float rad, float mass)
        {
            var go  = new GameObject();
            go.Body = BodyFactory.Instance.CreateCircleBody(rad, mass);
            go.Geom = GeomFactory.Instance.CreateCircleGeom(simulator, go.Body, rad, 20);
            go.ResourceName = resourceName;
            simulator.Add(go.Body);
            simulator.Add(go.Geom);
            return go;
        }

        public static GameObject CreateRectangleGameObject(PhysicsSimulator simulator, string resourceName, float width, float height, float mass)
        {
            var go = new GameObject();
            go.ResourceName = resourceName;
            go.Body = BodyFactory.Instance.CreateRectangleBody(width,height, mass);
            go.Geom = GeomFactory.Instance.CreateRectangleGeom(simulator,go.Body, width, height);
            simulator.Add(go.Body);
            simulator.Add(go.Geom);
            return go;
        }
    }
}
