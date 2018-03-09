namespace ICITN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.axMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.gbServiceAreaSolver = new System.Windows.Forms.GroupBox();
            this.btnSolve = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCutOff = new System.Windows.Forms.TextBox();
            this.ckbShowLines = new System.Windows.Forms.CheckBox();
            this.ckbUseRestriction = new System.Windows.Forms.CheckBox();
            this.cbCostAttribute = new System.Windows.Forms.ComboBox();
            this.lbOutput = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFeatureDataset = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtInputFacilities = new System.Windows.Forms.TextBox();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.txtNetworkDataset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWorkspacePath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdSolve = new System.Windows.Forms.Button();
            this.checkUseHierarchy = new System.Windows.Forms.CheckBox();
            this.checkUseRestriction = new System.Windows.Forms.CheckBox();
            this.textCutoff = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textTargetFacility = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboCostAttribute = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).BeginInit();
            this.gbServiceAreaSolver.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 507);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(958, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(52, 17);
            this.toolStripStatusLabel1.Text = "Waiting";
            // 
            // axMapControl
            // 
            this.axMapControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axMapControl.Location = new System.Drawing.Point(564, 45);
            this.axMapControl.Margin = new System.Windows.Forms.Padding(2);
            this.axMapControl.Name = "axMapControl";
            this.axMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl.OcxState")));
            this.axMapControl.Size = new System.Drawing.Size(391, 352);
            this.axMapControl.TabIndex = 12;
            // 
            // gbServiceAreaSolver
            // 
            this.gbServiceAreaSolver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbServiceAreaSolver.Controls.Add(this.btnSolve);
            this.gbServiceAreaSolver.Controls.Add(this.label3);
            this.gbServiceAreaSolver.Controls.Add(this.label2);
            this.gbServiceAreaSolver.Controls.Add(this.txtCutOff);
            this.gbServiceAreaSolver.Controls.Add(this.ckbShowLines);
            this.gbServiceAreaSolver.Controls.Add(this.ckbUseRestriction);
            this.gbServiceAreaSolver.Controls.Add(this.cbCostAttribute);
            this.gbServiceAreaSolver.Enabled = false;
            this.gbServiceAreaSolver.Location = new System.Drawing.Point(4, 125);
            this.gbServiceAreaSolver.Name = "gbServiceAreaSolver";
            this.gbServiceAreaSolver.Size = new System.Drawing.Size(555, 95);
            this.gbServiceAreaSolver.TabIndex = 15;
            this.gbServiceAreaSolver.TabStop = false;
            this.gbServiceAreaSolver.Text = "Service Area Solver";
            // 
            // btnSolve
            // 
            this.btnSolve.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSolve.Location = new System.Drawing.Point(443, 28);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(89, 44);
            this.btnSolve.TabIndex = 1;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Cut Off";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cost Attribute";
            // 
            // txtCutOff
            // 
            this.txtCutOff.Location = new System.Drawing.Point(123, 51);
            this.txtCutOff.Name = "txtCutOff";
            this.txtCutOff.Size = new System.Drawing.Size(120, 21);
            this.txtCutOff.TabIndex = 9;
            this.txtCutOff.Text = "0";
            // 
            // ckbShowLines
            // 
            this.ckbShowLines.AutoSize = true;
            this.ckbShowLines.Location = new System.Drawing.Point(258, 46);
            this.ckbShowLines.Name = "ckbShowLines";
            this.ckbShowLines.Size = new System.Drawing.Size(84, 16);
            this.ckbShowLines.TabIndex = 2;
            this.ckbShowLines.Text = "Show Lines";
            this.ckbShowLines.UseVisualStyleBackColor = true;
            // 
            // ckbUseRestriction
            // 
            this.ckbUseRestriction.AutoSize = true;
            this.ckbUseRestriction.Location = new System.Drawing.Point(258, 24);
            this.ckbUseRestriction.Name = "ckbUseRestriction";
            this.ckbUseRestriction.Size = new System.Drawing.Size(114, 16);
            this.ckbUseRestriction.TabIndex = 3;
            this.ckbUseRestriction.Text = "Use Restriction";
            this.ckbUseRestriction.UseVisualStyleBackColor = true;
            // 
            // cbCostAttribute
            // 
            this.cbCostAttribute.FormattingEnabled = true;
            this.cbCostAttribute.Location = new System.Drawing.Point(123, 25);
            this.cbCostAttribute.Name = "cbCostAttribute";
            this.cbCostAttribute.Size = new System.Drawing.Size(121, 20);
            this.cbCostAttribute.TabIndex = 4;
            // 
            // lbOutput
            // 
            this.lbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOutput.FormattingEnabled = true;
            this.lbOutput.ItemHeight = 12;
            this.lbOutput.Location = new System.Drawing.Point(564, 402);
            this.lbOutput.Name = "lbOutput";
            this.lbOutput.Size = new System.Drawing.Size(391, 100);
            this.lbOutput.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtFeatureDataset);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtInputFacilities);
            this.groupBox1.Controls.Add(this.btnLoadMap);
            this.groupBox1.Controls.Add(this.txtNetworkDataset);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtWorkspacePath);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(555, 115);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Configuration";
            // 
            // txtFeatureDataset
            // 
            this.txtFeatureDataset.Location = new System.Drawing.Point(106, 64);
            this.txtFeatureDataset.Name = "txtFeatureDataset";
            this.txtFeatureDataset.Size = new System.Drawing.Size(308, 21);
            this.txtFeatureDataset.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "Feature Dataset";
            // 
            // txtInputFacilities
            // 
            this.txtInputFacilities.Location = new System.Drawing.Point(106, 88);
            this.txtInputFacilities.Name = "txtInputFacilities";
            this.txtInputFacilities.Size = new System.Drawing.Size(308, 21);
            this.txtInputFacilities.TabIndex = 13;
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadMap.Location = new System.Drawing.Point(443, 20);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(89, 82);
            this.btnLoadMap.TabIndex = 11;
            this.btnLoadMap.Text = "Setup Service Area Problem";
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);
            // 
            // txtNetworkDataset
            // 
            this.txtNetworkDataset.Location = new System.Drawing.Point(106, 42);
            this.txtNetworkDataset.Name = "txtNetworkDataset";
            this.txtNetworkDataset.Size = new System.Drawing.Size(308, 21);
            this.txtNetworkDataset.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "Input Facilities";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Network Dataset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Workspace Path";
            // 
            // txtWorkspacePath
            // 
            this.txtWorkspacePath.Location = new System.Drawing.Point(106, 18);
            this.txtWorkspacePath.Name = "txtWorkspacePath";
            this.txtWorkspacePath.Size = new System.Drawing.Size(308, 21);
            this.txtWorkspacePath.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmdSolve);
            this.groupBox2.Controls.Add(this.checkUseHierarchy);
            this.groupBox2.Controls.Add(this.checkUseRestriction);
            this.groupBox2.Controls.Add(this.textCutoff);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textTargetFacility);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.comboCostAttribute);
            this.groupBox2.Location = new System.Drawing.Point(4, 226);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(555, 132);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ODCost Matrix Solver";
            // 
            // cmdSolve
            // 
            this.cmdSolve.Location = new System.Drawing.Point(443, 28);
            this.cmdSolve.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSolve.Name = "cmdSolve";
            this.cmdSolve.Size = new System.Drawing.Size(89, 70);
            this.cmdSolve.TabIndex = 14;
            this.cmdSolve.Text = "Find OD Cost Matrix";
            this.cmdSolve.UseVisualStyleBackColor = true;
            this.cmdSolve.Click += new System.EventHandler(this.cmdSolve_Click);
            // 
            // checkUseHierarchy
            // 
            this.checkUseHierarchy.AutoSize = true;
            this.checkUseHierarchy.Location = new System.Drawing.Point(258, 55);
            this.checkUseHierarchy.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseHierarchy.Name = "checkUseHierarchy";
            this.checkUseHierarchy.Size = new System.Drawing.Size(102, 16);
            this.checkUseHierarchy.TabIndex = 13;
            this.checkUseHierarchy.Text = "Use Hierarchy";
            this.checkUseHierarchy.UseVisualStyleBackColor = true;
            // 
            // checkUseRestriction
            // 
            this.checkUseRestriction.AutoSize = true;
            this.checkUseRestriction.Checked = true;
            this.checkUseRestriction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUseRestriction.Location = new System.Drawing.Point(258, 28);
            this.checkUseRestriction.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseRestriction.Name = "checkUseRestriction";
            this.checkUseRestriction.Size = new System.Drawing.Size(156, 16);
            this.checkUseRestriction.TabIndex = 12;
            this.checkUseRestriction.Text = "Use Oneway Restriction";
            this.checkUseRestriction.UseVisualStyleBackColor = true;
            // 
            // textCutoff
            // 
            this.textCutoff.Location = new System.Drawing.Point(153, 85);
            this.textCutoff.Margin = new System.Windows.Forms.Padding(2);
            this.textCutoff.Name = "textCutoff";
            this.textCutoff.Size = new System.Drawing.Size(90, 21);
            this.textCutoff.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 86);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "Cutoff";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 56);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "Target Facility Count";
            // 
            // textTargetFacility
            // 
            this.textTargetFacility.Location = new System.Drawing.Point(153, 55);
            this.textTargetFacility.Margin = new System.Windows.Forms.Padding(2);
            this.textTargetFacility.Name = "textTargetFacility";
            this.textTargetFacility.Size = new System.Drawing.Size(90, 21);
            this.textTargetFacility.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 29);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "Cost Attribute";
            // 
            // comboCostAttribute
            // 
            this.comboCostAttribute.FormattingEnabled = true;
            this.comboCostAttribute.Location = new System.Drawing.Point(153, 26);
            this.comboCostAttribute.Margin = new System.Windows.Forms.Padding(2);
            this.comboCostAttribute.Name = "comboCostAttribute";
            this.comboCostAttribute.Size = new System.Drawing.Size(90, 20);
            this.comboCostAttribute.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(4, 365);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(554, 137);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Identifying Critical Infrastructure";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(362, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 46);
            this.button3.TabIndex = 2;
            this.button3.Text = "Output";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(205, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "Solve";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import ODCost Matrix";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.axToolbarControl1.Location = new System.Drawing.Point(564, 12);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(391, 28);
            this.axToolbarControl1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 529);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbServiceAreaSolver);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.axMapControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lbOutput);
            this.Name = "Form1";
            this.Text = "ICITN";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).EndInit();
            this.gbServiceAreaSolver.ResumeLayout(false);
            this.gbServiceAreaSolver.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl;
        private System.Windows.Forms.GroupBox gbServiceAreaSolver;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCutOff;
        private System.Windows.Forms.CheckBox ckbShowLines;
        private System.Windows.Forms.CheckBox ckbUseRestriction;
        public System.Windows.Forms.ComboBox cbCostAttribute;
        private System.Windows.Forms.ListBox lbOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFeatureDataset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtInputFacilities;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.TextBox txtNetworkDataset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWorkspacePath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdSolve;
        private System.Windows.Forms.CheckBox checkUseHierarchy;
        private System.Windows.Forms.CheckBox checkUseRestriction;
        private System.Windows.Forms.TextBox textCutoff;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textTargetFacility;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboCostAttribute;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
    }
}