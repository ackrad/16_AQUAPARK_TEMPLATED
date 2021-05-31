using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace ElephantSDK
{
    // TODO implement external package version checks here
    public class VersionCheckUtils
    {
        private static VersionCheckUtils _instance;
        public string AdSdkVersion = "";
        public string MopubVersion = "";
        public string UnityVersion = "";
        public List<VersionData.MopubNetworkData> NetworkVersions;

        public static VersionCheckUtils GetInstance()
        {
            return _instance ?? (_instance = new VersionCheckUtils {AdSdkVersion = GetAdSdkVersion(),
                MopubVersion = GetMopubVersion(), 
                UnityVersion = GetUnityVersion(), 
                NetworkVersions = GetMopubNetworkVersions()});
        }

        private static string GetAdSdkVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var adsSdkVersion = "";

            try
            {
                var type = Array.Find(assembly.GetTypes(),
                    typeToFind =>
                        typeToFind.FullName != null
                        && typeToFind.FullName.Equals("RollicGames.Advertisements.Version"));

                if (type == null) return adsSdkVersion;
                var fieldInfo = type.GetField("SDK_VERSION",
                    BindingFlags.NonPublic | BindingFlags.Static);

                if (fieldInfo == null) return adsSdkVersion;
                adsSdkVersion = fieldInfo.GetValue(null).ToString();

                return adsSdkVersion;
            }
            catch (Exception e)
            {
                return adsSdkVersion;
            }
        }

        private static string GetMopubVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var mopubVersion = "";
            
            try
            {
                var type = Array.Find(assembly.GetTypes(),
                    typeToFind =>
                        typeToFind.FullName != null
                        && typeToFind.FullName.Equals("MoPub"));
                
                if (type == null) return mopubVersion;
                var fieldInfo = type.GetField("MoPubSdkVersion",
                    BindingFlags.Public | BindingFlags.Static);

                if (fieldInfo == null) return mopubVersion;
                mopubVersion = fieldInfo.GetValue(null).ToString();

                return mopubVersion;
            }
            catch (Exception e)
            {
                return mopubVersion;
            }
        }

        private static string GetUnityVersion() => Application.unityVersion;
        
        private static List<VersionData.MopubNetworkData> GetMopubNetworkVersions()
        {
            var mopubNetworkVersions = new List<VersionData.MopubNetworkData>();

            var text = Resources.Load<TextAsset>("MopubNetworkInfo");
            if (text == null) return mopubNetworkVersions;
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text.text);
            
            using (XmlReader reader = new XmlNodeReader(xmlDoc))  
            {  
                while(reader.Read())
                {
                    if((reader.NodeType == XmlNodeType.Element) && (reader.Name == "network"))
                    {
                        if (reader.HasAttributes)
                        {
                            var mopubNetworkData = new VersionData.MopubNetworkData(reader.GetAttribute("name") ?? "",
                                reader.GetAttribute("version") ?? "");
                            mopubNetworkVersions.Add(mopubNetworkData);
                        }
                                         
                    }
                }
            }

            return mopubNetworkVersions;
        }
    }
}