using ECEG_Migration.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
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
                String queryString = "SELECT * FROM (authors INNER JOIN Grammars ON Grammars.Author_id = authors.author_id) WHERE Grammars.Grammar = " + grammarId;
                connection.Open();
                OleDbCommand command = new OleDbCommand(queryString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //get direct data
                    res.Author_id = Convert.ToInt32(reader["authors.Author_id"]);
                    res.Name = reader["author_name"].ToString();
                    res.Gender = reader["gender_info"].ToString();
                    res.Gender_id = Convert.ToInt32(reader["gender"]);
                    res.City_name = reader["city_info"].ToString();
                    res.City_id = Convert.ToInt32(reader["PoB_City"]);
                    res.County_name = reader["county_info"].ToString();
                    res.County_id = Convert.ToInt32(reader["PoB_County"]);
                    res.Country_name = reader["country_info"].ToString();
                    res.Country_id = Convert.ToInt32(reader["PoB_Country"]);
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
            ArrayList cityArr = new ArrayList();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand query = new OleDbCommand("SELECT * FROM Counties", connection);
                OleDbDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    County newCity = new County();
                    newCity.County_id = Convert.ToInt32(reader["County_Id"]);
                    newCity.County_name = reader["County"].ToString();
                    newCity.County_id = Convert.ToInt32(reader["Country_Id"]);
                    cityArr.Add(newCity);
                }
            }

            return (County[])cityArr.ToArray(typeof(County));
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

                OleDbCommand query = new OleDbCommand("SELECT * FROM Grammars WHERE Grammar = ?", connection);
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

        public static GrammaticalCategory GetGrammaticalCategoryFromGrammar(string grammarId)
        {
            GrammaticalCategory res = new GrammaticalCategory();

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
    }
}