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
            Position += Velocity * ((float)elapsedTime.ElapsedMilliseconds / 1000);

            //THIS SHOULD BE WORKED UPON -- i think it's good tho
            if (foo.Lenght > 0)
            {
                Velocity -= foo;
            }
            else
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

