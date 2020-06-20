namespace Crypto.Hash
{
    partial class Hash
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
            this.inputText = new System.Windows.Forms.TextBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.hashLabel = new System.Windows.Forms.Label();
            this.hashText = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.browseButton = new System.Windows.Forms.Button();
            this.startTime = new System.Windows.Forms.TextBox();
            this.endTime = new System.Windows.Forms.TextBox();
            this.difTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // inputText
            // 
            this.inputText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.inputText.Location = new System.Drawing.Point(128, 125);
            this.inputText.Multiline = true;
            this.inputText.Name = "inputText";
            this.inputText.Size = new System.Drawing.Size(585, 33);
            this.inputText.TabIndex = 0;
            // 
            // inputLabel
            // 
            this.inputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.inputLabel.Location = new System.Drawing.Point(46, 125);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(67, 33);
            this.inputLabel.TabIndex = 1;
            this.inputLabel.Text = "Input";
            // 
            // hashLabel
            // 
            this.hashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.hashLabel.Location = new System.Drawing.Point(46, 231);
            this.hashLabel.Name = "hashLabel";
            this.hashLabel.Size = new System.Drawing.Size(67, 33);
            this.hashLabel.TabIndex = 3;
            this.hashLabel.Text = "Hash";
            // 
            // hashText
            // 
            this.hashText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.hashText.Location = new System.Drawing.Point(128, 231);
            this.hashText.Multiline = true;
            this.hashText.Name = "hashText";
            this.hashText.Size = new System.Drawing.Size(585, 33);
            this.hashText.TabIndex = 2;
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(327, 270);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(123, 49);
            this.generateButton.TabIndex = 4;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(327, 164);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(123, 49);
            this.browseButton.TabIndex = 5;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // startTime
            // 
            this.startTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.startTime.Location = new System.Drawing.Point(128, 335);
            this.startTime.Multiline = true;
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(246, 33);
            this.startTime.TabIndex = 6;
            // 
            // endTime
            // 
            this.endTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.endTime.Location = new System.Drawing.Point(467, 335);
            this.endTime.Multiline = true;
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(246, 33);
            this.endTime.TabIndex = 7;
            // 
            // difTime
            // 
            this.difTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.difTime.Location = new System.Drawing.Point(289, 374);
            this.difTime.Multiline = true;
            this.difTime.Name = "difTime";
            this.difTime.Size = new System.Drawing.Size(246, 33);
            this.difTime.TabIndex = 8;
            // 
            // Hash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.difTime);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.hashLabel);
            this.Controls.Add(this.hashText);
            this.Controls.Add(this.inputLabel);
            this.Controls.Add(this.inputText);
            this.Name = "Hash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputText;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.Label hashLabel;
        private System.Windows.Forms.TextBox hashText;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox startTime;
        private System.Windows.Forms.TextBox endTime;
        private System.Windows.Forms.TextBox difTime;
    }
}

