using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TheIdeaCompiler.Model;

namespace TheIdeaCompiler.Infrastructure
{
    public static class Utilities
    {



        /// <summary>
        /// This function returns a gender enumerated value
        /// based on the string input parameter.
        /// </summary>
        /// <param name="genderParam">String representing a gender value</param>
        /// <returns>Gender enumerated value</returns>
        public static GenderEnum ParseGender(string genderParam)
        {
            //Set a gender default value.
            GenderEnum result = GenderEnum.Unknown;

            try
            {
                if (String.IsNullOrEmpty(genderParam) == false && genderParam.Length >= 1)
                {
                    //Get first character of input parameter.
                    char genderToSearch = genderParam.ToLower()[0];
                    //Try to find an enumerated name that matches.
                    String genderEnumName = Enum.GetName(typeof(GenderEnum), genderToSearch);

                    //If found, set the return value accordingly.
                    if (!String.IsNullOrEmpty(genderEnumName))
                        result = Enum.Parse<GenderEnum>(genderEnumName);
                }
            }
            catch
            {
                Console.WriteLine($"Error parsing '{genderParam}' to a valid gender value.");
            }

            return result;
        }




        /// <summary>
        /// This function tries to parse a string input value as
        /// a valid datetime value.
        /// </summary>
        /// <param name="dateParam">String input value</param>
        /// <returns>DateTime value</returns>
        public static DateTime ParseDate(string dateParam)
        {
            DateTime result = default(DateTime);

            try
            {
                if (String.IsNullOrEmpty(dateParam) == false && dateParam.Length >= 8)
                {
                    String[] dateParts = dateParam.Split(new char[] { '-', '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (dateParts.Length == 3)
                    {
                        DateTime auxDate = new DateTime(Convert.ToInt32(dateParts[2]), Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]));
                        result = auxDate;
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Error parsing '{dateParam}' to a valid date value.");
            }

            return result;
        }



        /// <summary>
        /// This function searchs files that matches a specific pattern inside
        /// a specific folder and returns a list of strings, one for each file line
        /// of text.
        /// </summary>
        /// <param name="path">Path for search text files</param>
        /// <param name="searchPattern">Pattern for matching files</param>
        /// <returns>List of strings, one for each file line of text</returns>
        public static List<String> GetStringLinesFromFile(string path, string searchPattern)
        {
            List<string> result = new List<string>();

            try
            {
                //Get file names matching specified path and search pattern.
                foreach (String fileName in Directory.GetFiles(path, searchPattern))
                {
                    //Load all lines of text for each file.
                    result.AddRange(File.ReadLines(fileName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error found while loading data:{ex.Message}");
            }

            //Returns all file lines of text.
            return result;
        }


        /// <summary>
        /// This function gets a property info, and a ProfileData object instance then,
        /// using reflection, gets the instance value for the specified property.
        /// Finally returns the value converted to a padded right string.
        /// </summary>
        /// <param name="sortProperty">Info of the property whose value will be returned</param>
        /// <param name="profileData">ProfileData instance object containing the value to be returned.</param>
        /// <returns>The property value's string representation.</returns>
        public static string PropertyToString(PropertyInfo sortProperty, ProfileData profileData)
        {
            String result = String.Empty;

            //If the property is of type DateTime, the string representation will be yyyy-mm-dd for sorting purposes.
            if(sortProperty.PropertyType == typeof(DateTime))
            {
                result = Convert.ToDateTime(sortProperty.GetValue(profileData)).ToString("yyyy-MM-dd");
            }
            else 
            {
                result = sortProperty.GetValue(profileData).ToString();
            }

            return String.Format("{0:30}",result);
        }
    }
}
