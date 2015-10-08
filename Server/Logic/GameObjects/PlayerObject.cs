using System;
using System.Diagnostics;
using Server;
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
            Velocity += inputData.InputDirection.Normalize() * elapsedTime.ElapsedMilliseconds * 0.001f;
            Position += Velocity * elapsedTime.ElapsedMilliseconds * 0.001f;
            Velocity *= 0.9f;
        }
    }
}

