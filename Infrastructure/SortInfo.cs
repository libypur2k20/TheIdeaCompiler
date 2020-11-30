using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TheIdeaCompiler.Model;

namespace TheIdeaCompiler.Infrastructure
{
    public class SortInfo
    {

        #region PUBLIC PROPERTIES

        //Information about the object's property to be sorted.
        public PropertyInfo SortProperty { get; set; }
        //Sorting direction (Ascending|Descending)
        public SortEnum SortDirection { get; set; }
        //The resulting sorted data.
        public List<SortData> SortData { get; set; }

        #endregion


        #region CONSTRUCTORS

        public SortInfo(PropertyInfo pInfo, SortEnum sort)
        {
            this.SortProperty = pInfo;
            this.SortDirection = sort;
            this.SortData = new List<SortData>();
        }

        #endregion


        #region PUBLIC METHODS

        /// <summary>
        /// Creates a new sorting group with the
        /// provided key and first item.
        /// </summary>
        /// <param name="sortPropertyValue">Group's key value</param>
        /// <param name="profileData">First item that matches the key value.</param>
        public void CreateSortGroup(string sortPropertyValue, ProfileData profileData)
        {
            SortData sData = new SortData(sortPropertyValue, new List<ProfileData> { profileData });
            this.SortData.Add(sData);
        }


        /// <summary>
        /// Returns a sort group that matches the specified key.
        /// </summary>
        /// <param name="groupKey">Key to search on existing sort groups</param>
        /// <returns>Sort group that matches the key</returns>
        public SortData GetSortGroup(String groupKey)
        {
            SortData result = null;

            foreach(SortData sData in this.SortData)
            {
                if (sData.SortKey == groupKey)
                    result = sData;
            }

            return result;
        }


        /// <summary>
        /// Inserts a new sort group based on key value and 
        /// sort direction, only taking in account non
        /// completed groups.
        /// </summary>
        /// <param name="sortPropertyValue">Key value for the new group.</param>
        /// <param name="profileData">First item for the new group.</param>
        /// <param name="numGroupsSorted">Num of groups already sorted (set as completed).</param>
        public void InsertSortGroup(string sortPropertyValue, ProfileData profileData, int numGroupsSorted)
        {

            //If true, new group has been inserted, otherwise it needs to be appended at the end of existing ones.
            Boolean inserted = false;

            //New sort grup to insert/append.
            SortData newSortData = new SortData(sortPropertyValue, new List<ProfileData>() { profileData });

            //Check for insert position on non completed groups.
            for(Int32 i = numGroupsSorted; i < this.SortData.Count; i++)
            {
                String currKey = this.SortData[i].SortKey;

                //Checks if the property value to insert is greather, equal or less than the current group key.
                Int32 strCompareResult = String.Compare(sortPropertyValue, currKey);


                if ((strCompareResult <= 0 && this.SortDirection == SortEnum.Ascending) ||
                    (strCompareResult > 0 && this.SortDirection == SortEnum.Descending))
                {
                    //Inserts a new group on the right position.
                    this.SortData.Insert(i, new SortData(sortPropertyValue, new List<ProfileData>() { profileData }));
                    inserted = true;
                    break;
                }
            }

            //If new group could not be inserted, it is appended at the end of group's list.
            if (inserted == false)
            {
                this.SortData.Add(new SortData(sortPropertyValue, new List<ProfileData>() { profileData }));
            }
        }



        /// <summary>
        /// Sets all already sorted data as completed to
        /// avoid use it on comparison operations. 
        /// It prevents results from parent groups to be
        /// mixed.
        /// </summary>
        /// <returns>The number of grouped keys already sorted.</returns>
        public Int32 SetCompleted()
        {
            foreach (SortData sData in this.SortData)
                sData.Completed = true;

            return this.SortData.Count;
        }


        #endregion
    }
}
