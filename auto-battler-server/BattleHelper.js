function SimulateBattle(user1, user2) {
    const party1 = JSON.parse(JSON.stringify(user1.partyPets));
    const party2 = JSON.parse(JSON.stringify(user2.partyPets));
    const party1randomThings = []
    const party2randomThings = []
    
    let count = 0;
    
    while(!AreAllPetsDead(party1) && !AreAllPetsDead(party2) && count < 100) {
        MovePetsForward(party1,party2);
        
        if (count === 0) {
            DoStartBattleEffects(party1,party2, party1randomThings);
            DoStartBattleEffects(party2,party1, party2randomThings);
        }


        CheckDeath(party1, party2, party1randomThings, party2randomThings);
        
        // Do attacks
        party2[0].currentHealth -= party1[0].currentAttack;
        party1[0].currentHealth -= party2[0].currentAttack;
        
        // Check if any pet died and execute ability if any
        CheckDeath(party1, party2, party1randomThings, party2randomThings);
        
        count++;
    }
    
    
    // Check if dead
    let party1Dead = AreAllPetsDead(party1);
    let party2Dead = AreAllPetsDead(party2)
    
    // Log results
    if (party1[0] !== undefined && party1[0] !== null) {
        console.log(`${user1.username}'s ${party1[0].name} has ${party1[0].currentHealth} health left.`);
    }
    if (party2[0] !== undefined && party2[0] !== null) {
        console.log(`${user2.username}'s ${party2[0].name} has ${party2[0].currentHealth} health left.`);
    }
    
    if (party1Dead && party2Dead) {
        console.log("Its a draw!");
    } else if (party1Dead) {
        console.log(`${user2.username} wins!`);
    } else if (party2Dead) {
        console.log(`${user1.username} wins!`);
    } else {
        console.log("Something went wrong.");
    }
    
    return (
        {
            party1: party1randomThings,
            party2: party2randomThings
        })
    
}

function CheckDeath(party1,party2,party1randomThings,party2randomThings) {
    // Check if any pet died and execute ability if any
    for (let i = 0; i < party1.length; i++) {
        let shouldRemove = CheckDeadEffects(party1[i], party1, party1randomThings)
        if (shouldRemove && party1[i] != null && party1[i].currentHealth <= 0) {
            party1.splice(i, 1);
        }
    }

    for (let i = 0; i < party2.length; i++) {
        let shouldRemove = CheckDeadEffects(party2[i], party2, party2randomThings)
        if (shouldRemove && party2[i] != null && party2[i].currentHealth <= 0) {
            party2.splice(i, 1);
        }
    }

    MovePetsForward(party1,party2);
}

function MovePetsForward(party1,party2) {
    if(party1.some(p => p != null)) {
        while (party1[0] == null) {
            party1.shift();
        }
    }

    if(party2.some(p => p != null)) {
        while (party2[0] == null) {
            party2.shift();
        }
    }
}

function CheckDeadEffects(pet,party, randomThings) {
    let shouldRemove = true;

    if (pet != null && pet.currentHealth <= 0) {
        let ability = pet.level1Ability;
        if (ability != null && ability.trigger === "Faint" && ability.triggeredBy.kind === "Self") {
            if (ability.effect.kind === "ModifyStats") {
                let effect = ability.effect;
                if (effect.target.kind === "RandomFriend") {
                    if (party.length === 1) {
                        return;
                    }

                    for (let i = 0; i < effect.target.n; i++) {
                        let randomPet = party[Math.floor(Math.random() * (party.length - 1)) + 1];
                        while (randomPet == null || randomPet === pet || randomPet.currentHealth <= 0) {
                            randomPet = party[Math.floor(Math.random() * (party.length - 1)) + 1];
                        }
                        randomThings.push({
                            petTrigger: party.indexOf(pet),
                            petTarget: party.indexOf(randomPet),
                            abilityTrigger: ability.trigger
                        })
                        ModifyStats(randomPet, effect);
                    }
                }
            }

            if (ability.effect.kind === "SummonPet") {
                let effect = ability.effect;
                if (effect.team === "Friendly") {
                    shouldRemove = false;
                    // Remove pet that died
                    party.splice(party.indexOf(pet), 1);
                    // Add new pet
                    let newPet = effect.pet;
                    newPet.baseAttack = effect.withAttack;
                    newPet.baseHealth = effect.withHealth;
                    newPet.currentHealth = newPet.baseHealth;
                    newPet.currentAttack = newPet.baseAttack;
                    party.unshift(newPet);
                    CheckSummonedEffect(newPet,party)
                }
            }
        }

        return shouldRemove;
    }
}

function CheckSummonedEffect(pet,party) {
    for (let i = 0; i < party.length; i++) {
        if (party[i] != null) {
            let ability = party[i].level1Ability;
            
            if (ability != null && ability.trigger === "Summoned") {
                if (ability.effect.kind === "ModifyStats") {
                    let effect = ability.effect;
                    if(effect.target.kind === "TriggeringEntity") {
                        if(effect.attackAmount != undefined) {
                            pet.currentAttack += effect.attackAmount;
                        }
                        if(effect.healthAmount != undefined) {
                            pet.currentHealth += effect.healthAmount;
                        }
                    }
                }
            }
        }
    }
}

function DoStartBattleEffects(party1,party2, randomThings) {
    for (let i = 0; i < party1.length; i++) {
        let pet = party1[i];
        if(pet != null) {
            let ability = pet.level1Ability;
            
            if(ability != null && ability.trigger === "StartOfBattle") {
                if (ability.effect.kind === "DealDamage") {
                    let effect = ability.effect;
                    if (effect.target.kind === "RandomEnemy") {
                        for (let j = 0; j < effect.target.n; j++) {
                            let randomPet = party2[Math.floor(Math.random() * (party2.length))];
                            while (randomPet == null || randomPet.currentHealth <= 0) {
                                randomPet = party2[Math.floor(Math.random() * (party2.length))];
                            }
                            
                            randomThings.push({
                                petTrigger: party1.indexOf(pet),
                                petTarget: party2.indexOf(randomPet),
                                abilityTrigger: ability.trigger
                            })
                            randomPet.currentHealth -= effect.amount;
                        }
                    }
                }
            }
        }
    }
}

function ModifyStats(pet, effect) {
    pet.currentAttack += effect.attackAmount;
    pet.currentHealth += effect.healthAmount;
}

function AreAllPetsDead(party) {
    if (party.length === 0) {
        return true;
    }
    
    let allDead = true;
    
    for (let i = 0; i < party.length; i++) {
        if (party[i] != null && party[i].currentHealth > 0) {
            allDead = false;
            break;
        }
    }
    
    return allDead;
}

module.exports = {
    SimulateBattle: SimulateBattle
}