const PacketBuilder = require("./packet-builder.js").PacketBuilder;
const Server = require("./server.js").Server;

const Game = {
    whoseTurn:1,
    whoHasWon:0,
    board:[ 
        [0,0,0],
        [0,0,0],
        [0,0,0],
    ],
    clientRed:null, // player 1
    clientBlue:null, // player 2
    playMove(client, x, y){
        //someone already won:
        if(this.whoHasWon > 0) return;

        //ignore move packets from everyone but clientX on clientX's turn:
        if(this.whoseTurn == 1 && client != this.clientRed) return;

        //ignore move packets from everyone but clientY on clientY's turn:
        if(this.whoseTurn == 2 && client != this.clientRed) return;

        if(x < 0) return; // ignore illegal moves
        if(y < 0) return; // ignore illegal moves

        if(y >= this.board.length) return; // ignore illegal moves
        if(x >= this.board[y].length) return;

        if(this.board[y][x] > 0) return; // ignore moves on taken spaces

        this.board[y][x] = this.whoseTurn; // sets the state of the board
        this.whoseTurn = (this.whoseTurn == 1) ? 2 : 1; // toggles the turn
        this.checkStateAndUpdate();  
    },
    checkStateAndUpdate(){
        // TODO: check for win
        const packet = PacketBuilder.update(this);
        Server.broadcastPacket(packet);
    }
};

Server.start(Game);