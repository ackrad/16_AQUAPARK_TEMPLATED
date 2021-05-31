using System;
using System.Collections.Generic;

namespace ElephantSDK
{
    [Serializable]
    public class VersionData
    {
        public string appVersion = "";
        public string sdkVersion = "";
        public string osVersion = "";
        public string adsSdkVersion = "";
        public string mopubVersion = "";
        public string unityVersion = "";
        public List<MopubNetworkData> mopubNetworkData;
        
        public VersionData(string appVersion, string sdkVersion, string osVersion,
            string adsSdkVersion, string mopubVersion, string unityVersion,
            List<MopubNetworkData> mopubNetworkData)
        {
            this.appVersion = appVersion;
            this.sdkVersion = sdkVersion;
            this.osVersion = osVersion;
            this.adsSdkVersion = adsSdkVersion;
            this.mopubVersion = mopubVersion;
            this.unityVersion = unityVersion;
            this.mopubNetworkData = mopubNetworkData;
        }
        
        [Serializable]
        public class MopubNetworkData
        {
            public string name = "";
            public string version = "";
        
            public MopubNetworkData(string name, string version)
            {
                this.name = name;
                this.version = version;
            }
        }
    }
    
}