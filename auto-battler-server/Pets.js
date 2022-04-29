const https = require('https');
const pc = require('punycode');

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

            info.pets = info.pets.filter(p => p.name != 'Sloth')
            //info.pets.forEach(p => p.image.unicodeCodePoint = pc.toASCII(p.image.unicodeCodePoint));
            //console.log(info.pets[0].packs);
        });
    });
}

function GetRandomPet() {
    if (info === undefined) {
        Init();
    }
    
    var array = [];
    
    let filteredPets = info.pets.filter(pet => pet.tier == 1 && pet.packs.includes('StandardPack'));
    
    let randomPet = filteredPets[Math.floor(Math.random() * filteredPets.length)];;
    
    if (Number.isInteger(randomPet.baseAttack)) {
        randomPet.currentAttack = randomPet.baseAttack;
    }
    if (Number.isInteger(randomPet.baseHealth)) {
        randomPet.currentHealth = randomPet.baseHealth;
    }

    return randomPet;
}

module.exports = {
    Init: Init,
    GetRandomPet: GetRandomPet
};