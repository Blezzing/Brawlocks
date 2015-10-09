using System;
using CommonLibrary.Representation;
using CommonLibrary;

namespace Client.GameObjects
{
    public class PlayerObject : AbstractPlayerObject
    {
        //0 Time point for extrapolation
        public Func<Double, Vector2> PositionFunction;
        public Vector2 ExtrapolatedPosition
        {
            get { Client.Informer.SetDebugString(Client.GlobalTimer.ElapsedMilliseconds.ToString()); return PositionFunction(Client.GlobalTimer.Elapsed.TotalSeconds); }// Client.Informer.SetDebugString(PositionFunction((DateTime.Now).Ticks).x.ToString()); return PositionFunction((DateTime.Now).Ticks); }
        }


        public PlayerObject()
        {
        }

        public PlayerObject(String source)
            : base(source)
        {
        }

        public void UpdatePositionFunction(Core.ServerState s1, Core.ServerState s2, Core.ServerState s3)
        {
            Func<Double, float> xFunction = OneAxisFunction(new Vector2((float)s1.ContructionTime.TotalSeconds, s1.PlayerObjects.Find(po => po.ID == this.ID).Position.x),
                                                          new Vector2((float)s2.ContructionTime.TotalSeconds, s2.PlayerObjects.Find(po => po.ID == this.ID).Position.x),
                                                          new Vector2((float)s3.ContructionTime.TotalSeconds, s3.PlayerObjects.Find(po => po.ID == this.ID).Position.x));

            Func<Double, float> yFunction = OneAxisFunction(new Vector2((float)s1.ContructionTime.TotalSeconds, s1.PlayerObjects.Find(po => po.ID == this.ID).Position.y),
                                                          new Vector2((float)s2.ContructionTime.TotalSeconds, s2.PlayerObjects.Find(po => po.ID == this.ID).Position.y),
                                                          new Vector2((float)s3.ContructionTime.TotalSeconds, s3.PlayerObjects.Find(po => po.ID == this.ID).Position.y));

            PositionFunction = (time) => { return new Vector2(xFunction(time), yFunction(time)); };
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

