from flask import Flask, request, jsonify
from gomoku import Gomoku
from player_zero4 import ZeroPlayer, net

app = Flask(__name__)

SIZE = 9
WIN = 5
N_ITER = 50

model1 = net(SIZE)
model1.load_weights('D:/AlphaGomoku/AlphaGomoku-main/4feat 9x9/day10.h5')
game = Gomoku(SIZE, WIN)
p1 = ZeroPlayer('p1', +1, game, model1, N_ITER)
p1.tree.temp = .1

@app.route('/play', methods=['POST'])
def play():
    data = request.json
    row = data['row']
    col = data['col']
    game.play(row, col)
    ai_move = p1.play(game)
    game.play(ai_move[0], ai_move[1])
    return jsonify({'ai_row': ai_move[0], 'ai_col': ai_move[1], 'finished': game.finished})

if __name__ == '__main__':
    app.run(debug=True)
