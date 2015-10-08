using System;
using System.Diagnostics;
using Server;
using CommonLibrary;
using CommonLibrary.Representation;

namespace Server.Logic.GameObjects
{
    public class PlayerObject : AbstractPlayerObject
    {
        public PlayerObject()
            : base()
        {
        }

        public void UpdatePosition(Client.Input inputData, Stopwatch elapsedTime)
        {
            Velocity += inputData.InputDirection.Normalize() * ((float)elapsedTime.ElapsedMilliseconds / 1000);
            Position += Velocity * ((float)elapsedTime.ElapsedMilliseconds / 1000);

            //THIS SHOULD BE WORKED UPON
            Velocity -= Velocity * 0.9995f * ((float)elapsedTime.ElapsedMilliseconds / 1000);
        }
    }
}

