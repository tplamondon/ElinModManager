using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElinModManager.Models
{
    // contents from each mods package.xml
    public class Mod
    {
        /// <summary>
        /// Directory where mod data is
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// Title of the mod from it's package.xml title attribute
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Id of the mod from package.xml id attribute
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Author of the mod from package.xml author attribute
        /// </summary>
        public string? Author { get; set; }
        /// <summary>
        /// Load priority of mod from its package.xml loadPriority attribute
        /// </summary>
        public int? LoadPriority { get; set; }
        /// <summary>
        /// Decription of mod from its package.xml decription attribute. May not exist
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Tags of mod from package.xml tags attribute. Sometimes it's listed under a tag attribute instead
        /// </summary>
        public List<string>? Tags { get; set; }
        /// <summary>
        /// Version of mod from package.xml version attribute
        /// </summary>
        public string? Version { get; set; }
        
    }
}
