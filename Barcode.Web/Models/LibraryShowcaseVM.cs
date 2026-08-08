using System.Collections.Generic;

namespace Barcode.Web.Models
{
    public class LibraryShowcaseVM
    {
        public int SymbologyCount { get; set; }
        public List<string> Symbologies { get; set; } = new List<string>();
    }
}
