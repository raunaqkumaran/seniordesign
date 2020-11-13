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
            this.staticBalanceButton = new System.Windows.Forms.Button();
            this.dynamicBalanceButton = new System.Windows.Forms.Button();
            this.comLocation = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.weightLabel = new System.Windows.Forms.Label();
            this.correctionMoment = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.weightSelectionBox = new System.Windows.Forms.NumericUpDown();
            this.omegaBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.rotationRadius = new System.Windows.Forms.Label();
            this.portSelector = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.weightSelectionBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.omegaBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // staticBalanceButton
            // 
            this.staticBalanceButton.Location = new System.Drawing.Point(35, 144);
            this.staticBalanceButton.Margin = new System.Windows.Forms.Padding(4);
            this.staticBalanceButton.Name = "staticBalanceButton";
            this.staticBalanceButton.Size = new System.Drawing.Size(321, 46);
            this.staticBalanceButton.TabIndex = 1;
            this.staticBalanceButton.Text = "Run static";
            this.staticBalanceButton.UseVisualStyleBackColor = true;
            this.staticBalanceButton.Click += new System.EventHandler(this.startStaticButton);
            // 
            // dynamicBalanceButton
            // 
            this.dynamicBalanceButton.Location = new System.Drawing.Point(35, 316);
            this.dynamicBalanceButton.Margin = new System.Windows.Forms.Padding(4);
            this.dynamicBalanceButton.Name = "dynamicBalanceButton";
            this.dynamicBalanceButton.Size = new System.Drawing.Size(321, 46);
            this.dynamicBalanceButton.TabIndex = 2;
            this.dynamicBalanceButton.Text = "Start dynamic balancing";
            this.dynamicBalanceButton.UseVisualStyleBackColor = true;
            this.dynamicBalanceButton.Click += new System.EventHandler(this.startDynamicButton);
            // 
            // comLocation
            // 
            this.comLocation.AutoSize = true;
            this.comLocation.Location = new System.Drawing.Point(438, 155);
            this.comLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.comLocation.Name = "comLocation";
            this.comLocation.Size = new System.Drawing.Size(310, 25);
            this.comLocation.TabIndex = 4;
            this.comLocation.Text = "Center of mass location (m): (N/A)";
            this.comLocation.Click += new System.EventHandler(this.label1_Click);
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(904, 155);
            this.offsetLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(380, 25);
            this.offsetLabel.TabIndex = 5;
            this.offsetLabel.Text = "Required counter balance offset (m): (N/A)";
            this.offsetLabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // weightLabel
            // 
            this.weightLabel.AutoSize = true;
            this.weightLabel.Location = new System.Drawing.Point(29, 70);
            this.weightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.weightLabel.Name = "weightLabel";
            this.weightLabel.Size = new System.Drawing.Size(259, 25);
            this.weightLabel.TabIndex = 7;
            this.weightLabel.Text = "Counter balance mass (kg): ";
            // 
            // correctionMoment
            // 
            this.correctionMoment.AutoSize = true;
            this.correctionMoment.Location = new System.Drawing.Point(438, 327);
            this.correctionMoment.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.correctionMoment.Name = "correctionMoment";
            this.correctionMoment.Size = new System.Drawing.Size(287, 25);
            this.correctionMoment.TabIndex = 8;
            this.correctionMoment.Text = "Counterbalance correction (m): ";
            this.correctionMoment.Click += new System.EventHandler(this.label1_Click_2);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(35, 402);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(321, 46);
            this.button2.TabIndex = 10;
            this.button2.Text = "End dynamic balancing";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.endDynamicButton);
            // 
            // weightSelectionBox
            // 
            this.weightSelectionBox.DecimalPlaces = 2;
            this.weightSelectionBox.Location = new System.Drawing.Point(293, 66);
            this.weightSelectionBox.Margin = new System.Windows.Forms.Padding(6);
            this.weightSelectionBox.Name = "weightSelectionBox";
            this.weightSelectionBox.Size = new System.Drawing.Size(220, 29);
            this.weightSelectionBox.TabIndex = 11;
            this.weightSelectionBox.ValueChanged += new System.EventHandler(this.weightSelectionBox_ValueChanged);
            // 
            // omegaBox
            // 
            this.omegaBox.DecimalPlaces = 2;
            this.omegaBox.Location = new System.Drawing.Point(293, 257);
            this.omegaBox.Margin = new System.Windows.Forms.Padding(6);
            this.omegaBox.Name = "omegaBox";
            this.omegaBox.Size = new System.Drawing.Size(220, 29);
            this.omegaBox.TabIndex = 13;
            this.omegaBox.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 260);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 25);
            this.label2.TabIndex = 12;
            this.label2.Text = "Rotation rate (rad/s):";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // rotationRadius
            // 
            this.rotationRadius.AutoSize = true;
            this.rotationRadius.Location = new System.Drawing.Point(438, 425);
            this.rotationRadius.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.rotationRadius.Name = "rotationRadius";
            this.rotationRadius.Size = new System.Drawing.Size(208, 25);
            this.rotationRadius.TabIndex = 14;
            this.rotationRadius.Text = "Radius of rotation (m): ";
            this.rotationRadius.Click += new System.EventHandler(this.label1_Click_3);
            // 
            // portSelector
            // 
            this.portSelector.Location = new System.Drawing.Point(1062, 64);
            this.portSelector.Margin = new System.Windows.Forms.Padding(6);
            this.portSelector.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.portSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portSelector.Name = "portSelector";
            this.portSelector.Size = new System.Drawing.Size(220, 29);
            this.portSelector.TabIndex = 16;
            this.portSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portSelector.ValueChanged += new System.EventHandler(this.portBox);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(904, 66);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 25);
            this.label1.TabIndex = 15;
            this.label1.Text = "COM Port:";
            this.label1.Click += new System.EventHandler(this.label1_Click_4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 1043);
            this.Controls.Add(this.portSelector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rotationRadius);
            this.Controls.Add(this.omegaBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.weightSelectionBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.correctionMoment);
            this.Controls.Add(this.weightLabel);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.comLocation);
            this.Controls.Add(this.dynamicBalanceButton);
            this.Controls.Add(this.staticBalanceButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.weightSelectionBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.omegaBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button staticBalanceButton;
        private System.Windows.Forms.Button dynamicBalanceButton;
        private System.Windows.Forms.Label comLocation;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label weightLabel;
        private System.Windows.Forms.Label correctionMoment;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown weightSelectionBox;
        private System.Windows.Forms.NumericUpDown omegaBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label rotationRadius;
        private System.Windows.Forms.NumericUpDown portSelector;
        private System.Windows.Forms.Label label1;
    }
}

