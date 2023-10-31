
namespace aire
{
    partial class ImageUpload
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
            this.Submit = new System.Windows.Forms.Button();
            this.UploadImage = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Airline = new System.Windows.Forms.Label();
            this.Code = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Submit
            // 
            this.Submit.Location = new System.Drawing.Point(542, 398);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(102, 40);
            this.Submit.TabIndex = 1;
            this.Submit.Text = "Submit";
            this.Submit.UseVisualStyleBackColor = true;
            this.Submit.Click += new System.EventHandler(this.Submit_Click);
            // 
            // UploadImage
            // 
            this.UploadImage.Location = new System.Drawing.Point(598, 196);
            this.UploadImage.Name = "UploadImage";
            this.UploadImage.Size = new System.Drawing.Size(123, 35);
            this.UploadImage.TabIndex = 2;
            this.UploadImage.Text = "Upload Image";
            this.UploadImage.UseVisualStyleBackColor = true;
            this.UploadImage.Click += new System.EventHandler(this.UploadImage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(31, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(483, 357);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Airline
            // 
            this.Airline.AutoSize = true;
            this.Airline.Location = new System.Drawing.Point(574, 48);
            this.Airline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Airline.Name = "Airline";
            this.Airline.Size = new System.Drawing.Size(44, 16);
            this.Airline.TabIndex = 88;
            this.Airline.Text = "Airline";
            // 
            // Code
            // 
            this.Code.AutoSize = true;
            this.Code.Location = new System.Drawing.Point(574, 98);
            this.Code.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(40, 16);
            this.Code.TabIndex = 90;
            this.Code.Text = "Code";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(640, 98);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 91;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(640, 48);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(132, 22);
            this.textBox2.TabIndex = 92;
            // 
            // ImageUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.Airline);
            this.Controls.Add(this.UploadImage);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ImageUpload";
            this.Text = "ImageUpload";
            this.Load += new System.EventHandler(this.ImageUpload_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Submit;
        private System.Windows.Forms.Button UploadImage;
        private System.Windows.Forms.Label Airline;
        private System.Windows.Forms.Label Code;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}