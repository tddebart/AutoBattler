using System.ComponentModel;

namespace auto_battler_frontend
{
    partial class MainPage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.createRoomButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.roomIdBox = new System.Windows.Forms.TextBox();
            this.joinRoomButton = new System.Windows.Forms.Button();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.disableCoinsButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1582, 101);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pokemon battler";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // createRoomButton
            // 
            this.createRoomButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createRoomButton.Location = new System.Drawing.Point(161, 245);
            this.createRoomButton.Name = "createRoomButton";
            this.createRoomButton.Size = new System.Drawing.Size(212, 102);
            this.createRoomButton.TabIndex = 4;
            this.createRoomButton.Text = "Create room";
            this.createRoomButton.UseVisualStyleBackColor = true;
            this.createRoomButton.Click += new System.EventHandler(this.createRoomButton_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1140, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 51);
            this.label2.TabIndex = 5;
            this.label2.Text = "Join room";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1142, 293);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 38);
            this.label3.TabIndex = 6;
            this.label3.Text = "Room code";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // roomIdBox
            // 
            this.roomIdBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomIdBox.Location = new System.Drawing.Point(1125, 325);
            this.roomIdBox.Name = "roomIdBox";
            this.roomIdBox.Size = new System.Drawing.Size(196, 34);
            this.roomIdBox.TabIndex = 7;
            // 
            // joinRoomButton
            // 
            this.joinRoomButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.joinRoomButton.Location = new System.Drawing.Point(1140, 365);
            this.joinRoomButton.Name = "joinRoomButton";
            this.joinRoomButton.Size = new System.Drawing.Size(165, 47);
            this.joinRoomButton.TabIndex = 8;
            this.joinRoomButton.Text = "Join room";
            this.joinRoomButton.UseVisualStyleBackColor = true;
            this.joinRoomButton.Click += new System.EventHandler(this.joinRoomButton_Click);
            // 
            // userNameLabel
            // 
            this.userNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userNameLabel.Location = new System.Drawing.Point(1283, 9);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(287, 56);
            this.userNameLabel.TabIndex = 9;
            this.userNameLabel.Text = "userName";
            this.userNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // disableCoinsButton
            // 
            this.disableCoinsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disableCoinsButton.Location = new System.Drawing.Point(1320, 742);
            this.disableCoinsButton.Name = "disableCoinsButton";
            this.disableCoinsButton.Size = new System.Drawing.Size(230, 89);
            this.disableCoinsButton.TabIndex = 10;
            this.disableCoinsButton.Text = "DisableCoins";
            this.disableCoinsButton.UseVisualStyleBackColor = true;
            this.disableCoinsButton.Click += new System.EventHandler(this.disableCoinsButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetButton.Location = new System.Drawing.Point(20, 796);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(117, 45);
            this.resetButton.TabIndex = 11;
            this.resetButton.Text = "RESET";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 853);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.disableCoinsButton);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(this.joinRoomButton);
            this.Controls.Add(this.roomIdBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.createRoomButton);
            this.Controls.Add(this.label1);
            this.Name = "MainPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainPage";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button resetButton;

        private System.Windows.Forms.Button disableCoinsButton;

        private System.Windows.Forms.Label userNameLabel;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox roomIdBox;
        private System.Windows.Forms.Button joinRoomButton;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button createRoomButton;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}