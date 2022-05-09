const users = [];

// Create a new user
function newUser(id, username, room) {
  const user = new User(id, username, room);

  users.push(user);

  return user;
}

// Get current user
function getUser(id) {
  return users.find(user => user.id === id);
}

// User leaves
function exitRoom(id) {
  const index = users.findIndex(user => user.id === id);

  if (index !== -1) {
    return users.splice(index, 1)[0];
  }
}

// Get room users
function getIndividualRoomUsers(room) {
  return users.filter(user => user.room === room);
}

class User {
  constructor(id, username, room) {
    this.id = id;
    this.username = username;
    this.room = room;
    this.ready = false;
    this.opponent = null;
    this.shopPets = [];
    this.partyPets = [];
    this.coins = 13;
  }
}

module.exports = {
  newUser,
  getUser: getUser,
  exitRoom,
  getIndividualRoomUsers
};