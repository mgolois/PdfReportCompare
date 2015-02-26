﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace ReportCompare
{
    public partial class MainWindow : Form
    {
        TextWriter _writer = null;
        BindingList<ReportFile> reportList = new BindingList<ReportFile>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void resetProgressBar(int max)
        {
            progressBar.Maximum = max;
            progressBar.Step = 1;
            progressBar.Value = 0;
        }

        public void updateProgressBar()
        {
            progressBar.PerformStep();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
          //  backgroundWorker.ReportProgress(10);
            
          Program.start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var source = new BindingSource();
            source.DataSource = reportList;
            dataGrid.DataSource = source;
            dataGrid.AutoResizeColumns();
          //  dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            DataGridViewColumn column0 = dataGrid.Columns[0];
            column0.Width = 400;
            DataGridViewColumn column1 = dataGrid.Columns[1];
            column1.Width = 60;
  
            // Instantiate the writer
            _writer = new TextBoxStreamWriter(txtConsole);
            // Redirect the out Console stream
            Console.SetOut(_writer);

            Program.log("Ready to work");

            sourcePath.Text = Properties.Settings.Default.SourcePath;
            targetPath.Text = Properties.Settings.Default.TargetPath;
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            var optionsForm = new OptionsWindow();
            optionsForm.ShowDialog();
        }

        private void setSourcePathButton_Click(object sender, EventArgs e)
        {
            string s = Program.selectFolder();
            if (s != "" && s != null)
            {
                sourcePath.Text = s;
                Properties.Settings.Default.SourcePath = s;
                Properties.Settings.Default.Save();
            }
        }

        private void setTargetPathButton_Click(object sender, EventArgs e)
        {
            string s = Program.selectFolder();
            if (s != "" && s != null)
            {
                targetPath.Text = s;
                Properties.Settings.Default.TargetPath = s;
                Properties.Settings.Default.Save();
            }
        }

        public void updateFileStatus(string filename, string newStatus) 
        {
            int fileIndex = reportList.IndexOf(reportList.SingleOrDefault(p => p.Filename.Equals(filename)));
            ReportFile fileToChange = reportList[fileIndex];
            fileToChange.Status = newStatus;
            reportList[fileIndex] = fileToChange;
        }

        public void addReportToList(string filename, string status) {
            reportList.Add(new ReportFile(filename, status));
        }

        public void clearReportList()
        {
            reportList.Clear();
        }
        
    }
}
