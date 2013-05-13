// Fuzzy Auto Guided Vehicle Sample
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Fabio L. Caversan, 2008-2009
// fabio.caversan@gmail.com
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Diagnostics;

namespace FuzzyAGV
{
    public class MainForm : System.Windows.Forms.Form
    {

        #region Private members
        private string RunLabel;
        private Point InitialPos;
        private bool FirstInference;
        private int LastX;
        private int LastY;
        private int Time = 0;
        private double Angle, Speed;
        private Bitmap OriginalMap, InitialMap;
        private Thread thMovement;
        private FIS fis;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.PictureBox pbTerrain;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.CheckBox cbLasers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label txtFront0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label txtAngle;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pbRobot;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox gbComandos;
        private IContainer components;
        private Label txtSpeed;
        private Label label6;
        private System.Windows.Forms.Timer timer;
        private Label txtTime;
        private Label label8;
        private CheckedListBox lstFalha;
        private GroupBox groupBox3;
        private Label txtFront7;
        private Label label20;
        private Label txtFront6;
        private Label label18;
        private Label txtFront5;
        private Label label16;
        private Label txtFront4;
        private Label label14;
        private Label txtFront3;
        private Label label12;
        private Label txtFront2;
        private Label label10;
        private Label txtFront1;
        private Label label7;
        private Label label9;
        private Label label5;
        private Label label2;
        private Label label15;
        private Label label17;
        private Label label11;
        private Label label13;
        private PictureBox pictureBox2;
        private Label label23;
        private Label label22;
        private Label label21;
        private System.Windows.Forms.CheckBox cbTrajeto;
        #endregion

        #region Class constructor, destructor and Main method

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.Run(new MainForm());
        }


        public MainForm()
        {
            InitializeComponent();
            Angle = 0;
            OriginalMap = new Bitmap(pbTerrain.Image);
            InitialMap = new Bitmap(pbTerrain.Image);

            fis = new FIS();
            FirstInference = true;
            pbRobot.Top = pbTerrain.Bottom - 50;
            pbRobot.Left = pbTerrain.Left + 60;
            InitialPos = pbRobot.Location;
            RunLabel = btnRun.Text;
        }

        /// <summary>
        /// Stoping the movement thread
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            StopMovement();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pbTerrain = new System.Windows.Forms.PictureBox();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.cbLasers = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFront7 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtFront6 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtFront5 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtFront4 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtFront3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFront2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFront1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFront0 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.Label();
            this.gbComandos = new System.Windows.Forms.GroupBox();
            this.cbTrajeto = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pbRobot = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lstFalha = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbTerrain)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbComandos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRobot)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTerrain
            // 
            this.pbTerrain.BackColor = System.Drawing.SystemColors.ControlText;
            this.pbTerrain.ErrorImage = null;
            this.pbTerrain.Image = ((System.Drawing.Image)(resources.GetObject("pbTerrain.Image")));
            this.pbTerrain.InitialImage = null;
            this.pbTerrain.Location = new System.Drawing.Point(581, 8);
            this.pbTerrain.Name = "pbTerrain";
            this.pbTerrain.Size = new System.Drawing.Size(500, 500);
            this.pbTerrain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTerrain.TabIndex = 10;
            this.pbTerrain.TabStop = false;
            this.pbTerrain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTerrain_MouseDown);
            this.pbTerrain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTerrain_MouseMove);
            // 
            // btnStep
            // 
            this.btnStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep.Location = new System.Drawing.Point(6, 109);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 23);
            this.btnStep.TabIndex = 14;
            this.btnStep.Text = "&One Step";
            this.btnStep.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnRun
            // 
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Location = new System.Drawing.Point(6, 138);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 15;
            this.btnRun.Text = "&Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(6, 83);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(72, 20);
            this.txtInterval.TabIndex = 16;
            this.txtInterval.Text = "10";
            this.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbLasers
            // 
            this.cbLasers.Checked = true;
            this.cbLasers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLasers.Location = new System.Drawing.Point(8, 40);
            this.cbLasers.Name = "cbLasers";
            this.cbLasers.Size = new System.Drawing.Size(120, 24);
            this.cbLasers.TabIndex = 17;
            this.cbLasers.Text = "&Show Beams";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFront7);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtFront6);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtFront5);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtFront4);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtFront3);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtFront2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtFront1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtFront0);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 245);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor reading:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "------------------------------------";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "------------------------------------";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "------------------------------------";
            // 
            // txtFront7
            // 
            this.txtFront7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront7.Location = new System.Drawing.Point(104, 204);
            this.txtFront7.Name = "txtFront7";
            this.txtFront7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront7.Size = new System.Drawing.Size(32, 16);
            this.txtFront7.TabIndex = 43;
            this.txtFront7.Text = "0";
            this.txtFront7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(8, 204);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(88, 16);
            this.label20.TabIndex = 42;
            this.label20.Text = "Traseiro 7:";
            // 
            // txtFront6
            // 
            this.txtFront6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront6.Location = new System.Drawing.Point(104, 188);
            this.txtFront6.Name = "txtFront6";
            this.txtFront6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront6.Size = new System.Drawing.Size(32, 16);
            this.txtFront6.TabIndex = 41;
            this.txtFront6.Text = "0";
            this.txtFront6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 188);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(88, 16);
            this.label18.TabIndex = 40;
            this.label18.Text = "Traseiro 6:";
            // 
            // txtFront5
            // 
            this.txtFront5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront5.Location = new System.Drawing.Point(104, 145);
            this.txtFront5.Name = "txtFront5";
            this.txtFront5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront5.Size = new System.Drawing.Size(32, 16);
            this.txtFront5.TabIndex = 39;
            this.txtFront5.Text = "0";
            this.txtFront5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 145);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 16);
            this.label16.TabIndex = 38;
            this.label16.Text = "Frontal 5:";
            // 
            // txtFront4
            // 
            this.txtFront4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront4.Location = new System.Drawing.Point(104, 129);
            this.txtFront4.Name = "txtFront4";
            this.txtFront4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront4.Size = new System.Drawing.Size(32, 16);
            this.txtFront4.TabIndex = 37;
            this.txtFront4.Text = "0";
            this.txtFront4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 129);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 16);
            this.label14.TabIndex = 36;
            this.label14.Text = "Frontal 4:";
            // 
            // txtFront3
            // 
            this.txtFront3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront3.Location = new System.Drawing.Point(104, 89);
            this.txtFront3.Name = "txtFront3";
            this.txtFront3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront3.Size = new System.Drawing.Size(32, 16);
            this.txtFront3.TabIndex = 35;
            this.txtFront3.Text = "0";
            this.txtFront3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(8, 89);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 16);
            this.label12.TabIndex = 34;
            this.label12.Text = "Frontal 3:";
            // 
            // txtFront2
            // 
            this.txtFront2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront2.Location = new System.Drawing.Point(104, 73);
            this.txtFront2.Name = "txtFront2";
            this.txtFront2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront2.Size = new System.Drawing.Size(32, 16);
            this.txtFront2.TabIndex = 33;
            this.txtFront2.Text = "0";
            this.txtFront2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 32;
            this.label10.Text = "Frontal 2:";
            // 
            // txtFront1
            // 
            this.txtFront1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront1.Location = new System.Drawing.Point(104, 32);
            this.txtFront1.Name = "txtFront1";
            this.txtFront1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront1.Size = new System.Drawing.Size(32, 16);
            this.txtFront1.TabIndex = 31;
            this.txtFront1.Text = "0";
            this.txtFront1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 30;
            this.label7.Text = "Frontal 1:";
            // 
            // txtFront0
            // 
            this.txtFront0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFront0.Location = new System.Drawing.Point(104, 16);
            this.txtFront0.Name = "txtFront0";
            this.txtFront0.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFront0.Size = new System.Drawing.Size(32, 16);
            this.txtFront0.TabIndex = 27;
            this.txtFront0.Text = "0";
            this.txtFront0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 24;
            this.label1.Text = "Frontal 0:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtTime);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtSpeed);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtAngle);
            this.groupBox2.Location = new System.Drawing.Point(8, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(144, 112);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output:";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(96, 77);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 16);
            this.label15.TabIndex = 37;
            this.label15.Text = "0";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(8, 77);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 16);
            this.label17.TabIndex = 36;
            this.label17.Text = "Roda Esq:";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(96, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 16);
            this.label11.TabIndex = 35;
            this.label11.Text = "0";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 16);
            this.label13.TabIndex = 34;
            this.label13.Text = "Roda Dir:";
            // 
            // txtTime
            // 
            this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.Location = new System.Drawing.Point(94, 48);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(40, 16);
            this.txtTime.TabIndex = 33;
            this.txtTime.Text = "0";
            this.txtTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 32;
            this.label8.Text = "Time (s):";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed.Location = new System.Drawing.Point(94, 32);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(40, 16);
            this.txtSpeed.TabIndex = 31;
            this.txtSpeed.Text = "30,00";
            this.txtSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 30;
            this.label6.Text = "Speed (%):";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Angle (degrees):";
            // 
            // txtAngle
            // 
            this.txtAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAngle.Location = new System.Drawing.Point(94, 16);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(40, 16);
            this.txtAngle.TabIndex = 29;
            this.txtAngle.Text = "0,00";
            this.txtAngle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbComandos
            // 
            this.gbComandos.Controls.Add(this.label21);
            this.gbComandos.Controls.Add(this.label23);
            this.gbComandos.Controls.Add(this.label22);
            this.gbComandos.Controls.Add(this.cbTrajeto);
            this.gbComandos.Controls.Add(this.btnReset);
            this.gbComandos.Controls.Add(this.label4);
            this.gbComandos.Controls.Add(this.btnStep);
            this.gbComandos.Controls.Add(this.cbLasers);
            this.gbComandos.Controls.Add(this.btnRun);
            this.gbComandos.Controls.Add(this.txtInterval);
            this.gbComandos.Location = new System.Drawing.Point(160, 12);
            this.gbComandos.Name = "gbComandos";
            this.gbComandos.Size = new System.Drawing.Size(144, 200);
            this.gbComandos.TabIndex = 26;
            this.gbComandos.TabStop = false;
            this.gbComandos.Text = "Tools:";
            // 
            // cbTrajeto
            // 
            this.cbTrajeto.Location = new System.Drawing.Point(8, 16);
            this.cbTrajeto.Name = "cbTrajeto";
            this.cbTrajeto.Size = new System.Drawing.Size(120, 24);
            this.cbTrajeto.TabIndex = 19;
            this.cbTrajeto.Text = "&Track Path";
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(6, 167);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Rest&art";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Move Interval (ms):";
            // 
            // pbRobot
            // 
            this.pbRobot.BackColor = System.Drawing.Color.Transparent;
            this.pbRobot.Image = ((System.Drawing.Image)(resources.GetObject("pbRobot.Image")));
            this.pbRobot.Location = new System.Drawing.Point(640, 450);
            this.pbRobot.Name = "pbRobot";
            this.pbRobot.Size = new System.Drawing.Size(10, 10);
            this.pbRobot.TabIndex = 11;
            this.pbRobot.TabStop = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lstFalha
            // 
            this.lstFalha.FormattingEnabled = true;
            this.lstFalha.Items.AddRange(new object[] {
            "Sensor frontal - 0",
            "Sensor frontal - 1",
            "Sensor frontal - 2",
            "Sensor frontal - 3",
            "Sensor frontal - 4",
            "Sensor frontal - 5",
            "Sensor traseiro - 6",
            "Sensor traseiro - 7"});
            this.lstFalha.Location = new System.Drawing.Point(6, 19);
            this.lstFalha.Name = "lstFalha";
            this.lstFalha.Size = new System.Drawing.Size(128, 124);
            this.lstFalha.TabIndex = 27;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstFalha);
            this.groupBox3.Location = new System.Drawing.Point(160, 218);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 153);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Falha sensor";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(310, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(265, 296);
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(87, 114);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(30, 13);
            this.label21.TabIndex = 20;
            this.label21.Text = "O";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(87, 143);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(30, 13);
            this.label22.TabIndex = 21;
            this.label22.Text = "R";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(87, 172);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(30, 13);
            this.label23.TabIndex = 22;
            this.label23.Text = "A";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1186, 557);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbComandos);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbRobot);
            this.Controls.Add(this.pbTerrain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fault Tolerant AGV";
            ((System.ComponentModel.ISupportInitialize)(this.pbTerrain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbComandos.ResumeLayout(false);
            this.gbComandos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRobot)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        // Run one epoch of the Fuzzy Inference System 
        private void DoInference()
        {
            double NewAngle;
            //Direita
            fis.DoInference(Convert.ToDouble(txtFront5.Text),
                //Esquerda
                Convert.ToDouble(txtFront0.Text),
                //Frente
                ((Convert.ToDouble(txtFront2.Text) + Convert.ToDouble(txtFront3.Text)) / 2),
                out NewAngle, out Speed);
            txtAngle.Text = NewAngle.ToString("##0.#0");
            Angle += NewAngle;
        }

        // AGV's terrain drawing
        private void pbTerrain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pbTerrain.Image = CopyImage(OriginalMap);
            LastX = e.X;
            LastY = e.Y;
        }

        // AGV's terrain drawing
        private void pbTerrain_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(pbTerrain.Image);

            Color c = Color.Yellow;

            if (e.Button == MouseButtons.Left)
                c = Color.White;
            else if (e.Button == MouseButtons.Right)
                c = Color.Black;

            if (c != Color.Yellow)
            {
                g.FillRectangle(new SolidBrush(c), e.X - 40, e.Y - 40, 80, 80);

                LastX = e.X;
                LastY = e.Y;

                g.DrawImage(pbTerrain.Image, 0, 0);
                OriginalMap = CopyImage(pbTerrain.Image as Bitmap);
                pbTerrain.Refresh();
                g.Dispose();
            }

        }

        // Getting sensors measures
        private void GetMeasures()
        {
            // Getting AGV's position
            pbTerrain.Image = CopyImage(OriginalMap);
            Bitmap b = pbTerrain.Image as Bitmap;
            Point pPos = new Point(pbRobot.Left - pbTerrain.Left + 5, pbRobot.Top - pbTerrain.Top + 5);

            // AGV on the wall
            if ((b.GetPixel(pPos.X, pPos.Y).R == 0) && (b.GetPixel(pPos.X, pPos.Y).G == 0) && (b.GetPixel(pPos.X, pPos.Y).B == 0))
            {
                if (btnRun.Text != RunLabel)
                {
                    btnRun_Click(btnRun, null);
                }
                string Msg = "The vehicle is on the solid area!";
                MessageBox.Show(Msg, "Error!");
                throw new Exception(Msg);
            }

            // Getting distances
            Point pFrontObstacle1 = GetObstacle(new Point(pPos.X - 5, pPos.Y), b, -1, 0);
            Point pFrontObstacle2 = GetObstacle(new Point(pPos.X + 5, pPos.Y), b, -1, 0);

            Point pLeft45Obstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, -1, 45);
            Point pRight45Obstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, -1, -45);

            Point pLeftObstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, 1, 90);
            Point pRightObstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, 1, -90);


            Point pBehindObstacle1 = GetObstacle(new Point(pPos.X + 5, pPos.Y), b, 1, 0);
            Point pBehindObstacle2 = GetObstacle(new Point(pPos.X - 5, pPos.Y), b, 1, 0);


            // Showing beams
            Graphics g = Graphics.FromImage(b);
            if (cbLasers.Checked)
            {
                //Adicionando retas
                //Forntais
                //g.DrawLine(new Pen(Color.Green, 1), pFrontObstacle1, pPos);
                g.DrawLine(new Pen(Color.Green, 1), pFrontObstacle1, new Point(pPos.X - 5, pPos.Y));
                //g.DrawLine(new Pen(Color.Green, 1), pFrontObstacle2, pPos);
                g.DrawLine(new Pen(Color.Green, 1), pFrontObstacle2, new Point(pPos.X + 5, pPos.Y));

                //Traseiras
                //g.DrawLine(new Pen(Color.Green, 1), pBehindObstacle1, pPos);
                //g.DrawLine(new Pen(Color.Green, 1), pBehindObstacle2, pPos);
                g.DrawLine(new Pen(Color.Green, 1), pBehindObstacle1, new Point(pPos.X + 5, pPos.Y));
                g.DrawLine(new Pen(Color.Green, 1), pBehindObstacle2, new Point(pPos.X - 5, pPos.Y));

                //Laterais
                g.DrawLine(new Pen(Color.Red, 1), pLeftObstacle, pPos);
                g.DrawLine(new Pen(Color.Red, 1), pRightObstacle, pPos);
                g.DrawLine(new Pen(Color.Red, 1), pLeft45Obstacle, pPos);
                g.DrawLine(new Pen(Color.Red, 1), pRight45Obstacle, pPos);
            }

            // Drawing AGV
            if (btnRun.Text != RunLabel)
            {
                g.FillEllipse(new SolidBrush(Color.Navy), pPos.X - 5, pPos.Y - 5, 10, 10);
            }

            g.DrawImage(b, 0, 0);
            g.Dispose();

            pbTerrain.Refresh();

            // Updating distances texts
            //Esquerda
            txtFront0.Text = GetDistance(pPos, pLeftObstacle).ToString();
            //Esquerda 45
            txtFront1.Text = GetDistance(pPos, pLeft45Obstacle).ToString();
            //Frente
            txtFront2.Text = GetDistance(pPos, pFrontObstacle1).ToString();
            txtFront3.Text = GetDistance(pPos, pFrontObstacle2).ToString();
            //Direita 45
            txtFront4.Text = GetDistance(pPos, pRight45Obstacle).ToString();
            //Direita
            txtFront5.Text = GetDistance(pPos, pRightObstacle).ToString();
            //Trazeiro
            txtFront6.Text = GetDistance(pPos, pBehindObstacle1).ToString();
            txtFront7.Text = GetDistance(pPos, pBehindObstacle2).ToString();

            //txtFront0.Text = GetDistance(pPos, pFrontObstacle).ToString();
            //txtLeft.Text = GetDistance(pPos, pLeftObstacle).ToString();
            //txtRight.Text = GetDistance(pPos, pRightObstacle).ToString();
        }

        // Calculating distances
        private int GetDistance(Point p1, Point p2)
        {
            return (Convert.ToInt32(Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2))));
        }

        // Finding obstacles
        private Point GetObstacle(Point Start, Bitmap Map, int Inc, int AngleOffset)
        {
            Point p = new Point(Start.X, Start.Y);

            double rad = ((Angle + 90 + AngleOffset) * Math.PI) / 180;
            int IncX = 0;
            int IncY = 0;
            int Offset = 0;

            while ((p.X + IncX >= 0) && (p.X + IncX < Map.Width) && (p.Y + IncY >= 0) && (p.Y + IncY < Map.Height))
            {
                if ((Map.GetPixel(p.X + IncX, p.Y + IncY).R == 0) && (Map.GetPixel(p.X + IncX, p.Y + IncY).G == 0) && (Map.GetPixel(p.X + IncX, p.Y + IncY).B == 0))
                    break;
                Offset += Inc;
                IncX = Convert.ToInt32(Offset * Math.Cos(rad));
                IncY = Convert.ToInt32(Offset * Math.Sin(rad));
            }
            p.X += IncX;
            p.Y += IncY;

            return p;
        }

        // Copying bitmaps
        private Bitmap CopyImage(Bitmap Src)
        {
            return new Bitmap(Src);
        }


        // Restarting the AGVs simulation
        private void btnReset_Click(object sender, System.EventArgs e)
        {
            Angle = 0;
            pbTerrain.Image = new Bitmap(InitialMap);
            OriginalMap = new Bitmap(InitialMap);
            FirstInference = true;
            pbRobot.Location = InitialPos;
            txtFront0.Text = "0";
            txtFront1.Text = "0";
            txtFront2.Text = "0";
            txtFront3.Text = "0";
            txtFront4.Text = "0";
            txtFront5.Text = "0";
            txtFront6.Text = "0";
            txtFront7.Text = "0";

            //txtLeft.Text = "0";
            //txtRight.Text = "0";
            txtAngle.Text = "0,00";
        }

        // Moving the AGV
        private void MoveAGV()
        {
            double rad = ((Angle + 90) * Math.PI) / 180;
            int Offset = 0;
            int Inc = -Convert.ToInt32(Speed / 10);

            Offset += Inc;
            int IncX = Convert.ToInt32(Offset * Math.Cos(rad));
            int IncY = Convert.ToInt32(Offset * Math.Sin(rad));

            // Leaving the track 
            if (cbTrajeto.Checked)
            {
                Graphics g = Graphics.FromImage(OriginalMap);
                Point p1 = new Point(pbRobot.Left - pbTerrain.Left + pbRobot.Width / 2, pbRobot.Top - pbTerrain.Top + pbRobot.Height / 2);
                Point p2 = new Point(p1.X + IncX, p1.Y + IncY);
                g.DrawLine(new Pen(new SolidBrush(Color.Blue)), p1, p2);
                g.DrawImage(OriginalMap, 0, 0);
                g.Dispose();
            }

            pbRobot.Top = pbRobot.Top + IncY;
            pbRobot.Left = pbRobot.Left + IncX;
        }

        // Starting and stopping the AGV's moviment a
        private void btnRun_Click(object sender, System.EventArgs e)
        {
            Button b = (sender as Button);

            if (b.Text == RunLabel)
            {
                b.Text = "&Stop";
                btnStep.Enabled = false;
                btnReset.Enabled = false;
                txtInterval.Enabled = false;
                cbLasers.Enabled = false;
                cbTrajeto.Enabled = false;
                lstFalha.Enabled = false;
                pbRobot.Hide();
                StartMovement();
            }
            else
            {
                StopMovement();
                b.Text = RunLabel;
                lstFalha.Enabled = true;
                btnReset.Enabled = true;
                btnStep.Enabled = true;
                txtInterval.Enabled = true;
                cbLasers.Enabled = true;
                cbTrajeto.Enabled = true;
                pbRobot.Show();
                pbTerrain.Image = CopyImage(OriginalMap);
                pbTerrain.Refresh();
            }
        }

        // One step of the AGV
        private void button3_Click(object sender, System.EventArgs e)
        {
            pbRobot.Hide();
            AGVStep();
            pbRobot.Show();
        }

        // Thread for the AGVs movement
        private void StartMovement()
        {
            thMovement = new Thread(new ThreadStart(MoveCycle));
            thMovement.IsBackground = true;
            thMovement.Priority = ThreadPriority.AboveNormal;
            thMovement.Start();
            Time = 0;
            timer.Enabled = true;
        }

        // Thread main cycle
        private void MoveCycle()
        {
            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    MethodInvoker mi = new MethodInvoker(AGVStep);
                    this.BeginInvoke(mi);
                    Thread.Sleep(Convert.ToInt32(txtInterval.Text));
                }
            }
            catch (ThreadInterruptedException)
            {
            }
        }

        // One step of the AGV
        private void AGVStep()
        {
            if (FirstInference) GetMeasures();

            try
            {
                DoInference();
                MoveAGV();
                GetMeasures();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        // Stop background thread
        private void StopMovement()
        {
            if (thMovement != null)
            {
                thMovement.Interrupt();
                thMovement = null;
            }
            timer.Enabled = false;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Time += 100;
            txtTime.Text = (Time / 1000.0).ToString("#0.#0");
        }

    }
}
