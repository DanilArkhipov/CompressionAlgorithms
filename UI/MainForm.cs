﻿using Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Data.Enums;

namespace UI
{
    public partial class MainForm : Form
    {
        private string currentFileExtension;
        public MainForm()
        {
            InitializeComponent();
            comboBoxSelectAlgorithm.DataSource = EnumHelper.GetAlgorithmNames();
            comboBoxSelectAction.DataSource = EnumHelper.GetActionNames();
            labelChooseFile.Focus();

            comboBoxSelectAlgorithm.SelectedValueChanged += ComboBoxSelectAlgorithm_SelectedValueChanged;
            comboBoxSelectAction.SelectedValueChanged += ComboBoxSelectAction_SelectedValueChanged;

            btnChooseFile.Click += BtnChooseFile_Click;

            btnAccept.Click += BtnAccept_Click;
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {     
            saveFileDialog.Filter = GetSaveFileFilter();
            saveFileDialog.RestoreDirectory = true;

            string data;
            try
            {
                using (var sr = new StreamReader(labelFilePath.Text)) {
                    data = sr.ReadToEnd();
                }
            }
            catch
            {
                throw new ArgumentException("Не удалось открыть файл с данными");
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var sw = new StreamWriter(saveFileDialog.FileName))
                {
                    if ((string)comboBoxSelectAlgorithm.SelectedItem == EnumHelper.GetRussianAlgorithmName(Data.Enums.Algorithm.Hamming.ToString()))
                    {
                        if ((string)comboBoxSelectAction.SelectedItem == EnumHelper.GetRussianActionName(Data.Enums.Action.Encode.ToString()))
                        {
                            var encodedData = Hamming.Encode(data);
                            sw.WriteLine(encodedData);
                        }
                        else
                        {
                            var decodedData = Hamming.Decode(data);
                            sw.WriteLine(decodedData);
                        }
                    }
                    
                }

                
            }
        }

        private void BtnChooseFile_Click(object sender, EventArgs e)
        {
            openFileDialogChooseFile.Filter = GetChooseFileFilter();

            if (openFileDialogChooseFile.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialogChooseFile.FileName;
            // сохраняем текст в файл
            labelFilePath.Text = filename;
        }

        private void ComboBoxSelectAlgorithm_SelectedValueChanged(object sender, EventArgs e)
        {
            lblChooseAlgorithm.Focus();
        }

        private void ComboBoxSelectAction_SelectedValueChanged(object sender, EventArgs e)
        {
            lblSelectAction.Focus();
        }

        private string GetChooseFileFilter()
        {
            string comboboxSelectedText = (string)comboBoxSelectAction.SelectedValue;
            if (comboboxSelectedText == EnumHelper.GetRussianActionName(Data.Enums.Action.Encode.ToString()))
            {
                return "Text files(*.txt)|*.txt";
            }

            if (comboboxSelectedText == EnumHelper.GetRussianActionName(Data.Enums.Action.Decode.ToString()))
            {
                comboboxSelectedText = (string)comboBoxSelectAlgorithm.SelectedValue;

                if(comboboxSelectedText == EnumHelper.GetRussianAlgorithmName(Algorithm.Hamming.ToString()))
                {
                    return "Hamming encoded files(*.hef)|*.hef";
                }

                if (comboboxSelectedText == EnumHelper.GetRussianAlgorithmName(Algorithm.ShennonFano.ToString()))
                {
                    return "Shennon-Fano compressed files(*.sfc)|*.sfc";
                }

                throw new ArgumentException("Выберете алгоритм");
            }

            throw new ArgumentException("Выберете что нужно сделать с файлом");
        }

        private string GetSaveFileFilter()
        {
            string comboboxSelectedText = (string)comboBoxSelectAction.SelectedValue;
            if (comboboxSelectedText == EnumHelper.GetRussianActionName(Data.Enums.Action.Decode.ToString()))
            {
                return "Text files(*.txt)|*.txt";
            }

            if (comboboxSelectedText == EnumHelper.GetRussianActionName(Data.Enums.Action.Encode.ToString()))
            {
                comboboxSelectedText = (string)comboBoxSelectAlgorithm.SelectedValue;

                if (comboboxSelectedText == EnumHelper.GetRussianAlgorithmName(Algorithm.Hamming.ToString()))
                {
                    return "Hamming encoded files(*.hef)|*.hef";
                }

                if (comboboxSelectedText == EnumHelper.GetRussianAlgorithmName(Algorithm.ShennonFano.ToString()))
                {
                    return "Shennon-Fano compressed files(*.sfc)|*.sfc";
                }

                throw new ArgumentException("Выберете алгоритм");
            }

            throw new ArgumentException("Выберете что нужно сделать с файлом");
        }
    }
}
