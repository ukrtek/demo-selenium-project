using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.App.AutomatedTests.Models
{
    public class CodeList
    {

        private string _location;

        /// <summary>
        ///
        /// </summary>
        public string Vocabulary {get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name {get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Notes {get; set; }

        /// <summary>
        ///
        /// </summary>
        public Boolean IsGroupsEnabled {get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Location 
        { 
            get { return $"//span[contains(text(), \"{_location}\")]"; }
            set { _location = value; }
        }

        /// <summary>
		/// 
		/// </summary>
		public bool UseExistingCodelist { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public string ExistingCodelistURL { get; set; }     
    }
}