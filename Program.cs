using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TheIdeaCompiler.Infrastructure;
using TheIdeaCompiler.Model;

namespace TheIdeaCompiler
{
    class Program
    {
        public const string _outputFileName = "output.txt";

        static void Main(string[] args)
        {

            //Get raw text lines from files.
            List<String> rawData = Utilities.GetStringLinesFromFile(Path.Combine(AppContext.BaseDirectory, "Data"), "*.txt");

            //Parse raw lines to a list of 'ProfileData' instance objects.
            List<ProfileData> profileDataList = ProfileData.BulkLoad(rawData);

            //Instantiates and fills the list of parameters needed to sort the list of instance objects and output the sorted result.
            List<OutputParams> outputParamsList = new List<OutputParams>();

            //SORT: Gender ASC, LastName ASC
            outputParamsList.Add(new OutputParams("Gender ASC, LastName ASC").AddSortParam(nameof(ProfileData.Gender), SortEnum.Ascending).AddSortParam(nameof(ProfileData.LastName), SortEnum.Ascending));

            //SORT DateOfBirth ASC
            outputParamsList.Add(new OutputParams("DateOfBirth ASC").AddSortParam(nameof(ProfileData.DateOfBirth), SortEnum.Ascending));

            //SORT LastName DESC
            outputParamsList.Add(new OutputParams("LastName DESC").AddSortParam(nameof(ProfileData.LastName), SortEnum.Descending));


            SortAndOutput(profileDataList, outputParamsList);

            /*
             
            String outputTitle = String.Empty;
                         
            //SORT: Gender ASC, LastName ASC
            outputTitle = "Gender ASC, LastName ASC";
            SortInfo[] sortParams = { new SortInfo(typeof(ProfileData).GetProperty(nameof(ProfileData.Gender)), SortEnum.Ascending),
                new SortInfo(typeof(ProfileData).GetProperty(nameof(ProfileData.LastName)), SortEnum.Ascending) };

            List<ProfileData> sortedData = ProfileData.PerformSorting(profileDataList, sortParams);
            OutputSortedData($"SORT BY ({outputTitle})", sortedData, OutputEnum.Console);




            //SORT DateOfBirth ASC
            outputTitle = "DateOfBirth ASC";
            sortParams = new SortInfo[]{ new SortInfo(typeof(ProfileData).GetProperty(nameof(ProfileData.DateOfBirth)), SortEnum.Ascending) };

            sortedData = ProfileData.PerformSorting(profileDataList, sortParams);
            OutputSortedData($"SORT BY ({outputTitle})", sortedData, OutputEnum.Console);



            //SORT LastName DESC
            outputTitle = "LastName DESC";
            sortParams = new SortInfo[]{ new SortInfo(typeof(ProfileData).GetProperty(nameof(ProfileData.LastName)), SortEnum.Descending) };

            sortedData = ProfileData.PerformSorting(profileDataList, sortParams);
            OutputSortedData($"SORT BY ({outputTitle})", sortedData, OutputEnum.Console);
            */

            Console.Write("Press a key to exit...");
            Console.ReadKey();

        }



        /// <summary>
        /// This method gets a list of items to sort and
        /// a list of sort parameters. Each sort parameter
        /// contains the property name to sort by and the
        /// sort direction to apply.
        /// Once sorted, the resulting collection is outputted
        /// both to Console and text File 'output.txt'.
        /// </summary>
        /// <param name="profileDataList">List of data items to sort.</param>
        /// <param name="outputParamsList">Output title and list of both property name and sort direction.</param>
        private static void SortAndOutput(List<ProfileData> profileDataList, List<OutputParams> outputParamsList)
        {

            List<SortInfo> sortInfoList = null;
            List<ProfileData> sortedData = null;
            
            //Create TextWriter derived class to write results to file.
            FileTextWriter fileTextWriter = new FileTextWriter(_outputFileName);
            
            //Loops through each output the user wants.
            foreach(OutputParams outParam in outputParamsList){

                sortInfoList = new List<SortInfo>();

                //Creates a list of sort parameters (property name and sort direction).
                foreach(SortParams sortParam in outParam.SortParams)
                {
                    sortInfoList.Add(new SortInfo(typeof(ProfileData).GetProperty(sortParam.PropertyName), sortParam.SortDirection));
                }

                //Gets sorted data.
                sortedData = ProfileData.PerformSorting(profileDataList, sortInfoList.ToArray());

                //Outputs sorted data.
                //To console...
                OutputSortedData(outParam.Title, sortedData, Console.Out);
                //To file...
                OutputSortedData(outParam.Title, sortedData, fileTextWriter);

            }

            //Write all lines to file.
            fileTextWriter.Flush();
            
        }


        /// <summary>
        /// This method outputs the sorted results to a
        /// TextWriter instance received as argument.
        /// </summary>
        /// <param name="title">Title for sorted data header.</param>
        /// <param name="sortedData">Sorted data that is outputted</param>
        /// <param name="textWriter">TextWriter instance that outputs sorted data</param>
        static void OutputSortedData(String title, List<ProfileData> sortedData, TextWriter textWriter)
        {
                //Outputs title and headers.
                textWriter.WriteLine(title);
                textWriter.WriteLine(" ");
                textWriter.WriteLine($"{"LAST NAME",-25}{"FIRST NAME",-25}{"GENDER",-15}{"DATE OF BIRTH",-25}{"COLOR",-15}");
                textWriter.WriteLine(new String('-', 100));

                //Output sorted data.
                foreach (ProfileData profileData in sortedData)
                    textWriter.WriteLine($"{profileData.LastName,-25}{profileData.FirstName,-25}{profileData.Gender.ToString(),-15}{profileData.DateOfBirth.ToString("MM/dd/yyyy"),-25}{profileData.FavoriteColor,-15}");

            textWriter.WriteLine(" ");
            textWriter.WriteLine(" ");
        }


    }
}
