
namespace UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxSelectAlgorithm = new System.Windows.Forms.ComboBox();
            this.lblChooseAlgorithm = new System.Windows.Forms.Label();
            this.lblSelectAction = new System.Windows.Forms.Label();
            this.comboBoxSelectAction = new System.Windows.Forms.ComboBox();
            this.labelChooseFile = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.openFileDialogChooseFile = new System.Windows.Forms.OpenFileDialog();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.btnAccept = new System.Windows.Forms.Button();
            this.lblErrors = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // comboBoxSelectAlgorithm
            // 
            this.comboBoxSelectAlgorithm.FormattingEnabled = true;
            this.comboBoxSelectAlgorithm.Location = new System.Drawing.Point(168, 31);
            this.comboBoxSelectAlgorithm.Name = "comboBoxSelectAlgorithm";
            this.comboBoxSelectAlgorithm.Size = new System.Drawing.Size(207, 28);
            this.comboBoxSelectAlgorithm.TabIndex = 0;
            // 
            // lblChooseAlgorithm
            // 
            this.lblChooseAlgorithm.AutoSize = true;
            this.lblChooseAlgorithm.Location = new System.Drawing.Point(12, 31);
            this.lblChooseAlgorithm.Name = "lblChooseAlgorithm";
            this.lblChooseAlgorithm.Size = new System.Drawing.Size(150, 20);
            this.lblChooseAlgorithm.TabIndex = 1;
            this.lblChooseAlgorithm.Text = "Выберете алгоритм:";
            // 
            // lblSelectAction
            // 
            this.lblSelectAction.AutoSize = true;
            this.lblSelectAction.Location = new System.Drawing.Point(12, 94);
            this.lblSelectAction.Name = "lblSelectAction";
            this.lblSelectAction.Size = new System.Drawing.Size(147, 20);
            this.lblSelectAction.TabIndex = 2;
            this.lblSelectAction.Text = "Выберете действие:";
            // 
            // comboBoxSelectAction
            // 
            this.comboBoxSelectAction.FormattingEnabled = true;
            this.comboBoxSelectAction.Location = new System.Drawing.Point(168, 94);
            this.comboBoxSelectAction.Name = "comboBoxSelectAction";
            this.comboBoxSelectAction.Size = new System.Drawing.Size(207, 28);
            this.comboBoxSelectAction.TabIndex = 3;
            // 
            // labelChooseFile
            // 
            this.labelChooseFile.AutoSize = true;
            this.labelChooseFile.Location = new System.Drawing.Point(12, 162);
            this.labelChooseFile.Name = "labelChooseFile";
            this.labelChooseFile.Size = new System.Drawing.Size(392, 20);
            this.labelChooseFile.TabIndex = 4;
            this.labelChooseFile.Text = "Выберете файл, к которому будет применён алгоритм:";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(410, 158);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(170, 29);
            this.btnChooseFile.TabIndex = 5;
            this.btnChooseFile.Text = "Указать файл";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            // 
            // openFileDialogChooseFile
            // 
            this.openFileDialogChooseFile.FileName = "openFileDialog1";
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(66, 193);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(124, 20);
            this.labelFilePath.TabIndex = 6;
            this.labelFilePath.Text = "Файл не выбран";
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(12, 281);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(568, 42);
            this.btnAccept.TabIndex = 12;
            this.btnAccept.Text = "Применить";
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.Location = new System.Drawing.Point(12, 249);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(0, 20);
            this.lblErrors.TabIndex = 13;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(12, 193);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(48, 20);
            this.lblFile.TabIndex = 14;
            this.lblFile.Text = "Файл:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 332);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.lblErrors);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.labelChooseFile);
            this.Controls.Add(this.comboBoxSelectAction);
            this.Controls.Add(this.lblSelectAction);
            this.Controls.Add(this.lblChooseAlgorithm);
            this.Controls.Add(this.comboBoxSelectAlgorithm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Алгоритмы кодирования";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxSelectAlgorithm;
        private System.Windows.Forms.Label lblChooseAlgorithm;
        private System.Windows.Forms.Label lblSelectAction;
        private System.Windows.Forms.ComboBox comboBoxSelectAction;
        private System.Windows.Forms.Label labelChooseFile;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogChooseFile;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

