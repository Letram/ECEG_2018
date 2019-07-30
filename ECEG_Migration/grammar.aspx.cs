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
        private Grammar g = new Grammar();
        protected void Page_Load(object sender, EventArgs e)
        {
            g = dataInstance.LastSelectedGrammar;
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["grammar"] == null)
                {
                    Response.Redirect("/");
                }


                grammar_title.InnerText = g.GrammarTitle;

                //Debug.WriteLine(new JavaScriptSerializer().Serialize(g));

                Author au = g.GrammarAuthor;

                written_by.InnerHtml = "Written by <b>" + au.Name + "</b>, " + au.Gender;
                author_pob.InnerHtml = "Born in " + String.Format("{0}, {1}, {2}", au.City_name, au.County_name, au.Country_name);

                foreach (Occupation authorOcc in au.Occupations)
                {
                    TableRow row = new TableRow();
                    TableCell topic_cell = new TableCell();
                    TableCell details_cell = new TableCell();

                    topic_cell.Text = "<b>" + authorOcc.Topic_name + "</b>";
                    details_cell.Text = authorOcc.Details;

                    row.Cells.Add(topic_cell);
                    row.Cells.Add(details_cell);
                    table_occupation.Rows.Add(row);
                }

                author_bio.InnerHtml = au.Biographical_details;

                Imprint im = g.GrammarImprint;

                printed_in.InnerText = "Printed in " + im.City_name + ", " + im.County_name + ", " + im.Country_name;
                printed_by.InnerText = im.Printers;
                sold_by.InnerText = im.Booksellers;
                price.InnerText = im.Price;
                imprint_description.InnerText = im.Description;

                Edition[] editions = g.GrammarEditions;

                table_editions.DataSource = editions.OrderBy(ed => ed.Edition_year).ToList();
                table_editions.DataBind();
                
                Reference[] references = g.GrammarReferences;

                table_references.DataSource = references.OrderBy(r => r.Description).ToList();
                table_references.DataBind();

                Library[] libraries = g.GrammarHoldingLibraries;

                table_libraries.DataSource = libraries.OrderBy(lib => lib.Code.ToLower()).Select(lib => new {
                    library_name = string.Format("<span class='badge badge-secondary'>{0}</span> ", lib.Code) + lib.Library_name
                }).ToList();
                table_libraries.DataBind();

                TypeOfWork tow = g.GrammarTypeOfWork;
                GrammarDivision gc = g.GrammarDivision;
                table_work_types.Rows[1].Cells[0].Text = tow.Type_description;
                table_work_types.Rows[1].Cells[1].Text = gc.Category_name;

                SubsidiaryContent[] subsidiaryContents = g.GrammarSubsidiaryContents;

                table_sub_content.DataSource = subsidiaryContents.OrderBy(sub => sub.Sub_content_name).ToList();
                table_sub_content.DataBind();

                var (tAge, tGender, tIns, tSP) = (g.GrammarTargetAge, g.GrammarTargetGender, g.GrammarTargetInstruction, g.GrammarTargetSP);

                table_audience.Rows[1].Cells[0].Text = tAge.AudienceName;
                table_audience.Rows[1].Cells[1].Text = tGender.AudienceName;
                table_audience.Rows[1].Cells[2].Text = tIns.AudienceName;
                table_audience.Rows[1].Cells[3].Text = tSP.AudienceName;

                label_comments.Text = g.GrammarCommments;

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

        protected void table_editions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            table_editions.DataSource = g.GrammarEditions.OrderBy(ed => ed.Edition_year).ToList();
            table_editions.PageIndex = e.NewPageIndex;
            table_editions.DataBind();
        }

        protected void table_editions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    e.Row.Cells[1].Text = e.Row.Cells[1].Text == "-1" ? "n/a" : e.Row.Cells[1].Text == "0" ? e.Row.Cells[1].Text + ". Presumably first" : e.Row.Cells[1].Text;
                }
                catch
                {
                    // No hacemos nada
                }
            }
        }

        protected void table_references_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            table_references.DataSource = g.GrammarReferences.OrderBy(r => r.Description).ToList();
            table_references.PageIndex = e.NewPageIndex;
            table_references.DataBind();
        }

        protected void table_libraries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            table_libraries.DataSource = g.GrammarHoldingLibraries.OrderBy(lib => lib.Code.ToLower()).Select(lib => new {
                library_name = string.Format("<span class='badge badge-secondary'>{0}</span> ", lib.Code) + lib.Library_name
            }).ToList();
            table_libraries.PageIndex = e.NewPageIndex;
            table_libraries.DataBind();
        }

        protected void table_sub_content_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            table_sub_content.DataSource = g.GrammarSubsidiaryContents.OrderBy(sub => sub.Sub_content_name).ToList();
            table_sub_content.PageIndex = e.NewPageIndex;
            table_sub_content.DataBind();
        }
    }
}