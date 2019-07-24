using ECEG_Migration.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration
{
    public class Data
    {
        private static Data INSTANCE = null;

        private List<Grammar> allGrammars = new List<Grammar>();
        private List<Grammar> lastSearchResults = new List<Grammar>();
        private Grammar lastSelectedGrammar = null;
        private Data()
        {
            Grammar[] allGrammarsPartiallyCompleted = DbManager.GetAllGrammars_v2();
            Occupation[] allOccupations = DbManager.GetAllAuthorOccupations();
            Reference[] allReferences = DbManager.GetAllGrammarReferences();
            Library[] allLibraries = DbManager.GetAllGrammarHoldingLibraries();
            SubsidiaryContent[] allSubContents = DbManager.GetAllGrammarSubsidiaryContents();
            City[] allCities = DbManager.GetAllCities();
            County[] allCounties = DbManager.GetAllCounties();
            Country[] allCountries = DbManager.GetAllCountries();
            Edition[] allEditions = DbManager.GetAllGrammarEditions();

            foreach (Grammar grammar in allGrammarsPartiallyCompleted)
            {
                //Rellenamos las listas
                grammar.GrammarAuthor.Occupations = (from Occupation occ in allOccupations where grammar.GrammarAuthor.Author_id == occ.Author_id select occ).ToArray();
                grammar.GrammarReferences = (from Reference reference in allReferences where grammar.GrammarId == reference.Grammar_id select reference).ToArray();
                grammar.GrammarHoldingLibraries = (from Library lib in allLibraries where grammar.GrammarId == lib.Grammar_id select lib).ToArray();
                grammar.GrammarSubsidiaryContents = (from SubsidiaryContent subc in allSubContents where grammar.GrammarId == subc.Grammar_id select subc).ToArray();
                grammar.GrammarEditions = (from Edition ed in allEditions where grammar.GrammarId == ed.Grammar_id select ed).ToArray();

                //Nombramos los paises, ciudades y provincias
                grammar.GrammarAuthor.City_name = (from City city in allCities where grammar.GrammarAuthor.City_id == city.City_id select city.City_name).First();
                grammar.GrammarImprint.City_name = (from City city in allCities where grammar.GrammarImprint.City_id == city.City_id select city.City_name).First();

                grammar.GrammarAuthor.County_name = (from County county in allCounties where grammar.GrammarAuthor.County_id == county.County_id select county.County_name).First();
                grammar.GrammarImprint.County_name = (from County county in allCounties where grammar.GrammarImprint.County_id == county.County_id select county.County_name).First();

                grammar.GrammarAuthor.Country_name = (from Country country in allCountries where grammar.GrammarAuthor.Country_id == country.Country_id select country.Country_name).First();
                grammar.GrammarImprint.Country_name = (from Country country in allCountries where grammar.GrammarImprint.Country_id == country.Country_id select country.Country_name).First();
            }

            //Ahora mismo el array grammars está completo.
            AllGrammars = allGrammarsPartiallyCompleted.ToList();
            LastSearchResults = AllGrammars;
        }

        public List<Grammar> AllGrammars { get => allGrammars; set => allGrammars = value; }
        public List<Grammar> LastSearchResults { get => lastSearchResults; set => lastSearchResults = value; }
        public Grammar LastSelectedGrammar { get => lastSelectedGrammar; set => lastSelectedGrammar = value; }

        public static Data GetInstance()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new Data();
            }

            return INSTANCE;
        }
    }
}