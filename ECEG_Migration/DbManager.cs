using ECEG_Migration.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ECEG_Migration
{
    public class DbManager
    {
        private static readonly String connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\ECEG_2018.mdb;";

        public static Author GetAuthorDataFromGrammar(string grammarId)
        {
            Author res = new Author();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                //String queryString = "SELECT * FROM (authors INNER JOIN Grammars ON Grammars.Author_id = authors.author_id) WHERE Grammars.Grammar = " + grammarId;
                String queryString = "select a.*, City as city_name, County as county_name, Country as country_name from authors a, Cities c, Counties co, Country cr, Grammars where a.author_id = Grammars.Author_Id and Grammars.Grammar = " + grammarId + " and c.City_id = a.city_id and co.County_Id = a.county_id and cr.Country_Id = a.country_id";
                connection.Open();
                OleDbCommand command = new OleDbCommand(queryString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //get direct data
                    res.Author_id = Convert.ToInt32(reader["author_id"]);
                    res.Name = reader["author_name"].ToString();
                    res.Gender = reader["gender_info"].ToString();
                    res.City_name = reader["city_name"].ToString();
                    res.City_id = Convert.ToInt32(reader["city_id"]);
                    res.County_name = reader["county_name"].ToString();
                    res.County_id = Convert.ToInt32(reader["county_id"]);
                    res.Country_name = reader["country_name"].ToString();
                    res.Country_id = Convert.ToInt32(reader["country_id"]);
                    res.Biographical_details = reader["bio"].ToString();

                    ArrayList author_occupations = new ArrayList();
                    //get related data for occupation
                    OleDbCommand occupationCommand = new OleDbCommand("SELECT * FROM(authors a INNER JOIN authors_occupation ao ON a.author_id = ao.author_id) INNER JOIN occupation_categories oc ON ao.occ_id = oc.occ_id WHERE a.author_id = " + res.Author_id, connection);
                    OleDbDataReader occupationReader = occupationCommand.ExecuteReader();

                    while (occupationReader.Read())
                    {
                        Occupation newOcc = new Occupation();

                        newOcc.Topic_name = occupationReader["description"].ToString();
                        newOcc.Topic_id = Convert.ToInt32(occupationReader["ao.occ_id"]);
                        newOcc.Details = occupationReader["occ_text"].ToString();

                        author_occupations.Add(newOcc);
                    }
                    occupationReader.Close();
                    res.Occupations = (Occupation[]) author_occupations.ToArray(typeof(Occupation));
                }
                reader.Close();
            }
            return res;
        }

        public static Grammar[] GetAllGrammars_v2()
        {
            ArrayList grammars = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAllGrammars_incomplete", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                while (reader.Read())
                {
                    //Tengo que crear un autor y un libro de gramatica para irlos rellenando poco a poco.

                    Author grammarAuthor = new Author();
                    /**
                     * En el caso del autor rellenamos todos los campos menos:
                     *  - city_name -> Se puede rellenar luego teniendo la lista con las ciudades
                     *  - county_name -> Se puede rellenar luego teniendo la lista con las provincias
                     *  - country_name -> Se puede rellenar luego teniendo la lista con los paises
                     *  
                     *  - occupations -> Se puede rellenar luego teniendo una lista con todos los oficios de los autores
                     * */

                    grammarAuthor.Author_id = Convert.ToInt32(reader["authors.Author_Id"]);
                    grammarAuthor.Name = reader["author_name"].ToString();
                    grammarAuthor.Gender = reader["authors.gender_info"].ToString();
                    grammarAuthor.Country_id = Convert.ToInt32(reader["authors.country_id"]);
                    grammarAuthor.County_id = Convert.ToInt32(reader["authors.county_id"]);
                    grammarAuthor.City_id = Convert.ToInt32(reader["authors.city_id"]);
                    grammarAuthor.Biographical_details = reader["bio"].ToString();

                    Grammar grammar = new Grammar();
                    /**
                     * En el caso del libro de gramática rellenamos todos los campos menos:
                     *  # GrammarImprint
                     *      - city_name -> Se puede rellenar luego teniendo la lista con las ciudades
                     *      - county_name -> Se puede rellenar luego teniendo la lista con las provincias
                     *      - country_name -> Se puede rellenar luego teniendo la lista con los paises
                     *  
                     *  - GrammarReferences -> Se puede rellenar luego teniendo una lista con todas las referencias de los libros
                     *  - GrammarLibraries -> Se puede rellenar luego teniendo una lista con las bibliotecas que tienen los libros
                     *  - GrammarSubsidiaryContents -> Se puede rellenar luego teniendo una lista con los contenidos que tienen los libros
                     *  
                     * */

                    grammar.GrammarId = Convert.ToInt32(reader["Grammar"]);
                    grammar.GrammarPublicationYear = reader["YearP"].ToString();
                    grammar.GrammarTitle = reader["Title"].ToString();
                    grammar.GrammarAuthor = grammarAuthor;
                    grammar.GrammarFirstEdition = Convert.ToInt32(reader["Edition"]);

                    Imprint grammarImprint = new Imprint();
                    grammarImprint.Grammar_id = grammar.GrammarId;
                    grammarImprint.Country_id = Convert.ToInt32(reader["Grammars.country_id"]);
                    grammarImprint.County_id = Convert.ToInt32(reader["Grammars.county_id"]);
                    grammarImprint.City_id = Convert.ToInt32(reader["Grammars.city_id"]);
                    grammarImprint.Printers = reader["Printers"].ToString();
                    grammarImprint.Printers = reader["BookSellers"].ToString();
                    grammarImprint.Printers = reader["Price"].ToString();
                    grammarImprint.Printers = reader["Physical_Description"].ToString();

                    grammar.GrammarImprint = grammarImprint;

                    TypeOfWork grammarToW = new TypeOfWork();
                    grammarToW.Code = reader["Grammars.Type_Work"].ToString();
                    grammarToW.Type_description = reader["Description"].ToString();

                    grammar.GrammarTypeOfWork = grammarToW;

                    GrammarDivision grammarDivision = new GrammarDivision();
                    grammarDivision.Category_id = Convert.ToInt32(reader["Group"]);
                    grammarDivision.Category_name = reader["Division"].ToString();

                    grammar.GrammarDivision = grammarDivision;

                    TargetAudience grammarAgeAudience = new TargetAudience();
                    grammarAgeAudience.AudienceCriteria = Convert.ToInt32(reader["age_id"]);
                    grammarAgeAudience.AudienceName = reader["age_info"].ToString();

                    TargetAudience grammarGenderAudience = new TargetAudience();
                    grammarAgeAudience.AudienceCriteria = Convert.ToInt32(reader["gender_id"]);
                    grammarAgeAudience.AudienceName = reader["audience_genders.gender_info"].ToString();

                    TargetAudience grammarInstructionAudience = new TargetAudience();
                    grammarAgeAudience.AudienceCriteria = Convert.ToInt32(reader["instruction_id"]);
                    grammarAgeAudience.AudienceName = reader["instruction_info"].ToString();

                    TargetAudience grammarPurposeAudience = new TargetAudience();
                    grammarAgeAudience.AudienceCriteria = Convert.ToInt32(reader["purpose_id"]);
                    grammarAgeAudience.AudienceName = reader["purpose_info"].ToString();

                    grammar.GrammarTargetAge = grammarAgeAudience;
                    grammar.GrammarTargetGender = grammarGenderAudience;
                    grammar.GrammarTargetInstruction = grammarInstructionAudience;
                    grammar.GrammarTargetSP = grammarPurposeAudience;

                    grammar.GrammarCommments = reader["Comments"].ToString();

                    grammars.Add(grammar);
                }

            }

            return (Grammar[])grammars.ToArray(typeof(Grammar));
        }

        public static Occupation[] GetAllAuthorOccupations()
        {
            ArrayList occupations = new ArrayList();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAllAuthorOccupations", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                while (reader.Read())
                {
                    Occupation occupation = new Occupation();
                    occupation.Author_id = Convert.ToInt32(reader["author_id"]);
                    occupation.Topic_name = reader["description"].ToString();
                    occupation.Topic_id = Convert.ToInt32(reader["occ_id"]);
                    occupation.Details = reader["occ_text"].ToString();

                    occupations.Add(occupation);
                }
            }

            return (Occupation[])occupations.ToArray(typeof(Occupation));
        }

        public static Reference[] GetAllGrammarReferences()
        {
            ArrayList references = new ArrayList();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAllGrammarReferences", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                while (reader.Read())
                {
                    Reference reference = new Reference();
                    reference.Grammar_id = Convert.ToInt32(reader["Grammar"]);
                    reference.Group = Convert.ToInt32(reader["group_id"]);
                    reference.Reference_id = Convert.ToInt32(reader["Also_In"]);
                    reference.Description = reader["description"].ToString();

                    references.Add(reference);
                }
            }

            return (Reference[])references.ToArray(typeof(Reference));
        }

        public static Library[] GetAllGrammarHoldingLibraries()
        {
            ArrayList libraries = new ArrayList();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAllGrammarHoldingLibraries", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                while (reader.Read())
                {
                    Library library = new Library();
                    library.Grammar_id = Convert.ToInt32(reader["Grammar"]);
                    library.Code = reader["Library"].ToString();
                    library.Library_name = reader["Description"].ToString();

                    libraries.Add(library);
                }
            }

            return (Library[])libraries.ToArray(typeof(Library));
        }

        public static SubsidiaryContent[] GetAllGrammarSubsidiaryContents()
        {
            ArrayList subContents = new ArrayList();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAllGrammarSubsidiaryContents", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                while (reader.Read())
                {
                    SubsidiaryContent subContent = new SubsidiaryContent();
                    subContent.Grammar_id = Convert.ToInt32(reader["Grammar"]);
                    subContent.Sub_content_id = Convert.ToInt32(reader["content_id"]);
                    subContent.Sub_content_name = reader["Sub_contents"].ToString();

                    subContents.Add(subContent);
                }
            }

            return (SubsidiaryContent[])subContents.ToArray(typeof(SubsidiaryContent));
        }

        public static Edition[] GetAllGrammarEditions()
        {
            ArrayList editions = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAllGrammarEditions", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                while (reader.Read())
                {
                    Edition edition = new Edition();
                    edition.Edition_year = reader["Edition_Year"].ToString();
                    edition.Description = reader["description"] == DBNull.Value ? "n/a" : reader["description"].ToString();
                    edition.Printing_place = reader["printing_place"] == DBNull.Value ? "unknown" : reader["printing_place"].ToString();
                    edition.Grammar_id = Convert.ToInt32(reader["Grammar"]);
                    edition.Edition_number = reader["edition_number"] == DBNull.Value ? -1 : Convert.ToInt32(reader["edition_number"]);

                    editions.Add(edition);
                }
            }

            return (Edition[])editions.ToArray(typeof(Edition));
        }
        internal static ArrayList GetAllGrammars()
        {
            ArrayList grammars = new ArrayList();

            ArrayList ids = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammar FROM Grammars", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    ids.Add(reader["Grammar"].ToString());
                }
            }

            foreach (String id in ids)
            {
                Debug.WriteLine(id);
                grammars.Add(new Grammar(id));
            }

            return grammars;

        }

        internal static (string comments, string year, string title) getBasicInfoFromGrammar(string grammarId)
        {
            string comments = "", year = "", title = "";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT YearP, Title, Comments FROM Grammars WHERE Grammar = ?", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();

                if (reader.Read())
                {
                    comments = reader["Comments"].ToString();
                    year = reader["YearP"].ToString();
                    title = reader["Title"].ToString();
                }
            }
            return (comments, year, title);
        }

        internal static ArrayList GetAllGrammarsLite()
        {
            ArrayList grammars = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammar, YearP, Edition, Title FROM Grammars", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Grammar grammarAux = new Grammar();
                    grammarAux.GrammarId = Convert.ToInt32(reader["Grammar"]);
                    grammarAux.GrammarPublicationYear = reader["YearP"].ToString();
                    grammarAux.GrammarTitle = reader["Title"].ToString();

                    grammars.Add(grammarAux);
                }
            }

            return grammars;

        }
        public static Boolean UpdateGrammarWithPoPInfo(int[] pop)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("UPDATE Grammars SET city_id = ?, county_id = ?, country_id = ? WHERE Grammar = ?", connection);

                var city_param = query.CreateParameter();
                city_param.Value = pop[3].ToString();
                var county_param = query.CreateParameter();
                county_param.Value = pop[2].ToString();
                var country_param = query.CreateParameter();
                country_param.Value = pop[1].ToString();
                var grammar_param = query.CreateParameter();
                grammar_param.Value = pop[0].ToString();

                query.Parameters.Add(city_param);
                query.Parameters.Add(county_param);
                query.Parameters.Add(country_param);
                query.Parameters.Add(grammar_param);

                return query.ExecuteNonQuery() == 1;
            }
        }

        public static string getCommentsFromGrammar(string grammarId)
        {
            string res = "";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Comments FROM Grammars WHERE Grammar = ?", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();

                if (reader.Read())
                {
                    res = reader["Comments"].ToString();
                }
            }

            return res;
        }

        public static City[] GetAllCities()
        {
            ArrayList cityArr = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand query = new OleDbCommand("SELECT * FROM Cities", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    City newCity = new City();
                    newCity.City_id = Convert.ToInt32(reader["City_id"]);
                    newCity.City_name = reader["City"].ToString();
                    newCity.County_id = Convert.ToInt32(reader["County_Id"]);
                    cityArr.Add(newCity);
                }
            }

            return (City[])cityArr.ToArray(typeof(City));
        }

        public static County[] GetAllCounties()
        {
            ArrayList county_arr = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand query = new OleDbCommand("SELECT * FROM Counties", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    County county = new County();
                    county.County_id = Convert.ToInt32(reader["County_Id"]);
                    county.County_name = reader["County"].ToString();
                    county.Country_id = Convert.ToInt32(reader["Country_Id"]);
                    county_arr.Add(county);
                }
            }

            return (County[])county_arr.ToArray(typeof(County));
        }

        public static Country[] GetAllCountries()
        {
            ArrayList cityArr = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand query = new OleDbCommand("SELECT * FROM Country", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Country newCity = new Country();
                    newCity.Country_id = Convert.ToInt32(reader["Country_Id"]);
                    newCity.Country_name = reader["Country"].ToString();
                    cityArr.Add(newCity);
                }
            }

            return (Country[])cityArr.ToArray(typeof(Country));
        }

        public static int[,] GetAllPoPs()
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                int popRows = (int) new OleDbCommand("SELECT COUNT(*) FROM Grammars_Pop", connection).ExecuteScalar();

                int[,] popArr = new int[popRows,4];
                int index = 0;
                OleDbCommand query = new OleDbCommand("SELECT Grammar, PoP FROM Grammars_Pop", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    popArr[index, 0] = Convert.ToInt32(reader["Grammar"]);
                    string[] pop = reader["PoP"].ToString().Split(';');
                    popArr[index, 1] = Convert.ToInt32(pop[0]);
                    popArr[index, 2] = Convert.ToInt32(pop[1]);
                    popArr[index, 3] = Convert.ToInt32(pop[2]);
                    index++;
                }
                return popArr;
            }
        }

        public static Imprint GetImprintDataFromGrammar(string grammarId)
        {
            Imprint res = new Imprint();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                
                OleDbCommand query = new OleDbCommand("SELECT Grammars.*, City as city_name, County as county_name, Country as country_name FROM Grammars, Cities c, Counties co, Country cr WHERE Grammar = ? and Grammars.city_id = c.City_id and Grammars.county_id = co.County_Id and Grammars.country_id = cr.Country_Id", connection);
                var queryParam = query.CreateParameter();
                queryParam.Value = grammarId.ToString();
                query.Parameters.Add(queryParam);

                OleDbDataReader reader = query.ExecuteReader();
                if (reader.Read())
                {
                    res.Grammar_id = Convert.ToInt32(grammarId);
                    res.City_name = reader["city_name"].ToString();
                    res.County_name = reader["county_name"].ToString();
                    res.Country_name = reader["country_name"].ToString();
                    res.City_id = Convert.ToInt32(reader["city_id"]);
                    res.County_id = Convert.ToInt32(reader["county_id"]);
                    res.Country_id = Convert.ToInt32(reader["country_id"]);
                    res.Printers = reader["Printers"].ToString();
                    res.Booksellers = reader["Booksellers"].ToString();
                    res.Price = reader["Price"].ToString();
                    res.Description = reader["Physical_Description"].ToString();
                }
            }

            return res;
        }

        public static Reference[] GetReferenceDataFromGrammar(string grammarId)
        {
            ArrayList references = new ArrayList();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammars.Grammar, Also_In.Also_In as ref_id, Desciption_Also_In as description, [Agrupación] as group_id FROM Grammars INNER JOIN(Grammars_Also_In INNER JOIN Also_In ON Grammars_Also_In.Also_In = Also_In.Also_In) ON Grammars.Grammar = Grammars_Also_In.Grammar WHERE Grammars.Grammar = ?", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Reference newRef = new Reference();

                    newRef.Reference_id = Convert.ToInt32(reader["ref_id"]);
                    newRef.Group = Convert.ToInt32(reader["group_id"]);
                    newRef.Description = reader["description"].ToString();

                    references.Add(newRef);
                }
            }
            return (Reference[])references.ToArray(typeof(Reference));
        }

        public static Library[] GetHoldingLibrariesFromGrammar(string grammarId)
        {
            ArrayList libraries = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammars.Grammar, hl.Description as lib_desc, hl.Library as code FROM Grammars INNER JOIN( Grammars_Holding_Libraries ghl INNER JOIN Holding_Libraries hl ON ghl.Library = hl.Library ) ON Grammars.Grammar = ghl.Grammar WHERE Grammars.Grammar = ? ", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    Library newLib = new Library();

                    newLib.Code = reader["code"].ToString();
                    newLib.Library_name = reader["lib_desc"].ToString();

                    libraries.Add(newLib);
                }
            }

            return (Library[])libraries.ToArray(typeof(Library));
        }

        public static TypeOfWork GetTypeOfWorkFromGrammar(string grammarId)
        {
            TypeOfWork res = new TypeOfWork();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammar, Description, tw.Type_Work as work_id FROM Grammars g INNER JOIN Type_Work tw ON tw.Type_Work = g.Type_Work WHERE g.Grammar = ?", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();
                if (reader.Read())
                {
                    res.Code = reader["work_id"].ToString();
                    res.Type_description = reader["Description"].ToString();
                }
            }
            return res;
        }

        public static GrammarDivision GetGrammaticalCategoryFromGrammar(string grammarId)
        {
            GrammarDivision res = new GrammarDivision();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammar, Division, group as cat_id FROM Grammars g INNER JOIN Divisions_Grammar dg ON dg.group = g.Division_Grammar WHERE g.Grammar = ?", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();

                if (reader.Read())
                {
                    res.Category_id = Convert.ToInt32(reader["cat_id"]);
                    res.Category_name = reader["Division"].ToString();
                }
            }
            return res;
        }

        public static SubsidiaryContent[] GetSubsidiaryContentsFromGrammar(string grammarId)
        {
            ArrayList subsidiaryContents = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand query = new OleDbCommand("SELECT Grammars.Grammar, sc.Sub_contents, Sub_Contents_Id as sub_id FROM Grammars INNER JOIN (Grammar_Subsidiary_Contents gsc INNER JOIN Subsidiary_Contents sc ON gsc.Sub_contents = sc.Sub_contents_Id) ON Grammars.Grammar = gsc.Grammar WHERE Grammars.Grammar = ?", connection);
                OleDbParameter param_grammar = query.CreateParameter();
                param_grammar.Value = grammarId;
                query.Parameters.Add(param_grammar);

                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    SubsidiaryContent newSC = new SubsidiaryContent();

                    newSC.Sub_content_id = Convert.ToInt32(reader["sub_id"]);
                    newSC.Sub_content_name = reader["Sub_contents"].ToString();

                    subsidiaryContents.Add(newSC);
                }
            }

            return (SubsidiaryContent[])subsidiaryContents.ToArray(typeof(SubsidiaryContent));
        }

        public static (TargetAudience tAge, TargetAudience tGender, TargetAudience tInstruction, TargetAudience tSP) GetAudienceCriteriasFromGrammar(string grammarId)
        {
            TargetAudience age = new TargetAudience();
            TargetAudience gender = new TargetAudience();
            TargetAudience instruction = new TargetAudience();
            TargetAudience sp = new TargetAudience();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                OleDbCommand storedProcedure = new OleDbCommand("GetAudienceCriteriaFromId", connection);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                storedProcedure.Parameters.AddWithValue("@grammarId", grammarId);

                OleDbDataReader reader = storedProcedure.ExecuteReader();

                if (reader.Read())
                {
                    age.AudienceCriteria = Convert.ToInt32(reader["t_age_id"]);
                    age.AudienceName = reader["t_age"].ToString();

                    gender.AudienceCriteria = Convert.ToInt32(reader["t_gender_id"]);
                    gender.AudienceName = reader["t_gender"].ToString();

                    instruction.AudienceCriteria = Convert.ToInt32(reader["t_instruction_id"]);
                    instruction.AudienceName = reader["t_instruction"].ToString();

                    sp.AudienceCriteria = Convert.ToInt32(reader["t_sp_id"]);
                    sp.AudienceName = reader["t_sp"].ToString();
                }
            }
            return (age, gender, instruction, sp);
        }

        public static DataSet GetAllYears()
        {
            DataSet yearSet = new DataSet();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                //Open Database Connection
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT DISTINCT YearP FROM Grammars ORDER BY YearP", con);

                //Fill the DataSet
                da.Fill(yearSet);
            }
            return yearSet;
        }

        
    }
}