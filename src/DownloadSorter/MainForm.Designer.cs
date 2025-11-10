using System.Drawing;

namespace DownloadsSorter
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SortOldFiles_checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StandartSorterStart_button = new System.Windows.Forms.Button();
            this.panelRules = new System.Windows.Forms.Panel();
            this.btnSaveRules = new System.Windows.Forms.Button();
            this.btnAddRule = new System.Windows.Forms.Button();
            this.txtNewFolder = new System.Windows.Forms.TextBox();
            this.txtNewExtension = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.HelpButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.HelpButton)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // SortOldFiles_checkBox
            // 
            this.SortOldFiles_checkBox.AutoSize = true;
            this.SortOldFiles_checkBox.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SortOldFiles_checkBox.Location = new System.Drawing.Point(34, 290);
            this.SortOldFiles_checkBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SortOldFiles_checkBox.Name = "SortOldFiles_checkBox";
            this.SortOldFiles_checkBox.Size = new System.Drawing.Size(322, 24);
            this.SortOldFiles_checkBox.TabIndex = 7;
            this.SortOldFiles_checkBox.Text = "Отсортировать старые файлы в загрузках";
            this.SortOldFiles_checkBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(71, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Правила сортировки:";
            // 
            // StandartSorterStart_button
            // 
            this.StandartSorterStart_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.StandartSorterStart_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StandartSorterStart_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StandartSorterStart_button.Location = new System.Drawing.Point(404, 235);
            this.StandartSorterStart_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StandartSorterStart_button.Name = "StandartSorterStart_button";
            this.StandartSorterStart_button.Size = new System.Drawing.Size(262, 49);
            this.StandartSorterStart_button.TabIndex = 5;
            this.StandartSorterStart_button.Text = "Запустить сортировщик";
            this.StandartSorterStart_button.UseVisualStyleBackColor = false;
            this.StandartSorterStart_button.Click += new System.EventHandler(this.StandartSorterStart_button_Click);
            // 
            // panelRules
            // 
            this.panelRules.Location = new System.Drawing.Point(12, 52);
            this.panelRules.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelRules.Name = "panelRules";
            this.panelRules.Size = new System.Drawing.Size(362, 230);
            this.panelRules.TabIndex = 8;
            // 
            // btnSaveRules
            // 
            this.btnSaveRules.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(201)))), ((int)(((byte)(153)))));
            this.btnSaveRules.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveRules.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveRules.ForeColor = System.Drawing.Color.White;
            this.btnSaveRules.Location = new System.Drawing.Point(75, 322);
            this.btnSaveRules.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveRules.Name = "btnSaveRules";
            this.btnSaveRules.Size = new System.Drawing.Size(230, 39);
            this.btnSaveRules.TabIndex = 12;
            this.btnSaveRules.Text = "Сохранить правила";
            this.btnSaveRules.UseVisualStyleBackColor = false;
            // 
            // btnAddRule
            // 
            this.btnAddRule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRule.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddRule.Location = new System.Drawing.Point(470, 149);
            this.btnAddRule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(138, 33);
            this.btnAddRule.TabIndex = 11;
            this.btnAddRule.Text = "Добавить правило";
            this.btnAddRule.UseVisualStyleBackColor = true;
            this.btnAddRule.Click += new System.EventHandler(this.btnAddRule_Click);
            // 
            // txtNewFolder
            // 
            this.txtNewFolder.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtNewFolder.Location = new System.Drawing.Point(560, 118);
            this.txtNewFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewFolder.Name = "txtNewFolder";
            this.txtNewFolder.Size = new System.Drawing.Size(100, 23);
            this.txtNewFolder.TabIndex = 10;
            // 
            // txtNewExtension
            // 
            this.txtNewExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtNewExtension.Location = new System.Drawing.Point(414, 118);
            this.txtNewExtension.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewExtension.Name = "txtNewExtension";
            this.txtNewExtension.Size = new System.Drawing.Size(100, 22);
            this.txtNewExtension.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(437, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Добавление правила:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(411, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Расширение:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(557, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "Папка:";
            // 
            // HelpButton
            // 
            this.HelpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpButton.Image = global::DownloadsSorter.Properties.Resources.info;
            this.HelpButton.Location = new System.Drawing.Point(638, 3);
            this.HelpButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(46, 44);
            this.HelpButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.HelpButton.TabIndex = 1;
            this.HelpButton.TabStop = false;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(114)))), ((int)(((byte)(167)))));
            this.ClientSize = new System.Drawing.Size(689, 426);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSaveRules);
            this.Controls.Add(this.btnAddRule);
            this.Controls.Add(this.txtNewFolder);
            this.Controls.Add(this.txtNewExtension);
            this.Controls.Add(this.panelRules);
            this.Controls.Add(this.SortOldFiles_checkBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StandartSorterStart_button);
            this.Controls.Add(this.HelpButton);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(705, 465);
            this.MinimumSize = new System.Drawing.Size(705, 465);
            this.Name = "MainForm";
            this.Text = "Download Sorter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.HelpButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.PictureBox HelpButton;
        private System.Windows.Forms.CheckBox SortOldFiles_checkBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StandartSorterStart_button;
        private System.Windows.Forms.Panel panelRules;
        private System.Windows.Forms.Button btnSaveRules;
        private System.Windows.Forms.Button btnAddRule;
        private System.Windows.Forms.TextBox txtNewFolder;
        private System.Windows.Forms.TextBox txtNewExtension;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

