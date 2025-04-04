﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace FP_Auto_Video_Converter_2
{
    public partial class Form1 : Form
    {
        private const string FORMAT_UNKNOWN = "Unknown";
        private const string FORMAT_PENDING = "Pending...";
        private const string FORMAT_AVC = "H264 (AVC)";
        private const string FORMAT_HEVC = "H265 (HEVC)";
        private const string STATUS_WAITING = "Очікує";
        private Color STATUS_WAITING_COLOR = Color.LightCyan;
        private const string STATUS_PROCESSING = "Обробка";
        private Color STATUS_PROCESSING_COLOR = Color.Khaki;
        private const string STATUS_COMPLETED = "Готово";
        private Color STATUS_COMPLETED_COLOR = Color.LightGreen;
        private const string STATUS_ERROR = "Помилка";
        private Color STATUS_ERROR_COLOR = Color.LightCoral;
        private const string STATUS_SKIPPED = "Пропущений";
        private Color STATUS_SKIPPED_COLOR = Color.LightGray;

        string tmpfile;
        string recycledir;
        bool stop = false;
        Thread workingThread;
        Process ffmpeg = null;

        private Stopwatch convertTime = new Stopwatch();  //convert time
        private Stopwatch upTime = new Stopwatch(); //app start


        int logLineLength = 160;
        private string lastLog = "";
        private int lastLogCount = 0;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint SetThreadExecutionState(uint esFlags);

        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;


        public Form1()
        {
            InitializeComponent();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // Отримуємо шлях до папки з програмою
            tmpfile = Path.Combine(baseDir, "tmp.mp4");
            recycledir = Path.Combine(baseDir, "ConvertedRecycle");
        }

        bool isArgument(string argument)
        {
            try { 
                string[] args = Environment.GetCommandLineArgs();
                bool exists = Array.Exists(args, arg => arg.Equals(argument, StringComparison.OrdinalIgnoreCase));
                if(exists) {
                    log("Знайдено аргумент: " + argument);
                }
                return exists;
            }
            catch(Exception ex)
            {
                log(ex.Message);
                return false;
            }
        }

        bool getArgument(string pattern, out int value) //ex: -crf*
        {
            value = 0;
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                // Формуємо шаблон регулярного виразу на основі патерну (наприклад, "-crf*")
                string paramPrefix = pattern.TrimEnd('*'); // Отримуємо "-crf"
                string regexPattern = @"^" + Regex.Escape(paramPrefix) + @"(\d+)$";

                for (int i=1; i<args.Length; i++)
                {
                    string arg = args[i];
                    Match match = Regex.Match(arg, regexPattern);
                    if (match.Success)
                    {
                        log("Знайдено аргумент: " + arg);
                        value = int.Parse(match.Groups[1].Value);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log(ex.ToString());
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log("Дата збірки: " + File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location));

            //Виводити в заголовок кількість аргументів
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                if(args.Length > 1)
                {
                    Text += $" (прийнято {args.Length-1} аргументів)";
                }
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }

            //Виводити в заголовок кількість часу роботи
            try
            {
                upTime.Start();
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;
                string baseTitle = Text;
                timer.Tick += (ts, te) => Text = $"{baseTitle}   |   {TimeSpan.FromSeconds(upTime.ElapsedMilliseconds / 1000):hh\\:mm\\:ss}";
                timer.Start();
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }


            //перевірити чи проги на місці
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory; // Отримуємо шлях до папки з програмою
                string mediaInfoPath = Path.Combine(baseDir, "MediaInfo.exe");
                string ffmpegPath = Path.Combine(baseDir, "ffmpeg.exe");
                if (!File.Exists(mediaInfoPath) || !File.Exists(ffmpegPath))
                {
                    MessageBox.Show("Не знайдено необхідні файли:\n" +
                                    (!File.Exists(mediaInfoPath) ? "- MediaInfo.exe\n" : "") +
                                    (!File.Exists(ffmpegPath) ? "- ffmpeg.exe\n" : ""),
                                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }


            Directory.CreateDirectory(recycledir);
            updateStats();

            // Запобігаємо переходу в режим сну
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
            log("Комп’ютер не перейде в режим сну, поки працює ця програма.");

            processArgumentsOnLoad();

            
        }

        private string getArgumentsHelp()
        {
            return "Цю програму в задачах автоматизації можна викликати з командного рядка з аргументами." +
                "\nАргументи командного рядка. Ними можна буквально \"натикати\" потрібні кнопки аргументами." +
                    "\n\n \"Адреса папки\" - додати папку. Пишіть з лапками, так надійніше." +
                    "\n\n \"Адреса файлу\" - додати файл. Пишіть з лапками, так надійніше." +
                    "\n\n -help - вивести це повідомлення в лог." +
                    "\n\n -skipY - поставити галочку \"Пропускати файли які вийшли більші ніж були\"." +
                    "\n\n -skipN - зняти галочку \"Пропускати файли які вийшли більші ніж були\"." +
                    "\n\n -reduceFramerate30 - Поставити галочку \"Зменшити частоту кадрів до\" і вписати \"30\" (або інше число). " +
                    "\n\n -reduceFramerateN - Зняти галочку \"Зменшити частоту кадрів до \"." +
                    "\n\n -scale1080 - Поставити галочку \"Зменшити роздільну здатність до\" і вписати \"1080\" (або інше число)." +
                    "\n\n -scaleN - Зняти галочку \"Зменшити роздільну здатність до\"." +
                    "\n\n -crf33 - Задати CRF=33 (або інше число)." +
                    "\n\n -preset4 - Задати preset=faster (або інший, число до 10)." +
                    "\n\n -clearResolution1080 - Очистити зі списку всі файли менші за 1080 по меншій стороні (або інше число)." +
                    "\n\n -clearBitrate10 - Очистити зі списку всі файли бітрейтом менше 10 мегабіт  (або інше число)." +
                    "\n\n -clearH265 - Очистити зі списку всі файли що вже в кодеку H265 (HEVC)." +
                    "\n\n -start - автоматичний запуск стиснення без втручання користувача." +
                    "\n\n -exit - автоматично вийти коли завершиться стиснення." +
                    "\n\n -----------------------------" +
                    "\n Аргументи чутливі до регістру." +
                    "\n Невалідні аргументи ігноруються." +
                    "\n Значення які не уточнено аргументами залишаться за замовчуванням." +
                    "\n ";
        }

        void processArgumentsOnLoad()
        {
            string[] args = Environment.GetCommandLineArgs();
            // Перевірка на наявність аргументів
            if (args.Length < 2)
                return;
            if (isArgument("-help") || isArgument("-h") || isArgument("help") || isArgument("\\h") || isArgument("/h"))
                log(getArgumentsHelp());
            if (isArgument("-skipY"))
                checkBoxSkipIfBigger.Checked = true;
            if (isArgument("-skipN"))
                checkBoxSkipIfBigger.Checked = false;
            if (getArgument("-reduceFramerate*", out int reduceFramerateValue))
            {
                checkBoxReduceFramerate.Checked = true;
                textBoxReduceFramerateValue.Text = reduceFramerateValue.ToString();
            }
            if (isArgument("-reduceFramerateN"))
                checkBoxReduceFramerate.Checked = false;

            if (getArgument("-scale*", out int scale))
            {
                checkBoxScaleDown.Checked = true;
                textBoxScaleDownSmallerSide.Text = scale.ToString();
            }
            if (isArgument("-scaleN"))
                checkBoxScaleDown.Checked = false;
            if (getArgument("-crf*", out int crf))
                if (crf <= trackBarCRF.Maximum && crf >= trackBarCRF.Minimum)
                    trackBarCRF.Value = crf;
            if (getArgument("-preset*", out int preset))
                if (preset <= trackBarPreset.Maximum && preset >= trackBarPreset.Minimum)
                    trackBarPreset.Value = preset;


            // Перевірка, чи є адреса папки в аргументах
            List<string> filesToAdd = new List<string>();
            for (int i = 1; i < args.Length; i++)
            {
                string arg = args[i];
                if (Directory.Exists(arg))  // Перевірка, чи є папка
                {
                    log($"Знайдена папка: {arg}");
                    filesToAdd.Add(arg);
                }
                else if (File.Exists(arg))  // Перевірка на файл
                {
                    log($"Знайдений файл: {arg}");
                    filesToAdd.Add(arg);
                }
            }
            if (filesToAdd.Count > 0)
            {
                string[] files = filesToAdd.ToArray();
                log($"Починаю додавати {files.Length} файлів до списку...");
                workingThread = new Thread(() => addFilesAsync(files));
                workingThread.IsBackground = true;
                workingThread.Start();
            }
        }
        void processArgumentsOnFormatDetectingFinished()
        {
            try
            {
                if (isArgument("-clearH265"))
                    buttonRemoveH265_Click(null, null);
                if (getArgument("-clearBitrate*", out int bitrate))
                {
                    textBoxBitrateToClear.Text = bitrate.ToString();
                    buttonRemoveLessMBit_Click(null, null);
                }
                if (getArgument("-clearResolution*", out int resolution))
                {
                    textBoxResolutionToClear.Text = resolution.ToString();
                    buttonRemoveLessPx_Click(null, null);
                }
                if (isArgument("-start"))
                    buttonStart_Click(null, null);
            }
            catch (Exception e)
            {
                log(e.ToString());
            }
        }



        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (workingThread != null)
                {
                    log("Зараз не вийде дропнути файли оскільки триває інша робота.");
                    return;
                }
                log("Дропай!");
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (workingThread != null)
            {
                log("Файли не було прийнято оскільки триває інша робота.");
                return;
            }
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            log($"Починаю додавати {files.Length} файлів до списку...");
            workingThread = new Thread(() => addFilesAsync(files));
            workingThread.IsBackground = true;
            workingThread.Start();
        }
        void addFilesAsync(string[] files)
        {
            try
            {
                status("Додаю файли...");
                buttonsActive(false);
                Thread.Sleep(100);
                foreach (string file in files)
                {
                    Invoke(new Action(() => addFileToList(file))); // Оновлення UI
                    Thread.Sleep(20);  //to let UI be responsive
                }
                status("Готово.");

                Invoke(new Action(() =>
                {
                    log("Всього файлів: " + (dataGridView1.Rows.Count - 1));
                    buttonsActive(true);  //Вернути юзеру кнопочки
                    updateStats();  //Оновити циферки
                    workingThread = null;  //щоб другі функції знали що можна починати роботу
                    startDetectingFormat(); //якщо додались якісь файли - почати аналіз формату
                }));
            }
            finally
            {
                if(Thread.CurrentThread == workingThread)
                    workingThread = null;
            }
        }

        DateTime lastDoEventsTime = DateTime.MinValue;
        private void addFileToList(string filePath)
        {
            try
            {
                //check folder
                if (Directory.Exists(filePath))
                {
                    log("ВІДКРИТИ папку: " + Path.GetFileName(filePath));
                    foreach (string file in Directory.GetDirectories(filePath))
                        addFileToList(file);
                    foreach (string file in Directory.GetFiles(filePath))
                        addFileToList(file);
                    return;
                }

                //check format
                if (!filePath.ToLower().EndsWith(".mp4") && !filePath.ToLower().EndsWith(".avi") && !filePath.ToLower().EndsWith(".mov")
                    && !filePath.ToLower().EndsWith(".mpg") && !filePath.ToLower().EndsWith(".3gp") && !filePath.ToLower().EndsWith(".wmv")
                    && !filePath.ToLower().EndsWith(".m4a") && !filePath.ToLower().EndsWith(".mkv"))
                {
                    //log("ПРОПУСТИТИ, File have unsupported format: " + Path.GetFileName(filePath));
                    return;
                }
                // check duplicate
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (!dataGridView1.Rows[i].IsNewRow && dataGridView1.Rows[i].Cells["ColumnFilePath"].Value.ToString().Equals(filePath))
                    {
                        log("ПРОПУСТИТИ, Знайдено дублікат: " + Path.GetFileName(filePath));
                        return;
                    }
                }


                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells["ColumnFilePath"].Value = filePath;
                dataGridView1.Rows[rowIndex].Cells["ColumnFileName"].Value = Path.GetFileName(filePath);
                dataGridView1.Rows[rowIndex].Cells["ColumnFileFolder"].Value = Path.GetFileName(Path.GetDirectoryName(filePath)) ?? "-";
                dataGridView1.Rows[rowIndex].Cells["ColumnFileSize"].Value = FormatBytes(new FileInfo(filePath).Length);
                dataGridView1.Rows[rowIndex].Cells["ColumnFileFormat"].Value = FORMAT_PENDING;
                dataGridView1.Rows[rowIndex].Cells["ColumnFileStatus"].Value = STATUS_WAITING;
                dataGridView1.Rows[rowIndex].Cells["ColumnFileStatus"].Style.BackColor = STATUS_WAITING_COLOR;
                dataGridView1.Rows[rowIndex].Cells["ColumnFileNewSize"].Value = " ";
                log($"Файл додано ({rowIndex}): {filePath}");

                TimeSpan timeSinceLast = DateTime.Now - lastDoEventsTime;
                if(timeSinceLast.TotalMilliseconds > 1500)
                {
                    Application.DoEvents();
                    lastDoEventsTime = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                log(e.ToString());
            }
        }




        public string log(string s)
        {
            if (s == null)
                return null;
            if (s.Contains("\n"))
            {
                foreach (string s2 in s.Split('\n'))
                    log(s2);
                return s;
            }
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new MethodInvoker(() => { log(s); }));
                return s;
            }

            try
            {
                s = Regex.Replace(s, @"[^\p{L}\p{N}\p{P}\p{Z}\=]", ""); //фільтрувати недруковані символи
                string prefix =  DateTime.Now.ToString() + ":     ";
                int initialLength = richTextBox1.TextLength + prefix.Length;
                if (!s.Equals("") && s.Equals(lastLog))
                {
                    lastLogCount++;
                    changeLine(richTextBox1, richTextBox1.Lines.Length - 1, prefix + s + " (" + lastLogCount + ")");
                }
                else
                {
                    lastLogCount = 0;
                    if (s.Length + prefix.Length <= logLineLength)
                    {
                        richTextBox1.AppendText("\n" + prefix + s);
                    }
                    else
                    {
                        int threshold = logLineLength - prefix.Length; //make transition by words
                        for (int i = threshold; i > threshold - 20; i--)
                        {
                            if (s[i] == ' ')
                            {
                                threshold = i;
                                break;
                            }
                        }
                        richTextBox1.AppendText("\n" + prefix + s.Substring(0, threshold));
                        log("     " + s.Substring(threshold).Trim());
                    }
                }
                lastLog = s;
                //Application.DoEvents();
            }
            catch (Exception) { }
            return s;
        }
        void changeLine(RichTextBox RTB, int line, string text)
        {
            int s1 = RTB.GetFirstCharIndexFromLine(line);
            int s2 = line < RTB.Lines.Count() - 1 ?
                      RTB.GetFirstCharIndexFromLine(line + 1) - 1 :
                      RTB.Text.Length;
            RTB.Select(s1, s2 - s1);
            RTB.SelectedText = text;
        }
        static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}"; // Формат з двома знаками після коми
        }

        void startDetectingFormat()
        {
            if(workingThread == null)
            {
                log("Запуск визначення формату...");
                workingThread = new Thread(() => RunMediaInfoAsync());
                workingThread.Start();
            }
        }
        void RunMediaInfoAsync()
        {
            try
            {
                string mediaInfoPath = "MediaInfo.exe";
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    status($"Отримання інформації про формат ({i}/{dataGridView1.Rows.Count}) ...");
                    buttonsActive(false);
                    if (dataGridView1.Rows[i].IsNewRow)
                        continue;
                    string fileCurrentFormat = dataGridView1.Rows[i].Cells["ColumnFileFormat"].Value.ToString(); // Читання значення
                    if (!fileCurrentFormat.Equals(FORMAT_PENDING))
                        continue;
                    string fileName = dataGridView1.Rows[i].Cells["ColumnFileName"].Value.ToString();
                    string filePath = dataGridView1.Rows[i].Cells["ColumnFilePath"].Value.ToString();
                    string fileFormat = FORMAT_UNKNOWN;
                    string fileBitrate = "0";
                    string fileResolution = "0x0x0";
                    try
                    {
                        //get format, bitrate, resolution
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = mediaInfoPath, // Вказуємо шлях до MediaInfo.exe
                            Arguments = $"--Inform=\"Video;%Format%|%BitRate%|%Width%x%Height%x%Rotation%\" \"{filePath}\"", // Отримуємо формат
                            RedirectStandardOutput = true, // Перехоплюємо вивід
                            UseShellExecute = false, // Не використовуємо оболонку
                            CreateNoWindow = true // Не показуємо вікно
                        };
                        using (Process process = Process.Start(processStartInfo))
                        {
                            string output = process.StandardOutput.ReadToEnd();
                            process.WaitForExit(); // Чекаємо завершення процесу
                            //Ex:    AVC|27839570|1920x1080
                            string[] parts = output.Split('|');
                            fileFormat = parts[0].Trim();  // AVC
                            long bitrate = long.Parse(parts[1]);  // 27839570
                            double bitrateInMb = bitrate / 1000000.0; // Переведення в мегабіти (МБіт)
                            fileBitrate = bitrateInMb.ToString("F2") + " МБіт"; // Форматування для виведення користувачу "83.85 МБіт"
                            fileResolution = parts[2].Trim();  // 1920x1080x0.000
                            if (fileResolution.EndsWith("x")) // 1920x1080x   ->    1920x1080x0
                                fileResolution += "0";
                            if (fileResolution.EndsWith(".000"))  //1920x1080x0.000   ->   1920x1080x0
                                fileResolution = fileResolution.Replace(".000", "");
                        }

                        log($"Файл: {fileName} | Формат: {fileFormat} | Бітрейт: {fileBitrate} | Роздільна здатність: {fileResolution}");
                        updateFormatInDataGrid(i, fileFormat, fileBitrate, fileResolution);
                    }
                    catch (Exception ex)
                    {
                        updateFormatInDataGrid(i, FORMAT_UNKNOWN, "0", "0x0x0");
                        log($"Помилка при обробці файлу {fileName}: {ex.Message}");
                    }
                }
            }
            finally
            {
                log("Аналіз форматів завершено.");
                buttonsActive(true);
                status("Готово.");
                workingThread = null;
                Invoke(new Action(() =>
                {
                    processArgumentsOnFormatDetectingFinished();
                }));
            }
        }
        void updateFormatInDataGrid(int index, string format, string bitrate, string resolution)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() =>
                    {
                        updateFormatInDataGrid(index, format, bitrate, resolution);
                    }));
                    return;
                }
                if (format.Equals("AVC"))
                    dataGridView1.Rows[index].Cells["ColumnFileFormat"].Value = FORMAT_AVC;
                else if (format.Equals("HEVC"))
                    dataGridView1.Rows[index].Cells["ColumnFileFormat"].Value = FORMAT_HEVC;
                else
                    dataGridView1.Rows[index].Cells["ColumnFileFormat"].Value = format;
                dataGridView1.Rows[index].Cells["ColumnFileBitrate"].Value = bitrate;
                dataGridView1.Rows[index].Cells["ColumnFileResolution"].Value = resolution;

                if (index == dataGridView1.Rows.Count - 2)
                    dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                else if (dataGridView1.FirstDisplayedScrollingRowIndex < index - 22)
                    dataGridView1.FirstDisplayedScrollingRowIndex = index;
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
        }
        void updateStats()
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() =>
                    {
                        updateStats();
                    }));
                    return;
                }

                int total = 0;
                long bytesTotal = 0;

                int waiting = 0;
                long bytesWaiting = 0;

                int completed = 0;
                long bytesCompleted = 0;
                long bytesNewCompleted = 0;

                int skipped = 0;
                long bytesSkipped = 0;

                int errors = 0;
                long bytesErrors = 0;


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (!dataGridView1.Rows[i].IsNewRow)
                    {
                        long bytes = ConvertToBytes(dataGridView1.Rows[i].Cells["ColumnFileSize"].Value.ToString());
                        total++;
                        bytesTotal += bytes;
                        string status = dataGridView1.Rows[i].Cells["ColumnFileStatus"].Value.ToString();
                        if (status.Equals(STATUS_COMPLETED))
                        {
                            bytesCompleted += bytes;
                            completed++;
                            long newbytes = ConvertToBytes(dataGridView1.Rows[i].Cells["ColumnFileNewSize"].Value.ToString());
                            bytesNewCompleted += newbytes;
                        }
                        if (status.Equals(STATUS_WAITING))
                        {
                            bytesWaiting += bytes;
                            waiting++;
                        }
                        if (status.Equals(STATUS_ERROR))
                        {
                            bytesErrors += bytes;
                            errors++;
                        }
                        if (status.Equals(STATUS_SKIPPED))
                        {
                            bytesSkipped += bytes;
                            skipped++;
                        }
                    }
                }

                TimeSpan roundedUptime = TimeSpan.FromSeconds(Math.Round(convertTime.Elapsed.TotalSeconds));

                string percent = "";
                if (bytesNewCompleted > 0 && bytesCompleted > 0)
                {
                    double compressionPercent = (1 - (double)bytesNewCompleted / bytesCompleted) * 100;
                    percent = $" (-{compressionPercent:F2}%)";
                }
                long totalRecycleBytes = 0;
                foreach (string file in Directory.GetFiles(recycledir))
                {
                    totalRecycleBytes += new FileInfo(file).Length;
                }

                labelStats.Text =
                    $"Резервних копій: {Directory.GetFiles(recycledir).Length} шт ({FormatBytes(totalRecycleBytes)})\n" +
                    "\n" +
                    "Всього файлів: " + total + " шт (" + FormatBytes(bytesTotal) + ")\n" +
                    "Очікує:        " + waiting + " шт (" + FormatBytes(bytesWaiting) + ")\n" +
                    "Пропущено:     " + skipped + " шт (" + FormatBytes(bytesSkipped) + ")\n" +
                    "Помилок:       " + errors + " шт (" + FormatBytes(bytesErrors) + ")\n" +
                    $"Готово:        {completed} шт ({FormatBytes(bytesCompleted)})\n" +
                    $"\n" +
                    $"Стиснення:     {roundedUptime:hh\\:mm\\:ss} (-{FormatBytes(bytesCompleted - bytesNewCompleted)})\n" +
                    $"{FormatBytes(bytesCompleted)} -> {FormatBytes(bytesNewCompleted)}{percent}\n";
            }
            catch (Exception e)
            {
                log("Помилка оновлення статистики: " + e.Message);
            }
        }


        public static long ConvertToBytes(string size)
        {
            //try
            {
                // Регулярний вираз для пошуку числа та одиниці вимірювання
                var match = Regex.Match(size, @"(\d+[\.,]?\d*)\s*(KB|MB|GB|B)", RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    throw new ArgumentException("Невірний формат введеного розміру");
                }

                // Отримуємо число і одиницю вимірювання
                string numberString = match.Groups[1].Value;//.Replace(",", ".");
                double number = double.Parse(numberString);
                string unit = match.Groups[2].Value.ToUpper();

                // Переводимо в байти в залежності від одиниці вимірювання
                switch (unit)
                {
                    case "KB":
                        return (long)(number * 1024); // 1 KB = 1024 байти
                    case "MB":
                        return (long)(number * 1024 * 1024); // 1 MB = 1024 * 1024 байти
                    case "GB":
                        return (long)(number * 1024 * 1024 * 1024); // 1 GB = 1024 * 1024 * 1024 байти
                    case "B":
                        return (long)number; // Вже в байтах
                    default:
                        throw new ArgumentException("Невідома одиниця вимірювання");
                }
            }
            //catch (Exception e) { return 0; }
        }

        private void buttonClearSelected_Click(object sender, EventArgs e)
        {
            log("Видаляю зі списку всі вибрані строки");
            int removed = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                // Якщо рядок не є новим (порожнім)
                if (!row.IsNewRow)
                {
                    dataGridView1.Rows.Remove(row);
                    removed++;
                }
            }
            updateStats();
            log("Видалено "+removed+" рядків.");
        }
        public string status(string s)
        {
            if (s == null)
                return null;
            if (labelStatus.InvokeRequired)
            {
                labelStatus.Invoke(new MethodInvoker(() => { status(s); }));
                return s;
            }
            labelStatus.Text = s;
            Application.DoEvents();
            return s;
        }
        DataGridViewColumn sortedColumn;
        SortOrder sortOrder;
        public void buttonsActive(bool active)
        {
            if (labelStatus.InvokeRequired)
            {
                labelStatus.Invoke(new MethodInvoker(() => { buttonsActive(active); }));
            }
            buttonClearSelected.Enabled = active;
            buttonStart.Enabled = active;
            buttonRemoveH265.Enabled = active;
            textBoxBitrateToClear.Enabled = active;
            buttonRemoveLessMBit.Enabled = active;
            textBoxResolutionToClear.Enabled = active;
            buttonRemoveLessPx.Enabled = active;
            buttonRemoveAll.Enabled = active;
            trackBarCRF.Enabled = active;
            trackBarPreset.Enabled = active;
            checkBoxReduceFramerate.Enabled = active;
            textBoxReduceFramerateValue.Enabled = active;
            checkBoxSkipIfBigger.Enabled = active;
            textBoxScaleDownSmallerSide.Enabled = active;
            checkBoxScaleDown.Enabled = active;
            buttonExit.Enabled = active;
            if (active)
            {
                buttonStop.Enabled = false;
                //вернути сортування
                if (sortedColumn != null)
                {
                    ListSortDirection direction = (sortOrder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    dataGridView1.Sort(sortedColumn, direction);
                }
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            else
            {
                //відключити сортування
                sortedColumn = dataGridView1.SortedColumn;
                sortOrder = dataGridView1.SortOrder;
                foreach(DataGridViewColumn column in dataGridView1.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void buttonRemoveLessMBit_Click(object sender, EventArgs e)
        {
            try {
                double.TryParse(textBoxBitrateToClear.Text, out double filterBitrate);
                if (filterBitrate <= 0 || filterBitrate > 6000)
                {
                    log("Впишіть нормальне значення бітрейту.");
                    return;
                }
                status(log($"Видаляю зі списку всі що менше {filterBitrate} МБіт/с..."));
                int removed = 0;
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    if (!row.IsNewRow)
                    {
                        string inputText = row.Cells["ColumnFileBitrate"].Value.ToString();
                        string numericValueStr = inputText.Replace(" МБіт", "");
                        double numericBitrate = double.Parse(numericValueStr);
                        if (numericBitrate < filterBitrate)
                        {
                            dataGridView1.Rows.Remove(row);
                            removed++;
                        }
                    }
                }
                updateStats();
                status("Готово.");
                log("Видалено " + removed + " рядків.");
            }
            catch (Exception ex)
            {
                log("Помилка: " + ex.Message);
            }
        }

        private void buttonRemoveH265_Click(object sender, EventArgs e)
        {
            try
            {
                status(log("Видаляю зі списку всі що вже в 265 кодеку"));
                int removed = 0;
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    if (!row.IsNewRow)
                    {
                        string inputText = row.Cells["ColumnFileFormat"].Value.ToString();
                        if (inputText.Equals(FORMAT_HEVC))
                        {
                            dataGridView1.Rows.Remove(row);
                            removed++;
                        }
                    }
                }
                updateStats();
                status("Готово.");
                log("Видалено " + removed + " рядків.");
            }
            catch (Exception ex)
            {
                log("Помилка: " + ex.Message);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (workingThread != null)
                return;
            try
            {
                log("Збір даних...");
                int crf = trackBarCRF.Value;
                getPresetInfo(trackBarPreset.Value, out string preset, out string description);
                bool reduceFramerate = checkBoxReduceFramerate.Checked;
                int.TryParse(textBoxReduceFramerateValue.Text, out int reduceFramerateValue);
                if (reduceFramerateValue < 1 || reduceFramerateValue > 240)
                {
                    log("Впишіть нормальне значення кількості кадрів на секунду");
                    return;
                }
                bool skipBigger = checkBoxSkipIfBigger.Checked;
                bool downscale = checkBoxScaleDown.Checked;
                int.TryParse(textBoxScaleDownSmallerSide.Text, out int downscaleSmallerSide);
                if(downscaleSmallerSide < 100 || downscaleSmallerSide > 6000)
                {
                    log("Впишіть нормальне значення роздільної здатності");
                    return;
                }

                log($"CRF = {crf}");
                if(reduceFramerate)
                    log($"reduceFramerateValue = {reduceFramerateValue}");
                log($"skipBigger = {skipBigger}");
                if(downscale)
                    log($"downscaleSmallerSide = {downscaleSmallerSide}");

                log("Запуск стиснення...");
                stop = false;
                buttonStop.Enabled = true;
                convertTime.Start();
                workingThread = new Thread(() => runConvertAsync(crf, preset, reduceFramerate, reduceFramerateValue, skipBigger, downscale, downscaleSmallerSide));
                workingThread.Start();
            }
            catch (Exception ex)
            {
                log("Помилка: " + ex.Message);
            }
        }

        private void runConvertAsync(int crf, string preset, bool reduceFramerate, int reduceFramerateValue, bool skipBigger, bool downscale, double targetSmallerSide)
        {
            try
            {
                buttonsActive(false);
                for (int i = 0; !stop && i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].IsNewRow)
                        continue;
                    string fileCurrentStatus = dataGridView1.Rows[i].Cells["ColumnFileStatus"].Value.ToString(); 
                    if (!fileCurrentStatus.Equals(STATUS_WAITING) && !fileCurrentStatus.Equals(STATUS_ERROR))
                        continue;
                    updateStatusInDataGrid(i, STATUS_PROCESSING, STATUS_PROCESSING_COLOR, "");



                    string fileName = dataGridView1.Rows[i].Cells["ColumnFileName"].Value.ToString();
                    string filePath = dataGridView1.Rows[i].Cells["ColumnFilePath"].Value.ToString();
                    string fileResolution = dataGridView1.Rows[i].Cells["ColumnFileResolution"].Value.ToString();
                    string[] resolutionParts = fileResolution.Split('x');  //1920x1080x90
                    int width = int.Parse(resolutionParts[0]); // 1920
                    int height = int.Parse(resolutionParts[1]); // 1080
                    int rotation = int.Parse(resolutionParts[2]); // 0   90   180    270
                    float smallerSide = Math.Min(width, height);
                    log($"Стиснення файлу: {fileName}");
                    try
                    {
                        //Підготувати файл до стиснення
                        File.Delete(tmpfile);


                        //Стиснути відео в тимчасовий файл
                        double videoDuration = GetVideoDurationInSeconds(filePath);
                        string resolution = "";
                        if (downscale && smallerSide > targetSmallerSide)
                        {
                            double scaleFactor = targetSmallerSide / smallerSide; //less 1
                            int newWidth = (int)(width * scaleFactor);
                            int newHeight = (int)(height * scaleFactor);
                            // Округлюємо до парного значення (FFmpeg працює коректніше)
                            newWidth = newWidth % 2 == 0 ? newWidth : newWidth + 1;
                            newHeight = newHeight % 2 == 0 ? newHeight : newHeight + 1;
                            // Змінюємо ширину та висоту залежно від кута повороту
                            if (rotation == 90 || rotation == 270)
                            {
                                int temp = newWidth;
                                newWidth = newHeight;
                                newHeight = temp;
                            }
                            resolution = $"-vf \"scale={newWidth}:{newHeight}\" ";
                        }
                        string framerate = reduceFramerate? $"-r {reduceFramerateValue} " : "";
                        string arguments = $"-i \"{filePath}\" -c:v libx265 -preset {preset} -crf {crf} {framerate}{resolution}-progress pipe:1 \"{tmpfile}\"";
                        string exe = "ffmpeg.exe";
                        log(exe + " " + arguments);
                        ffmpeg = new Process();
                        ffmpeg.StartInfo.FileName = exe;
                        ffmpeg.StartInfo.Arguments = arguments;
                        ffmpeg.StartInfo.RedirectStandardOutput = true;
                        ffmpeg.StartInfo.UseShellExecute = false;
                        ffmpeg.StartInfo.CreateNoWindow = true;

                        ffmpeg.OutputDataReceived += (sender, args) => {
                            if (args.Data != null)
                            {
                                if (args.Data.Contains("time=")) // Шукаємо інформацію про час виводу
                                {
                                    log(args.Data);
                                    string timeStr = args.Data.Split('=')[1].Split(' ')[0]; //Парсимо поточний Час у форматі hh:mm:ss.xx
                                    TimeSpan currentTime = TimeSpan.Parse(timeStr);
                                    double percentage = (currentTime.TotalSeconds / videoDuration) * 100;
                                    status($"Прогрес файлу: {percentage:F2}%");
                                    updateStats();
                                }
                            }
                        };
                        ffmpeg.Start();
                        ffmpeg.BeginOutputReadLine();
                        ffmpeg.WaitForExit();


                        //перевірити чи то не користувач відмінив
                        if (stop)
                        {
                            updateStats();
                            string msg = $"Стиснення відмінено користувачем";
                            updateStatusInDataGrid(i, STATUS_WAITING, STATUS_WAITING_COLOR, msg);
                            log(msg);
                            continue;
                        }


                        //Перевірити чи валідним є файл
                        double resultingFileDuration = GetVideoDurationInSeconds(tmpfile);
                        log($"Довжина оригінального файлу: {videoDuration} секунд.");
                        log($"Довжина результуючого файлу: {resultingFileDuration} секунд.");
                        double difference = Math.Abs(resultingFileDuration- videoDuration);
                        log($"Різниця: {difference} секунд.");
                        if( difference > 5 ) 
                            throw new Exception($"Помилка валідації файлу {fileName} - відрізняється довжина на {difference} секунд!");

                        //перевірити розмір результуючого файлу
                        long oldSize = new FileInfo(filePath).Length;
                        long newSize = new FileInfo(tmpfile).Length;
                        double compressionPercent = (1 - (double)newSize / oldSize) * 100;
                        string percent = $"-{compressionPercent:F2}%";
                        log($"Розмір оригінального файлу: {FormatBytes(oldSize)} .");
                        log($"Розмір результуючого файлу: {FormatBytes(newSize)} .");
                        log($"Зміна розміру: {percent} .");
                        if(oldSize < newSize && skipBigger)
                        {
                            updateStats();
                            string msg = $"Стиснення пропущено через те що новий файл більший ({FormatBytes(newSize)})";
                            updateStatusInDataGrid(i, STATUS_SKIPPED, STATUS_SKIPPED_COLOR, msg);
                            log(msg);
                            continue;
                        }

                        //Забекапити оригінальний файл і замінити на стиснений
                        DateTime creationTime = File.GetCreationTime(filePath); // Отримуємо дати з оригінального файлу
                        DateTime lastWriteTime = File.GetLastWriteTime(filePath);
                        DateTime lastAccessTime = File.GetLastAccessTime(filePath);
                        string backupPath = recycledir + "/" + fileName;
                        while(File.Exists(backupPath))
                            backupPath += "1";
                        File.Move(filePath, backupPath);
                        //замінити розширення файла на .mp4 незалежно від того яке було
                        string filePathMP4 = Path.ChangeExtension(filePath, ".mp4");
                        File.Move(tmpfile, filePathMP4);
                        File.SetCreationTime(filePathMP4, creationTime); // Переносимо дати на новий файл
                        File.SetLastWriteTime(filePathMP4, lastWriteTime);
                        File.SetLastAccessTime(filePathMP4, lastAccessTime);

                        //Оновити дані в таблиці
                        updateStats();
                        updateStatusInDataGrid(i, STATUS_COMPLETED, STATUS_COMPLETED_COLOR, "");
                        updateNewSizeInDataGrid(i, FormatBytes(newSize), percent);
                        log($"Стиснення завершено: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        updateStats();
                        string err = $"Помилка при стисненні файлу {fileName}: {ex.Message}";
                        updateStatusInDataGrid(i, STATUS_ERROR, STATUS_ERROR_COLOR, err);
                        log(err);
                        log(ex.StackTrace);

                        //перевірити чи то не користувач відмінив
                        if (stop)
                        {
                            updateStats();
                            string msg = $"Стиснення відмінено користувачем";
                            updateStatusInDataGrid(i, STATUS_WAITING, STATUS_WAITING_COLOR, msg);
                            log(msg);
                            continue;
                        }
                    }
                }
            }
            finally
            {
                log("Стиснення завершено.");
                buttonsActive(true);
                status("Готово.");
                convertTime.Stop();
                workingThread = null;
                updateStats();

                if (isArgument("-exit"))
                    Application.Exit();
            }
        }


        void updateStatusInDataGrid(int index, string status, Color color, String tooltip)
        {
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() =>
                    {
                        updateStatusInDataGrid(index, status, color, tooltip);
                    }));
                    return;
                }
                dataGridView1.Rows[index].Cells["ColumnFileStatus"].Value = status;
                dataGridView1.Rows[index].Cells["ColumnFileStatus"].Style.BackColor = color;
                dataGridView1.Rows[index].Cells["ColumnFileStatus"].ToolTipText = tooltip;

                if (dataGridView1.FirstDisplayedScrollingRowIndex < index - 22)
                    dataGridView1.FirstDisplayedScrollingRowIndex = index;
            }
            catch(Exception ex)
            {
                log(ex.ToString());
            }
        }

        void updateNewSizeInDataGrid(int index, string newSize, string percent)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    updateNewSizeInDataGrid(index, newSize, percent);
                }));
                return;
            }
            dataGridView1.Rows[index].Cells["ColumnFileNewSize"].Value = newSize;
            dataGridView1.Rows[index].Cells["ColumnFileCompressPercent"].Value = percent;
        }

        double GetVideoDurationInSeconds(string filePath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-i \"{filePath}\"",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            string output = "";
            using (Process process = Process.Start(processStartInfo))
            {
                output = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            string durationStr = output.Split(new string[] { "Duration: " }, StringSplitOptions.None)[1].Split(',')[0];
            TimeSpan duration = TimeSpan.Parse(durationStr);
            log($"Довжина файлу {Path.GetFileName(filePath)} : {duration.TotalSeconds} секунд.");
            return duration.TotalSeconds;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (ffmpeg != null)
                {
                    if (!ffmpeg.HasExited)
                    {
                        ffmpeg.Kill();
                        stop = true;
                        ffmpeg.Close();
                        ffmpeg = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }   
        }

        private void buttonOpenRecycle_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", recycledir);
        }

        private void buttonRemoveAll_Click(object sender, EventArgs e)
        {
            log("Видаляю зі списку всі");
            dataGridView1.Rows.Clear();
            updateStats();
        }

        private void trackBarCRF_ValueChanged(object sender, EventArgs e)
        {
            int crf = trackBarCRF.Value;

            string description = crf + " (";
            if (crf < 18)
                description += "Майже ідеал, але файл величезний";
            else if (crf < 21)
                description += "Дуже хороша якість і файл великий";
            else if (crf < 26)
                description += "Хороша якість і норм файл";
            else if (crf < 31)
                description += "Середня якість і малий файл";
            else if (crf < 37)
                description += "Прям погано, але файл маленький";
            else 
                description += "Тупо мєсіво, але нічо не важитиме";
            description += ")";
            labelCrfMeaning.Text = description;
        }

        // Метод для отримання значення пресета та його опису
        void getPresetInfo(int sliderValue, out string presetName, out string presetDescription)
        {
            switch (sliderValue)
            {
                case 0:
                    presetName = "ultrafast";
                    presetDescription = "Дуже швидко, низька якість стиснення";
                    break;
                case 1:
                    presetName = "superfast";
                    presetDescription = "Дуже швидко, трохи краще стискає";
                    break;
                case 2:
                    presetName = "veryfast";
                    presetDescription = "Швидке кодування, менш ефективне стиснення";
                    break;
                case 3:
                    presetName = "faster";
                    presetDescription = "Швидше за середнє, непогане стиснення";
                    break;
                case 4:
                    presetName = "fast";
                    presetDescription = "Баланс між швидкістю та стисненням";
                    break;
                case 5:
                    presetName = "medium";
                    presetDescription = "Стандартний баланс";
                    break;
                case 6:
                    presetName = "slow";
                    presetDescription = "Повільніше, але краще стискає";
                    break;
                case 7:
                    presetName = "slower";
                    presetDescription = "Дуже повільно, але ще краще стискає";
                    break;
                case 8:
                    presetName = "veryslow";
                    presetDescription = "Довго, максимальне стиснення";
                    break;
                case 9:
                    presetName = "placebo";
                    presetDescription = "Дуже довго, майже без виграшу в якості";
                    break;
                default:
                    presetName = "medium";
                    presetDescription = "Стандартний баланс";
                    break;
            }
        }

        private void trackBarPreset_ValueChanged(object sender, EventArgs e)
        {
            getPresetInfo(trackBarPreset.Value, out string preset, out string description);
            labelPresetName.Text = preset;
            labelPresetMeaning.Text = description;
        }

        private void buttonClearBacups_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            $"Ви впевнені, що хочете видалити всі {Directory.GetFiles(recycledir).Length} бекапів?",
            "Підтвердження видалення",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                int deletedCount = 0;

                try
                {
                    if (Directory.Exists(recycledir))
                    {
                        string[] files = Directory.GetFiles(recycledir);
                        log($"Знайдено {files.Length} файлів для видалення.\n");

                        foreach (string file in files)
                        {
                            try
                            {
                                log($"Видаляю: {Path.GetFileName(file)}...");
                                //FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                                File.Delete(file);
                                deletedCount++;
                            }
                            catch (Exception ex)
                            {
                                log($"❌ Помилка при видаленні {file}: {ex.Message}");
                            }
                        }

                        log($"\n✅ Успішно видалено {deletedCount} з {files.Length} файлів.");
                        updateStats();
                    }
                    else
                    {
                        log("Папка не існує.");
                    }
                }
                catch (Exception ex)
                {
                    log($"Глобальна помилка: {ex.Message}");
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Перевірка, що клікнули на рядок, а не на заголовок
                {
                    string filePath = dataGridView1.Rows[e.RowIndex].Cells["ColumnFilePath"].Value?.ToString();
                    string filePathMP4 = Path.ChangeExtension(filePath, ".mp4");
                    if (e.ColumnIndex == 1) //open folder
                    {
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                        else if (!string.IsNullOrEmpty(filePathMP4) && File.Exists(filePathMP4))
                            Process.Start("explorer.exe", $"/select,\"{filePathMP4}\"");
                    }
                    if (e.ColumnIndex == 0) //open file
                    {
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                            Process.Start(filePath);
                        else if (!string.IsNullOrEmpty(filePathMP4) && File.Exists(filePathMP4))
                            Process.Start("explorer.exe", $"/select,\"{filePathMP4}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося відкрити файл:\n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRemoveLessPx_Click(object sender, EventArgs e)
        {
            try
            {
                double.TryParse(textBoxResolutionToClear.Text, out double filterSmallerSide);
                if (filterSmallerSide <= 10 || filterSmallerSide > 6000)
                {
                    log("Впишіть нормальне значення роздільної здатності.");
                    return;
                }
                status(log($"Видаляю зі списку всі що роздільною здатністю менше {filterSmallerSide} px по меншій стороні..."));
                int removed = 0;
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    if (!row.IsNewRow)
                    {
                        string fileResolution = row.Cells["ColumnFileResolution"].Value.ToString();
                        string[] resolutionParts = fileResolution.Split('x');  //1920x1080x90
                        int width = int.Parse(resolutionParts[0]); // 1920
                        int height = int.Parse(resolutionParts[1]); // 1080
                        int rotation = int.Parse(resolutionParts[2]); // 0   90   180    270
                        float smallerSide = Math.Min(width, height);
                        if (smallerSide < filterSmallerSide)
                        {
                            dataGridView1.Rows.Remove(row);
                            removed++;
                        }
                    }
                }
                updateStats();
                status("Готово.");
                log("Видалено " + removed + " рядків.");
            }
            catch (Exception ex)
            {
                log("Помилка: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Повертаємо систему в нормальний режим
            SetThreadExecutionState(ES_CONTINUOUS);

            //видаляти tmp файл
            if (File.Exists(tmpfile))
                File.Delete(tmpfile);

            // Створюємо шлях до каталогу Logs
            string logsDirectory = Path.Combine(Application.StartupPath, "Logs");

            // Якщо каталогу ще немає, створюємо його
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            // Формуємо ім'я файлу на основі поточної дати та часу
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";
            string filePath = Path.Combine(logsDirectory, fileName);

            // Зберігаємо текст з RichTextBox в файл
            File.WriteAllText(filePath, richTextBox1.Text);

            // Вивести повідомлення в консоль (за потребою)
            Console.WriteLine($"Лог збережено: {filePath}");
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            string description = "FP AutoVideoConverter 2.4" +
                "\n" +
                "\nЦя програма дозволяє автоматизувати процес стиснення відеофайлів у кодек H.265 (HEVC), " +
                "надаючи зручний інтерфейс для пакетного стиснення великої кількості відео." +
                "\nВона допоможе вам зменшити розмір старих відеоархівів, щоб вони займали менше місця." +
                "\nСтискання великої кількості відео може тривати дуже довго — від кількох годин до тижнів." +
                "\n" +
                "\nЯк користуватися програмою:" +
                "\n - Перетягніть файли або папки у вікно програми для стиснення;" +
                "\n - Програма одразу додасть усі відеофайли до черги;" +
                "\n - Вона автоматично проаналізує формат кожного відео в черзі;" +
                "\n - Виберіть налаштування стиснення: якість, роздільну здатність тощо;" +
                "\n - За потреби видаліть з черги відео, які не потребують стиснення;" +
                "\n - Натисніть \"Почати стиснення\" та дочекайтеся завершення процесу;" +
                "\n - Після завершення перевірте стиснені файли, щоб переконатися в їхній якості;" +
                "\n - Якщо якийсь файл виявився пошкодженим, відновіть його з папки резервних копій;" +
                "\n - Коли робота завершена, очистіть папку резервних копій, щоб вона не займала місце." +
                "\n" +
                "\nРозробник: Dr. Failov" +
                "\n2025";


            // Виведення опису через MessageBox
            MessageBox.Show(description, "Про програму", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                object value1object = e.CellValue1;
                object value2object = e.CellValue2;
                if (value1object == null || value2object == null)
                    return;
                string value1String = value1object.ToString();
                string value2String = value2object.ToString();
                if (value1String.Trim().Equals("") || value2String.Trim().Equals(""))
                    return;
                if (e.Column.Name.Equals("ColumnFileNewSize"))
                {
                    long value1 = ConvertToBytes(value1String);
                    long value2 = ConvertToBytes(value2String);
                    if (value1 < value2) e.SortResult = -1; // Якщо перше значення менше
                    else if (value1 > value2) e.SortResult = 1;  // Якщо перше значення більше
                    else e.SortResult = 0;  // Якщо рівні
                    e.Handled = true; // Уникаємо стандартного сортування
                }
                if (e.Column.Name.Equals("ColumnFileSize"))
                {
                    long value1 = ConvertToBytes(value1String);
                    long value2 = ConvertToBytes(value2String);
                    if (value1 < value2) e.SortResult = -1; // Якщо перше значення менше
                    else if (value1 > value2) e.SortResult = 1;  // Якщо перше значення більше
                    else e.SortResult = 0;  // Якщо рівні
                    e.Handled = true; // Уникаємо стандартного сортування
                }
                if (e.Column.Name.Equals("ColumnFileBitRate")) 
                {
                    double value1 = double.Parse(value1String.Replace(" МБіт", ""));
                    double value2 = double.Parse(value2String.Replace(" МБіт", ""));
                    if (value1 < value2) e.SortResult = -1; // Якщо перше значення менше
                    else if (value1 > value2) e.SortResult = 1;  // Якщо перше значення більше
                    else e.SortResult = 0;  // Якщо рівні
                    e.Handled = true; // Уникаємо стандартного сортування
                }
                if (e.Column.Name.Equals("ColumnFileResolution"))
                {
                    string[] resolution1Parts = value1String.Split('x');  //1920x1080x90
                    int width1 = int.Parse(resolution1Parts[0]); // 1920
                    int height1 = int.Parse(resolution1Parts[1]); // 1080
                    int rotation1 = int.Parse(resolution1Parts[2]); // 0   90   180    270
                    float smallerSide1 = Math.Min(width1, height1);

                    string[] resolution2Parts = value2String.Split('x');  //1920x1080x90
                    int width2 = int.Parse(resolution2Parts[0]); // 1920
                    int height2 = int.Parse(resolution2Parts[1]); // 1080
                    int rotation2 = int.Parse(resolution2Parts[2]); // 0   90   180    270
                    float smallerSide2 = Math.Min(width2, height2);

                    if (smallerSide1 < smallerSide2) e.SortResult = -1; // Якщо перше значення менше
                    else if (smallerSide1 > smallerSide2) e.SortResult = 1;  // Якщо перше значення більше
                    else e.SortResult = 0;  // Якщо рівні
                    e.Handled = true; // Уникаємо стандартного сортування
                }
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
        }

        private void buttonHelpTerminal_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                getArgumentsHelp() + 
                "\n\nВи можете скопіювати текст з цього вікна натиснувши CTRL+C.", 
                "Виклик через командний рядок", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}


/*
 2.1
- видаляти tmp файл за собою
- заміняти розширення з довільного на mp4 якщо було інше
- дабл клік на комірку відкриє або файл або папку залежно куди клікнути

2.2
- Не пропускати в лог спецсимволи (не ламається лог якщо якісь спецсимволи в назві)
- Оновив кольори статусів
- Захист від дропа файлів під час обробки
- Додавання файлів з дропа та з аргументів відбувається в фоновому потоці завдяки чому програма не зависає якщо файлів багато
- Виправлено сортування таблиці - тепер коректно працює сортування розміру та бітрейту
- Переробив інтерфейс програми (сподіваюсь покращив)
- Перенесення метаданих зі старого файла на новий
- Доповнено опис програми
- Запобігання вимкненню або сну компа поки відкрита програма
- Додано костиль щоб програма не висла якщо скормити їй 10тис файлів

2.3
- Додано лічильник при додаванні файлів
- Додано лічильник при визначенні формату
- Оновлено мімінамальний розмір вікна
- Додано лого
- Додано налаштування фільтра бітрейту і роздільної здатності
- правильне сортування роздільної здатності
- Змінено спосіб визначення шляху до папки з бекапами
- Якщо програма запущена з аргументами то про це писатиме в заголовку
- Доповнено зразки батніка
- Доповнено документацію
- Покращення підказок і інтерфейсу

2.4
- В інтерфейсі додано можливість обрати довільну частоту кадрів
- Додано відповідні аргументи для налаштування частоти стиснення
- Додано виведення в заголовок вікна часу роботи програми
- Оновлено документацію
 */