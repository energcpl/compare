using System;
using System.Data;
using System.Data.SqlClient;
using EnergLibrary;
using System.Data.Odbc;

namespace Compare
{
    class DBStuff
    {
        internal static DataTable GetAllOperationalUnits()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                String dbquery = @"
SELECT     Units.Unit_ID, Units.Display_ID
FROM         Units INNER JOIN
                      Grouping ON Units.Display_ID = Grouping.DisplayID
where operational = 'O'
  and (unit_style = 'DELPHI' or unit_style = 'DIRMS')
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetAllUnits()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                String dbquery = @"
SELECT     Units.Unit_ID, Units.Display_ID
FROM         Units INNER JOIN
                      Grouping ON Units.Display_ID = Grouping.DisplayID
where (unit_style = 'DELPHI' or unit_style = 'DIRMS')
  and (operational = 'O');
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetAllUnits(int displayID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                String dbquery = @"
SELECT     Units.Unit_ID, Units.Display_ID
FROM         Units INNER JOIN
                      Grouping ON Units.Display_ID = Grouping.DisplayID
where (unit_style = 'DELPHI' or unit_style = 'DIRMS')
  and (operational = 'O')
  and (display_id = @DID);
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@DID", displayID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetAllEPOWERUnits()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                String dbquery = @"
SELECT     Units.Unit_ID, Units.Display_ID
FROM         Units INNER JOIN
                      Grouping ON Units.Display_ID = Grouping.DisplayID
where (unit_style = 'EPOWER')
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetAllOperationalUnits(int display_id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                String dbquery = @"
SELECT     Units.Unit_ID, Units.Display_ID
FROM         Units INNER JOIN
                      Grouping ON Units.Display_ID = Grouping.DisplayID
where operational = 'O'
  and display_id = @DID
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@DID", display_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }


        internal static DataTable GetMthData(DataRow dr, DateTime mth_end)
        {
            int unit_id = Convert.ToInt32(dr["unit_id"]);
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select hrs_run, kw_run, gas, heat
  from mth_data
 where unit_id = @UID
   and mth_end = @MEND
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", unit_id);
                cmd.Parameters.AddWithValue("@MEND", mth_end);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetSumRunData(DataRow dr, DateTime mth_end)
        {
            int unit_id = Convert.ToInt32(dr["unit_id"]);
            DateTime startMth = new DateTime(mth_end.Year, mth_end.Month, 1);
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select sum(hrs_run) as hrs_run, sum(kw_run) as kw_run
  from run_data
 where unit_id = @UID
   and reading_date >= @START
   and reading_date <= @END
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", unit_id);
                cmd.Parameters.AddWithValue("@START", startMth);
                cmd.Parameters.AddWithValue("@END", mth_end);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static double GetChpUnitHrs(UnitInfo ui)
        {
            double hrs = -1;
            using (OdbcConnection conn = new OdbcConnection(DBConnections.DIRMSConnectionString))
            {
                string dbquery = @"
select dayhrs_used
  from chp_unit
 where chp_id = ?
";
                OdbcCommand cmd = new OdbcCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("", ui.UNIT_ID);
                conn.Open();
                OdbcDataReader odr = cmd.ExecuteReader();
                if (odr.Read())
                {
                    hrs = Convert.ToDouble(odr[0]);
                }
            }
            return hrs;
        }

        internal static double GetAvailHrs(UnitInfo ui)
        {
            double hrs = -1;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select avail_hrs
  from run_data
 where unit_id = @UID
   and reading_date = @RDATE
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                cmd.Parameters.AddWithValue("@RDATE", yesterday);
                conn.Open();
                SqlDataReader odr = cmd.ExecuteReader();
                if (odr.Read())
                {
                    hrs = Convert.ToDouble(odr[0]);
                }
            }
            return hrs;
        }

        internal static DataTable GetNewRunHrs(UnitInfo ui)
        {
            DataTable dt = new DataTable();
            using (OdbcConnection conn = new OdbcConnection(DBConnections.DIRMSConnectionString))
            {
                string dbquery = @"
select chp_id, reading_date, run_string, starttime, endtime
  from new_run_hrs
 where chp_id = ?
   and reading_date = ?
";
                OdbcCommand cmd = new OdbcCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("", ui.UNIT_ID);
                cmd.Parameters.AddWithValue("", new DateTime(2013, 12, 14));
                OdbcDataAdapter oda = new OdbcDataAdapter(cmd);
                oda.Fill(dt);
            }
            return dt;
        }

        internal static bool NewRunExists(UnitInfo ui)
        {
            bool found = false;
            using (OdbcConnection conn = new OdbcConnection(DBConnections.DIRMSConnectionString))
            {
                string dbquery = @"
select chp_id, reading_date, run_string, starttime, endtime
  from new_run_hrs
 where chp_id = ?
   and reading_date = ?
";
                OdbcCommand cmd = new OdbcCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("", ui.UNIT_ID);
                cmd.Parameters.AddWithValue("", new DateTime(2013, 12, 15));
                conn.Open();
                OdbcDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    found = true;
                }
                else
                {
                }
            }
            return found;
        }

        internal static void AppendNewRunHrs(DataTable old)
        {
            using (OdbcConnection conn = new OdbcConnection(DBConnections.DIRMSConnectionString))
            {
                string dbquery = @"
insert into new_run_hrs
    Values (?, ?, ?, ?, ?)
";
                OdbcCommand cmd = new OdbcCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("", old.Rows[0]["chp_id"]);
                cmd.Parameters.AddWithValue("", new DateTime(2013,12,15));
                cmd.Parameters.AddWithValue("", old.Rows[0]["run_string"]);
                cmd.Parameters.AddWithValue("", old.Rows[0]["starttime"]);
                cmd.Parameters.AddWithValue("", old.Rows[0]["endtime"]);
                conn.Open();
                int ok = cmd.ExecuteNonQuery();
            }
        }

        internal static bool ExistsInHeat(int id, DateTime dt)
        {
            bool found = false;
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select 1
  from heat_data
 where unit_id = @UID
   and reading_date = @RDATE
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", id);
                cmd.Parameters.AddWithValue("@RDATE", dt);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    found = true;
                }
                else
                {
                }
            }
            return found;
        }

        internal static void AddToHeat(int id, DateTime dt, double heatValue, int problemID, bool dataFound)
        {
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
insert into heat_DATA
    Values (@ID, @RDATE , @HEAT, @PROBLEM, @FOUND)
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@RDATE", dt);
                cmd.Parameters.AddWithValue("@HEAT", heatValue);
                cmd.Parameters.AddWithValue("@PROBLEM", problemID);
                cmd.Parameters.AddWithValue("@FOUND", dataFound);
                conn.Open();
                int ok = cmd.ExecuteNonQuery();
            }
        }

        internal static void UpdateHeat(int id, DateTime dt, double heatValue, int problemID, bool dataFound)
        {
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
update heat_DATA
    set actual_heat_value = @HEAT, 
        problem_id = @PROBLEM,
        datafound = @FOUND
 where  unit_id = @ID
   and  reading_date = @RDATE
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@RDATE", dt);
                cmd.Parameters.AddWithValue("@HEAT", heatValue);
                cmd.Parameters.AddWithValue("@PROBLEM", problemID);
                cmd.Parameters.AddWithValue("@FOUND", dataFound);
                conn.Open();
                int ok = cmd.ExecuteNonQuery();
            }
        }

        internal static int GetCustomer(UnitInfo ui)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select group_id 
  from CompanyGroup_Units
 where unit_id = @UID
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                conn.Open();
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }

        internal static bool DataIn2013(UnitInfo ui)
        {
            bool found = false;
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select 1
  from run_data
 where unit_id = @UID
   and reading_date >= '1-jan-2013'
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    found = true;
                }
                else
                {
                }
            }
            return found;
        }

        internal static bool HasDumpRad(UnitInfo ui)
        {
            bool found = false;
            int radid = -1;
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select radiator_type
  from dump_rad
 where display_id = @DID
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@DID", ui.DISPLAY_ID);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    radid = Convert.ToInt32(sdr[0]);
                    found = true;
                }
                else
                {
                }
            }
            if (found)
            {
                if (radid == 4)  // this is the code for no rad
                {
                    found = false;
                }
            }
            return found;
        }

        internal static DataTable GetHeatData(UnitInfo ui)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select unit_id, reading_date, actual_heat_value, problem_id, datafound
  from heat_data
 where unit_id = @UID
   and reading_date >= @RDATE
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                cmd.Parameters.AddWithValue("@RDATE", new DateTime(2013, 1, 1));
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetVisitsBefore(int year)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
SELECT     Visit_ID, Job_ID, Unit_ID, Status_From, Status_To, Date_Changed, Date_Visit, Eng_Visit, Visit_Scheduled, Comment, CommentID, LoginName, Confirmed, PoNumber, 
                      Customer_Informed, CurrentStatus, JVT_ID
FROM         Job_Visits
WHERE Date_visit < @DVISIT
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@DVISIT", new DateTime(year, 1, 1));
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static DataTable GetHeatDataLast7Days()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
SELECT     Units.Display_ID, Units.Name, Units.Manager, Units.DEP, Heat_Data.reading_date, Heat_Data.Actual_heat_value, Heat_Data.Problem_Id, Run_Data.Hrs_Run, 
                      Run_Data.Kw_Run
FROM         Heat_Data INNER JOIN
                      Units ON Heat_Data.Unit_Id = Units.Unit_ID INNER JOIN
                      CompanyGroup_Units ON Heat_Data.Unit_Id = CompanyGroup_Units.Unit_ID INNER JOIN
                      Run_Data ON Heat_Data.Unit_Id = Run_Data.Unit_ID AND Heat_Data.reading_date = Run_Data.Reading_date
WHERE     (Heat_Data.reading_date > @THEDATE) AND (Heat_Data.Problem_Id <> 0) AND (Heat_Data.Problem_Id <> - 1) AND (Heat_Data.Problem_Id <> 666) AND 
                      (Heat_Data.Actual_heat_value > 0) AND (CompanyGroup_Units.Group_ID = 1) AND (Units.unit_style = 'DELPHI') OR
                      (Heat_Data.reading_date > @THEDATE) AND (Heat_Data.Problem_Id <> 0) AND (Heat_Data.Problem_Id <> - 1) AND (Heat_Data.Problem_Id <> 666) AND 
                      (Heat_Data.Actual_heat_value > 0) AND (CompanyGroup_Units.Group_ID = 1) AND (Units.unit_style = 'DIRMS')
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@THEDATE", DateTime.Today.AddDays(-7));
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            dt.Columns.Add("DumpRad", typeof(Boolean));
            foreach (DataRow dr in dt.Rows)
            {
                int did = Convert.ToInt32(dr["Display_ID"]);
                bool hasarad = DBStuff.HasDumpRad(new UnitInfo(did));
                dr["DumpRad"] = hasarad;
            }
            return dt;
        }

        internal static bool IsInCHPQA(int unit_id)
        {
            bool exists = false;
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select 1 from CHPQA
 where chpid = @ID
   and submissionYear = 2014
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@ID", unit_id);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    exists = true;
                }
            }
            return exists;
        }

        internal static DataTable GetCumulative(UnitInfo ui)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select top 1 cum_hrs, cum_kw, cum_gas, cum_heat
  from mth_data
 where  unit_id = @UID
order by mth_end DESC
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }

        internal static string GetContactName(UnitInfo ui)
        {
            string name = "";
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select contactname
  from Addresses
 where unit_id = @UID
   and address_Type = 2
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    name = Convert.ToString(sdr[0]);
                }
            }
            return name;
        }

        private static string GetEmailName(UnitInfo ui)
        {
            string name = "";
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select faxNumber
  from Addresses
 where unit_id = @UID
   and address_Type = 2
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    name = Convert.ToString(sdr[0]);
                }
            }
            return name;
        }

        internal static string GetAllEmails(UnitInfo ui)
        {
            string email = "";
            string add = GetEmailName(ui);
            string dop = GetEmailDOP(ui);
            string inform = GetEmailInform(ui);
            return email + add + dop + inform;
        }

        private static string GetEmailInform(UnitInfo ui)
        {
            string name = "";
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
SELECT     cust_email
FROM         job_visits_inform_customer
WHERE     (unit_ID = @UID)
";
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.DISPLAY_ID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    name = name + Convert.ToString(dr["cust_email"]) + ";";
                }
            }
            return name;
        }

        private static string GetEmailDOP(UnitInfo ui)
        {
            string name = "";
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
SELECT     EMAIL.email
FROM         EMAIL INNER JOIN
                      Email_lookupID ON EMAIL.ID = Email_lookupID.EmailID
WHERE     (Email_lookupID.Lookup_ID = @DID)
";
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@DID", ui.DISPLAY_ID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    name = name + Convert.ToString(dr["email"]) + ";";
                }
            }
            return name;
        }

        internal static DataTable GetCHPQAData(UnitInfo ui, int year)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
select tfi, tpo, qho
  from chpqa
 where chpID = @UID
   and SubmissionYear = @YEAR
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", ui.UNIT_ID);
                cmd.Parameters.AddWithValue("@YEAR", year);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }



        internal static object GetCompanyGroup(int unitID)
        {
            string group = "";
            using (SqlConnection conn = new SqlConnection(DBConnections.EnergDB))
            {
                string dbquery = @"
SELECT     CompanyGroup_Names.Description
FROM         CompanyGroup_Names INNER JOIN
                      CompanyGroup_Units ON CompanyGroup_Names.Group_ID = CompanyGroup_Units.Group_ID
where       CompanyGroup_Units.Unit_ID = @UID
";
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", unitID);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    group = Convert.ToString(sdr[0]);
                }
            }
            return group;
        }

        internal static DataTable Test1()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnections.GCCMeasurementsConnectionString))
            {
                string dbquery = @"
select * from measurements 
  where m_unit_id = @UID
    and M_timestamp > '1-jan-2015'
";
                SqlCommand cmd = new SqlCommand(dbquery, conn);
                cmd.Parameters.AddWithValue("@UID", Convert.ToInt16(3066));
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }
    }
}
