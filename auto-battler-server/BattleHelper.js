function SimulateBattle(user1, user2) {
    const party1 = JSON.parse(JSON.stringify(user1.partyPets));
    const party2 = JSON.parse(JSON.stringify(user2.partyPets));
    
    // Do attacks
    party2[0].currentHealth -= party1[0].currentAttack;
    party1[0].currentHealth -= party2[0].currentAttack;
    
    // Check if dead
    let party1Dead;
    let party2Dead;
    
    if (party1[0].currentHealth <= 0) {
        party1[0].currentHealth = 0;
        party1Dead = true;
    }
    else {
        party1Dead = false;
    }
    
    if (party2[0].currentHealth <= 0) {
        party2[0].currentHealth = 0;
        party2Dead = true;
    }
    else {
        party2Dead = false;
    }
    
    // Log results
    console.log(`${user1.username}'s ${party1[0].name} has ${party1[0].currentHealth} health left.`);
    console.log(`${user2.username}'s ${party2[0].name} has ${party2[0].currentHealth} health left.`);
    
    if (party1Dead && party2Dead) {
        console.log("Its a draw!");
    } else if (party1Dead) {
        console.log(`${user2.username} wins!`);
    } else if (party2Dead) {
        console.log(`${user1.username} wins!`);
    }
}

function DoTurn() {
    
}

function AreAllPetsDead(party) {
    let allDead = true;
    
    for (let i = 0; i < party.length; i++) {
        if (party[i] != null && party[i].baseHealth > 0) {
            allDead = false;
            break;
        }
    }
    
    return allDead;
}

module.exports = {
    SimulateBattle: SimulateBattle
}