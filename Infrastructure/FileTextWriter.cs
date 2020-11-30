using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheIdeaCompiler.Infrastructure
{

    /// <summary>
    /// This class inherits from TextWriter and is 
    /// used to output sorted results to a text file.
    /// </summary>
    public class FileTextWriter : TextWriter
    {

        #region PRIVATE FIELDS

        //Full path to file where sorted results will be writen.
        private String _fileName = String.Empty;

        //List of text lines to be writen to file.
        private List<String> _lines;


        #endregion


        #region CONSTRUCTORS

        public FileTextWriter(string fileName)
        {
            //Creates full file path.
            _fileName = Path.Combine(AppContext.BaseDirectory, fileName);

            //Delete output file (if exists).
            if(File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }

            //Initializes text line collection.
            _lines = new List<string>();
        }


        #endregion


        #region OVERRIDES


        public override Encoding Encoding => Encoding.Default;



        /// <summary>
        /// This method's call writes all pending text lines to
        /// the output file.
        /// </summary>
        public override void Flush()
        {
            File.WriteAllLines(_fileName, _lines.ToArray());
        }




        public override void WriteLine(string value)
        {
            _lines.Add(value);
        }


        #endregion
    }
}
