using System;
using System.Collections.Generic;
using System.Text;

namespace TrueLayer.Model
{
    public class PokemonSpecies
    {
        public string name;
        public List<FlavorText> flavor_text_entries;
        public Habitat habitat;
        public bool is_legendary;
    }
}
