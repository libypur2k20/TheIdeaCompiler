using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TheIdeaCompiler.Infrastructure;

namespace TheIdeaCompiler.Model
{
    /// <summary>
    /// This class stores personal data.
    /// One instance is created for each record
    /// imported from .txt files on Data folder. 
    /// </summary>
    public class ProfileData
    {


        #region PROPERTIES


            public String LastName { get; set; }

            public String FirstName { get; set; }

            /// <summary>
            /// Gender can be 'Female' or 'Male'.
            /// </summary>
            public GenderEnum  Gender { get; set; }

            /// <summary>
            /// Dates need to be displayed as ##/##/####
            /// </summary>
            public DateTime DateOfBirth { get; set; }

            /// <summary>
            /// It can be any string representing a color or such,
            /// so no enum is used.
            /// </summary>
            public String FavoriteColor { get; set; }


        #endregion


        #region CONSTRUCTORS



        public ProfileData(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
            Gender = GenderEnum.Unknown;
        }


        #endregion


        #region STATIC FIELDS

        //Delimiters for fields on raw text data from files.
        private static char[] _rawPartsDelimiters = { ' ', '|', ',' };

        //Delimiters for date formats. Available formats: 'mm/dd/yyyy' or 'mm-dd-yyyy'.
        private static char[] _datePartsDelimiters = { '-', '/' };

        //Minimum length of raw data to be considered as a possible gender value.
        private static byte _minGenderRawLength = 1;

        //Minimum length of raw data to be considered as a possible date of birth value.
        private static byte _minDateRawLength = 8;

        #endregion


        #region STATIC METHODS


        /// <summary>
        /// This function parses a single raw line of text into a
        /// 'ProfileData' type object.
        /// </summary>
        /// <param name="fileLine">Single raw line of text.</param>
        /// <returns>ProfileData type object.</returns>
        private static ProfileData GetProfileFromFileLine(string fileLine)
        {
            ProfileData profileData = null;

            try
            {

                if (String.IsNullOrEmpty(fileLine) == false && fileLine.Length > 0)
                {

                    //Split raw line of text.
                    String[] profileRawParts = fileLine.Split(_rawPartsDelimiters, StringSplitOptions.RemoveEmptyEntries);

                    //To create a new ProfileData instance, it must have firstName and lastName.
                    if (profileRawParts[0].Length > 0 && profileRawParts[1].Length > 0)
                    {

                        //Create new ProfileData instance with first and last name.
                        profileData = new ProfileData(profileRawParts[0], profileRawParts[1]);

                        //Concatenate gender names to check if raw part matches any of them.
                        String strGenders = String.Join('_', Enum.GetNames(typeof(GenderEnum)));

                        //Loop throug raw parts from first name to the length of the array of parts
                        //to search for gender, date of birth and favorite color.
                        for(Int32 i = 2; i < profileRawParts.Length; i++)
                        {

                            //Get the gender.
                            if (profileRawParts[i].Length == _minGenderRawLength || strGenders.Contains(profileRawParts[i]))
                            {
                                GenderEnum getGender = Utilities.ParseGender(profileRawParts[i]);

                                //Only assign new gender value when parsing returns a value not 'Unknown'.
                                if (getGender != GenderEnum.Unknown)
                                    profileData.Gender = getGender;
                            }
                            //Get date of birth.
                            else if (profileRawParts[i].Length >= _minDateRawLength && profileRawParts[i].IndexOfAny(_datePartsDelimiters) > 0)
                            {
                                profileData.DateOfBirth = Utilities.ParseDate(profileRawParts[i]);
                            }
                            //Get favorite color
                            else
                            {
                                profileData.FavoriteColor = profileRawParts[i];
                            }

                        }

                    }

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unexpected error found while parsing profile raw data:{ex.Message}");
                profileData = null;
            }

            return profileData;
        }


        /// <summary>
        /// This function gets a list of strings.
        /// Each string represents a profile raw data.
        /// A call to 'GetProfileFromFileLine' function is made for
        /// each raw string data to parse it into a ProfileData
        /// type instance object.
        /// </summary>
        /// <param name="textData">List of profile's raw data</param>
        /// <returns>List of object instances of 'ProfileData' type.</returns>
        public static List<ProfileData> BulkLoad(IEnumerable<String> textData)
        {
            List<ProfileData> result = new List<ProfileData>();

            foreach (String line in textData)
            {
                //Each raw data is parsed into a ProfileData type object.
                var pData = GetProfileFromFileLine(line);

                //If parsed correctly, it's added to the list of results.
                if (pData != null)
                    result.Add(pData);
            }

            return result;
        }



        public static List<ProfileData> PerformSorting(List<ProfileData> data, SortInfo[] sortParams)
        {
            List<ProfileData> sortedData = new List<ProfileData>();

            try
            {
                if ((data != null && data.Count > 0) && (sortParams != null && sortParams.Length > 0))
                {
                    //Initialize sorting data dictionary.
                    List<SortData> iniData = new List<SortData>();
                    iniData.Add(new SortData("All", data));

                    //Loop through each sort property and direction.
                    for(Int32 i=0; i < sortParams.Length;i++)
                    {
                        SortInfo sortParam = sortParams[i];

                        //Loop through each dictionary.
                        foreach(SortData sortData in iniData)
                        {
                            //Set the already sorted data as completed.
                            Int32 numGroupsSorted = sortParam.SetCompleted();

                            //Loop through each ProfileData.
                            foreach(ProfileData profileData in sortData.SortItems)
                            {
                                String sortPropertyValue = Utilities.PropertyToString(sortParam.SortProperty, profileData);

                                //If all sorting groups are completed, add a new sorting group.
                                //(no need to search for insert position). 
                                if (numGroupsSorted == sortParam.SortData.Count)
                                {
                                    sortParam.CreateSortGroup(sortPropertyValue, profileData);
                                }
                                else  
                                {
                                    //Check if there is already a group with the same key.
                                    SortData sData = sortParam.GetSortGroup(sortPropertyValue);
                                    
                                    if (sData != null)
                                    {
                                        //The group exists, so add the data to it.
                                        sData.SortItems.Add(profileData);
                                    }
                                    else
                                    {
                                        sortParam.InsertSortGroup(sortPropertyValue, profileData, numGroupsSorted);
                                    }
                                }
                            }
                        }

                        if (i < sortParams.Length - 1)  //If not the last sort step...
                        {
                            //Copy sorted groups into initial data for next sort loop. 
                            iniData = new List<SortData>();
                            foreach (SortData sData in sortParam.SortData)
                            {
                                iniData.Add(new SortData(sData.SortKey, sData.SortItems));
                            }
                        }
                        else  //Copy final sorted items to result collection.
                        {
                            foreach (SortData sData in sortParam.SortData)
                                sortedData.AddRange(sData.SortItems);
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Either not enough parameters or valid values have been provided.");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unexpected error found while computing sort keys:{ex.Message}");
            }

            return sortedData;
        }

        #endregion


        #region OVERRIDES

        public override string ToString()
        {
            return String.Format("{0:20} {1:20} {2:10} {3:MM/dd/yyyy} {4:15}", this.LastName, this.FirstName, this.Gender.ToString(), this.DateOfBirth, this.FavoriteColor);
        }

        #endregion
    }
}
