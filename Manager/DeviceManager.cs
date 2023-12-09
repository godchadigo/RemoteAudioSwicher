using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAudio.Manager
{
    public class DeviceManager
    {
        public static DeviceManager Instance { get; private set; } = new DeviceManager();
        public List<DevcieProfile> DeviceProfiles = new List<DevcieProfile>();
        public CoreAudioController AudioCore = new CoreAudioController();        
        public DeviceManager()
        {
            DeviceProfiles = new List<DevcieProfile>();
            AudioCore = new CoreAudioController();
        }

    }
}
