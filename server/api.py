from flask import Flask, request

app = Flask(__name__)

@app.route("/", methods=["POST"])
def index():
    print(request.get_data())
    return b"Helllo World"

if __name__ == "__main__":
    app.debug = True
    app.run()