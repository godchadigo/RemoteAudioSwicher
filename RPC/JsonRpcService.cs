using NAudio.Manager;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TouchSocket.Http;
using TouchSocket.JsonRpc;
using TouchSocket.Rpc;
using TouchSocket.Sockets;

namespace NAudio.RPC
{
    public class JsonRpcService : RpcServer
    {
        /// <summary>
        /// 使用调用上下文。
        /// 可以从上下文获取调用的SocketClient。从而获得IP和Port等相关信息。
        /// </summary>
        /// <param name="callContext"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        [JsonRpc(MethodInvoke = true)]
        public string TestGetContext(ICallContext callContext, string str)
        {
            if (callContext.Caller is IHttpSocketClient socketClient)
            {
                if (socketClient.Protocol == Protocol.WebSocket)
                {
                    Console.WriteLine("WebSocket请求");
                    var client = callContext.Caller as IHttpSocketClient;
                    var ip = client.IP;
                    var port = client.Port;
                    Console.WriteLine($"WebSocket请求{ip}:{port}");
                }
                else
                {
                    Console.WriteLine("HTTP请求");
                    var client = callContext.Caller as IHttpSocketClient;
                    var ip = client.IP;
                    var port = client.Port;
                    Console.WriteLine($"HTTP请求{ip}:{port}");
                }
            }
            else if (callContext.Caller is ISocketClient)
            {
                Console.WriteLine("Tcp请求");
                var client = callContext.Caller as ISocketClient;
                var ip = client.IP;
                var port = client.Port;
                Console.WriteLine($"Tcp请求{ip}:{port}");
            }
            return "RRQM" + str;
        }

        [JsonRpc(MethodInvoke = true)]
        public JObject TestJObject(JObject obj)
        {
            return obj;
        }

        [JsonRpc(MethodInvoke = true)]
        public string TestJsonRpc(string str)
        {
            return "RRQM" + str;
        }

        [JsonRpc(MethodInvoke = true)]
        public string GetComputerDevice()
        {
            //List<DevcieProfile> 
            List<DevcieProfile> devices = DeviceManager.Instance.DeviceProfiles;
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(devices , Newtonsoft.Json.Formatting.Indented);
            return result;            
        }
        
        [JsonRpc(MethodInvoke = true)]
        public bool SetComputerDevice(string deviceName , string deviceUuid)
        {
            try
            {
                List<DevcieProfile> devices = DeviceManager.Instance.DeviceProfiles;
                var result = devices.Where(x => x.Uuid == new Guid(deviceUuid)).FirstOrDefault();
                if (result != null)
                {
                    result.Core.SetAsDefault();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }                        
        }

        [JsonRpc(MethodInvoke = true)]
        public double GetComputerDeviceVol(string deviceUuid)
        {            
            if (deviceUuid.Length == 0) return 0;
            List<DevcieProfile> devices = DeviceManager.Instance.DeviceProfiles;

            var result = devices.Where(x => x.Uuid == new Guid(deviceUuid)).FirstOrDefault();
            if (result != null)
            {
                return result.Core.Volume;                
            }
            return 0.0;
        }

        [JsonRpc(MethodInvoke = true)]
        public bool SetComputerDeviceVol(string deviceUuid , double vol)
        {
            try
            {
                List<DevcieProfile> devices = DeviceManager.Instance.DeviceProfiles;
                var result = devices.Where(x => x.Uuid == new Guid(deviceUuid)).FirstOrDefault();
                if (result != null)
                {
                    result.Core.SetVolumeAsync(vol);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [JsonRpc(MethodInvoke = true)]
        public string GetComputerDefaultDevice()
        {            
            List<DevcieProfile> devices = DeviceManager.Instance.DeviceProfiles;

            foreach (DevcieProfile device in devices)
            {
                if (device.Core.IsDefaultDevice && !device.Core.IsDefaultCommunicationsDevice)
                {
                    device.Vol = device.Core.Volume;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(device);
                }                    
            }
            return "";
;        }
    }

}
