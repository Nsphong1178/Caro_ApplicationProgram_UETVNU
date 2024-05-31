from flask import Flask, request, jsonify
import numpy as np

app = Flask(__name__)

class Gomoku:
    def __init__(self, size=15, win=5):
        self.size = size
        self.win = win
        self.board = np.zeros((size, size), dtype=np.int8)
        self.episode = []
        self.finished = False
        self.winner = 0
        self.curr_player = +1
        
    def play(self, x, y):
        assert not self.finished, "game has ended"
        assert self.board[x, y] == 0, "invalid move"
        self.board[x, y] = self.curr_player
        self.episode.append((x, y))
        self.curr_player *= -1
        if np.sum(self.available_actions()) == 0:
            self.finished = True
        self.winner = self.find_winner(x, y)
        if self.winner != 0:
            self.finished = True

    def available_actions(self):
        return self.board == 0
    
    def available_actions_list(self):
        return np.where(self.available_actions().flatten())[0]
    
    def forbidden_actions(self):
        return self.board != 0
    
    def forbidden_actions_list(self):
        return np.where(self.forbidden_actions().flatten())[0]

    def find_winner(self, x, y):
        piece = self.board[x, y]
        i, j = 0, 0
        while x-i-1 >= 0 and self.board[x-i-1, y] == piece: i += 1
        while x+j+1 < self.size and self.board[x+j+1, y] == piece: j += 1
        if i+j+1 >= self.win: return piece
        i, j = 0, 0
        while y-i-1 >= 0 and self.board[x, y-i-1] == piece: i += 1
        while y+j+1 < self.size and self.board[x, y+j+1] == piece: j += 1
        if i+j+1 >= self.win: return piece
        i, j = 0, 0
        while x-i-1 >= 0 and y-i-1 >= 0 and self.board[x-i-1, y-i-1] == piece: i += 1
        while x+j+1 < self.size and y+j+1 < self.size and self.board[x+j+1, y+j+1] == piece: j += 1
        if i+j+1 >= self.win: return piece
        i, j = 0, 0
        while x+i+1 < self.size and y-i-1 >= 0 and self.board[x+i+1, y-i-1] == piece: i += 1
        while x-j-1 >= 0 and y+j+1 < self.size and self.board[x-j-1, y+j+1] == piece: j += 1
        if i+j+1 >= self.win: return piece
        return 0

    def show(self):
        pieces = {0:'.', 1:'\u25CF', -1:'\u25CB'}
        colors = {0: "none", 1:"black", -1:"white"}
        if len(self.episode) >= 2:
            print("{0:s} played {1}.".format(colors[self.curr_player], self.episode[-2]))
        if len(self.episode) >= 1:
            print("{0:s} played {1}.".format(colors[-self.curr_player], self.episode[-1]))
        if self.finished:
            print("game has ended. winner: {0:s}".format(colors[self.winner]))
        else:
            print("{0:s}'s turn.".format(colors[self.curr_player]))
        print("  ", end='')
        for x in range(self.size):
            print("{0:2d}".format(x), end='')
        print()
        for y in range(self.size):
            print("{0:2d}".format(y), end=' ')
            for x in range(self.size):
                print(pieces[self.board[x, y]], end=' ')
            print()

    def copy(self):
        new_game = Gomoku(size=self.size, win=self.win)
        new_game.board = self.board.copy()
        new_game.episode = self.episode[:]
        new_game.finished = self.finished
        new_game.winner = self.winner
        new_game.curr_player = self.curr_player
        return new_game


class Player:
    def __init__(self, name, piece):
        self.name = name
        self.piece = piece

    def play(self, game):
        avail_acts = game.available_actions_list()
        move = np.random.choice(avail_acts)
        return move//game.size, move%game.size


# Initialize a new game
game = Gomoku(size=9, win=5)

@app.route('/make_move', methods=['POST'])
def make_move():
    data = request.get_json()
    player = data.get('player')
    move = data.get('move')
    
    if move is None:
        return jsonify({'error': 'Move is required'}), 400

    try:
        x, y = map(int, move)
        game.play(x, y)
        return jsonify({
            'Board': game.board.tolist(),
            'Finished': game.finished,
            'Winner': game.winner
        })
    except Exception as e:
        return jsonify({'error': str(e)}), 400

@app.route('/get_state', methods=['GET'])
def get_state():
    return jsonify({
        'Board': game.board.tolist(),
        'Finished': game.finished,
        'Winner': game.winner
    })

if __name__ == '__main__':
    app.run(debug=True)
