﻿
namespace WCS
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usu1 = new System.Windows.Forms.TextBox();
            this.pass1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.enter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(142, 63);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(45, 113);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(203, 124);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Playbill", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(13, 251);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 34);
            this.label1.TabIndex = 3;
            this.label1.Text = "ID";
            // 
            // usu1
            // 
            this.usu1.BackColor = System.Drawing.Color.Brown;
            this.usu1.Font = new System.Drawing.Font("Playbill", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.usu1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.usu1.Location = new System.Drawing.Point(13, 289);
            this.usu1.Multiline = true;
            this.usu1.Name = "usu1";
            this.usu1.Size = new System.Drawing.Size(264, 41);
            this.usu1.TabIndex = 4;
            // 
            // pass1
            // 
            this.pass1.BackColor = System.Drawing.Color.Brown;
            this.pass1.Font = new System.Drawing.Font("Playbill", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pass1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pass1.Location = new System.Drawing.Point(13, 371);
            this.pass1.Multiline = true;
            this.pass1.Name = "pass1";
            this.pass1.PasswordChar = '*';
            this.pass1.Size = new System.Drawing.Size(264, 41);
            this.pass1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Playbill", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(13, 333);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 34);
            this.label2.TabIndex = 5;
            this.label2.Text = "Senha";
            // 
            // enter
            // 
            this.enter.BackColor = System.Drawing.Color.Snow;
            this.enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enter.Font = new System.Drawing.Font("Playbill", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.enter.Location = new System.Drawing.Point(13, 439);
            this.enter.Name = "enter";
            this.enter.Size = new System.Drawing.Size(264, 58);
            this.enter.TabIndex = 7;
            this.enter.Text = "Entrar";
            this.enter.UseVisualStyleBackColor = false;
            this.enter.Click += new System.EventHandler(this.enter_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Brown;
            this.ClientSize = new System.Drawing.Size(289, 509);
            this.Controls.Add(this.enter);
            this.Controls.Add(this.pass1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.usu1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DarkRed;
            this.Load += new System.EventHandler(this.Login_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox usu1;
        private System.Windows.Forms.TextBox pass1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button enter;
    }
}
