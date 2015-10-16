using System;
using System.Diagnostics;
using Server;
using CommonLibrary;
using CommonLibrary.Representation;

namespace Server.Logic.GameObjects
{
    public class PlayerObject : AbstractPlayerObject
    {
        private const float AccelerationConstant = 5f;
        private float _accelerationModifier = 5f;

        public PlayerObject()
            : base()
        {
        }

        public void UpdatePosition(Client.Input inputData, Stopwatch elapsedTime)
        {
            //Add force
            Velocity += inputData.InputDirection.Normalize() * ((float)elapsedTime.ElapsedMilliseconds / 1000);

            //Apply force
            Position += Velocity * ((float)elapsedTime.ElapsedMilliseconds / 1000);

            //Apply counterforce
            Velocity -= Velocity * 0.9995f * ((float)elapsedTime.ElapsedMilliseconds / 1000);

            if (Velocity.Lenght <= 0.01f)
            {
                Velocity = new Vector2();
            }
        }

        public void AddForce()
        {
            //Eg, hit by an explotion or other impact spell of some kind
        }

        public void AccelerationModifier()
        {
            //Modify _accelerationModifier here
        }
    }
}

