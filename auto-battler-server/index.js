
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
    GetRandomPet
} = require('./Pets');

const {
    SimulateBattle
} = require('./BattleHelper')
const app = express();
const server = http.createServer(app);
const io = new Server(server);

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

        // General welcome
        // socket.emit('message', formatMessage("SERVER", 'Messages are limited to this room! '));

        // Broadcast everytime users connects
        // socket.broadcast
        //     .to(user.room)
        //     .emit(
        //         'message', formatMessage("SERVER", `${user.username} has joined the room`)
        //     );

        // Current active users and room name
        // io.to(user.room).emit('roomUsers', {
        //     room: user.room,
        //     users: getIndividualRoomUsers(user.room)
        // });

        console.log(`${user.username} has joined the room ${user.room}`);
    });
    
    socket.on("getShop", () => {
        const user = getUser(socket.id);
        
        if (user) {
            let pets = [];
            
            for (let i = 0; i < 5; i++) {
                pets.push(GetRandomPet());
            }
            
            console.log('Send shop: ' + pets.map(p => p.name));
            
            user.shopPets = pets;
            
            socket.emit('receiveShop', JSON.stringify(pets));
        }
    });
    
    socket.on('buyPet', (shopIndex, partyIndex) => {
        const user = getUser(socket.id);
        
        if (user && user.shopPets[shopIndex] != null) {
            console.log(`${user.username} bought ${user.shopPets[shopIndex].name}`);
            
            user.partyPets[partyIndex] = user.shopPets[shopIndex];
            user.shopPets[shopIndex] = null;
            
            socket.emit('receiveParty', JSON.stringify(user.partyPets));
            socket.emit('receiveShop', JSON.stringify(user.shopPets));
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
    
    socket.on('ready', () => {
        const user = getUser(socket.id);
        
        if (user) {
            user.ready = true;
            const opponent = getUser(user.opponent);
            
            if (user.ready && opponent != null && opponent.ready) {
                console.log(`${user.username} and ${opponent.username} are ready to battle!`);
                SimulateBattle(user, opponent);
                user.ready = false;
                opponent.ready = false;
            }
        }
    });
    
    

    // Listen for client message
    socket.on('attackMessage', msg => {
        const user = getUser(socket.id);

        console.log(`${user.username} has attacked ${msg.target}`);
        // io.to(user.room).emit('message', formatMessage(user.username, msg));
    });

    // Runs when client disconnects
    socket.on('disconnect', () => {
        const user = exitRoom(socket.id);

        if (user) {
            // io.to(user.room).emit(
            //     'message',
            //     formatMessage("SERVER", `${user.username} has left the room`)
            // );

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