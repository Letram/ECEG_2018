using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using ECEG_Migration.Models;
using System.Data;

namespace ECEG_Migration
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet yearSet = DbManager.GetAllYears();
            dropdown_year.DataSource = yearSet.Tables[0];
            dropdown_year.DataValueField = "YearP";
            dropdown_year.DataTextField = "YearP";
            dropdown_year.DataBind();
        }

        protected void table_AllGrammars_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    e.Row.Cells[3].Text = Truncate(e.Row.Cells[3].Text, 100);
                }
                catch
                {
                    // No hacemos nada
                }
            }
        }

        private string Truncate(string text, int length)
        {
            if (text.Length <= length) return text;
            return text.Substring(0, length) + "...";
        }
        
        protected void table_AllGrammars_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Select") return;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int grammarIndex = Convert.ToInt32(table_AllGrammars.Rows[rowIndex].Cells[0].Text);
            Response.Redirect("grammar.aspx?grammar=" + grammarIndex.ToString());
        }
        
    }
}