
using System.Windows.Forms;

namespace ZZnaCSV
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
            this.data1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.data2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // data1
            // 
            this.data1.Location = new System.Drawing.Point(390, 105);
            this.data1.Name = "data1";
            this.data1.Size = new System.Drawing.Size(143, 20);
            this.data1.Format = DateTimePickerFormat.Short;
            this.data1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Proszę wybrać datę wystawienia dokumentów RW: ";
            // 
            // data2
            // 
            this.data2.Location = new System.Drawing.Point(390, 54);
            this.data2.Name = "data2";
            this.data2.Size = new System.Drawing.Size(143, 20);
            this.data2.Format = DateTimePickerFormat.Short;
            this.data2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(348, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Proszę wybrać datę najwcześniejszego dokumentu FS: ";
            // 
            // button
            // 
            this.button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button.Location = new System.Drawing.Point(138, 152);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(298, 46);
            this.button.TabIndex = 4;
            this.button.Text = "Wydaj";
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 219);
            this.Controls.Add(this.data1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.data2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generowanie RW dla zaznaczonego towaru";
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.DateTimePicker data1;
        private Label label1;
        private System.Windows.Forms.DateTimePicker data2;
        private Label label2;
        private Button button;
    }
}