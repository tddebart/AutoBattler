using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using auto_battler_frontend.Properties;
using DebugTools.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pokemon_frontend.Properties;
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

        public int selectedShopIndex = -1;
        public int selectedPartyIndex = -1;
        
        public Pet?[] shopPets = new Pet[5];
        public Pet?[] partyPets = new Pet[5];

        public int coins = 10;

        public static Battler instance;
        
        
        // Animation Example:
        // var origPos = petContainer.Location;
        // petContainer.Animate(new XLocationEffect(), EasingFunctions.Linear, petContainer.Location.X + 100, 500, 50, true);
        // Task.Delay(1100).ContinueWith(_ =>
        // {
        //     petContainer.Location = origPos;
        // });
        

        public Battler(string roomCode, string username, SocketIO client)
        {
            instance = this;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            this.roomCode = roomCode;
            this.username = username;
            this.client = client;
            client.Off("receiveShop");

            inspector ??= new Inspector();
            
            
            roomCodeLabel.Text = $"RoomCode: {roomCode}\nUsername: {username}";

            client.On("receiveShop",  async(response) =>
            {
                await ClearControls("shop");
                var pets = JsonConvert.DeserializeObject<List<Pet>>(response.GetValue<string>());
                
                if(pets == null) return;

                shopPets = pets.ToArray();

                MakePets(pets, "shop", (Label, index) =>
                {
                    if (selectedShopIndex == index)
                    {
                        selectedShopIndex = -1;
                        Label.BackColor = Color.Transparent;
                    }
                    else
                    {
                        selectedShopIndex = index;
                        Label.BackColor = Color.LightBlue;
                    }
                });
            });
            
            client.On("receiveParty", async(response) =>
            {
                var pets = JsonConvert.DeserializeObject<List<Pet>>(response.GetValue<string>(), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                });
                
                
                if(pets == null) return;
                
                await ClearControls("party");
                
                while (pets.Count < 5)
                {
                    pets.Add(null);
                }
                
                partyPets = pets.ToArray();

                MakePets(pets, "party", (label, index) =>
                {
                    if (selectedPartyIndex == index)
                    {
                        selectedPartyIndex = -1;
                        label.BackColor = Color.Transparent;
                    }
                    else if (selectedPartyIndex != -1)
                    {
                        client.EmitAsync("swapPet", selectedPartyIndex, index);
                        selectedPartyIndex = -1;
                    }
                    else
                    {
                        selectedPartyIndex = index;
                        label.BackColor = Color.LightBlue;
                    }
                });
            });

            void BattleStarted(SocketIOResponse response)
            {
                var pets = new LinkedList<Pet?>((Pet[])partyPets.Clone());
                var info = JsonConvert.DeserializeObject(response.GetValue<string>(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
                
                if(info == null) return;
                
                var oppPetsArr = (info as JObject).GetValue("oppPets").ToArray().Select(p => p.ToObject<Pet?>()).ToArray();
                
                var party1RandomThings = (info as JObject).GetValue("party1RandomThings").ToArray().Select(p => p.ToObject<RandomThing>()).ToList();
                var party2RandomThings = (info as JObject).GetValue("party2RandomThings").ToArray().Select(p => p.ToObject<RandomThing>()).ToList();
                

                if (oppPetsArr == null) return;

                while (oppPetsArr.Length < 5)
                {
                    oppPetsArr = oppPetsArr.Concat(new[] { (Pet?)null }).ToArray();
                }

                ClearControls("battle");
                ClearControls("battleOp");

                MakePets(partyPets, "battle", fontSize: 13);
                MakePets(oppPetsArr, "battleOp", fontSize: 13, rightSide: true);

                var oppPets = new LinkedList<Pet?>(oppPetsArr);

                partyPanel.Invoke((Action)(() => partyPanel.Hide()));
                shopPanel.Invoke((Action)(() => shopPanel.Hide()));
                battlePanel.Invoke((Action)(() => battlePanel.Show()));

                BattleHelper.AnimateBattle(pets, oppPets, party1RandomThings, party2RandomThings);
            }

            client.On("battleStarted", BattleStarted);

            InitializeClickEvents();

            this.Shown += (sender, args) =>
            {
                client.EmitAsync("getShop");
            };
        }

        public void MakePets(IList<Pet?> pets, string controllerPrefix, Action<Control, int>? labelClickEvent = null, float fontSize = 17f, bool rightSide = false)
        {
            for (var i = 1; i < 6; i++)
            {
                var pet = pets[i - 1];
                
                var container = Controls.Find(controllerPrefix + i, true).First();
                container.Invoke((Action)(() => container.Controls.Clear()));
                
                if(pet == null) continue;
                

                var label = new Label
                {
                    Text = pet.Name + (pet.Name.Length < 9 ? "\n\n\n": "\n\n") + pet.CurrentAttack + "           " + pet.CurrentHealth,
                    TextAlign = ContentAlignment.BottomCenter,
                    Size = container.Size,
                    Location = new Point(0, 0),
                };

                label.Font = new Font(label.Font.FontFamily, fontSize,label.Font.Style);
                var unicodeCharacter = pet.Image.UnicodeCodePoint;

                label.Image = GetImage(unicodeCharacter, container, rightSide);

                var index = i-1;
                if (labelClickEvent != null)
                {
                    label.Click += (_,_) =>
                    {
                        labelClickEvent.Invoke(label, index);
                    };
                }

                label.MouseEnter += (_, _) =>
                {
                    hoverInfo.Text = $"{pet.Name}\n{pet.Level1Ability.Description}";
                };

                void Action()
                {
                    container.Controls.Add(label);
                }

                container.BeginInvoke((Action)Action);
            }
        }

        public System.Drawing.Image GetImage(string unicodeCharacter, Control sizeReference, bool rightSide = false)
        {
            var x = unicodeCharacter[0];
            var y = unicodeCharacter[1];

            var surrogateHexString = (((uint)x << 16) | y).ToString("X");

            var H = uint.Parse(surrogateHexString.Substring(0, 4), NumberStyles.HexNumber);
            var L = uint.Parse(surrogateHexString.Substring(4, 4), NumberStyles.HexNumber);

            var unicodeHexCode = (((H - 0xD800) * 0x400) + (L - 0xDC00) + 0x10000).ToString("X");

            var image = Resources.ResourceManager.GetObject($"emoji_u{unicodeHexCode.ToLower()}") as System.Drawing.Image;
            image = new Bitmap(image, new Size(sizeReference.Size.Width, sizeReference.Size.Height));
            if (!rightSide)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            
            return image;
        }

        public void InitializeClickEvents()
        {
            for (var i = 1; i < 6; i++)
            {
                var petContainer = Controls.Find("party" + i, true).First();

                var index = i;
                petContainer.Click += async(sender, args) =>
                {
                    if (selectedPartyIndex == -1 && selectedShopIndex != -1 &&shopPets[selectedShopIndex] != null)
                    {
                        if (coins < 3 && !MainPage.disableCoins)
                        {
                            coinText.ForeColor = Color.Red;
                            await Task.Delay(350).ConfigureAwait(false);
                            coinText.ForeColor = Color.Black;
                            return;
                        }
                        
                        await client.EmitAsync("buyPet", selectedShopIndex, index-1);
                        coins -= 3;
                        coinText.Text = $"Coins: {coins}";
                        selectedShopIndex = -1;
                    }
                    else
                    {
                        await client.EmitAsync("swapPet", selectedPartyIndex, index-1);
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

        private async void rollButton_Click(object sender, EventArgs e)
        {
            if (coins < 1 && !MainPage.disableCoins)
            {
                coinText.ForeColor = Color.Red;
                await Task.Delay(350).ConfigureAwait(false);
                coinText.ForeColor = Color.Black;
                return;
            }
            
            client.EmitAsync("getShop");
            coins -= 1;
            coinText.Text = $"Coins: {coins}";
        }

        public async Task ClearControls(string controlPrefix)
        {
            await Task.Run(() =>
            {
                for (var i = 1; i < 6; i++)
                {
                    var container = Controls.Find(controlPrefix + i, true)?.First();
                    if (container != null)
                    {
                        container.BeginInvoke((Action)delegate
                        {
                            container.Controls.Clear();
                        });
                    }
                }
            });
        }

        private void readyButton_Click(object sender, EventArgs e)
        {
            client.EmitAsync("ready");
        }

        // private void button1_Click(object sender, EventArgs e)
        // {
        //     var battleOrig = battle1.Location;
        //     battle1.Animate(new XLocationEffect(), EasingFunctions.BounceEaseOut, battle1.Location.X + 50, 400, 50, true);
        //     battle1.Animate(new YLocationEffect(), EasingFunctions.BounceEaseOut, battle1.Location.Y - 25, 400, 50, true);
        //     Task.Delay(1000).ContinueWith(_ =>
        //     {
        //         battle1.Location = battleOrig;
        //     });
        //     
        //     var partyOppOrig = battleOp1.Location;
        //     battleOp1.Animate(new XLocationEffect(), EasingFunctions.BounceEaseOut, battleOp1.Location.X - 50, 400, 50, true);
        //     battleOp1.Animate(new YLocationEffect(), EasingFunctions.BounceEaseOut, battleOp1.Location.Y - 25, 400, 50, true);
        //     Task.Delay(1000).ContinueWith(_ =>
        //     {
        //         battleOp1.Location = partyOppOrig;
        //     });
        // }

        public class RandomThing
        {
            [JsonProperty("petTrigger")]
            public int petTriggerIndex;
            [JsonProperty("petTarget")]
            public int petTargetIndex;
            [JsonProperty("abilityTrigger")]
            public Trigger abilityTrigger;
        }
    }
}