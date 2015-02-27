using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnergLibrary;
using System.Data.SqlClient;

namespace Compare
{
    public partial class DeleteUnit : Form
    {
        public DeleteUnit()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            UnitInfo ui = new UnitInfo(Convert.ToInt32(txtdisplayID.Text));
            if (ui.FOUND)
            {
                DeleteFromTableUnitID("downtime", ui);
                DeleteFromTableUnitID("service_plans", ui);
                DeleteFromTableUnitID("targets", ui);
                DeleteFromTableUnitID("companyGroup_Units", ui);
                DeleteFromTableDisplayID("grouping", "displayid", ui);
                DeleteFromTableDisplayID("capital_values", "display_id", ui);
                DeleteFromTableUnitID("service_history", ui);
                DeleteFromTableUnitID("unitDates", ui);
                DeleteFromTableDisplayID("invoice_mths", "display_id",  ui);
                DeleteFromTableDisplayID("BillingInfo", "display_id", ui);
                DeleteFromTableUnitID("addresses", ui);
                DeleteFromTableUnitID("units", ui);
            }
        }

        private void DeleteFromTableUnitID(string tablename, UnitInfo ui)
        {
            string query = String.Format("Delete from {0} where unit_id = {1}", tablename, ui.UNIT_ID);
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void DeleteFromTableDisplayID(string tablename,string columnname, UnitInfo ui)
        {
            string query = String.Format("Delete from {0} where {1} = {2}", tablename, columnname, ui.DISPLAY_ID);
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
