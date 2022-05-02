
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using auto_battler_frontend;
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

    public static async Task AnimateBattle(LinkedList<Pet?> party1, LinkedList<Pet?> party2, Control battle1, Control battleOp1,IList<Battler.RandomThing?> party1RandomThings, IList<Battler.RandomThing?> party2RandomThings, Battler battler)
    {
        var count = 0;
        while (!AreAllPetsDead(party1) && !AreAllPetsDead(party2) && count < 100)
        {
            MovePetsForward(party1,party2);
            
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
                CheckForDeadEffects(party1.ToArray(), party1RandomThings);
                
                party1.RemoveFirst();
                party1.AddLast((Pet?)null);
            }
            
            if (pet2.Value.CurrentHealth <= 0)
            {
                CheckForDeadEffects(party2.ToArray(), party2RandomThings);
                
                party2.RemoveFirst();
                party2.AddLast((Pet?)null);
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
    }

    public static void CheckForDeadEffects(IList<Pet?> party, IList<Battler.RandomThing> randomThings)
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

                    if (effect.Target.Kind == "RandomFriend" && randomThings.Any(t => party.IndexOf(pet) == t.petTriggerIndex))
                    {
                        var randomThing = randomThings.First(t => party.IndexOf(pet) == t.petTriggerIndex);
                        var target = party[randomThing.petTargetIndex];
                        if (target != null)
                        {
                            target.CurrentHealth += effect.HealthAmount;
                            target.CurrentAttack += int.Parse(effect.AttackAmount);
                        }
                    }
                }
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
    }
    
    public static bool AreAllPetsDead(IEnumerable<Pet?> party)
    {
        return party.All(pet => pet is not { CurrentHealth: > 0 });
    }
}
