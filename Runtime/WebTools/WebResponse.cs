using System;

namespace CustomTools.WebTools
{
    [Serializable]
    public struct WebResponse
    {
        public bool success;
        public long status;
        public string data;
        public string error;
    }
}