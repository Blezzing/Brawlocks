﻿using System;
using CommonLibrary.Representation;
using CommonLibrary;
using System.Diagnostics;

namespace Client.GameObjects
{
    public class PlayerObject : AbstractPlayerObject
    {
        //0 Time point for extrapolation
        public Stopwatch LastUpdatePosition = new Stopwatch();
        public Func<Double, Vector2> PositionFunction;
        public Vector2 ExtrapolatedPosition
        {
            get { return PositionFunction(LastUpdatePosition.Elapsed.TotalMilliseconds); }
        }

        public PlayerObject()
        {
            LastUpdatePosition.Start();
        }

        public PlayerObject(String source)
            : base(source)
        {
        }

        /*
        //Lineær extrapolation
        public void UpdatePositionFunction(Core.ServerState sOld, Core.ServerState sMid, Core.ServerState sNew)
        {
            PlayerObject pMid = sMid.PlayerObjects.Find(po => po.ID == this.ID);
            PlayerObject pNew = sNew.PlayerObjects.Find(po => po.ID == this.ID);

            Vector2 posDif = pNew.Position - pMid.Position;
            double timeDif = sNew.TimeSinceLastServerState.TotalMilliseconds;
            Vector2 velocity = new Vector2((float)(posDif.x / timeDif),(float)(posDif.y / timeDif));

            PositionFunction = (time) => {return Position+(velocity*(float)time);};
    
            LastUpdatePosition.Restart();
        }
        */


        //Exponential extrapolation
        public void UpdatePositionFunction(Core.ServerState sOld, Core.ServerState sMid, Core.ServerState sNew)
        {
            float timeSinceSOld = (float)(sNew.TimeSinceLastServerState.TotalMilliseconds);
            float timeSinceSMid = (float)(sNew.TimeSinceLastServerState.TotalMilliseconds + sNew.TimeSinceLastServerState.TotalMilliseconds);
            float timeSinceSNew = (float)(sNew.TimeSinceLastServerState.TotalMilliseconds + sNew.TimeSinceLastServerState.TotalMilliseconds + sNew.TimeSinceLastServerState.TotalMilliseconds);


            Func<Double, float> xFunction = OneAxisFunction(new Vector2(timeSinceSOld, sOld.PlayerObjects.Find(po => po.ID == this.ID).Position.x),
                                                          new Vector2(timeSinceSMid, sMid.PlayerObjects.Find(po => po.ID == this.ID).Position.x),
                                                          new Vector2(timeSinceSNew, sNew.PlayerObjects.Find(po => po.ID == this.ID).Position.x));

            Func<Double, float> yFunction = OneAxisFunction(new Vector2(timeSinceSOld, sOld.PlayerObjects.Find(po => po.ID == this.ID).Position.y),
                                                          new Vector2(timeSinceSMid, sMid.PlayerObjects.Find(po => po.ID == this.ID).Position.y),
                                                          new Vector2(timeSinceSNew, sNew.PlayerObjects.Find(po => po.ID == this.ID).Position.y));

            PositionFunction = (time) => { return new Vector2(xFunction(timeSinceSNew+time), yFunction(timeSinceSNew+time)); };
            LastUpdatePosition.Restart();
        }

        private Func<Double, float> OneAxisFunction(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            //Setup equations (form ax²+bx+c=y, with colums being a,b,c,y )
            float[] eq1 = { v1.x * v1.x, v1.x, 1f, v1.y };
            float[] eq2 = { v2.x * v2.x, v2.x, 1f, v2.y };
            float[] eq3 = { v3.x * v3.x, v3.x, 1f, v3.y };

            //Sort equations by value
            if (eq1[0] < eq2[0]) { Swap(ref eq1, ref eq2); }
            if (eq1[0] < eq3[0]) { Swap(ref eq1, ref eq3); }
            if (eq2[1] < eq3[1]) { Swap(ref eq2, ref eq3); }

            //Reduce bottom
            float mult;

            mult = eq2[0] / eq1[0];
            for (int i = 0; i < 4; i++)
                eq2[i] -= mult * eq1[i];

            mult = eq3[0] / eq1[0];
            for (int i = 0; i < 4; i++)
                eq3[i] -= mult * eq1[i];

            mult = eq3[1] / eq2[1];
            for (int i = 0; i < 4; i++)
                eq3[i] -= mult * eq2[i];

            //Reduce top

            mult = eq2[2] / eq3[2];
            for (int i = 0; i < 4; i++)
                eq2[i] -= mult * eq3[i];

            mult = eq1[2] / eq3[2];
            for (int i = 0; i < 4; i++)
                eq1[i] -= mult * eq3[i];

            mult = eq1[1] / eq2[1];
            for (int i = 0; i < 4; i++)
                eq1[i] -= mult * eq2[i];

            //Reduce to idendity

            mult = 1 / eq1[0];
            for (int i = 0; i < 4; i++)
                eq1[i] *= mult;

            mult = 1 / eq2[1];
            for (int i = 0; i < 4; i++)
                eq2[i] *= mult;

            mult = 1 / eq3[2];
            for (int i = 0; i < 4; i++)
                eq3[i] *= mult;

            //create result
            return (input) => { return (float)(eq1[3] * input * input + eq2[3] * input + eq3[3]); }; 
        }



        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

    }
}

