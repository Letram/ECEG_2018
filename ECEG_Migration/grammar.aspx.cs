using ECEG_Migration.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ECEG_Migration
{
    public partial class grammar : System.Web.UI.Page
    {
        private Data dataInstance = Data.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["grammar"] == null)
                {
                    Response.Redirect("/");
                }


                Grammar grammar = dataInstance.LastSelectedGrammar;
                grammar_title.InnerText = grammar.GrammarTitle;

                //Debug.WriteLine(new JavaScriptSerializer().Serialize(grammar));

                Author au = grammar.GrammarAuthor;

                written_by.InnerHtml = "Written by <b>" + au.Name + "</b>, " + au.Gender;
                author_pob.InnerHtml = "Born in " + String.Format("{0}, {1}, {2}", au.City_name, au.County_name, au.Country_name);

                foreach (Occupation authorOcc in au.Occupations)
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

                author_bio.InnerHtml = au.Biographical_details;

                Imprint im = grammar.GrammarImprint;

                printed_in.InnerText = "Printed in " + im.City_name + ", " + im.County_name + ", " + im.Country_name;
                printed_by.InnerText = im.Printers;
                sold_by.InnerText = im.Booksellers;
                price.InnerText = im.Price;
                imprint_description.InnerText = im.Description;

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
                GrammarDivision gc = grammar.GrammarDivision;

                table_work_types.Rows[0].Cells[0].Text = tow.Code;
                table_work_types.Rows[0].Cells[1].Text = tow.Type_description;
                table_work_types.Rows[1].Cells[0].Text = gc.Category_id.ToString();
                table_work_types.Rows[1].Cells[1].Text = gc.Category_name;

                Edition[] editions = grammar.GrammarEditions;

                TableRow editionsHeader = new TableRow();
                editionsHeader.TableSection = TableRowSection.TableHeader;

                TableCell yearCell = new TableCell();
                TableCell editionCell = new TableCell();
                TableCell placeCell = new TableCell();
                TableCell descriptionCell = new TableCell();
                yearCell.Text = "Year";
                editionCell.Text = "Edition";
                placeCell.Text = "Place";
                descriptionCell.Text = "Description";

                editionsHeader.Cells.Add(yearCell);
                editionsHeader.Cells.Add(editionCell);
                editionsHeader.Cells.Add(placeCell);
                editionsHeader.Cells.Add(descriptionCell);

                table_editions.Rows.Add(editionsHeader);
                foreach (Edition ed in editions)
                {
                    TableRow row = new TableRow();
                    row.TableSection = TableRowSection.TableBody;

                    TableCell edition_year_cell = new TableCell();
                    TableCell edition_number_cell = new TableCell();
                    TableCell edition_place_cell = new TableCell();
                    TableCell edition_description_cell = new TableCell();

                    edition_year_cell.Text = ed.Edition_year;
                    edition_number_cell.Text = ed.Edition_number == -1 ? "n/a" : ed.Edition_number == 0 ? ed.Edition_number + ". Presumably first" : ed.Edition_number.ToString();
                    edition_place_cell.Text = ed.Printing_place;
                    edition_description_cell.Text = ed.Description;

                    row.Cells.Add(edition_year_cell);
                    row.Cells.Add(edition_number_cell);
                    row.Cells.Add(edition_place_cell);
                    row.Cells.Add(edition_description_cell);

                    table_editions.Rows.Add(row);
                }
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

                grammar_page_counter.InnerText = (dataInstance.LastSearchResults.IndexOf(dataInstance.LastSelectedGrammar) + 1) + " of " + (dataInstance.LastSearchResults.Count);
                checkPosition(dataInstance.LastSearchResults.IndexOf(dataInstance.LastSelectedGrammar), dataInstance.LastSearchResults);

            }
        }

        private void checkPosition(int v, List<Grammar> arr)
        {
            if (v == 0)
                btn_prev.Visible = false;
            else
                btn_prev.Visible = true;

            if (v == arr.Count - 1)
                btn_forw.Visible = false;
            else
                btn_forw.Visible = true;
        }

        protected void btn_prev_ServerClick(object sender, EventArgs e)
        {
            int currentIndex = dataInstance.LastSearchResults.IndexOf(dataInstance.LastSelectedGrammar);
            if (currentIndex > 0)
                dataInstance.LastSelectedGrammar = dataInstance.LastSearchResults[currentIndex - 1];
            Response.Redirect("grammar?grammar=" + dataInstance.LastSelectedGrammar.GrammarId);
        }

        protected void btn_forw_ServerClick(object sender, EventArgs e)
        {
            int currentIndex = dataInstance.LastSearchResults.IndexOf(dataInstance.LastSelectedGrammar);
            if (currentIndex < dataInstance.LastSearchResults.Count - 1)
                dataInstance.LastSelectedGrammar = dataInstance.LastSearchResults[currentIndex + 1];
            Response.Redirect("grammar?grammar=" + dataInstance.LastSelectedGrammar.GrammarId);
        }
    }
}