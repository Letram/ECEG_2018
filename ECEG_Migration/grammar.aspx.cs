﻿using ECEG_Migration.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECEG_Migration
{
    public partial class grammar : System.Web.UI.Page
    {
        readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\ECEG_2018.mdb;";
        String queryString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["grammar_id"] = "";
                Session["grammar_ids"] = "";
                queryString = "";
                String grammarId = "1";
                try
                {
                    Session["grammar_id"] = Request.QueryString["grammar"];
                    grammarId = Session["grammar_id"].ToString();
                }
                catch (Exception ex)
                {
                    Response.Redirect("/");
                }

                Session["grammar_id"] = Request.QueryString["grammar"];
                grammarId = Session["grammar_id"].ToString();

                Grammar grammar = new Grammar(grammarId);

                //Debug.WriteLine(new JavaScriptSerializer().Serialize(grammar));

                Author au = grammar.GrammarAuthor;

                table_author.Rows[0].Cells[0].Text = au.Name;
                table_author.Rows[0].Cells[1].Text = au.Gender;
                table_author.Rows[0].Cells[2].Text = au.Biographical_details;
                table_author.Rows[0].Cells[3].Text = au.City_name;
                table_author.Rows[0].Cells[4].Text = au.County_name;
                table_author.Rows[0].Cells[5].Text = au.Country_name;

                foreach(Occupation authorOcc in au.Occupations)
                {
                    TableRow row = new TableRow();
                    TableCell topic_cell = new TableCell();
                    TableCell details_cell = new TableCell();

                    topic_cell.Text = authorOcc.Topic_name;
                    details_cell.Text = authorOcc.Details;

                    row.Cells.Add(topic_cell);
                    row.Cells.Add(details_cell);
                    table_occupation.Rows.Add(row);
                }

                Imprint im = grammar.GrammarImprint;

                table_imprint.Rows[0].Cells[0].Text = im.City_name;
                table_imprint.Rows[0].Cells[1].Text = im.County_name;
                table_imprint.Rows[0].Cells[2].Text = im.Country_name;
                table_imprint.Rows[0].Cells[3].Text = im.Printers;
                table_imprint.Rows[0].Cells[4].Text = im.Booksellers;
                table_imprint.Rows[0].Cells[5].Text = im.Description;

                Reference[] references = grammar.GrammarReferences;

                foreach (Reference reference in references)
                {
                    TableRow row = new TableRow();
                    TableCell reference_id_cell = new TableCell();
                    TableCell description_cell = new TableCell();

                    reference_id_cell.Text = reference.Reference_id.ToString();
                    description_cell.Text = reference.Description;

                    row.Cells.Add(reference_id_cell);
                    row.Cells.Add(description_cell);
                    table_references.Rows.Add(row);
                }

                Library[] libraries = grammar.GrammarHoldingLibraries;

                foreach (Library lib in libraries)
                {
                    TableRow row = new TableRow();
                    TableCell library_name_cell = new TableCell();
                    TableCell library_code_cell = new TableCell();

                    library_code_cell.Text = lib.Code;
                    library_name_cell.Text = lib.Library_name;

                    row.Cells.Add(library_code_cell);
                    row.Cells.Add(library_name_cell);

                    table_libraries.Rows.Add(row);
                }

                TypeOfWork tow = grammar.GrammarTypeOfWork;
                GrammaticalCategory gc = grammar.GrammarGrammaticalCategory;

                table_work_types.Rows[0].Cells[0].Text = tow.Code;
                table_work_types.Rows[0].Cells[1].Text = tow.Type_description;
                table_work_types.Rows[1].Cells[0].Text = gc.Category_id.ToString();
                table_work_types.Rows[1].Cells[1].Text = gc.Category_name;

                SubsidiaryContent[] subsidiaryContents = grammar.GrammarSubsidiaryContents;

                foreach (SubsidiaryContent content in subsidiaryContents)
                {
                    TableRow row = new TableRow();
                    TableCell sub_content_id = new TableCell();
                    TableCell sub_content_name = new TableCell();

                    sub_content_id.Text = content.Sub_content_id.ToString();
                    sub_content_name.Text = content.Sub_content_name;

                    row.Cells.Add(sub_content_id);
                    row.Cells.Add(sub_content_name);

                    table_sub_content.Rows.Add(row);
                }

                var (tAge, tGender, tIns, tSP) = (grammar.GrammarTargetAge, grammar.GrammarTargetGender, grammar.GrammarTargetInstruction, grammar.GrammarTargetSP);

                table_audience.Rows[0].Cells[0].Text = tAge.AudienceName;
                table_audience.Rows[0].Cells[1].Text = tGender.AudienceName;
                table_audience.Rows[0].Cells[2].Text = tIns.AudienceName;
                table_audience.Rows[0].Cells[3].Text = tSP.AudienceName;

                label_comments.Text = grammar.GrammarCommments;

                using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
                {
                    dbConnection.Open();

                    OleDbCommand query = new OleDbCommand("SELECT grammar FROM grammars ORDER BY grammar ASC", dbConnection);
                    OleDbDataReader reader = query.ExecuteReader();

                    while (reader.Read())
                    {
                        int readIndex = Convert.ToInt32(reader["grammar"].ToString());
                        Session["grammar_ids"] += reader["grammar"].ToString() + ",";
                    }
                    Session["grammar_ids"] = Session["grammar_ids"].ToString().Remove(Session["grammar_ids"].ToString().Length - 1);
                    String[] grammarArr = Session["grammar_ids"].ToString().Split(',');

                    grammar_page_counter.InnerText = (Array.IndexOf(grammarArr, grammarId) + 1) + " of " + grammarArr.Length;

                    checkPosition(Array.IndexOf(grammarArr, grammarId), grammarArr);
                }
            }
        }

        private void checkPosition(int v, String[] arr)
        {
            if(v == 0)
                btn_prev.Visible = false;
            else
                btn_prev.Visible = true;

            if(v == arr.Length - 1)
                btn_forw.Visible = false;
            else
                btn_forw.Visible = true;
        }

        protected void btn_prev_ServerClick(object sender, EventArgs e)
        {
            String[] grammarIdsArr = Session["grammar_ids"].ToString().Split(',');
            String grammarId = Session["grammar_id"].ToString();
            int currentIndex = Array.IndexOf(grammarIdsArr, grammarId);
            if (currentIndex != -1 && currentIndex > 0)
                Response.Redirect("grammar?grammar=" + grammarIdsArr[currentIndex - 1]);
            else
                Response.Redirect("grammar?grammar=" + grammarIdsArr[0]);
        }

        protected void btn_forw_ServerClick(object sender, EventArgs e)
        {
            String[] grammarIdsArr = Session["grammar_ids"].ToString().Split(',');
            String grammarId = Session["grammar_id"].ToString();
            int currentIndex = Array.IndexOf(grammarIdsArr, grammarId);
            if (currentIndex != -1 && currentIndex < grammarIdsArr.Length)
                Response.Redirect("grammar?grammar=" + grammarIdsArr[currentIndex + 1]);
            else
                Response.Redirect("grammar?grammar=" + grammarIdsArr[grammarIdsArr.Length - 1]);
        }
    }
}