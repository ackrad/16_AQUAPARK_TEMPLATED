using System;
using System.Collections.Generic;

namespace ElephantSdkManager.Model
{
    [Serializable]
    public class Manifest
    {
        public List<Sdk> sdkList;
    }

    [Serializable]
    public class Sdk
    {
        public string downloadUrl;
        public string sdkName;
        public string version;
        public string type;
        public string currentVersion;
    }
}