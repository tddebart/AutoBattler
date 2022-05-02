﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using SocketIOClient;

namespace auto_battler_frontend
{
    public partial class MainPage : Form
    {
        public SocketIO client;
        public string username;
        
        private static readonly Random random = new Random();
        
        public MainPage(string userName)
        {
            this.username = userName;
            InitializeComponent();
            
            userNameLabel.Text = $"Username: {userName}";
            Init();
        }

        public async void Init()
        {
            Pets.Init();
            
            client = new SocketIO("http://localhost:3000");

            await client.ConnectAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Exit();
            base.OnClosing(e);
        }

        private async void createRoomButton_Click(object sender, EventArgs e)
        {
            var roomCode = RandomRoomCode(5);
            await client.EmitAsync("joinRoom", username, roomCode);
            var battler = new Battler(roomCode, username, client);
            battler.Location = this.Location;
            battler.StartPosition = FormStartPosition.Manual;
            battler.FormClosing += delegate { this.Show(); };
            this.Hide();
            battler.ShowDialog();
        }
        
        private async void joinRoomButton_Click(object sender, EventArgs e)
        {
            var roomCode = roomIdBox.Text;
            if (roomCode.Length != 5)
            {
                MessageBox.Show("Room code must be 5 characters long");
                return;
            }
            await client.EmitAsync("joinRoom", username, roomCode);
            var battler = new Battler(roomCode, username, client);
            battler.Location = this.Location;
            battler.StartPosition = FormStartPosition.Manual;
            battler.FormClosing += delegate { this.Show(); };
            this.Hide();
            battler.ShowDialog();
        }
        
        public static string RandomRoomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}