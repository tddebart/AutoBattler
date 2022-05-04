using System.ComponentModel;

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
            this.party5 = new System.Windows.Forms.Panel();
            this.party4 = new System.Windows.Forms.Panel();
            this.party3 = new System.Windows.Forms.Panel();
            this.party2 = new System.Windows.Forms.Panel();
            this.party1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.shop5 = new System.Windows.Forms.Panel();
            this.shop4 = new System.Windows.Forms.Panel();
            this.shop3 = new System.Windows.Forms.Panel();
            this.shop2 = new System.Windows.Forms.Panel();
            this.shop1 = new System.Windows.Forms.Panel();
            this.rollButton = new System.Windows.Forms.Button();
            this.readyButton = new System.Windows.Forms.Button();
            this.battle1 = new System.Windows.Forms.Panel();
            this.battle2 = new System.Windows.Forms.Panel();
            this.battle3 = new System.Windows.Forms.Panel();
            this.battle4 = new System.Windows.Forms.Panel();
            this.battle5 = new System.Windows.Forms.Panel();
            this.battleOp5 = new System.Windows.Forms.Panel();
            this.battleOp4 = new System.Windows.Forms.Panel();
            this.battleOp3 = new System.Windows.Forms.Panel();
            this.battleOp2 = new System.Windows.Forms.Panel();
            this.battleOp1 = new System.Windows.Forms.Panel();
            this.battlePanel = new System.Windows.Forms.Panel();
            this.effectImage = new System.Windows.Forms.PictureBox();
            this.party1DamageText = new System.Windows.Forms.Label();
            this.party2DamageText = new System.Windows.Forms.Label();
            this.partyPanel = new System.Windows.Forms.Panel();
            this.coinText = new System.Windows.Forms.Label();
            this.shopPanel = new System.Windows.Forms.Panel();
            this.petHoverInfo = new auto_battler_frontend.PetHoverInfo();
            this.party1.SuspendLayout();
            this.battlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.effectImage)).BeginInit();
            this.partyPanel.SuspendLayout();
            this.shopPanel.SuspendLayout();
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
            // party5
            // 
            this.party5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party5.Location = new System.Drawing.Point(10, 199);
            this.party5.Name = "party5";
            this.party5.Size = new System.Drawing.Size(150, 150);
            this.party5.TabIndex = 2;
            // 
            // party4
            // 
            this.party4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party4.Location = new System.Drawing.Point(166, 199);
            this.party4.Name = "party4";
            this.party4.Size = new System.Drawing.Size(150, 150);
            this.party4.TabIndex = 3;
            // 
            // party3
            // 
            this.party3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party3.Location = new System.Drawing.Point(322, 199);
            this.party3.Name = "party3";
            this.party3.Size = new System.Drawing.Size(150, 150);
            this.party3.TabIndex = 4;
            // 
            // party2
            // 
            this.party2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party2.Location = new System.Drawing.Point(478, 199);
            this.party2.Name = "party2";
            this.party2.Size = new System.Drawing.Size(150, 150);
            this.party2.TabIndex = 5;
            // 
            // party1
            // 
            this.party1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.party1.Controls.Add(this.label1);
            this.party1.Location = new System.Drawing.Point(634, 199);
            this.party1.Name = "party1";
            this.party1.Size = new System.Drawing.Size(150, 150);
            this.party1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // shop5
            // 
            this.shop5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop5.Location = new System.Drawing.Point(638, 13);
            this.shop5.Name = "shop5";
            this.shop5.Size = new System.Drawing.Size(150, 150);
            this.shop5.TabIndex = 11;
            // 
            // shop4
            // 
            this.shop4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop4.Location = new System.Drawing.Point(482, 13);
            this.shop4.Name = "shop4";
            this.shop4.Size = new System.Drawing.Size(150, 150);
            this.shop4.TabIndex = 10;
            // 
            // shop3
            // 
            this.shop3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop3.Location = new System.Drawing.Point(326, 13);
            this.shop3.Name = "shop3";
            this.shop3.Size = new System.Drawing.Size(150, 150);
            this.shop3.TabIndex = 9;
            // 
            // shop2
            // 
            this.shop2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop2.Location = new System.Drawing.Point(170, 13);
            this.shop2.Name = "shop2";
            this.shop2.Size = new System.Drawing.Size(150, 150);
            this.shop2.TabIndex = 8;
            // 
            // shop1
            // 
            this.shop1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shop1.Location = new System.Drawing.Point(14, 13);
            this.shop1.Name = "shop1";
            this.shop1.Size = new System.Drawing.Size(150, 150);
            this.shop1.TabIndex = 7;
            // 
            // rollButton
            // 
            this.rollButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rollButton.Location = new System.Drawing.Point(17, 213);
            this.rollButton.Name = "rollButton";
            this.rollButton.Size = new System.Drawing.Size(146, 70);
            this.rollButton.TabIndex = 12;
            this.rollButton.Text = "Roll";
            this.rollButton.UseVisualStyleBackColor = true;
            this.rollButton.Click += new System.EventHandler(this.rollButton_Click);
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
            // battle1
            // 
            this.battle1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battle1.Location = new System.Drawing.Point(581, 236);
            this.battle1.Name = "battle1";
            this.battle1.Size = new System.Drawing.Size(125, 125);
            this.battle1.TabIndex = 11;
            // 
            // battle2
            // 
            this.battle2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battle2.Location = new System.Drawing.Point(450, 236);
            this.battle2.Name = "battle2";
            this.battle2.Size = new System.Drawing.Size(125, 125);
            this.battle2.TabIndex = 10;
            // 
            // battle3
            // 
            this.battle3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battle3.Location = new System.Drawing.Point(319, 236);
            this.battle3.Name = "battle3";
            this.battle3.Size = new System.Drawing.Size(125, 125);
            this.battle3.TabIndex = 9;
            // 
            // battle4
            // 
            this.battle4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battle4.Location = new System.Drawing.Point(188, 236);
            this.battle4.Name = "battle4";
            this.battle4.Size = new System.Drawing.Size(125, 125);
            this.battle4.TabIndex = 8;
            // 
            // battle5
            // 
            this.battle5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battle5.Location = new System.Drawing.Point(57, 236);
            this.battle5.Name = "battle5";
            this.battle5.Size = new System.Drawing.Size(125, 125);
            this.battle5.TabIndex = 7;
            // 
            // battleOp5
            // 
            this.battleOp5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battleOp5.Location = new System.Drawing.Point(1312, 236);
            this.battleOp5.Name = "battleOp5";
            this.battleOp5.Size = new System.Drawing.Size(125, 125);
            this.battleOp5.TabIndex = 16;
            // 
            // battleOp4
            // 
            this.battleOp4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battleOp4.Location = new System.Drawing.Point(1181, 236);
            this.battleOp4.Name = "battleOp4";
            this.battleOp4.Size = new System.Drawing.Size(125, 125);
            this.battleOp4.TabIndex = 15;
            // 
            // battleOp3
            // 
            this.battleOp3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battleOp3.Location = new System.Drawing.Point(1050, 236);
            this.battleOp3.Name = "battleOp3";
            this.battleOp3.Size = new System.Drawing.Size(125, 125);
            this.battleOp3.TabIndex = 14;
            // 
            // battleOp2
            // 
            this.battleOp2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battleOp2.Location = new System.Drawing.Point(919, 236);
            this.battleOp2.Name = "battleOp2";
            this.battleOp2.Size = new System.Drawing.Size(125, 125);
            this.battleOp2.TabIndex = 13;
            // 
            // battleOp1
            // 
            this.battleOp1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.battleOp1.Location = new System.Drawing.Point(788, 236);
            this.battleOp1.Name = "battleOp1";
            this.battleOp1.Size = new System.Drawing.Size(125, 125);
            this.battleOp1.TabIndex = 12;
            // 
            // battlePanel
            // 
            this.battlePanel.Controls.Add(this.effectImage);
            this.battlePanel.Controls.Add(this.party1DamageText);
            this.battlePanel.Controls.Add(this.party2DamageText);
            this.battlePanel.Controls.Add(this.battleOp5);
            this.battlePanel.Controls.Add(this.battle1);
            this.battlePanel.Controls.Add(this.battleOp4);
            this.battlePanel.Controls.Add(this.battleOp3);
            this.battlePanel.Controls.Add(this.battleOp2);
            this.battlePanel.Controls.Add(this.battle2);
            this.battlePanel.Controls.Add(this.battleOp1);
            this.battlePanel.Controls.Add(this.battle3);
            this.battlePanel.Controls.Add(this.battle4);
            this.battlePanel.Controls.Add(this.battle5);
            this.battlePanel.Location = new System.Drawing.Point(66, 104);
            this.battlePanel.Name = "battlePanel";
            this.battlePanel.Size = new System.Drawing.Size(1491, 386);
            this.battlePanel.TabIndex = 17;
            this.battlePanel.Visible = false;
            // 
            // effectImage
            // 
            this.effectImage.BackColor = System.Drawing.Color.Transparent;
            this.effectImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.effectImage.Location = new System.Drawing.Point(825, 138);
            this.effectImage.Name = "effectImage";
            this.effectImage.Size = new System.Drawing.Size(75, 75);
            this.effectImage.TabIndex = 23;
            this.effectImage.TabStop = false;
            this.effectImage.Visible = false;
            // 
            // party1DamageText
            // 
            this.party1DamageText.BackColor = System.Drawing.Color.Transparent;
            this.party1DamageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.party1DamageText.ForeColor = System.Drawing.Color.Red;
            this.party1DamageText.Location = new System.Drawing.Point(672, 258);
            this.party1DamageText.Name = "party1DamageText";
            this.party1DamageText.Size = new System.Drawing.Size(62, 45);
            this.party1DamageText.TabIndex = 21;
            this.party1DamageText.Text = "55";
            this.party1DamageText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.party1DamageText.Visible = false;
            // 
            // party2DamageText
            // 
            this.party2DamageText.BackColor = System.Drawing.Color.Transparent;
            this.party2DamageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.party2DamageText.ForeColor = System.Drawing.Color.Red;
            this.party2DamageText.Location = new System.Drawing.Point(753, 258);
            this.party2DamageText.Name = "party2DamageText";
            this.party2DamageText.Size = new System.Drawing.Size(62, 45);
            this.party2DamageText.TabIndex = 22;
            this.party2DamageText.Text = "55";
            this.party2DamageText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.party2DamageText.Visible = false;
            // 
            // partyPanel
            // 
            this.partyPanel.Controls.Add(this.coinText);
            this.partyPanel.Controls.Add(this.party1);
            this.partyPanel.Controls.Add(this.party2);
            this.partyPanel.Controls.Add(this.party3);
            this.partyPanel.Controls.Add(this.party4);
            this.partyPanel.Controls.Add(this.party5);
            this.partyPanel.Location = new System.Drawing.Point(47, 12);
            this.partyPanel.Name = "partyPanel";
            this.partyPanel.Size = new System.Drawing.Size(795, 362);
            this.partyPanel.TabIndex = 18;
            // 
            // coinText
            // 
            this.coinText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coinText.Location = new System.Drawing.Point(10, 7);
            this.coinText.Name = "coinText";
            this.coinText.Size = new System.Drawing.Size(228, 52);
            this.coinText.TabIndex = 20;
            this.coinText.Text = "Coins: 10";
            // 
            // shopPanel
            // 
            this.shopPanel.Controls.Add(this.rollButton);
            this.shopPanel.Controls.Add(this.shop5);
            this.shopPanel.Controls.Add(this.shop4);
            this.shopPanel.Controls.Add(this.shop3);
            this.shopPanel.Controls.Add(this.shop2);
            this.shopPanel.Controls.Add(this.shop1);
            this.shopPanel.Location = new System.Drawing.Point(47, 536);
            this.shopPanel.Name = "shopPanel";
            this.shopPanel.Size = new System.Drawing.Size(813, 294);
            this.shopPanel.TabIndex = 19;
            // 
            // petHoverInfo
            // 
            this.petHoverInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.petHoverInfo.Location = new System.Drawing.Point(1030, 618);
            this.petHoverInfo.Name = "petHoverInfo";
            this.petHoverInfo.Size = new System.Drawing.Size(248, 178);
            this.petHoverInfo.TabIndex = 20;
            this.petHoverInfo.Visible = false;
            // 
            // Battler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 853);
            this.Controls.Add(this.petHoverInfo);
            this.Controls.Add(this.shopPanel);
            this.Controls.Add(this.partyPanel);
            this.Controls.Add(this.battlePanel);
            this.Controls.Add(this.readyButton);
            this.Controls.Add(this.roomCodeLabel);
            this.Name = "Battler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battler";
            this.party1.ResumeLayout(false);
            this.battlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.effectImage)).EndInit();
            this.partyPanel.ResumeLayout(false);
            this.shopPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label1;

        public System.Windows.Forms.Label coinText;

        public System.Windows.Forms.PictureBox effectImage;

        public System.Windows.Forms.Label party2DamageText;

        public System.Windows.Forms.Label party1DamageText;

        public System.Windows.Forms.Panel partyPanel;
        public System.Windows.Forms.Panel shopPanel;

        public System.Windows.Forms.Panel battlePanel;

        private System.Windows.Forms.Panel battle5;
        private System.Windows.Forms.Panel battleOp5;
        private System.Windows.Forms.Panel battleOp4;
        private System.Windows.Forms.Panel battleOp3;
        private System.Windows.Forms.Panel battleOp2;

        public System.Windows.Forms.Panel battle1;
        private System.Windows.Forms.Panel battle2;
        private System.Windows.Forms.Panel battle3;
        private System.Windows.Forms.Panel battle4;
        public System.Windows.Forms.Panel battleOp1;

        private System.Windows.Forms.Button readyButton;

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

        private System.Windows.Forms.Label roomCodeLabel;

        #endregion

        private auto_battler_frontend.PetHoverInfo petHoverInfo;
    }
}