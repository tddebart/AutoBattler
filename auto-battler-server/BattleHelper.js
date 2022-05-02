function SimulateBattle(user1, user2) {
    const party1 = JSON.parse(JSON.stringify(user1.partyPets));
    const party2 = JSON.parse(JSON.stringify(user2.partyPets));
    const party1randomThings = []
    const party2randomThings = []
    
    let count = 0;
    
    while(!AreAllPetsDead(party1) && !AreAllPetsDead(party2) && count < 100) {
        while (party1[0] == null) {
            party1.shift();
        }
        while (party2[0] == null) {
            party2.shift();
        }
        
        if (count === 0) {
            DoStartBattleEffects(party1,party2, party1randomThings);
            DoStartBattleEffects(party2,party1, party2randomThings);
        }
        
        // Do attacks
        party2[0].currentHealth -= party1[0].currentAttack;
        party1[0].currentHealth -= party2[0].currentAttack;
        
        // Check if front pet died and execute ability if any
        if(party1[0].currentHealth <= 0) {
            CheckDeadEffects(party1, party1randomThings)
            party1.shift();
        }
        if(party2[0].currentHealth <= 0) {
            CheckDeadEffects(party2, party2randomThings)
            party2.shift();
        }
        
        count++;
    }
    
    
    // Check if dead
    let party1Dead = AreAllPetsDead(party1);
    let party2Dead = AreAllPetsDead(party2)
    
    // Log results
    // console.log(`${user1.username}'s ${party1[0].name} has ${party1[0].currentHealth} health left.`);
    // console.log(`${user2.username}'s ${party2[0].name} has ${party2[0].currentHealth} health left.`);
    
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

function CheckDeadEffects(party, randomThings) {
    let pet = party[0]
    if(pet != null) {
        let ability = pet.level1Ability;
        if(ability != null && ability.trigger === "Faint" && ability.triggeredBy.kind === "Self") {
            if (ability.effect.kind === "ModifyStats") {
                let effect = ability.effect;
                if (effect.target.kind === "RandomFriend") {
                    if(party.length === 1) {
                        return;
                    }
                    
                    for (let i = 0; i < effect.target.n; i++) {
                        let randomPet = party[Math.floor(Math.random() * (party.length-1))+1];
                        while (randomPet == null || randomPet === pet) {
                            randomPet = party[Math.floor(Math.random() * (party.length-1))+1];
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
                            while (randomPet == null) {
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