
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using auto_battler_frontend;
using pokemon_frontend.Properties;
using VisualEffects;
using VisualEffects.Animations.Effects;
using VisualEffects.Easing;

public static class BattleHelper
{
    public static LinkedList<Pet?> party1;
    public static LinkedList<Pet?> party2;
    public static List<Battler.RandomThing?> party1RandomThings;
    public static List<Battler.RandomThing?> party2RandomThings;

    public static async void AnimateBattle(LinkedList<Pet?> party1Arg, LinkedList<Pet?> party2Arg, List<Battler.RandomThing?>? party1RandomThingsArg, List<Battler.RandomThing?>? party2RandomThingsArg)
    {
        var battler = Battler.instance;
        var battle1 = Battler.instance.battle1;
        var battleOp1 = Battler.instance.battleOp1;
        
        party1 = party1Arg;
        party2 = party2Arg;
        party1RandomThings = party1RandomThingsArg ?? new List<Battler.RandomThing?>();
        party2RandomThings = party2RandomThingsArg ?? new List<Battler.RandomThing?>();
        
        var count = 0;
        while (!AreAllPetsDead(party1) && !AreAllPetsDead(party2) && count < 100)
        {
            // Make sure there are no gaps in the party
            MovePetsForward(party1,party2);

            await Task.Delay(350).ConfigureAwait(false);
            
            if (count == 0)
            {
                await DoStartBattleEffects(party1.ToArray(),party2.ToArray(),party1RandomThings, true);
                await DoStartBattleEffects(party2.ToArray(),party1.ToArray(),party2RandomThings, false);

                UpdateVisuals();
            
                await Task.Delay(350).ConfigureAwait(false);

                await CheckDeath();
            }
            else
            {
                UpdateVisuals();
            }


            var pet1 = party1.First;
            var pet2 = party2.First;

            // Do attacks
            pet1.Value.CurrentHealth -= pet2.Value.CurrentAttack;
            pet2.Value.CurrentHealth -= pet1.Value.CurrentAttack;
            
            if(pet1.Value.CurrentHealth < 0) pet1.Value.CurrentHealth = 0;
            if(pet2.Value.CurrentHealth < 0) pet2.Value.CurrentHealth = 0;
            
            
            // Do attack animations
            var battleOrig = battle1.Location;
            battle1.Animate(new XLocationEffect(), EasingFunctions.BounceEaseOut, battle1.Location.X + 50, 400, 50, true);
            battle1.Animate(new YLocationEffect(), EasingFunctions.BounceEaseOut, battle1.Location.Y - 25, 400, 50, true);
            
            var partyOppOrig = battleOp1.Location;
            battleOp1.Animate(new XLocationEffect(), EasingFunctions.BounceEaseOut, battleOp1.Location.X - 50, 400, 50, true);
            battleOp1.Animate(new YLocationEffect(), EasingFunctions.BounceEaseOut, battleOp1.Location.Y - 25, 400, 50, true);

            AnimateDamageText(battler, pet2.Value.CurrentAttack, pet1.Value.CurrentAttack);

            await Task.Delay(1000).ConfigureAwait(false);

            battle1.Invoke((Action)(() =>
            {
                battle1.Location = battleOrig;
            }));
            battleOp1.Invoke((Action)(() =>
            {
                battleOp1.Location = partyOppOrig;
            }));
            
            // Refresh the visuals
            UpdateVisuals();
            
            await Task.Delay(250).ConfigureAwait(false);

            await CheckDeath();
            
            await Task.Delay(250).ConfigureAwait(false);
            
            count++;
        }

        await Task.Delay(2000).ConfigureAwait(false);

        await battler.client.EmitAsync("getShop");
        battler.coins = 10;
        battler.coinText.Invoke((Action)(() =>
        {
            battler.coinText.Text = $"Coins: {battler.coins}";
        }));
        
        battler.readyButton.Invoke( (Action)(() =>
        {
            battler.readyButton.BackColor = Control.DefaultBackColor;
            battler.readyButton.Enabled = true;
            battler.readyButton.Show();
            battler.readyButton.Focus();
        }));
        battler.partyPanel.Invoke((Action)(() => battler.partyPanel.Show()));
        battler.shopPanel.Invoke((Action)(() => battler.shopPanel.Show()));
        battler.battlePanel.Invoke((Action)(() => battler.battlePanel.Hide()));

        party1 = null;
        party2 = null;
    }

    public static async Task CheckDeath()
    {
        foreach (var pet in party1.ToArray())
        {
            var shouldRemove = await CheckForDeadEffects(pet,party1, party1RandomThings, true);
            if (shouldRemove && pet != null && pet.CurrentHealth <= 0)
            {
                party1.Remove(pet);
                party1.AddLast((Pet?)null);
            }
        }
            
        foreach (var pet in party2.ToArray())
        {
            var shouldRemove = await CheckForDeadEffects(pet,party2, party2RandomThings, false);
            if (shouldRemove && pet != null && pet.CurrentHealth <= 0)
            {
                party2.Remove(pet);
                party2.AddLast((Pet?)null);
            }
        }

        MovePetsForward(party1,party2);
            
        UpdateVisuals();
    }

    public static void UpdateVisuals()
    {
        var battler = Battler.instance;
        // battler.ClearControls("battle");
        // battler.ClearControls("battleOp");
                
        battler.MakePets(party1.ToArray(), "battle", fontSize: 13);
        battler.MakePets(party2.ToArray(), "battleOp", fontSize: 13, rightSide: true);
    }

    public static async void AnimateDamageText(Battler battler, int damageParty1, int damageParty2)
    {
        await Task.Delay(400).ConfigureAwait(false);

        var origParty1Loc = battler.party1DamageText.Location;
        battler.party1DamageText.Invoke((Action)(() =>
                {
                    battler.party1DamageText.Text = $"{damageParty1}";
                    battler.party1DamageText.Visible = true;
                    battler.party1DamageText.Animate(new YLocationEffect(), EasingFunctions.Linear, battler.party1DamageText.Location.Y - 70, 400, 0);
                }
            ));
        
        var origParty2Loc = battler.party2DamageText.Location;
        battler.party2DamageText.Invoke((Action)(() =>
                {
                    battler.party2DamageText.Text = $"{damageParty2}";
                    battler.party2DamageText.Visible = true;
                    battler.party2DamageText.Animate(new YLocationEffect(), EasingFunctions.Linear, battler.party2DamageText.Location.Y - 70, 400, 0);
                }
            ));
        
        await Task.Delay(430).ConfigureAwait(false);
        
        battler.party1DamageText.Invoke((Action)(() =>
                {
                    battler.party1DamageText.Location = origParty1Loc;
                    battler.party1DamageText.Visible = false;
                }
            ));
        
        battler.party2DamageText.Invoke((Action)(() =>
                {
                    battler.party2DamageText.Location = origParty2Loc;
                    battler.party2DamageText.Visible = false;
                }
            ));
    }

    public static async Task AnimateEffectImage(int pet1Index, int pet2Index, bool pet1OurParty, bool pet2OurParty, string unicode)
    {
        var effectImage = Battler.instance.effectImage;
        pet1Index++;
        pet2Index++;

        effectImage.Invoke((Action)(() =>
        {
            effectImage.Parent.Controls.SetChildIndex(effectImage, 0);
            effectImage.Location = Battler.instance.Controls.Find(pet1OurParty ? "battle" + pet1Index : "battleOp" + pet1Index, true).First().Location;
            effectImage.Location = new Point(effectImage.Location.X, effectImage.Location.Y - effectImage.Height);
        }));
        
        var origPos = effectImage.Location;
        effectImage.Invoke((Action)(() =>
                {
                    effectImage.Visible = true;
                    var image = Resources.ResourceManager.GetObject($"emoji_u{unicode.ToLower()}") as System.Drawing.Image;
                    image = new Bitmap(image, new Size(effectImage.Size.Width, effectImage.Size.Height));
                    effectImage.Image = image;
                    
                    effectImage.Animate(new YLocationEffect(), EasingFunctions.Linear, effectImage.Location.Y - 75, 400, 0, true);
                    effectImage.Animate(new XLocationEffect(), EasingFunctions.Linear, Battler.instance.Controls.Find(pet2OurParty ? "battle" + pet2Index : "battleOp" + pet2Index, true).First().Location.X+5, 800, 0);
                }
            ));
        
        await Task.Delay(830).ConfigureAwait(false);
        
        effectImage.Invoke((Action)(() =>
                {
                    effectImage.Location = origPos;
                    effectImage.Visible = false;
                }
            ));
    }
    
    public static async Task AnimateEffectImageParty(int pet1Index, int pet2Index, string unicode)
    {
        var effectImage = Battler.instance.effectImageParty;
        pet1Index++;
        pet2Index++;

        effectImage.Invoke((Action)(() =>
        {
            effectImage.Parent.Controls.SetChildIndex(effectImage, 0);
            effectImage.Location = Battler.instance.Controls.Find("party" + pet1Index, true).First().Location;
            effectImage.Location = new Point(effectImage.Location.X, effectImage.Location.Y - effectImage.Height);
        }));
        
        var origPos = effectImage.Location;
        effectImage.Invoke((Action)(() =>
                {
                    effectImage.Visible = true;
                    var image = Resources.ResourceManager.GetObject($"emoji_u{unicode.ToLower()}") as System.Drawing.Image;
                    image = new Bitmap(image, new Size(effectImage.Size.Width, effectImage.Size.Height));
                    effectImage.Image = image;
                    
                    effectImage.Animate(new YLocationEffect(), EasingFunctions.Linear, effectImage.Location.Y - 75, 400, 0, true);
                    effectImage.Animate(new XLocationEffect(), EasingFunctions.Linear, Battler.instance.Controls.Find("party" + pet2Index, true).First().Location.X+5, 800, 0);
                }
            ));
        
        await Task.Delay(830).ConfigureAwait(false);
        
        effectImage.Invoke((Action)(() =>
                {
                    effectImage.Location = origPos;
                    effectImage.Visible = false;
                }
            ));
    }

    public static async Task<bool> CheckForDeadEffects(Pet? pet, LinkedList<Pet?> partyLinked, List<Battler.RandomThing?> randomThings, bool ourParty)
    {
        var shouldRemove = true;
        
        var party = partyLinked.ToList();
        if (pet != null && pet.CurrentHealth <= 0)
        {
            var ability = pet.Ability;
            if (ability != null && ability.Trigger == Trigger.Faint && ability.TriggeredBy.Kind == "Self")
            {
                if (ability.Effect.Kind == "ModifyStats")
                {
                    var effect = ability.Effect;

                    if (effect.Target.Kind == "RandomFriend" && randomThings.Any(t => party.IndexOf(pet) == t.petTriggerIndex && t.abilityTrigger == ability.Trigger))
                    {
                        var randomThing = randomThings.First(t => party.IndexOf(pet) == t.petTriggerIndex);
                        var target = party[randomThing.petTargetIndex];
                        if (target != null)
                        {
                            target.CurrentHealth += effect.HealthAmount;
                            target.CurrentAttack += int.Parse(effect.AttackAmount);
                        }
                        
                        await AnimateEffectImage(randomThing.petTriggerIndex, randomThing.petTargetIndex, ourParty,ourParty, "1f354");
                        randomThings.Remove(randomThing);
                        UpdateVisuals();
                        await Task.Delay(200).ConfigureAwait(false);
                    }
                }

                if (ability.Effect.Kind == "SummonPet")
                {
                    var effect = ability.Effect;
                    if (effect.Team == "Friendly")
                    {
                        shouldRemove = false;
                        // Remove the pet that died from the party
                        // Add a new pet to the party
                        var newPet = effect.Pet;
                        newPet.CurrentAttack = effect.WithAttack;
                        newPet.CurrentHealth = effect.WithHealth;
                        var petNode = partyLinked.Find(pet);
                        partyLinked.AddBefore(petNode,newPet);
                        partyLinked.Remove(pet);
                        
                        UpdateVisuals();
                        
                        await Task.Delay(200).ConfigureAwait(false);
                        await CheckSummonedEffect(newPet, partyLinked, ourParty);
                    }
                }
            }
        }

        return shouldRemove;
    }

    public static async Task CheckSummonedEffect(Pet summonedPet, LinkedList<Pet?> partyLinked, bool ourParty)
    {
        var party = partyLinked.ToList();
        foreach (var pet in partyLinked)
        {
            if (pet != null)
            {
                var ability = pet.Ability;
                if (ability != null && ability.Trigger == Trigger.Summoned)
                {
                    if (ability.Effect.Kind == "ModifyStats")
                    {
                        var effect = ability.Effect;
                        if (effect.Target.Kind == "TriggeringEntity")
                        {
                            if (!string.IsNullOrEmpty(effect.AttackAmount))
                            {
                                summonedPet.CurrentAttack += int.Parse(effect.AttackAmount);
                            }
                            
                            if (effect.HealthAmount != 0)
                            {
                                summonedPet.CurrentHealth += effect.HealthAmount;
                            }
                            
                            await AnimateEffectImage(party.IndexOf(pet), party.IndexOf(summonedPet), ourParty, ourParty, "1f354");
                            UpdateVisuals();
                            await Task.Delay(200).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
    
    public static async Task DoStartBattleEffects(IList<Pet?> party1,IList<Pet?> party2, IList<Battler.RandomThing?> randomThings, bool ourParty)
    {
        foreach (var pet in party1)
        {
            if (pet == null) continue;

            var ability = pet.Ability;

            if (ability.Trigger == Trigger.StartOfBattle)
            {
                if (ability.Effect.Kind == "DealDamage")
                {
                    var effect = ability.Effect;
                    if (effect.Target.Kind == "RandomEnemy")
                    {
                        for (var i = 0; i < effect.Target.N; i++)
                        {
                            var randomThing = randomThings.First(t => party1.IndexOf(pet) == t.petTriggerIndex && t.abilityTrigger == ability.Trigger);
                            var target = party2[randomThing.petTargetIndex];
                            if (target != null)
                            {
                                target.CurrentHealth -= Convert.ToInt32(effect.Amount);
                                await AnimateEffectImage(randomThing.petTriggerIndex, randomThing.petTargetIndex, ourParty,!ourParty, "1faa8");
                                randomThings.Remove(randomThing);
                                UpdateVisuals();
                                await Task.Delay(200).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
        }
        
        await Task.Delay(200).ConfigureAwait(false);
    }

    public static async Task DoSellEffect(Pet? pet, IList<Pet?> party, IList<Battler.RandomThing?> randomThings)
    {
        var ability = pet.Ability;
        if (ability != null && ability.Trigger is Trigger.Sell)
        {
            if (ability.TriggeredBy.Kind == "Self")
            {
                Battler.instance.shopPanel.Invoke((Action)(() => Battler.instance.shopPanel.Hide()));
                if (ability.Effect.Kind == "ModifyStats")
                {
                    await DoModifyEffect(pet, party, randomThings);
                } else if (ability.Effect.Kind == "GainGold")
                {
                    Battler.instance.coins += Convert.ToInt32(ability.Effect.Amount);
                    Battler.instance.coinText.Text = $"Coins: {Battler.instance.coins}";
                }
            }
        }


        party[party.IndexOf(pet)] = null;
        Battler.instance.UpdateParty(party);
        

        await Task.Delay(200).ConfigureAwait(false);
        Battler.instance.shopPanel.Invoke((Action)(() => Battler.instance.shopPanel.Show()));
    }

    public static async Task DoBuyEffect(Pet? pet, IList<Pet?> party, IList<Battler.RandomThing?> randomThings)
    {
        var ability = pet.Ability;
        if (ability != null && ability.Trigger is Trigger.Buy)
        {
            if (ability.TriggeredBy.Kind == "Self")
            {
                Battler.instance.shopPanel.Invoke((Action)(() => Battler.instance.shopPanel.Hide()));
                if (ability.Effect.Kind == "ModifyStats")
                {
                    await DoModifyEffect(pet, party, randomThings, false);
                }
            }
        }
        
        Battler.instance.UpdateParty(party);
        

        await Task.Delay(200).ConfigureAwait(false);
        Battler.instance.shopPanel.Invoke((Action)(() => Battler.instance.shopPanel.Show()));
    }

    public static async Task DoLevelUpEffect(Pet? pet, IList<Pet?> party)
    {
        var ability = pet.Ability;
        if (ability != null && ability.Trigger is Trigger.LevelUp)
        {
            if (ability.TriggeredBy.Kind == "Self")
            {
                Battler.instance.shopPanel.Invoke((Action)(() => Battler.instance.shopPanel.Hide()));
                if (ability.Effect.Kind == "ModifyStats")
                {
                    await DoModifyEffect(pet, party, null, false);
                }
            }
        }
        
        Battler.instance.shopPanel.Invoke((Action)(() => Battler.instance.shopPanel.Show()));
    }

    public static async Task DoModifyEffect(Pet? pet,IList<Pet> party,IList<Battler.RandomThing?> randomThings, bool modifyPet= true)
    {

        if (pet == null) return;
        
        var ability = pet.Ability;
        if(ability == null) return;
        
        var effect = ability.Effect;
        if (effect.Target.Kind == "RandomFriend")
        {
            for (var i = 0; i < effect.Target.N; i++)
            {
                if (randomThings.Count == 0) continue;
                            
                var randomThing = randomThings.First(t => party.IndexOf(pet) == t.petTriggerIndex);
                var target = party[randomThing.petTargetIndex];
                if (target != null && modifyPet)
                {
                    target.CurrentHealth += effect.HealthAmount;
                    if (!string.IsNullOrEmpty(effect.AttackAmount))
                    {
                        target.CurrentAttack += int.Parse(effect.AttackAmount);
                    }
                }
                        
                await AnimateEffectImageParty(randomThing.petTriggerIndex, randomThing.petTargetIndex, "1f354");
                randomThings.Remove(randomThing);
                Battler.instance.UpdateParty(party);
                await Task.Delay(200).ConfigureAwait(false);
            }
        }
        else if (effect.Target.Kind == "EachFriend")
        {
            foreach (var target in party.Where(p => p != null))
            {
                if (target != null && modifyPet)
                {
                    target.CurrentHealth += effect.HealthAmount;
                    if (!string.IsNullOrEmpty(effect.AttackAmount))
                    {
                        target.CurrentAttack += int.Parse(effect.AttackAmount);
                    }
                }

                await AnimateEffectImageParty(party.IndexOf(pet), party.IndexOf(target), "1f354");
            }
        }
    }

    public static void MovePetsForward(LinkedList<Pet?> party1, LinkedList<Pet?> party2)
    {
        if (party1.Any(n => n != null))
        {
            while (party1.First.Value == null)
            {
                party1.AddLast(party1.First.Value);
                party1.RemoveFirst();
            }
        }


        if (party2.Any(n => n != null))
        {
            while (party2.First.Value == null)
            {
                party2.AddLast(party2.First.Value);
                party2.RemoveFirst();
            }
        }
        UpdateVisuals();
    }

    public static bool AreAllPetsDead(IEnumerable<Pet?> party)
    {
        return party.All(pet => pet is not { CurrentHealth: > 0 });
    }
}
