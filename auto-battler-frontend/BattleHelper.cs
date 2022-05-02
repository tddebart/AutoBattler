
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using auto_battler_frontend;
using pokemon_frontend.Properties;
using VisualEffects;
using VisualEffects.Animations.Effects;
using VisualEffects.Easing;

public static class BattleHelper
{
    public static void SimulateBattle(LinkedList<Pet?> party1, LinkedList<Pet?> party2)
    {
        var count = 0;
        while (!AreAllPetsDead(party1) && !AreAllPetsDead(party2) && count < 100)
        {
            while (party1.First.Value == null)
            {
                party1.AddLast(party1.First.Value);
                party1.RemoveFirst();
            }
            
            while (party2.First.Value == null)
            {
                party2.AddLast(party2.First.Value);
                party2.RemoveFirst();
            }
            
            var pet1 = party1.First.Value;
            var pet2 = party2.First.Value;

            pet1.CurrentHealth -= pet2.CurrentAttack;
            pet2.CurrentHealth -= pet1.CurrentAttack;
            
            if (pet1.CurrentHealth <= 0)
            {
                party1.RemoveFirst();
                party1.AddLast((Pet?)null);
            }
            
            if (pet2.CurrentHealth <= 0)
            {
                party2.RemoveFirst();
                party2.AddLast((Pet?)null);
            }
            
            count++;
        }
        
        // Print out results
        var party1Dead = AreAllPetsDead(party1);
        var party2Dead = AreAllPetsDead(party2);
        
        if (party1Dead && party2Dead)
        {
            Debug.WriteLine("Draw");
        }
        else if (party1Dead)
        {
            Debug.WriteLine("Party 2 wins!");
        }
        else if (party2Dead)
        {
            Debug.WriteLine("Party 1 wins!");
        }
        else
        {
            Debug.WriteLine("Too many turns!");
        }
        
        party1.Clear();
        party2.Clear();
    }

    public static async void AnimateBattle(LinkedList<Pet?> party1, LinkedList<Pet?> party2, Control battle1, Control battleOp1,IList<Battler.RandomThing?> party1RandomThings, IList<Battler.RandomThing?> party2RandomThings, Battler battler)
    {
        var count = 0;
        while (!AreAllPetsDead(party1) && !AreAllPetsDead(party2) && count < 100)
        {
            MovePetsForward(party1,party2);

            await Task.Delay(350).ConfigureAwait(false);
            
            if (count == 0)
            {
                await DoStartBattleEffects(party1.ToArray(),party2.ToArray(),party1RandomThings, true);
                await DoStartBattleEffects(party2.ToArray(),party1.ToArray(),party2RandomThings, false);
            }
            
            battler.ClearControls("battle");
            battler.ClearControls("battleOp");
                
            battler.MakePets(party1.ToArray(), "battle", fontSize: 13);
            battler.MakePets(party2.ToArray(), "battleOp", fontSize: 13, rightSide: true);

            var pet1 = party1.First;
            var pet2 = party2.First;

            pet1.Value.CurrentHealth -= pet2.Value.CurrentAttack;
            pet2.Value.CurrentHealth -= pet1.Value.CurrentAttack;
            
            if(pet1.Value.CurrentHealth < 0) pet1.Value.CurrentHealth = 0;
            if(pet2.Value.CurrentHealth < 0) pet2.Value.CurrentHealth = 0;
            
            
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
            
            
            battler.ClearControls("battle");
            battler.ClearControls("battleOp");
                
            battler.MakePets(party1.ToArray(), "battle", fontSize: 13);
            battler.MakePets(party2.ToArray(), "battleOp", fontSize: 13, rightSide: true);
            
            await Task.Delay(250).ConfigureAwait(false);

            if (pet1.Value.CurrentHealth <= 0)
            {
                await CheckForDeadEffects(party1.ToArray(), party1RandomThings, true);

                if (party1.First.Value.CurrentHealth <= 0)
                {
                    party1.RemoveFirst();
                    party1.AddLast((Pet?)null);
                }
            }
            
            if (pet2.Value.CurrentHealth <= 0)
            {
                await CheckForDeadEffects(party2.ToArray(), party2RandomThings, false);

                if (party2.First.Value.CurrentHealth <= 0)
                {
                    party2.RemoveFirst();
                    party2.AddLast((Pet?)null);
                }
            }

            MovePetsForward(party1,party2);
            
            battler.ClearControls("battle");
            battler.ClearControls("battleOp");
                
            battler.MakePets(party1.ToArray(), "battle", fontSize: 13);
            battler.MakePets(party2.ToArray(), "battleOp", fontSize: 13, rightSide: true);
            
            await Task.Delay(250).ConfigureAwait(false);
            
            count++;
        }

        await Task.Delay(1000).ConfigureAwait(false);
        
        battler.partyPanel.Invoke((Action)(() => battler.partyPanel.Show()));
        battler.shopPanel.Invoke((Action)(() => battler.shopPanel.Show()));
        battler.battlePanel.Invoke((Action)(() => battler.battlePanel.Hide()));
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
                    
                    effectImage.Animate(new YLocationEffect(), EasingFunctions.Linear, effectImage.Location.Y - 50, 400, 0, true);
                    effectImage.Animate(new XLocationEffect(), EasingFunctions.Linear, Battler.instance.Controls.Find(pet2OurParty ? "battle" + pet2Index : "battleOp" + pet2Index, true).First().Location.X, 800, 0);
                }
            ));
        
        await Task.Delay(810).ConfigureAwait(false);
        
        effectImage.Invoke((Action)(() =>
                {
                    effectImage.Location = origPos;
                    effectImage.Visible = false;
                }
            ));
        
    }

    public static async Task CheckForDeadEffects(IList<Pet?> party, IList<Battler.RandomThing?> randomThings, bool ourParty)
    {
        var pet = party[0];
        if (pet != null)
        {
            var ability = pet.Level1Ability;
            if (ability.Trigger == Trigger.Faint && ability.TriggeredBy.Kind == "Self")
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
                    }
                }
            }
        }
        
        await Task.Delay(200).ConfigureAwait(false);
    }
    
    public static async Task DoStartBattleEffects(IList<Pet?> party1,IList<Pet?> party2, IList<Battler.RandomThing?> randomThings, bool ourParty)
    {
        foreach (var pet in party1)
        {
            if (pet == null) continue;

            var ability = pet.Level1Ability;

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
                            }
                        }
                    }
                }
            }
        }
        
        await Task.Delay(200).ConfigureAwait(false);
                
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
    }
    
    public static bool AreAllPetsDead(IEnumerable<Pet?> party)
    {
        return party.All(pet => pet is not { CurrentHealth: > 0 });
    }
}
