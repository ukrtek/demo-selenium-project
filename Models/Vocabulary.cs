using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.App.AutomatedTests.Models
{
    public class Vocabulary 
    {
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }

        public string Selector { get; set; }

        public string LocatorType { get; set; }

        public string SelectorValue 
        { 
            get 
            {
                return string.Format(Selector, Name, Country, DataType);
            }
        }
    }


}