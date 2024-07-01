using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomTools.WebTools
{
    public class WebRequest
    {
        public const string MethodGet = "GET";
        public const string MethodPost = "POST";
        public const string MethodDelete = "DELETE";
        public const int Timeout = 10;

        public const string ContentType = "Content-Type";
        public const string ApplicationJson = "application/json";
        public const string Authorization = "Authorization";

        public static UnityWebRequest Create(string url, string method = MethodGet, string json = null, string token = null)
        {
            UnityWebRequest request = new UnityWebRequest(url, method);
            request.SetRequestHeader(ContentType, ApplicationJson);
            
            if(token != null)
                request.SetRequestHeader(Authorization, token);

            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = Timeout;

            if (method != MethodGet && !string.IsNullOrEmpty(json))
            {
                UploadHandler uploader = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                uploader.contentType = ApplicationJson;
                request.uploadHandler = uploader;
            }

            return request;
        }

        public static WebResponse GetResponse(UnityWebRequest request)
        {
            WebResponse response = new WebResponse();
            response.success = request.responseCode >= 200 && request.responseCode < 300;
            response.status = request.responseCode;
            response.error = request.error;
            response.data = "";

            if (request.downloadHandler != null)
                response.data = request.downloadHandler.text;

            return response;
        }

        public static async Task<WebResponse> SendRequest(UnityWebRequest request)
        {
            int wait = 0;
            int delayMilliseconds = 200;
            int waitMax = request.timeout * 1000;
            request.timeout += 1; //increase the timeout to make sure we're aborting first
            
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Delay(delayMilliseconds);
                wait += delayMilliseconds;
                
                if(wait >= waitMax)
                    request.Abort();
            }

            WebResponse response = GetResponse(request);
            response.error = GetError(response);
            request.Dispose();
            return response;
        }

        private static string GetError(WebResponse response)
        {
            if (response.success)
                return string.Empty;

            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(response.data);

            if (errorResponse != null)
                return errorResponse.error;

            return response.error;
        }
    }
}