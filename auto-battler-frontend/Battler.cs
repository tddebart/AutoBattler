using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using auto_battler_frontend.Properties;
using DebugTools.Tools;
using Newtonsoft.Json;
using SocketIOClient;
using VisualEffects;
using VisualEffects.Animations.Effects;
using VisualEffects.Easing;
using VisualEffects.Effects.Bounds;
using Image = System.Drawing.Image;

namespace auto_battler_frontend
{
    public partial class Battler : Form
    {
        public string roomCode;
        public string username;
        public SocketIO client;
        public static Inspector inspector;

        public int selectedShopIndex;
        public int selectedPartyIndex = -1;
        
        public Pet[] shopPets = new Pet[5];
        public Pet[] partyPets = new Pet[5];
        
        
        //TODO: Animations.
        // Example:
        // var origPos = petContainer.Location;
        // petContainer.Animate(new XLocationEffect(), EasingFunctions.Linear, petContainer.Location.X + 100, 500, 50, true);
        // Task.Delay(1100).ContinueWith(_ =>
        // {
        //     petContainer.Location = origPos;
        // });
        

        public Battler(string roomCode, string username, SocketIO client)
        {
            InitializeComponent();
            this.roomCode = roomCode;
            this.username = username;
            this.client = client;
            client.Off("receiveShop");

            inspector ??= new Inspector();
            
            
            roomCodeLabel.Text = $"RoomCode: {roomCode}\nUsername: {username}";

            client.On("receiveShop",  async(response) =>
            {
                await ClearShop();
                var pets = JsonConvert.DeserializeObject<List<Pet>>(response.GetValue<string>());
                
                if(pets == null) return;
                

                shopPets = pets.ToArray();
                
                for (var i = 1; i < 6; i++)
                {
                    // var pet = Pets.pets.Where(p => p.Packs.Contains(Pack.StandardPack) && p.Tier == "1").Shuffle(new Random((int)DateTime.Now.Ticks+i)).First();

                    var pet = pets[i - 1];
                    
                    if(pet == null) continue;
                    
                    var shopContainer = Controls.Find("shop" + i, true).First();
                    
                    var label = new Label
                    {
                        Text = pet.Name + "\n\n\n" + pet.CurrentAttack + "           " + pet.CurrentHealth,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = shopContainer.Size,
                        Location = new Point(0, 0),
                    };

                    label.Font = new Font(label.Font.FontFamily, 17f,label.Font.Style);
                    var unicodeCharacter = pet.Image.UnicodeCodePoint;

                    label.Image = GetImage(unicodeCharacter, shopContainer);

                    var index = i;
                    label.Click += (sender, args) =>
                    {
                        selectedShopIndex = index-1;
                    };

                    label.MouseHover += (_, _) =>
                    {
                        hoverInfo.Text = $"{pet.Name}\n{pet.Level1Ability.Description}";
                    };

                    void Action()
                    {
                        shopContainer.Controls.Add(label);
                    }

                    shopContainer.BeginInvoke((Action)Action);
                }
            });
            
            client.On("receiveParty", async(response) =>
            {
                
                var pets = JsonConvert.DeserializeObject<List<Pet>>(response.GetValue<string>(), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                });
                
                
                if(pets == null) return;
                
                await ClearParty();
                
                while (pets.Count < 5)
                {
                    pets.Add(null);
                }
                
                partyPets = pets.ToArray();
                
                for (var i = 1; i < 6; i++)
                {
                    var pet = pets[i - 1];
                    
                    if(pet == null) continue;
                    
                    var partyContainer = Controls.Find("party" + i, true).First();
                    
                    var label = new Label
                    {
                        Text = pet.Name + "\n\n\n" + pet.CurrentAttack + "           " + pet.CurrentHealth,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = partyContainer.Size,
                        Location = new Point(0, 0),
                    };

                    label.Font = new Font(label.Font.FontFamily, 17f,label.Font.Style);
                    var unicodeCharacter = pet.Image.UnicodeCodePoint;

                    label.Image = GetImage(unicodeCharacter, partyContainer);

                    var index = i-1;
                    label.Click += (_, _) =>
                    {
                        if (selectedPartyIndex == index)
                        {
                            selectedPartyIndex = -1;
                        }
                        else if (selectedPartyIndex != -1)
                        {
                            client.EmitAsync("swapPet", selectedPartyIndex, index);
                            selectedPartyIndex = -1;
                        }
                        else
                        {
                            selectedPartyIndex = index;
                        }
                    };

                    label.MouseHover += (_, _) =>
                    {
                        hoverInfo.Text = $"{pet.Name}\n{pet.Level1Ability.Description}";
                    };

                    void Action()
                    {
                        partyContainer.Controls.Add(label);
                    }

                    partyContainer.BeginInvoke((Action)Action);
                }
            });

            InitializeClickEvents();

            this.Shown += (sender, args) =>
            {
                client.EmitAsync("getShop");
            };
        }

        public System.Drawing.Image GetImage(string unicodeCharacter, Control sizeReference)
        {
            var x = unicodeCharacter[0];
            var y = unicodeCharacter[1];

            var surrogateHexString = (((uint)x << 16) | y).ToString("X");

            var H = uint.Parse(surrogateHexString.Substring(0, 4), NumberStyles.HexNumber);
            var L = uint.Parse(surrogateHexString.Substring(4, 4), NumberStyles.HexNumber);

            var unicodeHexCode = (((H - 0xD800) * 0x400) + (L - 0xDC00) + 0x10000).ToString("X");

            var image = Resources.ResourceManager.GetObject($"emoji_u{unicodeHexCode.ToLower()}") as System.Drawing.Image;
            image = new Bitmap(image, new Size(sizeReference.Size.Width, sizeReference.Size.Height));
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            
            return image;
        }

        public void InitializeClickEvents()
        {
            for (var i = 1; i < 6; i++)
            {
                var petContainer = Controls.Find("party" + i, true).First();

                var index = i;
                petContainer.Click += (sender, args) =>
                {
                    if (selectedPartyIndex == -1)
                    {
                        client.EmitAsync("buyPet", selectedShopIndex, index-1);
                        selectedShopIndex = -1;
                    }
                    else
                    {
                        client.EmitAsync("swapPet", selectedPartyIndex, index-1);
                        selectedPartyIndex = -1;
                    }
                };
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            client.EmitAsync("disconnect");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            // PictureBox pictureBox = new PictureBox();
            //
            //
            // Image myBitmap = Resources.emoji_u1f422;
            // myBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            // Size bitmapSize = new Size(myBitmap.Width, myBitmap.Height);
            //
            // pictureBox.Size = panel1.Size;
            // pictureBox.Image = myBitmap;
            // pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            // pictureBox.Location = new Point(0, 0);
            // panel1.Controls.Add(pictureBox);

            // this.FormBorderStyle = FormBorderStyle.None;
        }

        private void rollButton_Click(object sender, EventArgs e)
        {
            client.EmitAsync("getShop");
        }

        public async Task ClearShop()
        {
            await Task.Run(() =>
            {
                for (var i = 1; i < 6; i++)
                {
                    var shopContainer = Controls.Find("shop" + i, true).First();
                    shopContainer.BeginInvoke((Action)delegate
                    {
                        shopContainer.Controls.Clear();
                    });
                }
            });
        }
        
        public async Task ClearParty()
        {
            await Task.Run(() =>
            {
                for (var i = 1; i < 6; i++)
                {
                    var petContainer = Controls.Find("party" + i, true).First();
                    petContainer.BeginInvoke((Action)delegate
                    {
                        petContainer.Controls.Clear();
                    });
                }
            });
        }

        private void readyButton_Click(object sender, EventArgs e)
        {
            client.EmitAsync("ready");
        }
    }
}