using System;
using System.Collections.Generic;
using System.Text;

namespace TheIdeaCompiler.Infrastructure
{

    /// <summary>
    /// This class is used to provide output and sort
    /// parameters to the method that actually performs
    /// those tasks 'Program.SortAndOutput'.
    /// </summary>
    public class OutputParams
    {

        #region PUBLIC PROPERTIES

        //Description for the sorted result.
        public String Title { get; set; }

        //Collection of property names and sort directions
        //that will be used to sort a list of items.
        public List<SortParams> SortParams { get; set; }

        #endregion


        #region CONSTRUCTORS

        public OutputParams(String title)
        {
            this.Title = $"SORT BY ({title})";
            this.SortParams = new List<SortParams>();
        }

        #endregion


        #region PUBLIC METHODS

        /// <summary>
        /// This method adds a pair of property name
        /// and sort direction values to the list of
        /// sort parameters and returns the current
        /// instance to be able to chain calls.
        /// </summary>
        /// <param name="propName">Property name to sort by</param>
        /// <param name="sortDir">Sort direction to apply</param>
        /// <returns></returns>
        public OutputParams AddSortParam(String propName, SortEnum sortDir)
        {
            if (this.SortParams != null)
            {
                this.SortParams.Add(new SortParams(propName, sortDir));
            }

            return this;
        }


        #endregion
    }
}
