const https = require('https');
const pc = require('punycode');
const {DoLevelUpEffect} = require('./BattleHelper');

// Load pets from https://superauto.pet/api.json

let info = undefined;

function Init() {
    https.get('https://superauto.pet/api.json', (res) => {
        let data = '';

        res.on('data', (chunk) => {
            data += chunk;
        });

        res.on('end', () => {
            info = JSON.parse(data);
            info.pets = Object.values(info.pets);

            info.pets = info.pets.filter(p => p.name !== 'Sloth')
            info.pets.forEach(p => {
                if (p.level1Ability?.effect?.pet !== undefined) {
                    p.level1Ability.effect.pet = info.pets.find(p2 => p2.id === p.level1Ability.effect.pet);
                } 
                if (p.level2Ability?.effect?.pet !== undefined) {
                    p.level2Ability.effect.pet = info.pets.find(p2 => p2.id === p.level2Ability.effect.pet);
                } 
                if (p.level3Ability?.effect?.pet !== undefined) {
                    p.level3Ability.effect.pet = info.pets.find(p2 => p2.id === p.level3Ability.effect.pet);
                } 
                p.experience = 0;
                p.level = 1;
            } );
            //info.pets.forEach(p => p.image.unicodeCodePoint = pc.toASCII(p.image.unicodeCodePoint));
            // console.log(info.pets[0].packs);
        });
    });
}

function GetRandomPet() {
    if (info === undefined) {
        Init();
    }
    
    var array = [];
    
    let filteredPets = info.pets.filter(pet => pet.tier === 1 && pet.packs.includes('StandardPack'));
    
    let randomPet = filteredPets[Math.floor(Math.random() * filteredPets.length)];
    
    if (Number.isInteger(randomPet.baseAttack)) {
        randomPet.currentAttack = randomPet.baseAttack;
    }
    if (Number.isInteger(randomPet.baseHealth)) {
        randomPet.currentHealth = randomPet.baseHealth;
    }

    return JSON.parse(JSON.stringify(randomPet));
}

function MergePets(index1, index2, pet1, pet2,user) {
    
    pet1.experience += pet2.experience+1;
    pet1.level = pet2.level;
    
    let newHealth = Math.max(pet1.currentHealth, pet2.currentHealth);
    let newAttack = Math.max(pet1.currentAttack, pet2.currentAttack);

    let didLevelUp = CheckExperience(user, pet1)
    
    pet1.currentHealth = newHealth;
    pet1.currentAttack = newAttack;
    
    pet1.currentAttack += 1;
    pet1.currentHealth += 1;
    
    user.partyPets[index2] = pet1;
    if (index1 !== -1) {
        user.partyPets[index1] = null;
    }

    return didLevelUp;
}

function CheckExperience(user, pet) {
    let didLevelUp = false;
    switch(pet.level) {
        case 1:
            if (pet.experience >= 2) {
                console.log('Level up! level 2');
                DoLevelUpEffect(pet, user);
                didLevelUp = true;
                pet.level = 2;
                //CheckExperience(pet);
            }
            break;
        case 2:
            if (pet.experience >= 5) {
                console.log('Level up! level 3');
                didLevelUp = true;
                DoLevelUpEffect(pet, user);
                pet.level = 3;
            }
    }
    
    return didLevelUp;
}

function GetLevel(pet) {
    if (pet.experience > 2) {
        return 2;
    } else if (pet.experience > 5) {
        return 3;
    } else {
        return 1;
    }
}

module.exports = {
    Init: Init,
    GetRandomPet: GetRandomPet,
    MergePets: MergePets
};