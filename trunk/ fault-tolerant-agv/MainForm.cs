//Fault Tolerant AGV
//Utilizado como projeto matriz:
//      Fuzzy Auto Guided Vehicle Sample
//      AForge.NET framework
//      http://www.aforgenet.com/framework/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;


namespace AGVFaultTolerant
{
    public class MainForm : System.Windows.Forms.Form
    {

        #region Private members


        DataTable dtResultados;
        int ctControleParent = 0;
        int ctControleClones = 0;
        int ctPaisDecrescente = 3;
        private bool _bCiclo = false;
        private bool _ponto1 = true;
        private bool _ponto2 = false;
        Point p1, p2;
        List<int> lstDist;

        private string RunLabel;
        private bool _traz;
        private bool _virou;
        private Point InitialPos;
        private bool FirstInference;
        private int LastX;
        private int LastY;
        private int Time = 0;
        private double Angle, Speed;
        private Bitmap OriginalMap, InitialMap;
        private Thread thMovement;
        private EA _ga;
        private bool[] sensors;
        CircuitoChromosome bestIndividualLatGeneration;


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
        private GroupBox groupBox3;
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
        private PictureBox pictureBox2;
        private Label label23;
        private Label label22;
        private Label label21;
        private GroupBox groupBox6;
        private Label lblFitness4;
        private Label lblGeneration;
        private Label label19;
        private Label label24;
        private PictureBox pbLimiar0;
        private PictureBox pbLimiar5;
        private PictureBox pbLimiar4;
        private PictureBox pbLimiar1;
        private PictureBox pbLimiar2;
        private PictureBox pbLimiar3;
        private TextBox txtGeneAgv;
        private Label lblGeneCircuito;
        private GroupBox groupBox4;
        private PictureBox pbMotor2;
        private PictureBox pbMotor1;
        private Label label27;
        private Label label28;
        private GroupBox groupBox5;
        private Label label25;
        private TextBox txtLimiarDistancia;
        private CheckBox chkFalhaSensorFrontal5;
        private CheckBox chkFalhaSensorFrontal3;
        private CheckBox chkFalhaSensorFrontal2;
        private CheckBox chkFalhaSensorFrontal1;
        private CheckBox chkFalhaSensorFrontal4;
        private CheckBox chkFalhaSensorFrontal0;
        private Label lblNumeroPortasAlteradas;
        private Label label17;
        private TextBox txtk;
        private Label label11;
        private Label label13;
        private TextBox txtTempoImplementacao;
        private Label label15;
        private Label label18;
        private Label lblCtViraEsq;
        private Label lblCtViraDir;
        private TextBox txtVelFim;
        private Label label20;
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
            sensors = new bool[8];
            sensors[0] = false;
            sensors[1] = false;
            sensors[2] = false;
            sensors[3] = false;
            sensors[4] = false;
            sensors[5] = false;
            sensors[6] = false;
            sensors[7] = false;
            _traz = false;

            //_ga = new GA(16, 3, sensors);
            //_ga = new EA(4, 3, sensors);
            //_ga = new EA(3, 3, sensors);
            _ga = new EA(3, 2, sensors);
            ctPaisDecrescente = 3;
            //_ga.K = 13;
            //_ga.K = 13;
            //_ga.K = 13;
            _ga.K = Convert.ToInt32(txtk.Text);
            dtResultados = new DataTable();
            dtResultados.Columns.Add("geracao", typeof(int));
            dtResultados.Columns.Add("Bestfitness", typeof(int));
            dtResultados.Columns.Add("Averagefitness", typeof(int));



            FirstInference = true;
            pbRobot.Top = pbTerrain.Bottom - 100;
            pbRobot.Left = pbTerrain.Left + 100;
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
            this.btnStep = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.cbLasers = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.txtTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.Label();
            this.gbComandos = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cbTrajeto = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkFalhaSensorFrontal5 = new System.Windows.Forms.CheckBox();
            this.chkFalhaSensorFrontal3 = new System.Windows.Forms.CheckBox();
            this.chkFalhaSensorFrontal2 = new System.Windows.Forms.CheckBox();
            this.chkFalhaSensorFrontal1 = new System.Windows.Forms.CheckBox();
            this.chkFalhaSensorFrontal4 = new System.Windows.Forms.CheckBox();
            this.chkFalhaSensorFrontal0 = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblNumeroPortasAlteradas = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblFitness4 = new System.Windows.Forms.Label();
            this.lblGeneration = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtGeneAgv = new System.Windows.Forms.TextBox();
            this.lblGeneCircuito = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pbMotor2 = new System.Windows.Forms.PictureBox();
            this.pbMotor1 = new System.Windows.Forms.PictureBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTempoImplementacao = new System.Windows.Forms.TextBox();
            this.txtk = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.txtLimiarDistancia = new System.Windows.Forms.TextBox();
            this.pbLimiar3 = new System.Windows.Forms.PictureBox();
            this.pbLimiar2 = new System.Windows.Forms.PictureBox();
            this.pbLimiar1 = new System.Windows.Forms.PictureBox();
            this.pbLimiar4 = new System.Windows.Forms.PictureBox();
            this.pbLimiar5 = new System.Windows.Forms.PictureBox();
            this.pbLimiar0 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pbRobot = new System.Windows.Forms.PictureBox();
            this.pbTerrain = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblCtViraEsq = new System.Windows.Forms.Label();
            this.lblCtViraDir = new System.Windows.Forms.Label();
            this.txtVelFim = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbComandos.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMotor2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMotor1)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRobot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTerrain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStep
            // 
            this.btnStep.Enabled = false;
            this.btnStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep.Location = new System.Drawing.Point(6, 109);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 23);
            this.btnStep.TabIndex = 14;
            this.btnStep.Text = "&One Step";
            this.btnStep.Visible = false;
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
            this.txtInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidaNumero_KeyPress);
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
            this.groupBox1.Size = new System.Drawing.Size(144, 204);
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
            // txtTime
            // 
            this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.Location = new System.Drawing.Point(219, 66);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(40, 18);
            this.txtTime.TabIndex = 33;
            this.txtTime.Text = "0";
            this.txtTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(123, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 32;
            this.label8.Text = "Tempo (s):";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed.Location = new System.Drawing.Point(89, 66);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(28, 16);
            this.txtSpeed.TabIndex = 31;
            this.txtSpeed.Text = "12,0";
            this.txtSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 30;
            this.label6.Text = "Velocidade:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(121, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Angulo (graus):";
            // 
            // txtAngle
            // 
            this.txtAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAngle.Location = new System.Drawing.Point(219, 38);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(40, 17);
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
            // label21
            // 
            this.label21.Enabled = false;
            this.label21.Location = new System.Drawing.Point(87, 114);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(30, 13);
            this.label21.TabIndex = 20;
            this.label21.Text = "O";
            this.label21.Visible = false;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(87, 172);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(30, 13);
            this.label23.TabIndex = 22;
            this.label23.Text = "A";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(87, 143);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(30, 13);
            this.label22.TabIndex = 21;
            this.label22.Text = "R";
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
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkFalhaSensorFrontal5);
            this.groupBox3.Controls.Add(this.chkFalhaSensorFrontal3);
            this.groupBox3.Controls.Add(this.chkFalhaSensorFrontal2);
            this.groupBox3.Controls.Add(this.chkFalhaSensorFrontal1);
            this.groupBox3.Controls.Add(this.chkFalhaSensorFrontal4);
            this.groupBox3.Controls.Add(this.chkFalhaSensorFrontal0);
            this.groupBox3.Location = new System.Drawing.Point(160, 212);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 167);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Falha sensor";
            // 
            // chkFalhaSensorFrontal5
            // 
            this.chkFalhaSensorFrontal5.Location = new System.Drawing.Point(6, 141);
            this.chkFalhaSensorFrontal5.Name = "chkFalhaSensorFrontal5";
            this.chkFalhaSensorFrontal5.Size = new System.Drawing.Size(120, 26);
            this.chkFalhaSensorFrontal5.TabIndex = 21;
            this.chkFalhaSensorFrontal5.Text = "Falha sensor 5";
            this.chkFalhaSensorFrontal5.CheckedChanged += new System.EventHandler(this.chkFalhaSensorFrontal5_CheckedChanged);
            // 
            // chkFalhaSensorFrontal3
            // 
            this.chkFalhaSensorFrontal3.Location = new System.Drawing.Point(6, 93);
            this.chkFalhaSensorFrontal3.Name = "chkFalhaSensorFrontal3";
            this.chkFalhaSensorFrontal3.Size = new System.Drawing.Size(120, 26);
            this.chkFalhaSensorFrontal3.TabIndex = 20;
            this.chkFalhaSensorFrontal3.Text = "Falha sensor 3";
            this.chkFalhaSensorFrontal3.CheckedChanged += new System.EventHandler(this.chkFalhaSensorFrontal3_CheckedChanged);
            // 
            // chkFalhaSensorFrontal2
            // 
            this.chkFalhaSensorFrontal2.Location = new System.Drawing.Point(6, 68);
            this.chkFalhaSensorFrontal2.Name = "chkFalhaSensorFrontal2";
            this.chkFalhaSensorFrontal2.Size = new System.Drawing.Size(120, 26);
            this.chkFalhaSensorFrontal2.TabIndex = 20;
            this.chkFalhaSensorFrontal2.Text = "Falha sensor 2";
            this.chkFalhaSensorFrontal2.CheckedChanged += new System.EventHandler(this.chkFalhaSensorFrontal2_CheckedChanged);
            // 
            // chkFalhaSensorFrontal1
            // 
            this.chkFalhaSensorFrontal1.Location = new System.Drawing.Point(6, 42);
            this.chkFalhaSensorFrontal1.Name = "chkFalhaSensorFrontal1";
            this.chkFalhaSensorFrontal1.Size = new System.Drawing.Size(120, 26);
            this.chkFalhaSensorFrontal1.TabIndex = 20;
            this.chkFalhaSensorFrontal1.Text = "Falha sensor 1";
            this.chkFalhaSensorFrontal1.CheckedChanged += new System.EventHandler(this.chkFalhaSensorFrontal1_CheckedChanged);
            // 
            // chkFalhaSensorFrontal4
            // 
            this.chkFalhaSensorFrontal4.Location = new System.Drawing.Point(6, 119);
            this.chkFalhaSensorFrontal4.Name = "chkFalhaSensorFrontal4";
            this.chkFalhaSensorFrontal4.Size = new System.Drawing.Size(120, 26);
            this.chkFalhaSensorFrontal4.TabIndex = 20;
            this.chkFalhaSensorFrontal4.Text = "Falha sensor 4";
            this.chkFalhaSensorFrontal4.CheckedChanged += new System.EventHandler(this.chkFalhaSensorFrontal4_CheckedChanged);
            // 
            // chkFalhaSensorFrontal0
            // 
            this.chkFalhaSensorFrontal0.Location = new System.Drawing.Point(6, 19);
            this.chkFalhaSensorFrontal0.Name = "chkFalhaSensorFrontal0";
            this.chkFalhaSensorFrontal0.Size = new System.Drawing.Size(120, 26);
            this.chkFalhaSensorFrontal0.TabIndex = 20;
            this.chkFalhaSensorFrontal0.Text = "Falha sensor 0";
            this.chkFalhaSensorFrontal0.CheckedChanged += new System.EventHandler(this.chkFalhaSensorFrontal0_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtTime);
            this.groupBox6.Controls.Add(this.lblNumeroPortasAlteradas);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.txtAngle);
            this.groupBox6.Controls.Add(this.txtSpeed);
            this.groupBox6.Controls.Add(this.lblFitness4);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.lblGeneration);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Location = new System.Drawing.Point(310, 308);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(265, 101);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Melhor Cromossomo:";
            // 
            // lblNumeroPortasAlteradas
            // 
            this.lblNumeroPortasAlteradas.AutoSize = true;
            this.lblNumeroPortasAlteradas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroPortasAlteradas.Location = new System.Drawing.Point(219, 16);
            this.lblNumeroPortasAlteradas.Name = "lblNumeroPortasAlteradas";
            this.lblNumeroPortasAlteradas.Size = new System.Drawing.Size(14, 13);
            this.lblNumeroPortasAlteradas.TabIndex = 63;
            this.lblNumeroPortasAlteradas.Text = "?";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(121, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(103, 13);
            this.label17.TabIndex = 62;
            this.label17.Text = "Num.  de Portas alt.:";
            // 
            // lblFitness4
            // 
            this.lblFitness4.AutoSize = true;
            this.lblFitness4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFitness4.Location = new System.Drawing.Point(94, 42);
            this.lblFitness4.Name = "lblFitness4";
            this.lblFitness4.Size = new System.Drawing.Size(21, 13);
            this.lblFitness4.TabIndex = 3;
            this.lblFitness4.Text = "12";
            // 
            // lblGeneration
            // 
            this.lblGeneration.AutoSize = true;
            this.lblGeneration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneration.Location = new System.Drawing.Point(94, 16);
            this.lblGeneration.Name = "lblGeneration";
            this.lblGeneration.Size = new System.Drawing.Size(14, 13);
            this.lblGeneration.TabIndex = 61;
            this.lblGeneration.Text = "1";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 42);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Func. de Ava.:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(56, 13);
            this.label24.TabIndex = 60;
            this.label24.Text = "Gerações:";
            // 
            // txtGeneAgv
            // 
            this.txtGeneAgv.Location = new System.Drawing.Point(8, 411);
            this.txtGeneAgv.MaxLength = 1000;
            this.txtGeneAgv.Multiline = true;
            this.txtGeneAgv.Name = "txtGeneAgv";
            this.txtGeneAgv.Size = new System.Drawing.Size(567, 98);
            this.txtGeneAgv.TabIndex = 128;
            this.txtGeneAgv.Text = resources.GetString("txtGeneAgv.Text");
            // 
            // lblGeneCircuito
            // 
            this.lblGeneCircuito.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneCircuito.Location = new System.Drawing.Point(12, 393);
            this.lblGeneCircuito.Name = "lblGeneCircuito";
            this.lblGeneCircuito.Size = new System.Drawing.Size(129, 16);
            this.lblGeneCircuito.TabIndex = 129;
            this.lblGeneCircuito.Text = "Gene do Circuito:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pbMotor2);
            this.groupBox4.Controls.Add(this.pbMotor1);
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Location = new System.Drawing.Point(7, 218);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(146, 101);
            this.groupBox4.TabIndex = 130;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Atuadores:";
            // 
            // pbMotor2
            // 
            this.pbMotor2.BackColor = System.Drawing.Color.Transparent;
            this.pbMotor2.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbMotor2.Location = new System.Drawing.Point(56, 61);
            this.pbMotor2.Name = "pbMotor2";
            this.pbMotor2.Size = new System.Drawing.Size(10, 10);
            this.pbMotor2.TabIndex = 69;
            this.pbMotor2.TabStop = false;
            // 
            // pbMotor1
            // 
            this.pbMotor1.BackColor = System.Drawing.Color.Transparent;
            this.pbMotor1.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbMotor1.Location = new System.Drawing.Point(56, 19);
            this.pbMotor1.Name = "pbMotor1";
            this.pbMotor1.Size = new System.Drawing.Size(10, 10);
            this.pbMotor1.TabIndex = 68;
            this.pbMotor1.TabStop = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 58);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(43, 13);
            this.label27.TabIndex = 2;
            this.label27.Text = "Motor2:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 16);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(43, 13);
            this.label28.TabIndex = 60;
            this.label28.Text = "Motor1:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtVelFim);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtTempoImplementacao);
            this.groupBox5.Controls.Add(this.txtk);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.label25);
            this.groupBox5.Controls.Add(this.txtLimiarDistancia);
            this.groupBox5.Location = new System.Drawing.Point(7, 515);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(297, 101);
            this.groupBox5.TabIndex = 131;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Limiar Distancia Sensor:";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(10, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(125, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "Tempo Imp:";
            // 
            // txtTempoImplementacao
            // 
            this.txtTempoImplementacao.Location = new System.Drawing.Point(10, 75);
            this.txtTempoImplementacao.Name = "txtTempoImplementacao";
            this.txtTempoImplementacao.Size = new System.Drawing.Size(72, 20);
            this.txtTempoImplementacao.TabIndex = 23;
            this.txtTempoImplementacao.Text = "20";
            this.txtTempoImplementacao.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTempoImplementacao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidaNumero_KeyPress);
            // 
            // txtk
            // 
            this.txtk.Location = new System.Drawing.Point(159, 32);
            this.txtk.Name = "txtk";
            this.txtk.Size = new System.Drawing.Size(72, 20);
            this.txtk.TabIndex = 22;
            this.txtk.Text = "17";
            this.txtk.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidaNumero_KeyPress);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(156, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "K:";
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(10, 16);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(125, 13);
            this.label25.TabIndex = 20;
            this.label25.Text = "Distancia Limiar:";
            // 
            // txtLimiarDistancia
            // 
            this.txtLimiarDistancia.Location = new System.Drawing.Point(10, 32);
            this.txtLimiarDistancia.Name = "txtLimiarDistancia";
            this.txtLimiarDistancia.Size = new System.Drawing.Size(72, 20);
            this.txtLimiarDistancia.TabIndex = 19;
            this.txtLimiarDistancia.Text = "100";
            this.txtLimiarDistancia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLimiarDistancia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidaNumero_KeyPress);
            // 
            // pbLimiar3
            // 
            this.pbLimiar3.BackColor = System.Drawing.Color.Transparent;
            this.pbLimiar3.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbLimiar3.Location = new System.Drawing.Point(470, 59);
            this.pbLimiar3.Name = "pbLimiar3";
            this.pbLimiar3.Size = new System.Drawing.Size(10, 10);
            this.pbLimiar3.TabIndex = 68;
            this.pbLimiar3.TabStop = false;
            // 
            // pbLimiar2
            // 
            this.pbLimiar2.BackColor = System.Drawing.Color.Transparent;
            this.pbLimiar2.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbLimiar2.Location = new System.Drawing.Point(397, 59);
            this.pbLimiar2.Name = "pbLimiar2";
            this.pbLimiar2.Size = new System.Drawing.Size(10, 10);
            this.pbLimiar2.TabIndex = 67;
            this.pbLimiar2.TabStop = false;
            // 
            // pbLimiar1
            // 
            this.pbLimiar1.BackColor = System.Drawing.Color.Transparent;
            this.pbLimiar1.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbLimiar1.Location = new System.Drawing.Point(381, 66);
            this.pbLimiar1.Name = "pbLimiar1";
            this.pbLimiar1.Size = new System.Drawing.Size(10, 10);
            this.pbLimiar1.TabIndex = 66;
            this.pbLimiar1.TabStop = false;
            // 
            // pbLimiar4
            // 
            this.pbLimiar4.BackColor = System.Drawing.Color.Transparent;
            this.pbLimiar4.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbLimiar4.Location = new System.Drawing.Point(486, 66);
            this.pbLimiar4.Name = "pbLimiar4";
            this.pbLimiar4.Size = new System.Drawing.Size(10, 10);
            this.pbLimiar4.TabIndex = 65;
            this.pbLimiar4.TabStop = false;
            // 
            // pbLimiar5
            // 
            this.pbLimiar5.BackColor = System.Drawing.Color.Transparent;
            this.pbLimiar5.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbLimiar5.Location = new System.Drawing.Point(500, 87);
            this.pbLimiar5.Name = "pbLimiar5";
            this.pbLimiar5.Size = new System.Drawing.Size(10, 10);
            this.pbLimiar5.TabIndex = 64;
            this.pbLimiar5.TabStop = false;
            // 
            // pbLimiar0
            // 
            this.pbLimiar0.BackColor = System.Drawing.Color.Transparent;
            this.pbLimiar0.Image = global::FaultTolerantAGV.Properties.Resources.bolaVermelha;
            this.pbLimiar0.Location = new System.Drawing.Point(372, 87);
            this.pbLimiar0.Name = "pbLimiar0";
            this.pbLimiar0.Size = new System.Drawing.Size(10, 10);
            this.pbLimiar0.TabIndex = 63;
            this.pbLimiar0.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::FaultTolerantAGV.Properties.Resources.robo;
            this.pictureBox2.Location = new System.Drawing.Point(310, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(265, 296);
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            // 
            // pbRobot
            // 
            this.pbRobot.BackColor = System.Drawing.Color.Transparent;
            this.pbRobot.Image = ((System.Drawing.Image)(resources.GetObject("pbRobot.Image")));
            this.pbRobot.Location = new System.Drawing.Point(660, 411);
            this.pbRobot.Name = "pbRobot";
            this.pbRobot.Size = new System.Drawing.Size(10, 10);
            this.pbRobot.TabIndex = 11;
            this.pbRobot.TabStop = false;
            // 
            // pbTerrain
            // 
            this.pbTerrain.BackColor = System.Drawing.SystemColors.ControlText;
            this.pbTerrain.ErrorImage = null;
            this.pbTerrain.Image = global::FaultTolerantAGV.Properties.Resources.Mapa4;
            this.pbTerrain.InitialImage = null;
            this.pbTerrain.Location = new System.Drawing.Point(581, 8);
            this.pbTerrain.Name = "pbTerrain";
            this.pbTerrain.Size = new System.Drawing.Size(654, 570);
            this.pbTerrain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTerrain.TabIndex = 10;
            this.pbTerrain.TabStop = false;
            this.pbTerrain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTerrain_MouseDown);
            this.pbTerrain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTerrain_MouseMove);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 346);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 132;
            this.label15.Text = "Ct Vira Esq:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(5, 324);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 13);
            this.label18.TabIndex = 133;
            this.label18.Text = "Ct Vira Dir:";
            // 
            // lblCtViraEsq
            // 
            this.lblCtViraEsq.AutoSize = true;
            this.lblCtViraEsq.Location = new System.Drawing.Point(73, 346);
            this.lblCtViraEsq.Name = "lblCtViraEsq";
            this.lblCtViraEsq.Size = new System.Drawing.Size(13, 13);
            this.lblCtViraEsq.TabIndex = 134;
            this.lblCtViraEsq.Text = "0";
            // 
            // lblCtViraDir
            // 
            this.lblCtViraDir.AutoSize = true;
            this.lblCtViraDir.Location = new System.Drawing.Point(73, 324);
            this.lblCtViraDir.Name = "lblCtViraDir";
            this.lblCtViraDir.Size = new System.Drawing.Size(13, 13);
            this.lblCtViraDir.TabIndex = 135;
            this.lblCtViraDir.Text = "0";
            // 
            // txtVelFim
            // 
            this.txtVelFim.Location = new System.Drawing.Point(162, 75);
            this.txtVelFim.Name = "txtVelFim";
            this.txtVelFim.Size = new System.Drawing.Size(72, 20);
            this.txtVelFim.TabIndex = 26;
            this.txtVelFim.Text = "30";
            this.txtVelFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtVelFim.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidaNumero_KeyPress);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(159, 59);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(125, 13);
            this.label20.TabIndex = 25;
            this.label20.Text = "Vel Fim:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1238, 616);
            this.Controls.Add(this.lblCtViraEsq);
            this.Controls.Add(this.lblCtViraDir);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.lblGeneCircuito);
            this.Controls.Add(this.txtGeneAgv);
            this.Controls.Add(this.pbLimiar3);
            this.Controls.Add(this.pbLimiar2);
            this.Controls.Add(this.pbLimiar1);
            this.Controls.Add(this.pbLimiar4);
            this.Controls.Add(this.pbLimiar5);
            this.Controls.Add(this.pbLimiar0);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbComandos);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbRobot);
            this.Controls.Add(this.pbTerrain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vel Fim:";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbComandos.ResumeLayout(false);
            this.gbComandos.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMotor2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMotor1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLimiar0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRobot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTerrain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        //private void DoInferenceGA(int distance)
        //{
        //    //Considerando a distancia de 50 = 0.5m
        //    //1000 = 40mm (0.04)
        //    //Value algorithm       x       m
        //    //------------------------------------
        //    //1000                          0.04
        //    //12500                         0.5

        //    //Se o contador "ctControleParent" atingir o tamanho da população de pais estabelecido no Algoritimo Genético, deve ser resetado
        //    //Se avaiou todos os país, e todos os clones do último pai
        //    if ((ctControleParent == _ga.PopulationSize) && (ctControleClones == (_ga.Clonepopulation + 2)))
        //    {
        //        //Fazer o processo de seleção
        //        //Manter apenas o melhor individuo (População de Pais)
        //        _ga.FindSolution();
        //        int intTemp = Convert.ToInt32(lblGeneration.Text);
        //        intTemp++;
        //        lblGeneration.Text = intTemp.ToString();
        //        //lblFitness4.Text = _ga.GetBestIndividual().GetFitness().ToString();
        //        lblFitness4.Text = _ga.BestCircuit.Fitness.ToString();
        //        bestIndividualLatGeneration = _ga.BestCircuit;
        //        //bestIndividualLatGeneration = _ga.GetBestIndividual();

        //        ctControleParent = 0;
        //    }


        //    //Se o contador "ctControleClones" atingir o número de clones estabelecido no Algoritimo Genético, deve ser resetado
        //    if (ctControleClones == (_ga.Clonepopulation + 2))
        //        ctControleClones = 0;

        //    CircuitoChromosome current = null;

        //    ///WARNING:
        //    //Se o contador de clones esta em 0, significa que o primeiro circuito da população de PAIS SERÁ

        //    //Se o contador de clones esta em 1, significa que o primeiro circuito da população de PAIS foi avaliado por 140 ms
        //    //e agora, é possível atribuir seu fitness, e a partir disto
        //    //iniciar o processo de clonagem e avaliação da popuação de clones
        //    if (ctControleClones == 0)
        //        current = _ga.GetCurrentChromosome(ctControleParent);
        //    //A primeira vez, é necesário atribuir para "current" apenas para o proposito de as informações do gene
        //    //e atualizar as saídas do atuador para as saídas do circuito


        //    if (ctControleClones == 1)
        //    {
        //        //Atribuir os valores medidos para o circuito
        //        _ga.GetCurrentChromosome((ctControleParent)).Distance = distance;
        //        _ga.GetCurrentChromosome((ctControleParent)).Time = 140;
        //        _ga.GetCurrentChromosome((ctControleParent)).GetFitness();

        //        //current = _ga.GetCurrentChromosome((ctControleParent));

        //        //Inicializa população de clones para o determinado pai (Levando-se em consideração o fitness do pai)
        //        _ga.InitializeClones(ctControleParent, sensors);
        //        //Atribuir o primeiro clone para avaliação                
        //        current = _ga.GetCurrentCloneChromosome(ctControleParent, (ctControleClones - 1));

        //    }
        //    //Se o contador de clones é maior que 1, significa que o primeiro circuito da população de CLONES foi avaliado por 140 ms
        //    if (ctControleClones > 1)
        //    {
        //        _ga.GetCurrentCloneChromosome(ctControleParent, (ctControleClones - 2)).Distance = distance;
        //        _ga.GetCurrentCloneChromosome(ctControleParent, (ctControleClones - 2)).Time = 140;

        //        //Atribuir o clone seguinte para avaliação clone para avaliação
        //        if ((ctControleClones - 1) < 3)
        //            current = _ga.GetCurrentCloneChromosome(ctControleParent, (ctControleClones - 1));
        //    }

        //    if (current != null)
        //    {
        //        //{Cartesian genetic programming”,
        //        ShowChromosome(current, lblFitness4);
        //        lblGeneration.Text = _ga.Generation.ToString();
        //        AtualizaDirecao(current);

        //    }

        //    ctControleClones++;

        //    //Se terminou de avaliar os cloenes de determinado pai
        //    if (ctControleClones == (_ga.Clonepopulation + 2))
        //    {
        //        ctControleParent++;
        //    }
        //}

        private void DoInferenceGA(int distance)
        {
            bool cicloPais = true;
            bool AvaliarTodaPopulacao = false;
            CircuitoChromosome current = null;
            if (ctControleParent == _ga.PopulationSize)
                cicloPais = false;
            if (cicloPais)
            {
                //Atribuir os valores medidos para o circuito
                _ga.GetCurrentChromosome((ctControleParent)).Distance = distance;
                _ga.GetCurrentChromosome((ctControleParent)).Time = 140;
                _ga.GetCurrentChromosome((ctControleParent)).GetFitness();

                current = _ga.GetCurrentChromosome(ctControleParent);
                lblNumeroPortasAlteradas.Text = "-";
            }
            else
                if (ctControleClones == 0)
                {
                    _ga.CalculaNormFit();
                    _ga.InitializeClones((ctControleParent - ctPaisDecrescente), sensors);
                    current = _ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), 0);

                }
                else
                {
                    //_ga.GetCurrentCloneChromosome(ctControleParent, (ctControleClones - 2)).Distance = distance;
                    //_ga.GetCurrentCloneChromosome(ctControleParent, (ctControleClones - 2)).Time = 140;
                    _ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), (ctControleClones - 1)).Distance = distance;
                    _ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), (ctControleClones - 1)).Time = 140;
                    //_ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), (ctControleClones - 1)).GetFitness();

                    //Mostra o numero de portas alteradas
                    lblNumeroPortasAlteradas.Text = _ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), (ctControleClones - 1)).N.ToString();
                    //lblNumeroPortasAlteradas.Text = _ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), (ctControleClones - 1)).RetornaValorN().ToString();



                    //Atribuir o clone seguinte para avaliação clone para avaliação
                    //if ((ctControleClones - 1) < 3)
                    //if ((ctControleClones - 1) < 2)
                    if ((ctControleClones - 1) < 3)
                        current = _ga.GetCurrentCloneChromosome((ctControleParent - ctPaisDecrescente), (ctControleClones - 1));
                }
            if (ctControleParent == _ga.PopulationSize && (cicloPais))
            {
                ctPaisDecrescente = 3;
                cicloPais = false;
            }


            if ((!cicloPais) && (ctControleClones < _ga.Clonepopulation + 1))
                ctControleClones++;


            if (ctControleClones > (_ga.Clonepopulation))
            {
                //ctControleClones = 0;
                ctPaisDecrescente--;
            }

            if (ctControleClones > (_ga.Clonepopulation) && (ctPaisDecrescente == 0))
            {
                AvaliarTodaPopulacao = true;
            }

            if (ctControleClones > (_ga.Clonepopulation))
            {
                ctControleClones = 0;
                //ctPaisDecrescente--;
            }

            if (cicloPais && (ctControleClones <= _ga.Clonepopulation))
                ctControleParent++;

            if (AvaliarTodaPopulacao)
            {
                //Thread.Sleep(5);
                cicloPais = true;
                //Fazer o processo de seleção
                //Manter apenas o melhor individuo (População de Pais)
                _ga.FindSolution();
                ctPaisDecrescente = 3;
                int intTemp = Convert.ToInt32(lblGeneration.Text);
                intTemp++;

                DataRow drResultado = dtResultados.NewRow();
                drResultado["Bestfitness"] = (int)_ga.BestCircuit.Fitness;
                drResultado["Averagefitness"] = -1;
                drResultado["geracao"] = Convert.ToInt32(lblGeneration.Text.Trim());
                dtResultados.Rows.Add(drResultado);

                lblGeneration.Text = intTemp.ToString();
                lblFitness4.Text = _ga.BestCircuit.Fitness.ToString();
                bestIndividualLatGeneration = _ga.BestCircuit;
                current = _ga.BestCircuit;

                ctControleParent = 0;
                if (intTemp % 15 == 0)
                {
                    intTemp = intTemp;
                }


            }

            if (current != null)
            {
                //{Cartesian genetic programming”,
                ShowChromosome(current, lblFitness4);
                lblGeneration.Text = _ga.Generation.ToString();
                AtualizaDirecao(current);

            }

        }

        public void AtualizaDirecao(CircuitoChromosome chromoRobo)
        {
            double NewAngle = -1;
            //_traz = false;
            _virou = false;



            if ((!chromoRobo.OutputBits[0].Output && !chromoRobo.OutputBits[1].Output) || (chromoRobo.OutputBits[0].Output && chromoRobo.OutputBits[1].Output))
                pbMotor1.Visible = pbMotor2.Visible = true;
            else
            {
                pbMotor1.Visible = true;// !chromoRobo.OutputBits[0].Output;
                pbMotor2.Visible = true; // !chromoRobo.OutputBits[1].Output;
            }




            if ((chromoRobo.OutputBits[0].Output == false && chromoRobo.OutputBits[1].Output == false) || (chromoRobo.OutputBits[0].Output == true && chromoRobo.OutputBits[1].Output == true))
            {
                //NewAngle = 0;
                //_traz = true;
                NewAngle = 0;
                _traz = false;
            }
            //vira esquerda
            else if (chromoRobo.OutputBits[0].Output == true && chromoRobo.OutputBits[1].Output == false)
            {
                double vlTmp = Convert.ToDouble(lblCtViraEsq.Text);
                vlTmp++;
                lblCtViraEsq.Text = vlTmp.ToString();

                //NewAngle = -10;
                NewAngle = -5;
                _virou = true;
            }
            else if (chromoRobo.OutputBits[0].Output == false && chromoRobo.OutputBits[1].Output == true)
            {
                double vlTmp = Convert.ToDouble(lblCtViraDir.Text);
                vlTmp++;
                lblCtViraDir.Text = vlTmp.ToString();

                //NewAngle = +10;
                NewAngle = +5;
                _virou = true;

            }
            //else if (chromoRobo.OutputBits[0].Output == true && chromoRobo.OutputBits[1].Output == true)
            //{
            //    //_traz = false;
            //    //NewAngle = 0;
            //    NewAngle = 0;
            //    //_traz = true;
            //    //TODO: Arrumar lógica de andar para traz
            //    _traz = false;
            //}

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
            bool blVirou = false;
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
                if (btnRun.Enabled)
                {
                    string Msg = "O veiculo atingiu uma barreira!";
                    MessageBox.Show(Msg, "Error!");
                    btnRun.Enabled = false;
                }
            }
            double radAngle = ((Angle + 90) * Math.PI) / 180;


            #region Nova logica retas

            Point pFront = GetObstacle(new Point(pPos.X, pPos.Y), b, -1, 0);
            Reta rPrincipal = new Reta(pPos, pFront);


            double mperpendicularprincipal = rPrincipal.CalculaMPerpendicularAReta();
            Reta rperpendicularprincipal = new Reta(mperpendicularprincipal, pPos, null);

            Circulo crobo = new Circulo(pPos, 5);
            double[] xsecantes = crobo.CalculaValorXSecanteCircuferencia(rperpendicularprincipal);
            double[] ysecantes = new double[2];
            if (xsecantes.Length == 2)
            {
                if (rperpendicularprincipal.RetaVertical)
                {
                    //Caso a reta perpendicular a principal seja uma reta vertical, utilizar o circulo para determinar a posião dos Y, pois, a equação da reta estara definida como "x = k"
                    ysecantes = crobo.CalculaValorY(xsecantes[0]);
                }
                else
                {
                    ysecantes[0] = rperpendicularprincipal.CalculaValorY(xsecantes[0]);
                    ysecantes[1] = rperpendicularprincipal.CalculaValorY(xsecantes[1]);
                }
            }
            Point pPrincipalParalela1 = new Point((int)xsecantes[0], (int)ysecantes[0]);
            Point pPrincipalParalela2 = new Point((int)xsecantes[1], (int)ysecantes[1]);
            Reta rPrincipalParalela1 = new Reta(rPrincipal.M, pPrincipalParalela1, null);
            Reta rPrincipalParalela2 = new Reta(rPrincipal.M, pPrincipalParalela2, null);





            Point pFrontObstacle1 = GetObstacle(new Point(pPrincipalParalela1.X, pPrincipalParalela1.Y), b, -1, 0);
            Point pFrontObstacle2 = GetObstacle(new Point(pPrincipalParalela2.X, pPrincipalParalela2.Y), b, -1, 0);

            //Usado para teste
            //System.Diagnostics.Debug.WriteLine(string.Format("XpFront={0} ; YpFront={1} ; Angle={2} ; radAngle={3} ", pFront.X, pFront.Y, Angle, radAngle));
            //System.Diagnostics.Debug.WriteLine(string.Format("XpPos={0} ; YpPos={1} ; Angle={2} ; radAngle={3} ", pPos.X, pPos.Y, Angle, radAngle));
            //System.Diagnostics.Debug.WriteLine(string.Format("rPrincipal.M={0} ; rPrincipal.B={1} ; rPrincipal.P1.X={2} ; rPrincipal.P1.Y={3} ; ", rPrincipal.M, rPrincipal.B, rPrincipal.P1.X, rPrincipal.P1.Y));
            //System.Diagnostics.Debug.WriteLine(string.Format("rperpendicularprincipal.M={0} ; rperpendicularprincipal.B={1} ; rperpendicularprincipal.P1.X={2} ; rperpendicularprincipal.P1.Y={3} ; ", rperpendicularprincipal.M, rperpendicularprincipal.B, rperpendicularprincipal.P1.X, rperpendicularprincipal.P1.Y));
            //System.Diagnostics.Debug.WriteLine(string.Format("rPrincipalParalela1.M={0} ; rPrincipalParalela1.B={1} ; rPrincipalParalela1.P1.X={2} ; rPrincipalParalela1.P1.Y={3} ; ", rPrincipalParalela1.M, rPrincipalParalela1.B, rPrincipalParalela1.P1.X, rPrincipalParalela1.P1.Y));
            //System.Diagnostics.Debug.WriteLine(string.Format("rPrincipalParalela2.M={0} ; rPrincipalParalela2.B={1} ; rPrincipalParalela2.P1.X={2} ; rPrincipalParalela2.P1.Y={3} ; ", rPrincipalParalela2.M, rPrincipalParalela2.B, rPrincipalParalela2.P1.X, rPrincipalParalela2.P1.Y));
            //System.Diagnostics.Debug.WriteLine(string.Format("XObstaculo1={0} ; YObstaculo4={1}", pFrontObstacle1.X, pFrontObstacle1.Y));
            //System.Diagnostics.Debug.WriteLine(string.Format("XObstaculo2={0} ; YObstaculo3={1}", pFrontObstacle2.X, pFrontObstacle2.Y));
            //System.Diagnostics.Debug.WriteLine(string.Format("-------------------------------------------------------------------------------------------------------------"));

            Point pLeft45Obstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, -1, -45);
            Point pRight45Obstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, -1, 45);

            Point pLeftObstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, 1, 90);
            Point pRightObstacle = GetObstacle(new Point(pPos.X, pPos.Y), b, 1, -90);

            //Point pBehindObstacle1 = GetObstacle(new Point(pPrincipalParalela1.X, pPrincipalParalela1.Y), b, 1, 0);
            //Point pBehindObstacle2 = GetObstacle(new Point(pPrincipalParalela2.X, pPrincipalParalela2.Y), b, 1, 0);


            #endregion


            // Showing beams
            Graphics g = Graphics.FromImage(b);
            if (cbLasers.Checked)
            {
                //Adicionando retas
                //Frontais
                g.DrawLine(new Pen(Color.Green, 1), pFrontObstacle1, pPrincipalParalela1);
                g.DrawLine(new Pen(Color.Green, 1), pFrontObstacle2, pPrincipalParalela2);
                //Utilizado para desenhar a reta principal, apenas para testes
                //g.DrawLine(new Pen(Color.Green, 1), pFront, pPos);

                //Traseiras
                //g.DrawLine(new Pen(Color.Green, 1), pBehindObstacle1, pPrincipalParalela1);
                //g.DrawLine(new Pen(Color.Green, 1), pBehindObstacle2, pPrincipalParalela2);

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



            double VlLimiarDistancia = Convert.ToDouble(txtLimiarDistancia.Text);
            // Updating distances texts
            //Esquerda
            if (!chkFalhaSensorFrontal0.Checked)
            {
                txtFront0.Text = GetDistance(pPos, pLeftObstacle).ToString();
                sensors[0] = !(Convert.ToDouble(txtFront0.Text) > VlLimiarDistancia);
                pbLimiar0.Visible = sensors[0];
            }
            //Esquerda 45
            if (!chkFalhaSensorFrontal1.Checked)
            {
                txtFront1.Text = GetDistance(pPos, pLeft45Obstacle).ToString();
                sensors[1] = !(Convert.ToDouble(txtFront1.Text) > VlLimiarDistancia);
                pbLimiar1.Visible = sensors[1];
            }
            //Frente
            if (!chkFalhaSensorFrontal2.Checked)
            {
                txtFront2.Text = GetDistance(pPrincipalParalela1, pFrontObstacle1).ToString();
                sensors[2] = !(Convert.ToDouble(txtFront2.Text) > VlLimiarDistancia);
                pbLimiar2.Visible = sensors[2];
            }
            if (!chkFalhaSensorFrontal3.Checked)
            {
                txtFront3.Text = GetDistance(pPrincipalParalela2, pFrontObstacle2).ToString();
                sensors[3] = !(Convert.ToDouble(txtFront3.Text) > VlLimiarDistancia);
                pbLimiar3.Visible = sensors[3];
            }
            //Direita 45
            if (!chkFalhaSensorFrontal4.Checked)
            {
                txtFront4.Text = GetDistance(pPos, pRight45Obstacle).ToString();
                sensors[4] = !(Convert.ToDouble(txtFront4.Text) > VlLimiarDistancia);
                pbLimiar4.Visible = sensors[4];
            }
            //Direita
            if (!chkFalhaSensorFrontal5.Checked)
            {
                txtFront5.Text = GetDistance(pPos, pRightObstacle).ToString();
                sensors[5] = !(Convert.ToDouble(txtFront5.Text) > VlLimiarDistancia);
                pbLimiar5.Visible = sensors[5];
            }


            //Trazeiro
            //txtFront6.Text = GetDistance(pPrincipalParalela1, pBehindObstacle1).ToString();
            //sensors[6] = !(Convert.ToDouble(txtFront6.Text) > VlLimiarDistancia);
            //pbLimiar6.Visible = sensors[6];
            //txtFront7.Text = GetDistance(pPrincipalParalela2, pBehindObstacle2).ToString();
            //sensors[7] = !(Convert.ToDouble(txtFront7.Text) > VlLimiarDistancia);
            //pbLimiar7.Visible = sensors[7];




            //A velocidade é determinada em tempo real de acordo com a distancia dos obstaculos
            //Se um dos sensores atingiu o valor da limiar diminui a velocidade            
            //Utilizar dois sensores proximos para determinar qundo esta p´rximo de um obstaculo
            //if (!((sensors[0] && sensors[1]) || (sensors[2] && sensors[3]) || (sensors[4] && sensors[5]) || (sensors[6] && sensors[7])))
            if (!((sensors[0] && sensors[1]) || (sensors[1] && sensors[2]) || (sensors[2] && sensors[3]) || (sensors[3] && sensors[4]) || (sensors[4] && sensors[5])))
            {
                //txtSpeed.Text = "12,0";
                if (Speed < Convert.ToInt32(txtVelFim.Text))
                    Speed += 3;
                if (_virou)
                    Speed = 0;
            }
            else
            {
                //txtSpeed.Text = "5,0";
                //Speed = 5;
                if (Speed > 3)
                    Speed -= 1;
                else if (!_virou)
                    Speed = 3;

                if (_virou)
                    Speed = 0;
            }
            txtSpeed.Text = Speed.ToString();
            Angle = Angle % 360;
            txtAngle.Text = Angle.ToString();
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

            chkFalhaSensorFrontal0.Checked = false;
            chkFalhaSensorFrontal1.Checked = false;
            chkFalhaSensorFrontal2.Checked = false;
            chkFalhaSensorFrontal3.Checked = false;
            chkFalhaSensorFrontal4.Checked = false;
            chkFalhaSensorFrontal5.Checked = false;

            Angle = 0;

            sensors = new bool[8];
            sensors[0] = false;
            sensors[1] = false;
            sensors[2] = false;
            sensors[3] = false;
            sensors[4] = false;
            sensors[5] = false;
            sensors[6] = false;
            sensors[7] = false;
            _traz = false;
            //_ga = new EA(4, 3, sensors);
            _ga = new EA(3, 2, sensors);
            //_ga.K = 13;
            _ga.K = Convert.ToInt32(txtk.Text);

            dtResultados = new DataTable();
            dtResultados.Columns.Add("geracao", typeof(int));
            dtResultados.Columns.Add("Bestfitness", typeof(int));
            dtResultados.Columns.Add("Averagefitness", typeof(int));



            FirstInference = true;
            pbRobot.Top = pbTerrain.Bottom - 100;
            pbRobot.Left = pbTerrain.Left + 100;
            InitialPos = pbRobot.Location;
            RunLabel = btnRun.Text;

            btnRun.Enabled = true;
            ctControleParent = 0;
            ctControleClones = 0;
            ctControleParent = 0;
            ctControleClones = 0;
            ctPaisDecrescente = 3;

            lblFitness4.Text = "0";
            lblGeneration.Text = "1";

            _bCiclo = false;
            pbLimiar0.Visible = false;
            pbLimiar1.Visible = false;
            pbLimiar2.Visible = false;
            pbLimiar3.Visible = false;
            pbLimiar4.Visible = false;
            pbLimiar5.Visible = false;


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
            //txtFront6.Text = "0";
            //txtFront7.Text = "0";

            txtAngle.Text = "0,00";
        }

        // Moving the AGV
        private void MoveAGV()
        {
            bestIndividualLatGeneration = null;
            //Passado 140 milisegundos, coletar a distancia percorrida pelo robo
            if (_bCiclo)
            {
                _ponto1 = true;
                p2 = new Point(pbRobot.Left - pbTerrain.Left + pbRobot.Width / 2, pbRobot.Top - pbTerrain.Top + pbRobot.Height / 2);

                _bCiclo = false;
                //Passa a distancia ao controle para avaliação do mesmo
                DoInferenceGA(GetDistance(p1, p2));
            }


            Speed = Convert.ToDouble(txtSpeed.Text);

            if (_traz)//Se for p/ traz;
            {
                if (Speed > 0)
                    Speed = Speed * -1;

                txtSpeed.Text = Speed.ToString();
            }
            else
            {
                //Volta para velocidade positiva
                if (Speed < 0)
                    Speed = Speed * -1;

                txtSpeed.Text = Speed.ToString();
            }

            double rad = ((Angle + 90) * Math.PI) / 180;
            int Offset = 0;
            int Inc = (-1 * Convert.ToInt32(Speed / 5));

            Offset += Inc;
            int IncX = Convert.ToInt32(Offset * Math.Cos(rad));
            int IncY = Convert.ToInt32(Offset * Math.Sin(rad));

            //pbRobot.Top = pbRobot.Top + IncY;
            //pbRobot.Left = pbRobot.Left + IncX;

            if (_traz)
            {
                pbRobot.Top = (int)(pbRobot.Top - IncY);
                pbRobot.Left = (int)(pbRobot.Left - IncX);
            }
            else
            {
                pbRobot.Top = (int)(pbRobot.Top + IncY);
                pbRobot.Left = (int)(pbRobot.Left + IncX);
            }


            if (_ponto1)
            {
                p1 = new Point(pbRobot.Left - pbTerrain.Left + pbRobot.Width / 2, pbRobot.Top - pbTerrain.Top + pbRobot.Height / 2);
                _ponto1 = false;
            }

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
        }

        // Starting and stopping the AGV's moviment a
        private void btnRun_Click(object sender, System.EventArgs e)
        {

            Button b = (sender as Button);

            if (b.Text == RunLabel)
            {
                //Atribuições iniciais                
                //_ga.Time = 140;
                //QSChromosome current = _ga.GetCurrentChromosome(0, 0);
                ////if (solution != null)
                ////{Cartesian genetic programming”,
                //ShowChromosome(current, lblFitness4);
                //lblGeneration.Text = _ga.Generation.ToString();
                //AtualizaDirecao(current);

                b.Text = "&Stop";
                btnStep.Enabled = false;
                btnReset.Enabled = false;
                txtInterval.Enabled = false;
                txtLimiarDistancia.Enabled = false;
                cbLasers.Enabled = false;
                cbTrajeto.Enabled = false;
                txtLimiarDistancia.Enabled = false;
                txtk.Enabled = false;
                txtTempoImplementacao.Enabled = false;
                txtVelFim.Enabled = false;


                pbRobot.Hide();
                StartMovement();
            }
            else
            {
                StopMovement();
                b.Text = RunLabel;

                btnReset.Enabled = true;
                btnStep.Enabled = true;
                txtInterval.Enabled = true;
                txtLimiarDistancia.Enabled = true;
                cbLasers.Enabled = true;
                cbTrajeto.Enabled = true;
                txtLimiarDistancia.Enabled = true;
                txtk.Enabled = true;
                txtTempoImplementacao.Enabled = true;
                txtVelFim.Enabled = true;
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
            TimeSpan tInicio = new TimeSpan();
            TimeSpan tFim = new TimeSpan();
            tInicio = DateTime.Now.TimeOfDay;
            lstDist = new List<int>();

            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    tFim = DateTime.Now.TimeOfDay;

                    MethodInvoker mi = new MethodInvoker(AGVStep);
                    this.BeginInvoke(mi);
                    Thread.Sleep(Convert.ToInt32(txtInterval.Text));


                    //TimeSpan tresult = tFim.Subtract(tInicio);
                    //if (tFim.Subtract(tInicio).Milliseconds > 10)
                    //if (tFim.Subtract(tInicio).Milliseconds > 140)
                    //if (tFim.Subtract(tInicio).Milliseconds > 140)
                    if (tFim.Subtract(tInicio).Milliseconds > Convert.ToInt32(txtTempoImplementacao.Text))
                    {
                        _bCiclo = true;
                        tInicio = DateTime.Now.TimeOfDay;
                    }
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
                GetMeasures();
                MoveAGV();
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
            Time += 70;
            txtTime.Text = (Time / 1000.0).ToString("#0.#0");
        }

        /// <summary>
        /// Mostra o fitness e desenha o quadrado resposta (cromossomo)
        /// </summary>
        /// <param name="circuitChromo"></param>
        /// <param name="lFitness"></param>
        private void ShowChromosome(CircuitoChromosome circuitChromo, Label lFitness)
        {
            if (circuitChromo == null) return;
            lblGeneration.Text = (_ga.Generation + 1).ToString();
            lFitness.Text = circuitChromo.GetFitness().ToString();
            lblFitness4.Text = circuitChromo.GetFitness().ToString();
            //DrawTable(eqc.Valores);
            txtGeneAgv.Text = String.Empty;
            foreach (int i in circuitChromo.Cgp.Genotype)
                txtGeneAgv.Text = txtGeneAgv.Text + i + ";";
            //Remove o ultimo ';'
            txtGeneAgv.Text = txtGeneAgv.Text.Remove(txtGeneAgv.Text.Length - 1);

            DataRow drResultado = dtResultados.NewRow();
            drResultado["Bestfitness"] = -1;
            drResultado["Averagefitness"] = (int)circuitChromo.Fitness;
            drResultado["geracao"] = Convert.ToInt32(lblGeneration.Text.Trim());
            dtResultados.Rows.Add(drResultado);


        }


        private void lstFalha_SelectedValueChanged(object sender, EventArgs e)
        {



            //for (int i = 0; i < ((ListBox)sender).SelectedItems.Count; i++)
            //{
            //    string strIndice = ((ListBox)sender).SelectedItems[i].ToString().Trim().Split('-')[1].Trim();
            //    switch (strIndice)
            //    {
            //        case "0":
            //            txtFront0.Text = "0";
            //            break;
            //        case "1":
            //            txtFront1.Text = "0";
            //            break;
            //        case "2":
            //            txtFront2.Text = "0";
            //            break;
            //        case "3":
            //            txtFront3.Text = "0";
            //            break;
            //        case "4":
            //            txtFront4.Text = "0";
            //            break;
            //        case "5":
            //            txtFront5.Text = "0";
            //            break;
            //        default:
            //            break;


            //    }

            //    //string strNomeComponente = "txtFront" + strIndice;

            //    //Form currentForm = Form.ActiveForm;
            //    //Control[] controles = currentForm.Controls.Find("strNomeComponente", true);
            //    //if (controles != null)
            //    //    if (controles.Length > 0)
            //    //        ((TextBox)controles[0]).Text = "0";
            //}



        }





        private void chkFalhaSensorFrontal0_CheckedChanged(object sender, EventArgs e)
        {
            pbLimiar0.Visible = true;
            txtFront0.Text = "0";
            sensors[0] = true;
        }

        private void chkFalhaSensorFrontal1_CheckedChanged(object sender, EventArgs e)
        {
            pbLimiar1.Visible = true;
            txtFront1.Text = "0";
            sensors[1] = true;
        }

        private void chkFalhaSensorFrontal2_CheckedChanged(object sender, EventArgs e)
        {
            pbLimiar2.Visible = true;
            txtFront2.Text = "0";
            sensors[2] = true;
        }

        private void chkFalhaSensorFrontal3_CheckedChanged(object sender, EventArgs e)
        {
            pbLimiar3.Visible = true;
            txtFront3.Text = "0";
            sensors[3] = true;
        }

        private void chkFalhaSensorFrontal4_CheckedChanged(object sender, EventArgs e)
        {
            pbLimiar4.Visible = true;
            txtFront4.Text = "0";
            sensors[4] = true;
        }

        private void chkFalhaSensorFrontal5_CheckedChanged(object sender, EventArgs e)
        {
            pbLimiar5.Visible = true;
            txtFront5.Text = "0";
            sensors[5] = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 'A':
                case 'a':
                    btnReset_Click(null, null);
                    break;
                case 'r':
                case 'R':
                    if (btnRun.Enabled)
                        btnRun_Click(this.btnRun, null);
                    break;
                default:
                    break;

            }
        }

        private void txtValidaNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

    }
}
