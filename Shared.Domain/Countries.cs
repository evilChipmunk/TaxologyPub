using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Domain
{
    public static class Countries
    {
        public static List<Country> los = new List<Country> {
            //us
            new Country("USA", "United States of America"),
            new Country("CAN", "Canada"),
          
        };

        public static List<string> Abbreviations()
        {
            return los.Select(s => s.Abbreviation).ToList();
        }

        public static List<string> Names()
        {
            return los.Select(s => s.Name).ToList();
        }

        public static string GetName(string abbreviation)
        {
            return los.Where(s => s.Abbreviation.Equals(abbreviation, StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Name).FirstOrDefault();
        }

        public static string GetAbbreviation(string name)
        {
            return los.Where(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Abbreviation).FirstOrDefault();
        }

        public static List<Country> ToList()
        {
            return los;
        }

        public static Country GetByAbbreviation(string ab)
        {
            return los.FirstOrDefault(x => x.Abbreviation.Equals(ab));
        }
        public static Country GetByName(string name)
        {
            return los.FirstOrDefault(x => x.Name.Equals(name));
        }
    }
}