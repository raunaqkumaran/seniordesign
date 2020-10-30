namespace WindowsFormsApp1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.staticBalanceButton = new System.Windows.Forms.Button();
            this.dynamicBalanceButton = new System.Windows.Forms.Button();
            this.accelerometerScatter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comLocation = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.counterBalanceWeight = new System.Windows.Forms.TextBox();
            this.weightLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.accelerometerScatter)).BeginInit();
            this.SuspendLayout();
            // 
            // staticBalanceButton
            // 
            this.staticBalanceButton.Location = new System.Drawing.Point(35, 144);
            this.staticBalanceButton.Name = "staticBalanceButton";
            this.staticBalanceButton.Size = new System.Drawing.Size(320, 46);
            this.staticBalanceButton.TabIndex = 1;
            this.staticBalanceButton.Text = "Start static balancing";
            this.staticBalanceButton.UseVisualStyleBackColor = true;
            this.staticBalanceButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // dynamicBalanceButton
            // 
            this.dynamicBalanceButton.Location = new System.Drawing.Point(35, 315);
            this.dynamicBalanceButton.Name = "dynamicBalanceButton";
            this.dynamicBalanceButton.Size = new System.Drawing.Size(320, 46);
            this.dynamicBalanceButton.TabIndex = 2;
            this.dynamicBalanceButton.Text = "Start dynamic balancing";
            this.dynamicBalanceButton.UseVisualStyleBackColor = true;
            this.dynamicBalanceButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // accelerometerScatter
            // 
            chartArea1.Name = "ChartArea1";
            this.accelerometerScatter.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.accelerometerScatter.Legends.Add(legend1);
            this.accelerometerScatter.Location = new System.Drawing.Point(472, 47);
            this.accelerometerScatter.Name = "accelerometerScatter";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.accelerometerScatter.Series.Add(series1);
            this.accelerometerScatter.Size = new System.Drawing.Size(851, 550);
            this.accelerometerScatter.TabIndex = 3;
            this.accelerometerScatter.Text = "chart1";
            // 
            // comLocation
            // 
            this.comLocation.AutoSize = true;
            this.comLocation.Location = new System.Drawing.Point(66, 545);
            this.comLocation.Name = "comLocation";
            this.comLocation.Size = new System.Drawing.Size(275, 25);
            this.comLocation.TabIndex = 4;
            this.comLocation.Text = "Center of mass location: (N/A)";
            this.comLocation.Click += new System.EventHandler(this.label1_Click);
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(66, 673);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(345, 25);
            this.offsetLabel.TabIndex = 5;
            this.offsetLabel.Text = "Required counter balance offset: (N/A)";
            this.offsetLabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // counterBalanceWeight
            // 
            this.counterBalanceWeight.Location = new System.Drawing.Point(336, 763);
            this.counterBalanceWeight.Name = "counterBalanceWeight";
            this.counterBalanceWeight.Size = new System.Drawing.Size(100, 29);
            this.counterBalanceWeight.TabIndex = 6;
            // 
            // weightLabel
            // 
            this.weightLabel.AutoSize = true;
            this.weightLabel.Location = new System.Drawing.Point(71, 767);
            this.weightLabel.Name = "weightLabel";
            this.weightLabel.Size = new System.Drawing.Size(259, 25);
            this.weightLabel.TabIndex = 7;
            this.weightLabel.Text = "Counter balance mass (kg): ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 1043);
            this.Controls.Add(this.weightLabel);
            this.Controls.Add(this.counterBalanceWeight);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.comLocation);
            this.Controls.Add(this.accelerometerScatter);
            this.Controls.Add(this.dynamicBalanceButton);
            this.Controls.Add(this.staticBalanceButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accelerometerScatter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button staticBalanceButton;
        private System.Windows.Forms.Button dynamicBalanceButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart accelerometerScatter;
        private System.Windows.Forms.Label comLocation;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.TextBox counterBalanceWeight;
        private System.Windows.Forms.Label weightLabel;
    }
}

