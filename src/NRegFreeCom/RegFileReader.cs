using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NRegFreeCom
{



    /// <summary>
    ///  Reads the given reg file and stores the content as  a Dictionary of registry keys and values as a Dictionary of registry values <see cref="RegValueObject"/>
    /// </summary>
    public class RegFileReader
    {
        private Encoding _encoding = System.Text.Encoding.UTF8;

        /// Raw content of the reg file
        private string _content;

        /// the dictionary containing parsed registry values
        private Dictionary<String, Dictionary<string, RegValueObject>> _regvalues = new Dictionary<string, Dictionary<string, RegValueObject>>();

        /// <summary>
        /// Gets the dictionary containing all entries
        /// </summary>
        public Dictionary<String, Dictionary<string, RegValueObject>> RegValues
        {
            get { return _regvalues; }
        }

        /// <summary>
        /// Gets encoding schema of the reg file (UTF8 or Default). Encoding of the reg file (Regedit 4 - ANSI; Regedit 5 - UTF8)
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
        }



        public RegFileReader(Stream regFile)
        {
            _regvalues = new Dictionary<String, Dictionary<string, RegValueObject>>();
            Read(regFile);
        }

        /// <summary>
        /// Imports the reg file
        /// </summary>
        public void Read(Stream regFile)
        {
            Dictionary<String, Dictionary<String, String>> normalizedContent = null;

            using (var reader = new StreamReader(regFile, System.Text.Encoding.UTF8))
            {
                _content = reader.ReadToEnd();
            }
            _encoding = getEncoding();
            try
            {
                normalizedContent = parseFile();
            }
            catch (Exception ex)
            {
                throw new RegFileParsingException("Error reading reg file.", ex);
            }

            if (normalizedContent == null)
                throw new Exception("Error normalizing reg file content.");

            foreach (KeyValuePair<String, Dictionary<String, String>> entry in normalizedContent)
            {
                var regValueList = new Dictionary<string, RegValueObject>();

                foreach (KeyValuePair<String, String> item in entry.Value)
                {
                    try
                    {
                        regValueList.Add(item.Key, new RegValueObject(entry.Key, item.Key, item.Value, this.Encoding));
                    }
                    catch (Exception ex)
                    {
                        throw new RegFileParsingException(String.Format("Exception thrown on processing string {0}", item), ex);
                    }
                }
                _regvalues.Add(entry.Key, regValueList);
            }


        }

        /// <summary>
        /// Parses the reg file for reg keys and reg values
        /// </summary>
        /// <returns>A Dictionary with reg keys as Dictionary keys and a Dictionary of (valuename, valuedata)</returns>
        private Dictionary<String, Dictionary<String, String>> parseFile()
        {
            var retValue = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                //Get registry keys and values content string
                Dictionary<String, String> dictKeys = normalizeDictionary("^[\t ]*\\[.+\\]\r\n", _content, true);

                //Get registry values for a given key
                foreach (KeyValuePair<String, String> item in dictKeys)
                {
                    Dictionary<String, String> dictValues = normalizeDictionary("^[\t ]*(\".+\"|@)=", item.Value, false);
                    retValue.Add(item.Key, dictValues);
                }
            }
            catch (Exception ex)
            {
                throw new RegFileParsingException("Exception thrown on parsing reg file.", ex);
            }
            return retValue;
        }

        /// <summary>
        /// Creates a flat Dictionary using given searcn pattern
        /// </summary>
        /// <param name="searchPattern">The search pattern</param>
        /// <param name="content">The content string to be parsed</param>
        /// <param name="stripeBraces">Flag for striping braces (true for reg keys, false for reg values)</param>
        /// <returns>A Dictionary with retrieved keys and remaining content</returns>
        private Dictionary<String, String> normalizeDictionary(String searchPattern, String content, bool stripeBraces)
        {
            MatchCollection matches = Regex.Matches(content, searchPattern, RegexOptions.Multiline);

            int startIndex = 0;
            int lengthIndex = 0;
            var dictKeys = new Dictionary<string, string>();

            foreach (Match match in matches)
            {
                try
                {
                    //Retrieve key
                    String sKey = match.Value;
                    if (sKey.EndsWith("\r\n")) sKey = sKey.Substring(0, sKey.Length - 2);
                    if (sKey.EndsWith("=")) sKey = sKey.Substring(0, sKey.Length - 1);
                    if (stripeBraces) sKey = this.stripeBraces(sKey);
                    if (sKey == "@")
                        sKey = "";
                    else
                        sKey = stripeLeadingChars(sKey, "\"");

                    //Retrieve value
                    startIndex = match.Index + match.Length;
                    Match nextMatch = match.NextMatch();
                    lengthIndex = ((nextMatch.Success) ? nextMatch.Index : content.Length) - startIndex;
                    String sValue = content.Substring(startIndex, lengthIndex);
                    //Removing the ending CR
                    if (sValue.EndsWith("\r\n")) sValue = sValue.Substring(0, sValue.Length - 2);
                    dictKeys.Add(sKey, sValue);
                }
                catch (Exception ex)
                {
                    throw new RegFileParsingException(String.Format("Exception thrown on processing string {0}", match.Value), ex);
                }
            }
            return dictKeys;
        }

        /// <summary>
        /// Removes the leading and ending characters from the given string
        /// </summary>
        /// <param name="sLine">given string</param>
        /// <returns>edited string</returns>
        /// <remarks></remarks>
        private string stripeLeadingChars(string sLine, string leadChar)
        {
            string tmpvalue = sLine.Trim();
            if (tmpvalue.StartsWith(leadChar) & tmpvalue.EndsWith(leadChar))
            {
                return tmpvalue.Substring(1, tmpvalue.Length - 2);
            }
            return tmpvalue;
        }

        /// <summary>
        /// Removes the leading and ending parenthesis from the given string
        /// </summary>
        /// <param name="sLine">given string</param>
        /// <returns>edited string</returns>
        /// <remarks></remarks>
        private string stripeBraces(string sLine)
        {
            string tmpvalue = sLine.Trim();
            if (tmpvalue.StartsWith("[") & tmpvalue.EndsWith("]"))
            {
                return tmpvalue.Substring(1, tmpvalue.Length - 2);
            }
            return tmpvalue;
        }

        /// <summary>
        /// Retrieves the ecoding of the reg file, checking the word "REGEDIT4"
        /// </summary>
        /// <returns></returns>
        private System.Text.Encoding getEncoding()
        {
            if (Regex.IsMatch(_content, "([ ]*(\r\n)*)REGEDIT4", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                return System.Text.Encoding.ASCII;
            else
                return System.Text.Encoding.UTF8;
        }

    }
}
