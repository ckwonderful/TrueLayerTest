using System;
using System.Collections.Generic;
using System.Text;

namespace TrueLayer.Model
{
    public class TranslateResponse
    {
        public Contents contents { get; set; }
    }


    public class Contents
    {
        public string translated { get; set; }

    }
}
