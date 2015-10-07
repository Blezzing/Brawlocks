using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class UserSettings
    {
        // SOUND SETTINGS:
        public bool MUSIC   = true;
        public bool SOUND   = true;
        public float VOLUME = 100f;

        public void UpdateFromFile(string userPrefDoc)
        {
            string[] lines = System.IO.File.ReadAllLines(userPrefDoc); //Puts each line into the array

            int first;  //First occurence of "\""
            int last;   //Last --||--
            string str; //The string in between
            foreach (string line in lines) 
            {
                if (line.Count(r => r.ToString() == "\"") == 2) 
                {
                    first = line.IndexOf("\"") + 1;
                    last = line.LastIndexOf("\"");
                    str = line.Substring(first, last - first);

                    if (line.StartsWith("MUSIC:"))
                    {
                        if (str.CompareTo("ON") == 0)
                        { MUSIC = true; }
                        else if (str.CompareTo("OFF") == 0)
                        { MUSIC = false; }
                        else { MUSIC = true; }
                    }
                    if (line.StartsWith("SOUND:"))
                    {
                        if (str.CompareTo("ON") == 0)
                        { SOUND = true; }
                        else if (str.CompareTo("OFF") == 0)
                        { SOUND = false; }
                        else { SOUND = true; }
                    }
                    if (line.StartsWith("VOLUME:"))
                    {
                        int foo;
                        if (Int32.TryParse(str, out foo))
                            { VOLUME = foo; }
                        else { VOLUME = 100; }
                    }
            }
        }
    }

        public void UpdateFile(string userPrefDoc)
        {
            string[] lines = System.IO.File.ReadAllLines(userPrefDoc); //Puts each line into the array

            int first;  //First occurence of "\""
            int last;   //Last --||--
            string str; //The string in between
            foreach (string line in lines)
            {
                // This doesnt actually do anything.. Yet!
                if (line.Count(r => r.ToString() == "\"") == 2)
                {
                    first = line.IndexOf("\"") + 1;
                    last = line.LastIndexOf("\"");
                    str = line.Substring(first, last - first);

                    if (line.StartsWith("MUSIC:"))
                    {
                        if (str.CompareTo(MUSIC.ToString()) != 0)
                        { str = MUSIC.ToString(); }
                        else if (str.CompareTo("OFF") == 0)
                        { MUSIC = false; }
                        else { MUSIC = true; }
                    }
                    if (line.StartsWith("SOUND:"))
                    {
                        if (str.CompareTo("ON") == 0)
                        { SOUND = true; }
                        else if (str.CompareTo("OFF") == 0)
                        { SOUND = false; }
                        else { SOUND = true; }
                    }
                    if (line.StartsWith("VOLUME:"))
                    {
                        int foo;
                        if (Int32.TryParse(str, out foo))
                        { VOLUME = foo; }
                        else { VOLUME = 100; }
                    }
                }
            }
        }
    }
}
