﻿using System.ComponentModel;

namespace auto_battler_frontend
{
    partial class Battler
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
            this.roomCodeLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.party5 = new System.Windows.Forms.Panel();
            this.party4 = new System.Windows.Forms.Panel();
            this.party3 = new System.Windows.Forms.Panel();
            this.party2 = new System.Windows.Forms.Panel();
            this.party1 = new System.Windows.Forms.Panel();
            this.shop5 = new System.Windows.Forms.Panel();
            this.shop4 = new System.Windows.Forms.Panel();
            this.shop3 = new System.Windows.Forms.Panel();
            this.shop2 = new System.Windows.Forms.Panel();
            this.shop1 = new System.Windows.Forms.Panel();
            this.rollButton = new System.Windows.Forms.Button();
            this.hoverInfo = new System.Windows.Forms.Label();
            this.readyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // roomCodeLabel
            // 
            this.roomCodeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomCodeLabel.Location = new System.Drawing.Point(1281, 12);
            this.roomCodeLabel.Name = "roomCodeLabel";
            this.roomCodeLabel.Size = new System.Drawing.Size(289, 89);
            this.roomCodeLabel.TabIndex = 0;
            this.roomCodeLabel.Text = "room code";
            this.roomCodeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 67);
            this.button1.TabIndex = 1;
            this.button1.Text = "Attack1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // party5
            // 
            this.party5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party5.Location = new System.Drawing.Point(30, 12);
            this.party5.Name = "party5";
            this.party5.Size = new System.Drawing.Size(150, 150);
            this.party5.TabIndex = 2;
            // 
            // party4
            // 
            this.party4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party4.Location = new System.Drawing.Point(186, 12);
            this.party4.Name = "party4";
            this.party4.Size = new System.Drawing.Size(150, 150);
            this.party4.TabIndex = 3;
            // 
            // party3
            // 
            this.party3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party3.Location = new System.Drawing.Point(342, 12);
            this.party3.Name = "party3";
            this.party3.Size = new System.Drawing.Size(150, 150);
            this.party3.TabIndex = 4;
            // 
            // party2
            // 
            this.party2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party2.Location = new System.Drawing.Point(498, 12);
            this.party2.Name = "party2";
            this.party2.Size = new System.Drawing.Size(150, 150);
            this.party2.TabIndex = 5;
            // 
            // party1
            // 
            this.party1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party1.Location = new System.Drawing.Point(654, 12);
            this.party1.Name = "party1";
            this.party1.Size = new System.Drawing.Size(150, 150);
            this.party1.TabIndex = 6;
            // 
            // shop5
            // 
            this.shop5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop5.Location = new System.Drawing.Point(663, 548);
            this.shop5.Name = "shop5";
            this.shop5.Size = new System.Drawing.Size(150, 150);
            this.shop5.TabIndex = 11;
            // 
            // shop4
            // 
            this.shop4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop4.Location = new System.Drawing.Point(507, 548);
            this.shop4.Name = "shop4";
            this.shop4.Size = new System.Drawing.Size(150, 150);
            this.shop4.TabIndex = 10;
            // 
            // shop3
            // 
            this.shop3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop3.Location = new System.Drawing.Point(351, 548);
            this.shop3.Name = "shop3";
            this.shop3.Size = new System.Drawing.Size(150, 150);
            this.shop3.TabIndex = 9;
            // 
            // shop2
            // 
            this.shop2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop2.Location = new System.Drawing.Point(195, 548);
            this.shop2.Name = "shop2";
            this.shop2.Size = new System.Drawing.Size(150, 150);
            this.shop2.TabIndex = 8;
            // 
            // shop1
            // 
            this.shop1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop1.Location = new System.Drawing.Point(39, 548);
            this.shop1.Name = "shop1";
            this.shop1.Size = new System.Drawing.Size(150, 150);
            this.shop1.TabIndex = 7;
            // 
            // rollButton
            // 
            this.rollButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rollButton.Location = new System.Drawing.Point(42, 748);
            this.rollButton.Name = "rollButton";
            this.rollButton.Size = new System.Drawing.Size(146, 70);
            this.rollButton.TabIndex = 12;
            this.rollButton.Text = "Roll";
            this.rollButton.UseVisualStyleBackColor = true;
            this.rollButton.Click += new System.EventHandler(this.rollButton_Click);
            // 
            // hoverInfo
            // 
            this.hoverInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hoverInfo.Location = new System.Drawing.Point(1074, 577);
            this.hoverInfo.Name = "hoverInfo";
            this.hoverInfo.Size = new System.Drawing.Size(317, 181);
            this.hoverInfo.TabIndex = 13;
            // 
            // readyButton
            // 
            this.readyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readyButton.Location = new System.Drawing.Point(1411, 760);
            this.readyButton.Name = "readyButton";
            this.readyButton.Size = new System.Drawing.Size(146, 70);
            this.readyButton.TabIndex = 14;
            this.readyButton.Text = "Ready";
            this.readyButton.UseVisualStyleBackColor = true;
            this.readyButton.Click += new System.EventHandler(this.readyButton_Click);
            // 
            // Battler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 853);
            this.Controls.Add(this.readyButton);
            this.Controls.Add(this.hoverInfo);
            this.Controls.Add(this.rollButton);
            this.Controls.Add(this.shop5);
            this.Controls.Add(this.party1);
            this.Controls.Add(this.shop4);
            this.Controls.Add(this.party2);
            this.Controls.Add(this.shop3);
            this.Controls.Add(this.party3);
            this.Controls.Add(this.shop2);
            this.Controls.Add(this.party4);
            this.Controls.Add(this.shop1);
            this.Controls.Add(this.party5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.roomCodeLabel);
            this.Name = "Battler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battler";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button readyButton;

        private System.Windows.Forms.Label hoverInfo;

        private System.Windows.Forms.Panel shop1;
        private System.Windows.Forms.Panel shop2;
        private System.Windows.Forms.Panel shop3;
        private System.Windows.Forms.Panel shop4;
        private System.Windows.Forms.Panel shop5;
        private System.Windows.Forms.Button rollButton;

        private System.Windows.Forms.Panel party5;
        private System.Windows.Forms.Panel party3;
        private System.Windows.Forms.Panel party2;
        private System.Windows.Forms.Panel party1;

        private System.Windows.Forms.Panel party4;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label roomCodeLabel;

        #endregion
    }
}