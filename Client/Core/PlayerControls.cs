using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Client
{
    public class PlayerControls
    {
        //Used to check bindings
        private char[] _alphabet      = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private string[] _useableKeys = { "UP", "DOWN", "LEFT", "RIGHT" };

        //Defaults - these are the player controls
        private Key _moveUp    = Key.Up;
        private Key _moveDown  = Key.Down;
        private Key _moveLeft  = Key.Left;
        private Key _moveRight = Key.Right;

        public PlayerControls(string userPrefDoc) //The "Userprefs.txt" -->
        {
            string[] lines = System.IO.File.ReadAllLines(userPrefDoc); //Puts each line into the array

                //This would print the file
            //Console.WriteLine("\n\nContents of userpref.txt:");
            //foreach (string line in lines)
            //{
            //    // Use a tab to indent each line of the file.
            //    Console.WriteLine(line);
            //}

            int first;  //First occurence of "\""
            int last;   //Last --||--
            string str; //The string in between, aka the keyboard key in question
            foreach (string line in lines) //Should probably do something smarter, but this will do for now..
            {
                if (line.Count(r => r.ToString() == "\"") == 2) //We should only check lines that contain two "\""s, check "Userpref.txt" -->
                {
                    first = line.IndexOf("\"") + 1;
                    last = line.LastIndexOf("\"");
                    str = line.Substring(first, last - first); //This should get the key

                    //Now we compare the key to the avaiable player controls
                    if (line.StartsWith("UP"))
                    {
                        if (canConvertToKey(str))        //Is the "key" an actual key?
                        { _moveUp = convertToKey(str); } //Yes? Then lets convert!
                    }
                    if (line.StartsWith("DOWN"))
                    {
                        if (canConvertToKey(str))
                        { _moveDown = convertToKey(str); }
                    }
                    if (line.StartsWith("LEFT"))
                    {
                        if (canConvertToKey(str))
                        { _moveLeft = convertToKey(str); }
                    }
                    if (line.StartsWith("RIGHT"))
                    {
                        if (canConvertToKey(str))
                        { _moveRight = convertToKey(str); }
                    }
                }
            }
        }

        private bool canConvertToKey(string str) //Checks if str corresponds to an actual keyboard key
        {
            foreach (char c in _alphabet) //Checks for keyboard characters
            {
                if (string.Compare(c.ToString(), str) == 0)
                {
                    return true;
                }
            }
            foreach (string s in _useableKeys) //Checks other keys, such as the arrow keys or the "enter" key
            {
                if (string.Compare(s, str) == 0)
                {
                    return true;
                }
            }
            return false; //Hm, seems "str" was not a valid key
        }

        private Key convertToKey(string str) //Returns the actual Keyboard key. There must be a smarter way to do this, but w/e..
        {
            switch(str)
            {
                case ("UP"):
                    return Key.Up;
                case ("DOWN"):
                    return Key.Down;
                case ("LEFT"):
                    return Key.Left;
                case ("RIGHT"):
                    return Key.Right;
                case ("A"):
                    return Key.A;
                case ("B"):
                    return Key.B;
                case ("C"):
                    return Key.C;
                case ("D"):
                    return Key.D;
                case ("E"):
                    return Key.E;
                case ("F"):
                    return Key.F;
                case ("G"):
                    return Key.G;
                case ("H"):
                    return Key.H;
                case ("I"):
                    return Key.I;
                case ("J"):
                    return Key.J;
                case ("K"):
                    return Key.K;
                case ("L"):
                    return Key.L;
                case ("M"):
                    return Key.M;
                case ("N"):
                    return Key.N;
                case ("O"):
                    return Key.O;
                case ("P"):
                    return Key.P;
                case ("Q"):
                    return Key.Q;
                case ("R"):
                    return Key.R;
                case ("S"):
                    return Key.S;
                case ("T"):
                    return Key.T;
                case ("U"):
                    return Key.U;
                case ("V"):
                    return Key.V;
                case ("W"):
                    return Key.W;
                case ("X"):
                    return Key.X;
                case ("Y"):
                    return Key.Y;
                case ("Z"):
                    return Key.Z;
                default:
                    return Key.Unknown;
            }
        }

        //Getters - so client can see which action is performed by which key
        public Key MoveUp
        {
            get { return _moveUp; }
        }
        public Key MoveDown
        {
            get { return _moveDown; }
        }
        public Key MoveLeft
        {
            get { return _moveLeft; }
        }
        public Key MoveRight
        {
            get { return _moveRight; }
        }
    }
}
