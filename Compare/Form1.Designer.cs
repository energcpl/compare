namespace Compare
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gvResults = new System.Windows.Forms.DataGridView();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnAvail = new System.Windows.Forms.Button();
            this.newhrs = new System.Windows.Forms.Button();
            this.btnheatIn = new System.Windows.Forms.Button();
            this.bthCPHQA = new System.Windows.Forms.Button();
            this.btnEPSummary = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRandom = new System.Windows.Forms.Button();
            this.btnCo2 = new System.Windows.Forms.Button();
            this.btntest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // gvResults
            // 
            this.gvResults.AllowUserToAddRows = false;
            this.gvResults.AllowUserToDeleteRows = false;
            this.gvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvResults.Location = new System.Drawing.Point(12, 51);
            this.gvResults.Name = "gvResults";
            this.gvResults.ReadOnly = true;
            this.gvResults.Size = new System.Drawing.Size(1188, 332);
            this.gvResults.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(13, 13);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnAvail
            // 
            this.btnAvail.Location = new System.Drawing.Point(111, 13);
            this.btnAvail.Name = "btnAvail";
            this.btnAvail.Size = new System.Drawing.Size(75, 23);
            this.btnAvail.TabIndex = 2;
            this.btnAvail.Text = "Avail";
            this.btnAvail.UseVisualStyleBackColor = true;
            this.btnAvail.Click += new System.EventHandler(this.btnAvail_Click);
            // 
            // newhrs
            // 
            this.newhrs.Location = new System.Drawing.Point(219, 12);
            this.newhrs.Name = "newhrs";
            this.newhrs.Size = new System.Drawing.Size(106, 23);
            this.newhrs.TabIndex = 3;
            this.newhrs.Text = "new_hrs_update";
            this.newhrs.UseVisualStyleBackColor = true;
            this.newhrs.Click += new System.EventHandler(this.newhrs_Click);
            // 
            // btnheatIn
            // 
            this.btnheatIn.Location = new System.Drawing.Point(1035, 13);
            this.btnheatIn.Name = "btnheatIn";
            this.btnheatIn.Size = new System.Drawing.Size(165, 23);
            this.btnheatIn.TabIndex = 4;
            this.btnheatIn.Text = "HeatImport from CSV file";
            this.btnheatIn.UseVisualStyleBackColor = true;
            this.btnheatIn.Click += new System.EventHandler(this.btnheatIn_Click);
            // 
            // bthCPHQA
            // 
            this.bthCPHQA.Location = new System.Drawing.Point(350, 13);
            this.bthCPHQA.Name = "bthCPHQA";
            this.bthCPHQA.Size = new System.Drawing.Size(75, 23);
            this.bthCPHQA.TabIndex = 5;
            this.bthCPHQA.Text = "btnCHPQA";
            this.bthCPHQA.UseVisualStyleBackColor = true;
            this.bthCPHQA.Click += new System.EventHandler(this.bthCPHQA_Click);
            // 
            // btnEPSummary
            // 
            this.btnEPSummary.Location = new System.Drawing.Point(444, 13);
            this.btnEPSummary.Name = "btnEPSummary";
            this.btnEPSummary.Size = new System.Drawing.Size(75, 23);
            this.btnEPSummary.TabIndex = 6;
            this.btnEPSummary.Text = "Epower Summary";
            this.btnEPSummary.UseVisualStyleBackColor = true;
            this.btnEPSummary.Click += new System.EventHandler(this.btnEPSummary_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(542, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Heat Problems";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(647, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete Unit";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(744, 12);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(75, 23);
            this.btnRandom.TabIndex = 9;
            this.btnRandom.Text = "Random";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // btnCo2
            // 
            this.btnCo2.Location = new System.Drawing.Point(841, 13);
            this.btnCo2.Name = "btnCo2";
            this.btnCo2.Size = new System.Drawing.Size(75, 23);
            this.btnCo2.TabIndex = 10;
            this.btnCo2.Text = "Carbon Saving";
            this.btnCo2.UseVisualStyleBackColor = true;
            this.btnCo2.Click += new System.EventHandler(this.btnCo2_Click);
            // 
            // btntest
            // 
            this.btntest.Location = new System.Drawing.Point(936, 11);
            this.btntest.Name = "btntest";
            this.btntest.Size = new System.Drawing.Size(75, 23);
            this.btntest.TabIndex = 11;
            this.btntest.Text = "test";
            this.btntest.UseVisualStyleBackColor = true;
            this.btntest.Click += new System.EventHandler(this.btntest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 395);
            this.Controls.Add(this.btntest);
            this.Controls.Add(this.btnCo2);
            this.Controls.Add(this.btnRandom);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEPSummary);
            this.Controls.Add(this.bthCPHQA);
            this.Controls.Add(this.btnheatIn);
            this.Controls.Add(this.newhrs);
            this.Controls.Add(this.btnAvail);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.gvResults);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvResults;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnAvail;
        private System.Windows.Forms.Button newhrs;
        private System.Windows.Forms.Button btnheatIn;
        private System.Windows.Forms.Button bthCPHQA;
        private System.Windows.Forms.Button btnEPSummary;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Button btnCo2;
        private System.Windows.Forms.Button btntest;
    }
}

