using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnergLibrary;
using System.IO;

namespace Compare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DataTable results = new DataTable();
            results.Columns.Add("Unit_ID", typeof(Int32));
            results.Columns.Add("Display_ID", typeof(Int32));
            results.Columns.Add("CorD", typeof(string));
            results.Columns.Add("Name", typeof(String));
            results.Columns.Add("MthHrs", typeof(Int32));
            results.Columns.Add("MthKw", typeof(Int32));
            results.Columns.Add("SumHrs", typeof(Int32));
            results.Columns.Add("SumKw", typeof(Int32));
            results.Columns.Add("diffHrs", typeof(Int32));
            results.Columns.Add("diffKw", typeof(Int32));

            DataTable units = DBStuff.GetAllOperationalUnits();
            //DataTable units = DBStuff.GetAllOperationalUnits(301404);
            DateTime mth_end = new DateTime(2014, 3, 31);
            foreach (DataRow dr in units.Rows)
            {
                DataTable dt = DBStuff.GetMthData(dr, mth_end);
                DataTable st = DBStuff.GetSumRunData(dr, mth_end);

                try
                {
                    int mthHrs = Convert.ToInt32(dt.Rows[0]["hrs_run"]);
                    int mthkW = Convert.ToInt32(dt.Rows[0]["kw_run"]);

                    int sumHrs = Convert.ToInt32(st.Rows[0]["hrs_run"]);
                    int sumKw = Convert.ToInt32(st.Rows[0]["kw_run"]);

                    int diffHrs = sumHrs - mthHrs;
                    int diffkW = sumKw - mthkW;

                    int display_id = Convert.ToInt32(dr["display_id"]);
                    UnitInfo ui = new UnitInfo(display_id);
                    DataRow rr = results.NewRow();
                    rr["Unit_ID"] = ui.UNIT_ID;
                    rr["Display_ID"] = ui.DISPLAY_ID;
                    rr["CorD"] = ui.DEP;
                    rr["Name"] = ui.NAME;
                    rr["MthHrs"] = mthHrs;
                    rr["MthKw"] = mthkW;
                    rr["SumHrs"] = sumHrs;
                    rr["SumKw"] = sumKw;
                    rr["diffHrs"] = diffHrs;
                    rr["diffKw"] = diffkW;
                    results.Rows.Add(rr);
                }
                catch
                {
                }
            }
            gvResults.DataSource = results;
        }

        private void btnAvail_Click(object sender, EventArgs e)
        {
            DataTable result = new DataTable();
            result.Columns.Add("ID", typeof(int));
            result.Columns.Add("CHPUNIT", typeof(double));
            result.Columns.Add("AVAIL", typeof(double));

            DataTable dt = DBStuff.GetAllOperationalUnits();
            foreach (DataRow dr in dt.Rows)
            {
                int display_id = Convert.ToInt32(dr["display_id"]);
                UnitInfo ui = new UnitInfo(display_id);
                double chprun = DBStuff.GetChpUnitHrs(ui);
                double availhrs = DBStuff.GetAvailHrs(ui);
                if (chprun != availhrs)
                {
                    DataRow nr = result.NewRow();
                    nr["ID"] = dr["Display_ID"];
                    nr["CHPUNIT"] = chprun;
                    nr["AVAIL"] = availhrs;
                    result.Rows.Add(nr);
                }
            }
            gvResults.DataSource = result;
        }

        private void newhrs_Click(object sender, EventArgs e)
        {
            DataTable units = DBStuff.GetAllOperationalUnits();
            foreach (DataRow dr in units.Rows)
            {
                int display_id = Convert.ToInt32(dr["display_id"]);
                UnitInfo ui = new UnitInfo(display_id);

                DataTable old = DBStuff.GetNewRunHrs(ui);
                if (old.Rows.Count == 1)
                {
                    if (DBStuff.NewRunExists(ui))
                    {
                    }
                    else
                    {
                        DBStuff.AppendNewRunHrs(old);
                    }
                }
                
            }
        }

        private void btnheatIn_Click(object sender, EventArgs e)
        {
            string line;
            StreamReader file =  new StreamReader(@"C:\HeatData.csv");
            while ((line = file.ReadLine()) != null)
            {
                string[] split = line.Split(',');
                try
                {
                    int id = Convert.ToInt32(split[0]);
                    DateTime dt = Convert.ToDateTime(split[1]);
                    double heatValue = Convert.ToDouble(split[2]);
                    int problemID = Convert.ToInt32(split[3]);
                    bool dataFound = Convert.ToBoolean(split[4]);
                    //bool dataFound;
                    //if (IdataFound == 0)
                    //{
                    //    dataFound = false;
                    //}
                    //else
                    //{
                    //    dataFound = true;
                    //}
                    if (DBStuff.ExistsInHeat(id, dt))
                    {
                        DBStuff.UpdateHeat(id, dt, heatValue, problemID, dataFound);
                    }
                    else
                    {
                        DBStuff.AddToHeat(id, dt, heatValue, problemID, dataFound);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing {0} Exception {0}", line, ex.Message);
                }
            }
        }

        private void btnNoRad_Click(object sender, EventArgs e)
        {
            
        }

        private void bthCPHQA_Click(object sender, EventArgs e)
        {
            DataTable result = new DataTable();
            result.Columns.Add("Display_Id", typeof(int));
            result.Columns.Add("unit_id", typeof(int));
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("CorD", typeof(string));
            result.Columns.Add("DumpRad", typeof(bool));
            result.Columns.Add("Stage", typeof(string));

            DataTable dt = DBStuff.GetAllOperationalUnits();
            foreach (DataRow dr in dt.Rows)
            {
                int display_id = Convert.ToInt32(dr["display_id"]);
                UnitInfo ui = new UnitInfo(display_id);

                int custid = DBStuff.GetCustomer(ui);
                if (custid == 1)   //ECPL
                {
                    bool hasData = DBStuff.DataIn2013(ui);
                    if (hasData)
                    {
                        bool dump = DBStuff.HasDumpRad(ui);

                        DataRow aRow = result.NewRow();
                        aRow["Display_ID"] = ui.DISPLAY_ID;
                        aRow["Unit_ID"] = ui.UNIT_ID;
                        aRow["Name"] = ui.NAME;
                        aRow["DumpRad"] = dump;
                        aRow["CorD"] = ui.DEP;
                        aRow["Stage"] = ui.OPERATIONAL;
                        result.Rows.Add(aRow);
                    }
                }
            }

            result.Columns.Add("Problems", typeof(int));
            result.Columns.Add("NoData", typeof(int));
            foreach (DataRow dr in result.Rows)
            {
                int display_id = Convert.ToInt32(dr["display_id"]);
                UnitInfo ui = new UnitInfo(display_id);
                DataTable heatStuff = DBStuff.GetHeatData(ui);
                int problems = 0;
                int nodata = 0;

                foreach (DataRow hr in heatStuff.Rows)
                {
                    int infoColumn = Convert.ToInt32(hr["problem_id"]);
                    if (infoColumn > 0)
                    {
                        problems++;
                    }
                    if (infoColumn == -1)
                    {
                        nodata++;
                    }
                }
                
                dr["Problems"] = problems;
                dr["NoData"] = nodata;
            }
            gvResults.DataSource = result;
        }

        private string ConvertToBinary(int infoColumn)
        {
            int BinaryHolder;
            char[] BinaryArray;
            string BinaryResult = "";

            while (infoColumn > 0)
            {
                BinaryHolder = infoColumn % 2;
                BinaryResult += BinaryHolder;
                infoColumn = infoColumn / 2;
            }
            BinaryArray = BinaryResult.ToCharArray();
            Array.Reverse(BinaryArray);
            BinaryResult = new string(BinaryArray);
            return BinaryResult.PadLeft(15,'0');
        }

        private void btnEPSummary_Click(object sender, EventArgs e)
        {
            for (int x = 3; x < 10; x++)  // month number 
            {
                DateTime startnextMonth = new DateTime(2014, x + 1, 1);
                DateTime mthEnd = startnextMonth.AddDays(-1);

                DataTable epUnits = DBStuff.GetAllEPOWERUnits();
                foreach (DataRow dr in epUnits.Rows)
                {
                    int displayID = Convert.ToInt32(dr["display_ID"]);

                    DataTable rd = DBStuff.GetSumRunData(dr, mthEnd);

                    try
                    {
                        double mthHrs = Convert.ToDouble(rd.Rows[0]["hrs_run"]);
                        double kwRun = Convert.ToDouble(rd.Rows[0]["kw_run"]);

                    }
                    catch
                    {
                        //no data for this month for this unit
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form frm = new HeatProblems();
            frm.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Form frm = new DeleteUnit();
            frm.Show();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            DataTable dt = DBStuff.GetAllOperationalUnits();
            int count = dt.Rows.Count;
            List<int> numbers = new List<int>();
            Random r = new Random();
            for (int x = 0; x < 6; x++)
            {
                int num = r.Next(0, count);
                numbers.Add(Convert.ToInt32(dt.Rows[num]["Display_ID"]));
            }

            gvResults.DataSource = numbers;
            
        }

        private void btnCo2_Click(object sender, EventArgs e)
        {
            //Result
            //•	Site number
            //•	Site name
            //•	CO2 saved from 1st January 2014 to 1st January 2015
            //•	Cars off the road 1st January 2014 to 1st January 2015
            //•	Cumulative CO2 from operation to 1st January 2015
            //•	Cars off the road from operation to 1st January 2015
            //•	Name of site contact
            //•	Email address of site contact
            //  Cars are worked out at 3 cars per tonne (1tonne = 1000 kg)
            DataTable result = new DataTable();
            result.Columns.Add("Site_Number", typeof(int));
            result.Columns.Add("Site_Name", typeof(string));
            result.Columns.Add("CO2", typeof(double));
            result.Columns.Add("Cars", typeof(double));
            result.Columns.Add("CO2Cum", typeof(double));
            result.Columns.Add("CarsCum", typeof(double));
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("Email", typeof(string));
            result.Columns.Add("Group", typeof(string));

            //get all units
            DataTable dt = DBStuff.GetAllUnits();
            //DataTable dt = DBStuff.GetAllUnits(922);
            //for each unit
            foreach (DataRow dr in dt.Rows)
            {
                int unit_id = Convert.ToInt32(dr["unit_id"]);
                int display_id = Convert.ToInt32(dr["display_id"]);
                UnitInfo ui = new UnitInfo(display_id);
                bool inCHPQA = DBStuff.IsInCHPQA(unit_id);
                DataTable cumulative = DBStuff.GetCumulative(ui);
                double cum_kw = 0;
                double cum_gas = 0;
                double cum_heat = 0;
                try
                {
                    cum_kw = Convert.ToDouble(cumulative.Rows[0]["cum_kw"]);
                    cum_gas = Convert.ToDouble(cumulative.Rows[0]["cum_gas"]);
                    cum_heat = Convert.ToDouble(cumulative.Rows[0]["cum_heat"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No cumulative for {0}", ui.DISPLAY_ID);
                }

                if (inCHPQA)
                {
                    DataTable chpqadt = DBStuff.GetCHPQAData(ui, 2014);
                    if (chpqadt.Rows.Count == 1)
                    {
                        double gas = Convert.ToDouble(chpqadt.Rows[0]["tfi"]) * 1000;
                        double elec = Convert.ToDouble(chpqadt.Rows[0]["tpo"]) * 1000;
                        double heat = Convert.ToDouble(chpqadt.Rows[0]["qho"]) * 1000;
                        double co2 = CalcCo2(elec, heat, gas);

                        DataRow newRow = result.NewRow();
                        newRow["Site_Number"] = ui.DISPLAY_ID;
                        newRow["Site_Name"] = ui.NAME;
                        newRow["CO2"] = co2;
                        newRow["Cars"] = co2 / 3000.0f;

                        double co2cum = CalcCo2(cum_kw, cum_heat, cum_gas);
                        newRow["CO2Cum"] = co2cum;
                        newRow["CarsCum"] = co2cum / 3000.0f;
                        string cName = DBStuff.GetContactName(ui);
                        newRow["Name"] = cName;
                        string email = DBStuff.GetAllEmails(ui);
                        newRow["Email"] = email;
                        newRow["Group"] = DBStuff.GetCompanyGroup(ui.UNIT_ID);
                        result.Rows.Add(newRow);
                        //Is in in CHPQA table
                        //YES
                        //use sums in chpqa calculate co2
                        //values in here are MW so *1000 for kW
                    }
                }
                else
                {
                    List<DateTime> mthEnds = new List<DateTime>();
                    mthEnds.Add(new DateTime(2014, 1, 31));
                    mthEnds.Add(new DateTime(2014, 2, 28));
                    mthEnds.Add(new DateTime(2014, 3, 31));
                    mthEnds.Add(new DateTime(2014, 4, 30));
                    mthEnds.Add(new DateTime(2014, 5, 31));
                    mthEnds.Add(new DateTime(2014, 6, 30));
                    mthEnds.Add(new DateTime(2014, 7, 31));
                    mthEnds.Add(new DateTime(2014, 8, 31));
                    mthEnds.Add(new DateTime(2014, 9, 30));
                    mthEnds.Add(new DateTime(2014, 10, 31));
                    mthEnds.Add(new DateTime(2014, 11, 30));
                    mthEnds.Add(new DateTime(2014, 12, 31));

                    double sumco2 = 0;
                    double sumgas = 0;
                    double sumheat = 0;
      
                    foreach (DateTime medt in mthEnds)
                    {
                        DataTable thisMth = DBStuff.GetMthData(dr, medt);
                        try
                        {
                            double elec = Convert.ToDouble(thisMth.Rows[0]["kw_run"]);
                            double gas = Convert.ToDouble(thisMth.Rows[0]["gas"]);
                            sumgas += gas;
                            double heat = Convert.ToDouble(thisMth.Rows[0]["heat"]);
                            sumheat += heat;
                            double co2 = CalcCo2(elec, heat, gas);
                            sumco2 += co2;
                        }
                        catch 
                        {
                        }
                    }
                    //DBStuff.GetMthData(dr, new DateTime(2014, 1, 31));
                    //NO
                    //sum all data for 2014, hrs kw, calculate heat, gas = calculate co2
                    DataRow newRow = result.NewRow();
                    newRow["Site_Number"] = ui.DISPLAY_ID;
                    newRow["Site_Name"] = ui.NAME;
                    newRow["CO2"] = sumco2;
                    newRow["Cars"] = sumco2 / 3000.0f;
                    double co2cum = CalcCo2(cum_kw, cum_heat, cum_gas);
                    newRow["CO2Cum"] = co2cum;
                    newRow["CarsCum"] = co2cum / 3000.0f;
                    string cName = DBStuff.GetContactName(ui);
                    newRow["Name"] = cName;
                    string email = DBStuff.GetAllEmails(ui);
                    newRow["Email"] = email;
                    newRow["Group"] = DBStuff.GetCompanyGroup(ui.UNIT_ID);
                    result.Rows.Add(newRow);
                }
            }
            gvResults.DataSource = result;
            
        }

        private double CalcCo2(double elec, double heat, double gas)
        {
            double co2 = 0;
            co2 = (elec * 0.49023) + ((heat / 0.8) * (double)0.184557) - (gas * (double)0.184557);
            if (co2 < 0) co2 = 0;
            return co2;
        }

        private void btntest_Click(object sender, EventArgs e)
        {
            //List<DateTime> results = new List<DateTime>();
            //DateTime now = DateTime.Now;
            //results.Add(now);
            //DataTable dt = DBStuff.Test1();
            //now = DateTime.Now;
            //results.Add(now);

            //gvResults.DataSource = results;
            Form frm = new SendParams(2);
            frm.Show();
        }
    }
}
