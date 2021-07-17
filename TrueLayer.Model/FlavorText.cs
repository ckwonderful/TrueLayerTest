using System;
using System.Collections.Generic;
using System.Text;

namespace TrueLayer.Model
{
    public class FlavorText
    {
        public Language language { get; set; }
        public string flavor_text { get; set; }
    }

    public class Language
    {        
        public string name { get; set; }

    }
}
