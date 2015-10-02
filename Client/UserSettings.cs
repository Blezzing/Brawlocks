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
    }
}
