namespace PosiljanjeDatotek
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
            this.Streznik = new System.Windows.Forms.Button();
            this.Odjemalec = new System.Windows.Forms.Button();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Streznik
            // 
            this.Streznik.Location = new System.Drawing.Point(0, 36);
            this.Streznik.Name = "Streznik";
            this.Streznik.Size = new System.Drawing.Size(533, 123);
            this.Streznik.TabIndex = 0;
            this.Streznik.Text = "Prejemaj datoteke";
            this.Streznik.UseVisualStyleBackColor = true;
            this.Streznik.Click += new System.EventHandler(this.Streznik_Click);
            // 
            // Odjemalec
            // 
            this.Odjemalec.Location = new System.Drawing.Point(0, 165);
            this.Odjemalec.Name = "Odjemalec";
            this.Odjemalec.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Odjemalec.Size = new System.Drawing.Size(533, 128);
            this.Odjemalec.TabIndex = 1;
            this.Odjemalec.Text = "Pošlji datoteko";
            this.Odjemalec.UseVisualStyleBackColor = true;
            this.Odjemalec.Click += new System.EventHandler(this.Odjemalec_Click);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(315, 12);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(100, 20);
            this.ipBox.TabIndex = 2;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(421, 12);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(100, 20);
            this.portBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 292);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.Odjemalec);
            this.Controls.Add(this.Streznik);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Streznik;
        private System.Windows.Forms.Button Odjemalec;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.TextBox portBox;
    }
}

