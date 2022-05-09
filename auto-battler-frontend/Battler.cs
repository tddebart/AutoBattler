using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DebugTools.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pokemon_frontend.Properties;
using SocketIOClient;

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
                // await ClearControls("shop");
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
                        if (selectedShopIndex != -1)
                        {
                            Controls.Find( $"shop{selectedShopIndex+1}", true)[0].Controls[0].BackColor = Color.Transparent;
                        }
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

                UpdateParty(pets);
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
            
            client.On("soldSuccess", async(response) =>
            {
                var info = JsonConvert.DeserializeObject(response.GetValue<string>(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
                
                if(info == null) return;

                var soldIndex = (info as JObject).GetValue("soldIndex").ToObject(typeof(int)) as int?;
                var soldPet = partyPets[soldIndex ?? -1];
                
                var randomThings = (info as JObject).GetValue("randomThings").ToArray().Select(p => p.ToObject<RandomThing>()).ToList();

                shopPanel.Invoke((Action)(() => shopPanel.Hide()));
                await BattleHelper.DoSellEffect(soldPet, partyPets, randomThings);
                shopPanel.Invoke((Action)(() => shopPanel.Show()));
            });

            client.On("buySuccess", async (response) =>
            {
                await Task.Delay(50).ConfigureAwait(false);
                var info = JsonConvert.DeserializeObject(response.GetValue<string>(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
                
                if(info == null) return;
                
                var buyIndex = (info as JObject).GetValue("buyIndex").ToObject(typeof(int)) as int?;
                var boughtPet = partyPets[buyIndex ?? -1];
                
                var randomThings = (info as JObject).GetValue("randomThings").ToArray().Select(p => p.ToObject<RandomThing>()).ToList();
                
                shopPanel.Invoke((Action)(() => shopPanel.Hide()));
                await BattleHelper.DoBuyEffect(boughtPet, partyPets, randomThings);
                shopPanel.Invoke((Action)(() => shopPanel.Show()));
            });

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
                
                var levelContainer = Controls.Find(controllerPrefix + "Level" + i, true).FirstOrDefault() as PetLevel;

                if (levelContainer != null)
                {
                    levelContainer.Invoke((Action)(() =>
                    {
                        if (pet == null)
                        {
                            levelContainer.Hide();
                        }
                        else
                        {
                            levelContainer.Show();
                        }
                    }));
                }
                    
                
                
                if(pet == null) continue;

                if (levelContainer != null)
                {
                    levelContainer.lvlText.Invoke((Action)(() => levelContainer.lvlText.Text = $"Lvl{pet.Level}"));
                    levelContainer.experienceBar.Invoke((Action)(() =>
                    {
                        switch (pet.Level)
                        {
                            case 1:
                                if (pet.Experience != 0)
                                {
                                    levelContainer.experienceBar.Width = (int)(pet.Experience / 2f * levelContainer.expBack.Width);
                                } else
                                {
                                    levelContainer.experienceBar.Width = 10;
                                }
                                
                                break;
                            
                            case 2:
                                if (pet.Experience != 2)
                                {
                                    levelContainer.experienceBar.Width = (int)(pet.Experience / 5f * levelContainer.expBack.Width);
                                }
                                else
                                {
                                    levelContainer.experienceBar.Width = 10;
                                }

                                break;
                            case 3:
                                levelContainer.experienceBar.Width = levelContainer.expBack.Width;
                                break;
                        }
                    }));
                }
                
                

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
                
                label.MouseEnter += (_,_) =>
                {
                    petHoverInfo.Visible = true;
                };

                label.MouseMove += (_, eventArgs) =>
                {
                    var point = PointToClient(label.PointToScreen(label.Location));
                    
                    point.Offset(-petHoverInfo.Width/2, -petHoverInfo.Height/2 - 180);
                    point.Offset(label.Width/2, label.Height/2);
                    
                    petHoverInfo.Location = point;
                    
                    petHoverInfo.nameLabel.Text = pet.Name;
                    petHoverInfo.tierLabel.Text = $"Tier {pet.Tier}";
                    petHoverInfo.descriptionLabel.Text = pet.Ability?.Description;
                };
                
                label.MouseLeave += (_, _) =>
                {
                    petHoverInfo.Visible = false;
                };

                void Action()
                {
                    container.Controls.Add(label);
                }

                container.BeginInvoke((Action)Action);
            }
        }

        public void UpdateParty(IList<Pet?> pets)
        {
            partyPets = pets.ToArray();
            MakePets(pets, "party", async (label, index) =>
                {
                    // Check if there is already a pet in this slot and if the shop selected pet is the same as the party pet
                    if (selectedPartyIndex == -1 && selectedShopIndex != -1 && shopPets[selectedShopIndex] != null && partyPets[index] != null)
                    {
                        if (partyPets[index]?.Id == shopPets[selectedShopIndex]?.Id)
                        {
                            if (coins < 3 && !MainPage.disableCoins)
                            {
                                coinText.ForeColor = Color.Red;
                                await Task.Delay(350).ConfigureAwait(false);
                                coinText.ForeColor = Color.Black;
                                return;
                            }
                            
                            await client.EmitAsync("buyPet", selectedShopIndex, index);
                            coins -= 3;
                            coinText.Text = $"Coins: {coins}";
                            selectedShopIndex = -1;
                        }

                        return;
                    }
                    
                    // Unselect the pet
                    if (selectedPartyIndex == index)
                    {
                        selectedPartyIndex = -1;
                        label.BackColor = Color.Transparent;
                        sellButton.Visible = false;
                    }
                    else if (selectedPartyIndex != -1)
                    {
                        // label.BackColor = Color.LightBlue;
                        Controls.Find( $"party{selectedPartyIndex+1}", true)[0].Controls[0].BackColor = Color.Transparent;
                        
                        // Check if the pets are the same type
                        if (partyPets[selectedPartyIndex]?.Id == partyPets[index]?.Id && partyPets[selectedPartyIndex]?.Level != 3 && partyPets[index]?.Level != 3)
                        {
                            await client.EmitAsync("mergePets", selectedPartyIndex, index);
                        }
                        else
                        {
                            await client.EmitAsync("swapPet", selectedPartyIndex, index);
                        }

                        selectedPartyIndex = -1;
                        sellButton.Visible = false;
                    }
                    else
                    {
                        selectedPartyIndex = index;
                        label.BackColor = Color.LightBlue;
                        
                        sellButton.Visible = true;
                    }
                });
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
                        sellButton.Visible = false;
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
        
        private void sellButton_Click(object sender, EventArgs e)
        {
            if (selectedPartyIndex != -1)
            {
                coins += partyPets[selectedPartyIndex].Level;
                coinText.Text = $"Coins: {coins}";
                client.EmitAsync("sellPet", selectedPartyIndex);
                selectedPartyIndex = -1;
                sellButton.Visible = false;
            }
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