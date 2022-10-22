from flask import Flask

app = Flask(__name__)

@app.route("/", methods=["POST"])
def index(request):
    print(request)
    return b"Helllo World"

if __name__ == "__main__":
    app.debug = True
    app.run()