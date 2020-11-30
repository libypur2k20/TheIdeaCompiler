using System;
using System.Collections.Generic;
using System.Text;
using TheIdeaCompiler.Model;

namespace TheIdeaCompiler.Infrastructure
{
    public class SortData
    {
        //The string representation of the property's value.
        public String SortKey { get; set; }

        //List of items whose property's value matches the key.
        public List<ProfileData> SortItems { get; set; }

        //Set data as already sorted to prevent it to continue
        //taking part of the sorting process.
        public Boolean Completed { get; set; }


        /// <summary>
        /// Used for the load of the initial data
        /// at the beginning of the sorting process.
        /// </summary>
        /// <param name="key">Initial key value, set as 'All'</param>
        /// <param name="values">Initial collection of ProfileData object instances to be sorted.</param>
        public SortData(String key, List<ProfileData> values = null)
        {
            this.SortKey = key;
            this.SortItems = (values != null ? values : new List<ProfileData>());
        }
    }
}
