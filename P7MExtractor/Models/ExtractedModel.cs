using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7MExtractor.Models
{
    public class ExtractedModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extractedObject"></param>
        /// <param name="displayName"></param>
        /// <param name="objType">0 - file, 1 - cert</param>
        public ExtractedModel(object extractedObject, string displayName, int objType)
        {
            ExtractedObject = extractedObject;
            DisplayName = displayName;
            ObjectType = objType;
        }   
        public object ExtractedObject { get; set; }
        public string DisplayName { get; set; }
        public int ObjectType { get; set; }
    }
}
