using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Net.Http.Headers;
using AudioSwitcher.AudioApi;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TouchSocket.Rpc;
using NAudio.RPC;
using TouchSocket.Http;
using NAudio.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using SoundSwitch.Audio.Manager;

namespace NAudio
{
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
            //TestThread();            
            InitDevices();
            InitCombobox();
            InitTrackBar();
            InitEventBinding();
            CreateJsonRpcServer();
        }        
        public void InitDevices()
        {            
            var devices = DeviceManager.Instance.AudioCore.GetDevices();

            foreach (var device in devices)
            {
                DevcieProfile model = new DevcieProfile ();
                model.Name = device.FullName;
                model.Uuid = device.Id;
                model.Vol = device.Volume;
                model.Core = device;


                DeviceManager.Instance.DeviceProfiles.Add(model);
            }
        }
        public void InitCombobox()
        {
            string defaultDeviceName = string.Empty;
            foreach (var item in DeviceManager.Instance.DeviceProfiles)
            {
                string info = $"音響名稱:{item.Name} ID:{item.Uuid} 音量:{item.Vol}";
                comboBox1.Items.Add(info);
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.Text = comboBox1.Items[0].ToString();
                }

                if (item.Core.IsDefaultDevice && !item.Core.IsDefaultCommunicationsDevice)
                {
                    defaultDeviceName = info;
                    var volumeObserver = new VolumeChangedObserver(trackBar1, label1);
                    // 訂閱音量變化事件
                    item.Core.VolumeChanged.Subscribe(volumeObserver);
                }
            }
            comboBox1.Text = defaultDeviceName;
        }
        public void InitTrackBar()
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            int newIndex = comboBox1.SelectedIndex;
            if (newIndex <= DeviceManager.Instance.DeviceProfiles.Count && newIndex != -1)
            {
                var newDevice = DeviceManager.Instance.DeviceProfiles[newIndex];
                trackBar1.Value = (int)newDevice.Vol;
                label1.Text = newDevice.Vol.ToString();
            }
            else
            {
                trackBar1.Value = 0;
                label1.Text = "0";
            }
            
        }

        public void InitEventBinding()
        {

            var subscription = DeviceManager.Instance.AudioCore.AudioDeviceChanged.Subscribe(
                new AudioDeviceChangedObserver(
                    ev => 
                    {
                        if (!ev.Device.IsDefaultDevice)
                        {
                            Console.WriteLine("偵測到輸出被變更，原先的設備為:" +  ev.Device.FullName);
                            
                        }
                        else
                        {
                            Console.WriteLine("偵測到輸出被變更，變更後設備為:" + ev.Device.FullName);
                            var volumeObserver = new VolumeChangedObserver(trackBar1 , label1);
                            // 訂閱音量變化事件
                            ev.Device.VolumeChanged.Subscribe(volumeObserver);

                            //委派修改設備名稱
                            var res = DeviceManager.Instance.DeviceProfiles.Where(x => x.Uuid == ev.Device.Id).FirstOrDefault();
                            if (res != null)
                            {
                                string info = $"音響名稱:{res.Name} ID:{res.Uuid} 音量:{res.Vol}";
                                if (comboBox1.InvokeRequired)
                                {

                                    comboBox1.Invoke((Action)(() => comboBox1.Text = info));
                                }
                                else
                                {
                                    comboBox1.Text = info;
                                }
                            }                                                        
                        }
                        
                    },
                    error => Console.WriteLine($"發生錯誤：{error}"),
                    () => Console.WriteLine("觀察者完成")
                )
            );
        }
       
        public void CreateJsonRpcServer()
        {
            var service = new HttpService();

            service.Setup(new TouchSocketConfig()
                 .SetListenIPHosts(7706)
                 .ConfigureContainer(a =>
                 {
                     a.AddRpcStore(store =>
                     {
                         store.RegisterServer<JsonRpcService>();
                     });
                 })
                 .ConfigurePlugins(a =>
                 {
                     a.UseHttpJsonRpc()
                     .SetJsonRpcUrl("/jsonRpc");
                 }));

            service.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //AudioSwitcher.Instance.SwitchTo("", );
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ddeadda1-f6c5-493e-85cf-e84236d4b3e6  MR4
            var core = new CoreAudioController();
            var devices = core.GetDevices();

            var res = devices.Where(x => x.Id == new Guid("ddeadda1-f6c5-493e-85cf-e84236d4b3e6")).FirstOrDefault();

            if (res != null)
            {
                Console.WriteLine(res.FullName);
                res.SetAsDefault();
                
            }

            /*
            Console.WriteLine("------------------------");
            foreach (var device in devices)
            {
                Console.WriteLine(device.Id + " " + device.Name);
            }
            Console.WriteLine("------------------------");
            */
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //var device = AudioSwitcher.Instance.GetDevice("{0.0.0.00000000}.{69044ed5-f7ad-4b06-a561-7f92f80dc325}");
            //Console.WriteLine(device.FriendlyName);
            //EDIFIER MR4 (Realtek(R) Audio)
            //{0.0.0.00000000}.{69044ed5-f7ad-4b06-a561-7f92f80dc325}
            /*
            var devices = GetAudioDevices();
            var result = devices.Where(x => x.FriendlyName == "EDIFIER MR4 (Realtek(R) Audio)").FirstOrDefault();

            if (result != null)
            {
                Console.WriteLine("MR4 ID : " + result.ID);
            }

            */
            //AudioSwitcher.Instance.SwitchTo("", );
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            int newIndex = comboBox1.SelectedIndex;
            if (newIndex <= DeviceManager.Instance.DeviceProfiles.Count && newIndex != -1)
            {
                var newDevice = DeviceManager.Instance.DeviceProfiles[newIndex];
                trackBar1.Value = (int)newDevice.Vol;
            }
            else
            {
                trackBar1.Value = 0;
            }
        }
        /// <summary>
        /// 設為預設輸出喇吧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            int newIndex = comboBox1.SelectedIndex;
            if (newIndex <= DeviceManager.Instance.DeviceProfiles.Count && newIndex != -1)
            {
                var newDevice = DeviceManager.Instance.DeviceProfiles[newIndex];
                newDevice.Core.SetAsDefault();
                Console.WriteLine($"設定{newDevice.Name}為新的輸出喇吧!");
            }
        }
        /// <summary>
        /// 音量+
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button8_Click(object sender, EventArgs e)
        {
            int newIndex = comboBox1.SelectedIndex;
            if (newIndex <= DeviceManager.Instance.DeviceProfiles.Count && newIndex != -1)
            {
                var newDevice = DeviceManager.Instance.DeviceProfiles[newIndex];
                var vol = await newDevice.Core.GetVolumeAsync();
                if (vol < 100)
                {
                    await newDevice.Core.SetVolumeAsync(vol + 1);
                }                
            }
        }
        /// <summary>
        /// 音量-
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button9_Click(object sender, EventArgs e)
        {
            int newIndex = comboBox1.SelectedIndex;
            if (newIndex <= DeviceManager.Instance.DeviceProfiles.Count && newIndex != -1)
            {
                var newDevice = DeviceManager.Instance.DeviceProfiles[newIndex];
                var vol = await newDevice.Core.GetVolumeAsync();
                if (vol > 1)
                {
                    await newDevice.Core.SetVolumeAsync(vol - 1);
                }                
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int newIndex = comboBox1.SelectedIndex;
            int newValue = trackBar1.Value;
            if (newIndex <= DeviceManager.Instance.DeviceProfiles.Count && newIndex != -1)
            {
                var newDevice = DeviceManager.Instance.DeviceProfiles[newIndex];
                newDevice.Core.SetVolumeAsync(newValue);
                
            }
            
        }
    }
    public class DevcieProfile
    {
        public string Name { get; set; }
        public Guid Uuid { get; set; }
        public double Vol { get; set; }
        [JsonIgnore]
        public CoreAudioDevice Core { get; set; }
    }
    public class AudioDeviceChangedObserver : IObserver<DeviceChangedArgs>
    {
        private readonly Action<DeviceChangedArgs> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        public AudioDeviceChangedObserver(
            Action<DeviceChangedArgs> onNext,
            Action<Exception> onError,
            Action onCompleted
        )
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnCompleted() => _onCompleted?.Invoke();

        public void OnError(Exception error) => _onError?.Invoke(error);

        public void OnNext(DeviceChangedArgs value) => _onNext?.Invoke(value);
    }

    public class VolumeChangedObserver : IObserver<DeviceVolumeChangedArgs>
    {
        private System.Windows.Forms.TrackBar _trackBar; // 假設你有一個 TrackBar 控制項
        private System.Windows.Forms.Label _label; // 假設你有一個 TrackBar 控制項

        public VolumeChangedObserver(System.Windows.Forms.TrackBar trackBar , System.Windows.Forms.Label label)
        {
            _trackBar = trackBar;
            _label = label;
        }

        public void OnCompleted()
        {
            // 在這裡處理觀察者完成的邏輯
        }

        public void OnError(Exception error)
        {
            // 在這裡處理觀察者錯誤的邏輯
        }

        public void OnNext(DeviceVolumeChangedArgs value)
        {
            // 在這裡處理音量變化事件
            Console.WriteLine($"音量變化：新音量 {value.Volume}");

            // 將 TrackBar 的值設置為音量值            
            // 在 UI 線程上執行相應的 UI 操作
            
            if (_trackBar.InvokeRequired)
            {
                _trackBar.Invoke((Action)(() => _trackBar.Value = (int)value.Volume));
            }
            else
            {
                _trackBar.Value = (int)value.Volume;
            }

            if (_label.InvokeRequired)
            {
                _label.Invoke((Action)(() => _label.Text = value.Volume.ToString()));
            }
            else
            {
                _label.Text = value.Volume.ToString();
            }
            
        }
    }




}
