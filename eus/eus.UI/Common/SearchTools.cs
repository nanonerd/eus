using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace eus.UI.Common
{
    public class SearchTools
    {
        // Break down topic and return topicURL using words from title. 
        // E.g., "the best burger ever" will return "best-burger-ever".
        public static string CreateSearchURL(string topic)
        {
            try
            {
                // Regex replaces any non-letters, etc. with a dash. "Hello world, Hi!" turns into "Hello-world-Hi-". 
                string cleansed = Regex.Replace(topic, @"[^A-Za-z0-9_\.~]+", "-");
                cleansed = StringExtension.Clean(topic);

                string[] words = cleansed.Split(new Char[] { ' ', ',', '.', ':' });

                var list = words.ToList();
                list.RemoveAll(item => item == "");
                words = list.ToArray();

                //int blankCount = 0;
                //for (int i = 0; i < words.Count(); i++)
                //{
                //    words[i] = words[i].Trim();
                //    if (words[i] == "")  // if index is blank, increment blankCount
                //    {
                //        blankCount++;
                //    }
                //}

                //string[] final = RemoveBlankItemsFromArray(words, blankCount);

                return string.Join("-", words);   // creates hyphenated string       
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //// Can't remove item from array, so create new one without blank items.
        //private static string[] RemoveBlankItemsFromArray(string[] array, int blankCount)
        //{
        //    try
        //    {
        //        string[] newArray = new string[array.Length - blankCount];

        //        int i = 0;
        //        int j = 0;
        //        while (i < array.Length)
        //        {
        //            if (array[i] != "")  // If array item is blank, don't place in new array 
        //            {
        //                newArray[j] = array[i];
        //                j++;
        //            }
        //            i++;
        //        }

        //        return newArray;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }


    // Remove common words and characters from line and return hypenated string
    public static class StringExtension
    {
        public static Dictionary<string, string> _replace = new Dictionary<string, string>();

        static StringExtension()
        {
            _replace["!"] = " ";
            _replace["?"] = " ";
            _replace["&"] = " ";
            _replace[","] = " ";
            _replace[":"] = " ";
            _replace[";"] = " ";
            _replace["!"] = " ";
            _replace["-"] = " ";
            _replace["_"] = " ";
            _replace["("] = " ";
            _replace[")"] = " ";
            _replace["{"] = " ";
            _replace["}"] = " ";
            _replace["["] = " ";
            _replace["]"] = " ";
            _replace["'"] = " ";
            _replace[" a "] = " ";
            _replace[" an "] = " ";
            _replace[" and "] = " ";
            _replace[" can "] = " ";
            _replace[" do "] = " ";
            _replace["does"] = " ";
            _replace["how"] = " ";
            _replace[" is "] = " ";
            _replace["it "] = " ";
            _replace[" it "] = " ";
            _replace["the "] = "";
            _replace[" the "] = " ";
            _replace["this"] = " ";
            _replace["why"] = " ";
        }

        public static string Clean(this string s)
        {
            try
            {
                // if a set text is found in _replace dictionary list, replace it with its respective replacement value
                foreach (string to_replace in _replace.Keys)
                {
                    s = s.Replace(to_replace, _replace[to_replace]);   // s.Replace(oldValue, newValue)
                }
                return s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}