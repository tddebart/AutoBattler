
const path = require('path');
const http = require('http');
const express = require('express');
const { Server } = require("socket.io");
const {
    getUser,
    exitRoom,
    newUser,
    getIndividualRoomUsers
} = require('./userHelper');
const {
    Init,
    GetRandomPet,
    MergePets
} = require('./Pets');

const {
    SimulateBattle,
    DoSellEffect,
    DoBuyEffect,
} = require('./BattleHelper')
const app = express();
const server = http.createServer(app);
const io = new Server(server);

let disableCoins = false;

// this block will run when the client connects
io.on('connection', socket => {
    Init();
    socket.on('joinRoom', ( username, room ) => {
        const user = newUser(socket.id, username, room);
        
        const users = getIndividualRoomUsers(room);
        
        if (users.length > 2) {
            socket.emit('roomFull', room);
            return;
        } else {
            Init();
            socket.join(room);
            
            if(users.length === 2) {
                user.opponent = users[0].id;
                users[0].opponent = user.id;
            }
            
            socket.emit('roomJoined', room);
        }

        socket.join(user.room);

        console.log(`${user.username} has joined the room ${user.room}`);
    });
    
    socket.on("getShop", () => {
        const user = getUser(socket.id);
        
        if (user && (user.coins >= 1 || disableCoins)) {
            let pets = [];
            
            for (let i = 0; i < 5; i++) {
                pets.push(GetRandomPet());
            }
            
            console.log('Send shop: ' + pets.map(p => p.name) + ' to ' + user.username);
            
            user.shopPets = pets;
            user.coins -= 1;
            
            socket.emit('receiveShop', JSON.stringify(pets));
        }
    });
    
    socket.on('buyPet', (shopIndex, partyIndex) => {
        const user = getUser(socket.id);
        
        if (user && user.shopPets[shopIndex] != null && (user.coins >= 3 || disableCoins)) {
            console.log(`${user.username} bought ${user.shopPets[shopIndex].name}`);
            
            if (user.partyPets[partyIndex] != null && user.shopPets[shopIndex].id === user.partyPets[partyIndex]?.id && user.partyPets[partyIndex]?.level !== 3) {
                user.coins -= 3;
                
                let didLevelUp = MergePets(-1, partyIndex,user.shopPets[shopIndex], user.partyPets[partyIndex], user);

                let currentRandomEffects = DoBuyEffect(user.partyPets[partyIndex], user);
                
                user.shopPets[shopIndex] = null;
                socket.emit('receiveShop', JSON.stringify(user.shopPets));
                socket.emit('receiveParty', JSON.stringify(user.partyPets));
                socket.emit('buySuccess', JSON.stringify({
                    randomEffects: currentRandomEffects,
                    buyIndex: partyIndex
                }))
                socket.emit("mergeSuccess", JSON.stringify({
                    index1: partyIndex,
                    didLevelUp: didLevelUp
                }) );
            } 
            else 
            {
                user.partyPets[partyIndex] = user.shopPets[shopIndex];
                user.shopPets[shopIndex] = null;
                user.coins -= 3;

                let currentRandomEffects = DoBuyEffect(user.partyPets[partyIndex], user);
                
                socket.emit('receiveParty', JSON.stringify(user.partyPets));
                socket.emit('receiveShop', JSON.stringify(user.shopPets));
                socket.emit('buySuccess', JSON.stringify({
                    randomEffects: currentRandomEffects,
                    buyIndex: partyIndex
                }))
            }
            
        }
    });
    
    socket.on('swapPet', (index1, index2) => {
        const user = getUser(socket.id);
        
        //console.log(`${user.username} swapped ${user.partyPets[index1].name} and ${user.partyPets[index2].name}`);
        
        if (user) {
            const temp = user.partyPets[index1];
            user.partyPets[index1] = user.partyPets[index2];
            user.partyPets[index2] = temp;
            
            socket.emit('receiveParty', JSON.stringify(user.partyPets));
        }
    });
    
    socket.on('mergePets', (index1, index2) => {
        const user = getUser(socket.id);
        
        if (user) {
            const pet1 = user.partyPets[index1];
            const pet2 = user.partyPets[index2];
            
            if (pet1.id === pet2.id && pet1.level !== 3 && pet2.level !== 3) {
                console.log(`${user.username} merged ${pet1.name} and ${pet2.name}`);
                let didLevelUp = MergePets(index1, index2,pet1, pet2, user)
                
                socket.emit('receiveParty', JSON.stringify(user.partyPets));
                socket.emit("mergeSuccess", JSON.stringify({
                    index1: index2,
                    didLevelUp: didLevelUp
                }) );
            }
        }
    });
    
    socket.on('sellPet', (index) => {
        const user = getUser(socket.id);
        
        if (user) {
            console.log(`${user.username} sold ${user.partyPets[index].name}`);
            user.coins += user.partyPets[index].level;
            
            let currentRandomEffects = DoSellEffect(user.partyPets[index], user);
            let soldPet = JSON.parse(JSON.stringify(user.partyPets[index]));
            
            user.partyPets[index] = null;

            // socket.emit('receiveParty', JSON.stringify(user.partyPets));
            socket.emit('receiveShop', JSON.stringify(user.shopPets));
            socket.emit('soldSuccess', JSON.stringify({
                randomEffects: currentRandomEffects,
                soldIndex: index
            }));
        }
    });
    socket.on('ready', () => {
        const user = getUser(socket.id);
        
        if (user) {
            user.ready = true;
            const opponent = getUser(user.opponent);
            
            if (user.ready && opponent != null && opponent.ready) {
                console.log(`${user.username} and ${opponent.username} are ready to battle!`);
                
                
                let randomEffects = SimulateBattle(user, opponent);
                
                
                // Send party to user with socket id
                socket.emit('receiveParty', JSON.stringify(user.partyPets));
                socket.broadcast.to(user.room).emit('receiveParty', JSON.stringify(opponent.partyPets));
                socket.emit('battleStarted', JSON.stringify({
                    oppPets: opponent.partyPets,
                    party1RandomEffects: randomEffects.party1,
                    party2RandomEffects: randomEffects.party2
                }));
                socket.broadcast.to(user.room).emit('battleStarted', JSON.stringify({
                    oppPets: user.partyPets,
                    party1RandomEffects: randomEffects.party2,
                    party2RandomEffects: randomEffects.party1
                }));


                
                
                user.ready = false;
                user.coins = 13;
                opponent.ready = false;
                opponent.coins = 13;
                user.turn++;
            }
            
            if(opponent == null) {
                console.log(`${user.username} is going to battle themself!`);
                let user2 = Object.assign({}, user);
                user2.username = user2.username + '2';
                
                let randomEffects = SimulateBattle(user, user2);
                
                socket.emit('receiveParty', JSON.stringify(user.partyPets));
                socket.emit('battleStarted', JSON.stringify({
                    oppPets: user2.partyPets,
                    party1RandomEffects: randomEffects.party1,
                    party2RandomEffects: randomEffects.party2
                }));
                
                user.ready = false;
                
                user.coins = 13;
                user.turn++;
            }
        }
    });
    
    socket.on('disableCoins', () => {
        disableCoins = true; 
        console.log('Coins disabled');
    });

    // Runs when client disconnects
    socket.on('disconnect', () => {
        const user = exitRoom(socket.id);

        if (user) {
            // Current active users and room name
            io.to(user.room).emit('roomUsers', {
                room: user.room,
                users: getIndividualRoomUsers(user.room)
            });
            console.log(`${user.username} has left the room ${user.room}`);
        }
    });
});

const PORT = process.env.PORT || 3000;

Init();

// app.listen(PORT, () => console.log(`Server running at http://127.0.0.1:${PORT}/`))
server.listen(PORT, () => console.log(`Server running at http://127.0.0.1:${PORT}/`));