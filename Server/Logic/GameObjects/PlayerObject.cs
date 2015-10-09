using System;
using System.Diagnostics;
using Server;
using CommonLibrary;
using CommonLibrary.Representation;

namespace Server.Logic.GameObjects
{
    public class PlayerObject : AbstractPlayerObject
    {
        float accelerationConstant = 5f;

        public PlayerObject()
            : base()
        {
        }

        public void UpdatePosition(Client.Input inputData, Stopwatch elapsedTime)
        {
            Velocity += inputData.InputDirection.Normalize() * ((float)elapsedTime.ElapsedMilliseconds / 1000) * accelerationConstant;
            Position += Velocity * ((float)elapsedTime.ElapsedMilliseconds / 1000);

            //THIS SHOULD BE WORKED UPON -- i think it's good tho
            Vector2 foo = Velocity * ((float)elapsedTime.ElapsedMilliseconds / 1000) * accelerationConstant;
            if (foo.Lenght > 0)
            {
                Velocity -= foo;
            }
            else
            {
                Velocity = new Vector2();
            }
        }
    }
}

