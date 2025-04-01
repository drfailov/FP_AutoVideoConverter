namespace FP_Auto_Video_Converter_2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelStats = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.trackBarCRF = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelCrfMeaning = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelPresetMeaning = new System.Windows.Forms.Label();
            this.trackBarPreset = new System.Windows.Forms.TrackBar();
            this.checkBoxSkipIfBigger = new System.Windows.Forms.CheckBox();
            this.checkBoxReduceFramerate = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBoxScaleDown = new System.Windows.Forms.CheckBox();
            this.textBoxScaleDownSmallerSide = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonRemoveLess720 = new System.Windows.Forms.Button();
            this.buttonRemoveLess1080 = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonClearBacups = new System.Windows.Forms.Button();
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.buttonRemoveH265 = new System.Windows.Forms.Button();
            this.buttonClearSelected = new System.Windows.Forms.Button();
            this.buttonRemoveLess4MBit = new System.Windows.Forms.Button();
            this.buttonRemoveLess2MBit = new System.Windows.Forms.Button();
            this.buttonOpenRecycle = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.ColumnFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileBitRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileResolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileFormat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileNewSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFileCompressPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCRF)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPreset)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnFileName,
            this.ColumnFileFolder,
            this.ColumnFileSize,
            this.ColumnFileBitRate,
            this.ColumnFileResolution,
            this.ColumnFileFormat,
            this.ColumnFileStatus,
            this.ColumnFileNewSize,
            this.ColumnFileCompressPercent,
            this.ColumnFilePath});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1249, 777);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(1249, 147);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "---------------------------------- START ----------";
            this.richTextBox1.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1249, 928);
            this.splitContainer1.SplitterDistance = 777;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonRemoveLess720);
            this.panel1.Controls.Add(this.buttonRemoveLess1080);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.buttonClearBacups);
            this.panel1.Controls.Add(this.textBoxScaleDownSmallerSide);
            this.panel1.Controls.Add(this.checkBoxScaleDown);
            this.panel1.Controls.Add(this.buttonRemoveAll);
            this.panel1.Controls.Add(this.buttonRemoveH265);
            this.panel1.Controls.Add(this.checkBoxReduceFramerate);
            this.panel1.Controls.Add(this.buttonClearSelected);
            this.panel1.Controls.Add(this.checkBoxSkipIfBigger);
            this.panel1.Controls.Add(this.buttonRemoveLess4MBit);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.buttonRemoveLess2MBit);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonOpenRecycle);
            this.panel1.Controls.Add(this.buttonStop);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1249, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 978);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(19, 446);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 32);
            this.label1.TabIndex = 9;
            this.label1.Text = "Оригінальні файли будуть замінятись\r\nстисненими";
            this.toolTip1.SetToolTip(this.label1, "А стиснені будуть перенесені в папку з бекапами");
            // 
            // labelStats
            // 
            this.labelStats.AutoSize = true;
            this.labelStats.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStats.Location = new System.Drawing.Point(6, 9);
            this.labelStats.Name = "labelStats";
            this.labelStats.Size = new System.Drawing.Size(152, 162);
            this.labelStats.TabIndex = 0;
            this.labelStats.Text = "Резеврних копій: 0\r\n\r\nВсього файлів: 0\r\n\r\nОчікує: 0\r\n\r\nГотово: 0\r\n\r\nТривалість: 0" +
    "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 928);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1249, 50);
            this.panel2.TabIndex = 4;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(12, 15);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(197, 16);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Перетащіть файли або папки";
            // 
            // trackBarCRF
            // 
            this.trackBarCRF.Location = new System.Drawing.Point(6, 21);
            this.trackBarCRF.Maximum = 48;
            this.trackBarCRF.Minimum = 10;
            this.trackBarCRF.Name = "trackBarCRF";
            this.trackBarCRF.Size = new System.Drawing.Size(319, 56);
            this.trackBarCRF.TabIndex = 10;
            this.trackBarCRF.Value = 30;
            this.trackBarCRF.ValueChanged += new System.EventHandler(this.trackBarCRF_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelCrfMeaning);
            this.groupBox2.Controls.Add(this.trackBarCRF);
            this.groupBox2.Location = new System.Drawing.Point(16, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 82);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Рівень стиснення H265 (CRF)";
            this.toolTip1.SetToolTip(this.groupBox2, "Зменшення значення CRF покращує якість відео, але збільшує розмір файлу");
            // 
            // labelCrfMeaning
            // 
            this.labelCrfMeaning.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCrfMeaning.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelCrfMeaning.Location = new System.Drawing.Point(6, 54);
            this.labelCrfMeaning.Name = "labelCrfMeaning";
            this.labelCrfMeaning.Size = new System.Drawing.Size(315, 23);
            this.labelCrfMeaning.TabIndex = 12;
            this.labelCrfMeaning.Text = "30 (Середня якість і малий файл)";
            this.labelCrfMeaning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelPresetMeaning);
            this.groupBox3.Controls.Add(this.trackBarPreset);
            this.groupBox3.Location = new System.Drawing.Point(16, 121);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(331, 97);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Гбилина стиснення H265 (Preset)";
            this.toolTip1.SetToolTip(this.groupBox3, "Наскільки старанно буде стискатись кожен файл.Більш повільні пресети забезпечують" +
        " кращу якість і стиснення");
            // 
            // labelPresetMeaning
            // 
            this.labelPresetMeaning.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPresetMeaning.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelPresetMeaning.Location = new System.Drawing.Point(6, 54);
            this.labelPresetMeaning.Name = "labelPresetMeaning";
            this.labelPresetMeaning.Size = new System.Drawing.Size(315, 40);
            this.labelPresetMeaning.TabIndex = 12;
            this.labelPresetMeaning.Text = "slow\r\nПовільніше, але краще стискає";
            this.labelPresetMeaning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarPreset
            // 
            this.trackBarPreset.Location = new System.Drawing.Point(6, 21);
            this.trackBarPreset.Maximum = 9;
            this.trackBarPreset.Name = "trackBarPreset";
            this.trackBarPreset.Size = new System.Drawing.Size(319, 56);
            this.trackBarPreset.TabIndex = 10;
            this.trackBarPreset.Value = 6;
            this.trackBarPreset.ValueChanged += new System.EventHandler(this.trackBarPreset_ValueChanged);
            // 
            // checkBoxSkipIfBigger
            // 
            this.checkBoxSkipIfBigger.Checked = true;
            this.checkBoxSkipIfBigger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSkipIfBigger.Location = new System.Drawing.Point(20, 238);
            this.checkBoxSkipIfBigger.Name = "checkBoxSkipIfBigger";
            this.checkBoxSkipIfBigger.Size = new System.Drawing.Size(243, 41);
            this.checkBoxSkipIfBigger.TabIndex = 14;
            this.checkBoxSkipIfBigger.Text = "Пропускати файли які вийшли більші ніж були";
            this.toolTip1.SetToolTip(this.checkBoxSkipIfBigger, "Якщо після конвертації виявиться що файл став більше ніж був то його не буде замі" +
        "нено.");
            this.checkBoxSkipIfBigger.UseVisualStyleBackColor = true;
            // 
            // checkBoxReduceFramerate
            // 
            this.checkBoxReduceFramerate.Checked = true;
            this.checkBoxReduceFramerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReduceFramerate.Location = new System.Drawing.Point(20, 282);
            this.checkBoxReduceFramerate.Name = "checkBoxReduceFramerate";
            this.checkBoxReduceFramerate.Size = new System.Drawing.Size(305, 41);
            this.checkBoxReduceFramerate.TabIndex = 15;
            this.checkBoxReduceFramerate.Text = "Зменшити частоту кадрів до 30";
            this.toolTip1.SetToolTip(this.checkBoxReduceFramerate, "Якщо файл більше 30 кадрів на секунду, то його частота кадрів зменшиться.");
            this.checkBoxReduceFramerate.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonExit);
            this.panel3.Controls.Add(this.buttonAbout);
            this.panel3.Controls.Add(this.labelStats);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 679);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(364, 299);
            this.panel3.TabIndex = 16;
            // 
            // checkBoxScaleDown
            // 
            this.checkBoxScaleDown.Checked = true;
            this.checkBoxScaleDown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScaleDown.Location = new System.Drawing.Point(20, 324);
            this.checkBoxScaleDown.Name = "checkBoxScaleDown";
            this.checkBoxScaleDown.Size = new System.Drawing.Size(267, 41);
            this.checkBoxScaleDown.TabIndex = 17;
            this.checkBoxScaleDown.Text = "Зменшити роздільну здатність до";
            this.toolTip1.SetToolTip(this.checkBoxScaleDown, "Якщо відео по меншій стороні більше ніж це число - то відео буле зменшено до цьог" +
        "о розміру по меншій стороні.");
            this.checkBoxScaleDown.UseVisualStyleBackColor = true;
            // 
            // textBoxScaleDownSmallerSide
            // 
            this.textBoxScaleDownSmallerSide.Location = new System.Drawing.Point(293, 332);
            this.textBoxScaleDownSmallerSide.Name = "textBoxScaleDownSmallerSide";
            this.textBoxScaleDownSmallerSide.Size = new System.Drawing.Size(48, 22);
            this.textBoxScaleDownSmallerSide.TabIndex = 18;
            this.textBoxScaleDownSmallerSide.Text = "1080";
            this.toolTip1.SetToolTip(this.textBoxScaleDownSmallerSide, "Менша сторона");
            // 
            // buttonRemoveLess720
            // 
            this.buttonRemoveLess720.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonRemoveLess720.Location = new System.Drawing.Point(194, 635);
            this.buttonRemoveLess720.Name = "buttonRemoveLess720";
            this.buttonRemoveLess720.Size = new System.Drawing.Size(153, 38);
            this.buttonRemoveLess720.TabIndex = 21;
            this.buttonRemoveLess720.Text = "До 720p";
            this.buttonRemoveLess720.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveLess720.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveLess720, "Прибрати файли які і так з малою роздільною здатністю (мешне 720 по меншій сторон" +
        "і)");
            this.buttonRemoveLess720.UseVisualStyleBackColor = true;
            this.buttonRemoveLess720.Click += new System.EventHandler(this.buttonRemoveLess720_Click);
            // 
            // buttonRemoveLess1080
            // 
            this.buttonRemoveLess1080.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonRemoveLess1080.Location = new System.Drawing.Point(16, 635);
            this.buttonRemoveLess1080.Name = "buttonRemoveLess1080";
            this.buttonRemoveLess1080.Size = new System.Drawing.Size(172, 38);
            this.buttonRemoveLess1080.TabIndex = 20;
            this.buttonRemoveLess1080.Text = "До 1080p";
            this.buttonRemoveLess1080.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveLess1080.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveLess1080, "Прибрати файли які і так з малою роздільною здатністю (мешне 1080 по меншій сторо" +
        "ні)");
            this.buttonRemoveLess1080.UseVisualStyleBackColor = true;
            this.buttonRemoveLess1080.Click += new System.EventHandler(this.buttonRemoveLess1080_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.logout;
            this.buttonExit.Location = new System.Drawing.Point(110, 249);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(242, 38);
            this.buttonExit.TabIndex = 23;
            this.buttonExit.Text = "Вийти";
            this.buttonExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.information_button__1_;
            this.buttonAbout.Location = new System.Drawing.Point(110, 205);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(242, 38);
            this.buttonAbout.TabIndex = 22;
            this.buttonAbout.Text = "Про програму";
            this.buttonAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // buttonClearBacups
            // 
            this.buttonClearBacups.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.trash_bin__1_;
            this.buttonClearBacups.Location = new System.Drawing.Point(279, 507);
            this.buttonClearBacups.Name = "buttonClearBacups";
            this.buttonClearBacups.Size = new System.Drawing.Size(68, 38);
            this.buttonClearBacups.TabIndex = 19;
            this.buttonClearBacups.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClearBacups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonClearBacups, "Очистити всі бекапи там");
            this.buttonClearBacups.UseVisualStyleBackColor = true;
            this.buttonClearBacups.Click += new System.EventHandler(this.buttonClearBacups_Click);
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonRemoveAll.Location = new System.Drawing.Point(16, 551);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(101, 38);
            this.buttonRemoveAll.TabIndex = 7;
            this.buttonRemoveAll.Text = "Всі";
            this.buttonRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveAll, "Очистити весь список");
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            this.buttonRemoveAll.Click += new System.EventHandler(this.buttonRemoveAll_Click);
            // 
            // buttonRemoveH265
            // 
            this.buttonRemoveH265.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonRemoveH265.Location = new System.Drawing.Point(245, 551);
            this.buttonRemoveH265.Name = "buttonRemoveH265";
            this.buttonRemoveH265.Size = new System.Drawing.Size(102, 38);
            this.buttonRemoveH265.TabIndex = 6;
            this.buttonRemoveH265.Text = "H265";
            this.buttonRemoveH265.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveH265.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveH265, "Вибрати файли які вже в кодеку Н265.");
            this.buttonRemoveH265.UseVisualStyleBackColor = true;
            this.buttonRemoveH265.Click += new System.EventHandler(this.buttonRemoveH265_Click);
            // 
            // buttonClearSelected
            // 
            this.buttonClearSelected.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonClearSelected.Location = new System.Drawing.Point(123, 551);
            this.buttonClearSelected.Name = "buttonClearSelected";
            this.buttonClearSelected.Size = new System.Drawing.Size(116, 38);
            this.buttonClearSelected.TabIndex = 2;
            this.buttonClearSelected.Text = "Вибрані";
            this.buttonClearSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClearSelected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonClearSelected, "Прибрати зі списку виділені рядки (вибрати треба весь рядок)");
            this.buttonClearSelected.UseVisualStyleBackColor = true;
            this.buttonClearSelected.Click += new System.EventHandler(this.buttonClearSelected_Click);
            // 
            // buttonRemoveLess4MBit
            // 
            this.buttonRemoveLess4MBit.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonRemoveLess4MBit.Location = new System.Drawing.Point(194, 595);
            this.buttonRemoveLess4MBit.Name = "buttonRemoveLess4MBit";
            this.buttonRemoveLess4MBit.Size = new System.Drawing.Size(153, 38);
            this.buttonRemoveLess4MBit.TabIndex = 5;
            this.buttonRemoveLess4MBit.Text = "До 4 МБіт/с";
            this.buttonRemoveLess4MBit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveLess4MBit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveLess4MBit, "Прибрати зі списку файли що вже і так малі і менше 4 мегабіт/с");
            this.buttonRemoveLess4MBit.UseVisualStyleBackColor = true;
            this.buttonRemoveLess4MBit.Click += new System.EventHandler(this.buttonRemoveLess4MBit_Click);
            // 
            // buttonRemoveLess2MBit
            // 
            this.buttonRemoveLess2MBit.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.clean;
            this.buttonRemoveLess2MBit.Location = new System.Drawing.Point(16, 595);
            this.buttonRemoveLess2MBit.Name = "buttonRemoveLess2MBit";
            this.buttonRemoveLess2MBit.Size = new System.Drawing.Size(172, 38);
            this.buttonRemoveLess2MBit.TabIndex = 4;
            this.buttonRemoveLess2MBit.Text = "До 2 МБіт/с";
            this.buttonRemoveLess2MBit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveLess2MBit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRemoveLess2MBit, "Прибрати зі списку файли що вже і так малі і менше 2 мегабіт/с");
            this.buttonRemoveLess2MBit.UseVisualStyleBackColor = true;
            this.buttonRemoveLess2MBit.Click += new System.EventHandler(this.buttonRemoveLess2MBit_Click);
            // 
            // buttonOpenRecycle
            // 
            this.buttonOpenRecycle.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.upload__1_;
            this.buttonOpenRecycle.Location = new System.Drawing.Point(16, 506);
            this.buttonOpenRecycle.Name = "buttonOpenRecycle";
            this.buttonOpenRecycle.Size = new System.Drawing.Size(257, 39);
            this.buttonOpenRecycle.TabIndex = 8;
            this.buttonOpenRecycle.Text = "Відкрити папку з бекапами";
            this.buttonOpenRecycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonOpenRecycle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonOpenRecycle, "Туди переміщаються оригінали відео щоб їх у випадку чого можна було врятувати");
            this.buttonOpenRecycle.UseVisualStyleBackColor = true;
            this.buttonOpenRecycle.Click += new System.EventHandler(this.buttonOpenRecycle_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.stop;
            this.buttonStop.Location = new System.Drawing.Point(279, 385);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(68, 60);
            this.buttonStop.TabIndex = 7;
            this.buttonStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Image = global::FP_Auto_Video_Converter_2.Properties.Resources.play;
            this.buttonStart.Location = new System.Drawing.Point(16, 385);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(257, 60);
            this.buttonStart.TabIndex = 3;
            this.buttonStart.Text = "Запустити конвертування";
            this.buttonStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // ColumnFileName
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ColumnFileName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnFileName.HeaderText = "Файл";
            this.ColumnFileName.MinimumWidth = 6;
            this.ColumnFileName.Name = "ColumnFileName";
            this.ColumnFileName.ReadOnly = true;
            this.ColumnFileName.ToolTipText = "Ім\'я файлу";
            this.ColumnFileName.Width = 190;
            // 
            // ColumnFileFolder
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ColumnFileFolder.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnFileFolder.HeaderText = "Папка";
            this.ColumnFileFolder.MinimumWidth = 6;
            this.ColumnFileFolder.Name = "ColumnFileFolder";
            this.ColumnFileFolder.ReadOnly = true;
            this.ColumnFileFolder.ToolTipText = "В якій папці";
            this.ColumnFileFolder.Width = 135;
            // 
            // ColumnFileSize
            // 
            this.ColumnFileSize.HeaderText = "Розмір";
            this.ColumnFileSize.MinimumWidth = 6;
            this.ColumnFileSize.Name = "ColumnFileSize";
            this.ColumnFileSize.ReadOnly = true;
            this.ColumnFileSize.ToolTipText = "Скільки займає файл до конвертації";
            this.ColumnFileSize.Width = 70;
            // 
            // ColumnFileBitRate
            // 
            this.ColumnFileBitRate.HeaderText = "Бітрейт";
            this.ColumnFileBitRate.MinimumWidth = 6;
            this.ColumnFileBitRate.Name = "ColumnFileBitRate";
            this.ColumnFileBitRate.ReadOnly = true;
            this.ColumnFileBitRate.ToolTipText = "Скільки місця займає одна секунда відео";
            this.ColumnFileBitRate.Width = 70;
            // 
            // ColumnFileResolution
            // 
            this.ColumnFileResolution.HeaderText = "Роздільна здатність";
            this.ColumnFileResolution.MinimumWidth = 6;
            this.ColumnFileResolution.Name = "ColumnFileResolution";
            this.ColumnFileResolution.ReadOnly = true;
            this.ColumnFileResolution.ToolTipText = "Ширина, висота і поворот";
            this.ColumnFileResolution.Width = 85;
            // 
            // ColumnFileFormat
            // 
            this.ColumnFileFormat.HeaderText = "Формат";
            this.ColumnFileFormat.MinimumWidth = 6;
            this.ColumnFileFormat.Name = "ColumnFileFormat";
            this.ColumnFileFormat.ReadOnly = true;
            this.ColumnFileFormat.ToolTipText = "Кодек відео. Не залежить від формату файла (контейнера)";
            this.ColumnFileFormat.Width = 75;
            // 
            // ColumnFileStatus
            // 
            this.ColumnFileStatus.HeaderText = "Статус";
            this.ColumnFileStatus.MinimumWidth = 6;
            this.ColumnFileStatus.Name = "ColumnFileStatus";
            this.ColumnFileStatus.ReadOnly = true;
            this.ColumnFileStatus.ToolTipText = "Що робиться";
            this.ColumnFileStatus.Width = 85;
            // 
            // ColumnFileNewSize
            // 
            this.ColumnFileNewSize.HeaderText = "Новий розмір";
            this.ColumnFileNewSize.MinimumWidth = 6;
            this.ColumnFileNewSize.Name = "ColumnFileNewSize";
            this.ColumnFileNewSize.ReadOnly = true;
            this.ColumnFileNewSize.ToolTipText = "Який розмір став після конвертування";
            this.ColumnFileNewSize.Width = 70;
            // 
            // ColumnFileCompressPercent
            // 
            this.ColumnFileCompressPercent.HeaderText = "%";
            this.ColumnFileCompressPercent.MinimumWidth = 6;
            this.ColumnFileCompressPercent.Name = "ColumnFileCompressPercent";
            this.ColumnFileCompressPercent.ReadOnly = true;
            this.ColumnFileCompressPercent.ToolTipText = "На скільки відсотків файл став менше займати місця";
            this.ColumnFileCompressPercent.Width = 50;
            // 
            // ColumnFilePath
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Silver;
            this.ColumnFilePath.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnFilePath.HeaderText = "Шлях";
            this.ColumnFilePath.MinimumWidth = 6;
            this.ColumnFilePath.Name = "ColumnFilePath";
            this.ColumnFilePath.ReadOnly = true;
            this.ColumnFilePath.ToolTipText = "Де знаходиться файл";
            this.ColumnFilePath.Width = 50;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1613, 978);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "FP AutoVideoConverter 2.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCRF)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPreset)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelStats;
        private System.Windows.Forms.Button buttonClearSelected;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonRemoveLess2MBit;
        private System.Windows.Forms.Button buttonRemoveH265;
        private System.Windows.Forms.Button buttonRemoveLess4MBit;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonOpenRecycle;
        private System.Windows.Forms.Button buttonRemoveAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarCRF;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelCrfMeaning;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelPresetMeaning;
        private System.Windows.Forms.TrackBar trackBarPreset;
        private System.Windows.Forms.CheckBox checkBoxSkipIfBigger;
        private System.Windows.Forms.CheckBox checkBoxReduceFramerate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox checkBoxScaleDown;
        private System.Windows.Forms.TextBox textBoxScaleDownSmallerSide;
        private System.Windows.Forms.Button buttonClearBacups;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonRemoveLess1080;
        private System.Windows.Forms.Button buttonRemoveLess720;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileBitRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileResolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileFormat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileNewSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFileCompressPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFilePath;
    }
}

