import socket
import requests
import json

from flask import Flask,render_template,request,jsonify
from flask_cors import CORS

app = Flask(__name__)
CORS(app)  # 啟用 CORS

JSON_RPC_RUI = "http://127.0.0.1:7706/jsonrpc"
PORT = 5000

@app.route('/setDevice', methods=['POST'])
def setDevice():
    try:
        data = request.get_json()
        device_name = data.get('deviceName')
        device_uuid = data.get('deviceUuid')

        #setting default device
        setComputerDevice(device_name , device_uuid)

        response_data = {'success': f"設定喇吧為{device_name}，Uuid:{device_uuid}"}
        return jsonify(response_data), 200  # Return a valid response with a 200 status code
    except Exception as e:
        print(f"Error processing JSON data: {e}")
        response_data = {'success': False, 'error': 'Invalid JSON data'}
        return jsonify(response_data), 400  # Return a valid response with a 400 status code

@app.route('/getComputerDeviceVol', methods=['GET'])
def getComputerDeviceVol():
    deviceUuid = request.args.get('deviceUuid')

    vol = getComputerDeviceVolume(deviceUuid)

    return vol

@app.route('/setComputerDeviceVol', methods=['POST'])
def setComputerDeviceVol():
    data = request.get_json()    
    device_uuid = data.get('deviceUuid')
    device_vol = data.get('deviceVol')

    res = setComputerDeviceVolume(device_uuid , device_vol)

    return res

@app.route('/getComputerDefaultDevice', methods=['GET'])
def getComputerDefaultDevice():    

    res = getDefaultDevice()

    return res

@app.route('/device', methods=['GET', 'POST'])
def index():
    response = getComputerDevice()
    data = json.loads(response["result"])
    
    if request.method == 'POST':
        selected_device_uuid = request.form.get('device')
        # 在這裡處理提交表單後的邏輯，可以使用選擇的設備 UUID（selected_device_uuid）


    return render_template('index.html', data=data , ip=get_ip() , port = PORT)

@app.route('/')
def hello_world():
    response = getComputerDevice()
    print(response)
    return  json.loads(response["result"])

def getComputerDevice():
    getComputerDevice= {
        "method": "GetComputerDevice",    
        "params": [],    
        "jsonrpc": "2.0",
        "id": 0,
    }
    response = requests.post(JSON_RPC_RUI, json=getComputerDevice).json()
    print(response)
    return response

def setComputerDevice(deviceName , deviceUuid):
    getComputerDevice= {
        "method": "SetComputerDevice",    
        "params": [deviceName, deviceUuid],    
        "jsonrpc": "2.0",
        "id": 0,
    }
    response = requests.post(JSON_RPC_RUI, json=getComputerDevice).json()
    print(response)
    return response

def getComputerDeviceVolume(deviceUuid):
    cmd= {
        "method": "GetComputerDeviceVol",    
        "params": [deviceUuid],    
        "jsonrpc": "2.0",
        "id": 0,
    }
    response = requests.post(JSON_RPC_RUI, json=cmd).json()
    print(response)
    return response


def setComputerDevice(deviceName , deviceUuid):
    getComputerDevice= {
        "method": "SetComputerDevice",    
        "params": [deviceName, deviceUuid],    
        "jsonrpc": "2.0",
        "id": 0,
    }
    response = requests.post(JSON_RPC_RUI, json=getComputerDevice).json()
    print(response)
    return response
    
def setComputerDeviceVolume(deviceUuid , vol):
    cmd= {
        "method": "SetComputerDeviceVol",    
        "params": [deviceUuid , vol],    
        "jsonrpc": "2.0",
        "id": 0,
    }
    response = requests.post(JSON_RPC_RUI, json=cmd).json()
    print(response)
    return response
    
def getDefaultDevice():
    cmd= {
        "method": "GetComputerDefaultDevice",    
        "params": [],    
        "jsonrpc": "2.0",
        "id": 0,
    }
    response = requests.post(JSON_RPC_RUI, json=cmd).json()
    print(response)
    return response
    

def get_ip():
    try:
        # 使用 socket.gethostbyname(socket.gethostname()) 獲取主機名稱的 IP 地址
        ip = socket.gethostbyname(socket.gethostname())
        return ip
    except Exception as e:
        return str(e)
        
if __name__ == "__main__":    
    print ("Local IP IS " , get_ip())
    app.run(debug=True, host='0.0.0.0',port=PORT)
