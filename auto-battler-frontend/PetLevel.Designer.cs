using System.ComponentModel;

namespace auto_battler_frontend
{
    partial class PetLevel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.expBack = new System.Windows.Forms.Panel();
            this.experienceBar = new System.Windows.Forms.Panel();
            this.lvlText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // expBack
            // 
            this.expBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(89)))), ((int)(((byte)(0)))));
            this.expBack.Location = new System.Drawing.Point(3, 34);
            this.expBack.Name = "expBack";
            this.expBack.Size = new System.Drawing.Size(75, 20);
            this.expBack.TabIndex = 0;
            // 
            // experienceBar
            // 
            this.experienceBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(185)))), ((int)(((byte)(0)))));
            this.experienceBar.Location = new System.Drawing.Point(3, 34);
            this.experienceBar.Name = "experienceBar";
            this.experienceBar.Size = new System.Drawing.Size(10, 20);
            this.experienceBar.TabIndex = 1;
            // 
            // lvlText
            // 
            this.lvlText.Font = new System.Drawing.Font("Orbitron", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvlText.Location = new System.Drawing.Point(1, 6);
            this.lvlText.Name = "lvlText";
            this.lvlText.Size = new System.Drawing.Size(85, 25);
            this.lvlText.TabIndex = 2;
            this.lvlText.Text = "Lvl1";
            // 
            // PetLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lvlText);
            this.Controls.Add(this.experienceBar);
            this.Controls.Add(this.expBack);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "PetLevel";
            this.Size = new System.Drawing.Size(89, 66);
            this.ResumeLayout(false);
        }

        public System.Windows.Forms.Label lvlText;

        public System.Windows.Forms.Panel experienceBar;

        public System.Windows.Forms.Panel expBack;

        #endregion
    }
}