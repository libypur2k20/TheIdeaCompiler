using System;
using System.Collections.Generic;
using System.Text;

namespace TheIdeaCompiler.Infrastructure
{
    /// <summary>
    /// This class is used to specify the property name
    /// and direction for sorting.
    /// </summary>
    public class SortParams
    {

        //Property name to sort by.
        public String PropertyName { get; set; }
        //Direction to sort by.
        public SortEnum SortDirection { get; set; }

        public SortParams(string propName, SortEnum sortDir)
        {
            this.PropertyName = propName;
            this.SortDirection = sortDir;
        }
    }
}
