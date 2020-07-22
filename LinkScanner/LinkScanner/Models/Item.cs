using System;

namespace LinkScanner.Models
{
    /// <summary>
    /// Class which represents an Item
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Path where the image is stored
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// URL which is scanned from image
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Creation time of the item
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// ID of each item
        /// </summary>
        public string Id { get; set; }
    }
}
