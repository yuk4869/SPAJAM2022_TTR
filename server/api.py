from flask import Flask, request
import base64
import io
import warnings
import cv2
from PIL import Image
import numpy as np

from paz.pipelines import DetectMiniXceptionFER

app = Flask(__name__)

pipeline = DetectMiniXceptionFER()

@app.route("/", methods=["POST"])
def index():
    img_pil = Image.open(io.BytesIO(request.files["request_data"].read()))
    img_numpy = np.asarray(img_pil)
    img_bgr = cv2.cvtColor(img_numpy, cv2.COLOR_RGBA2BGR)
    detect_result = pipeline(img_bgr)
    if not detect_result["boxes2D"]:
        return "0"
    class_name, score = detect_result["boxes2D"][0]._class_name, detect_result["boxes2D"][0]._score 
    print(class_name, score)
    if class_name != "happy":
        return "0"
    return f"{score}"

if __name__ == "__main__":
    warnings.simplefilter('ignore')
    app.run(debug=True)