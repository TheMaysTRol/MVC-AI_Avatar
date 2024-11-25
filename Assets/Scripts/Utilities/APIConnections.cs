using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.IO;

namespace KJAvatar
{
    /// <summary>
    /// Static class for handling API JSON connections in Unity using UnityWebRequest.
    /// </summary>
    public static class APIConnections
    {
        #region Public Methods

        /// <summary>
        /// Coroutine for making a GET request with a JSON response to the API.
        /// Calls the success callback with the parsed response or the fail callback with an error message.
        /// </summary>
        /// <typeparam name="T">The type of the response data.</typeparam>
        /// <param name="url">The API URL.</param>
        /// <param name="callbackOnSuccess">Callback to invoke on a successful request, passing the parsed data.</param>
        /// <param name="callbackOnFail">Callback to invoke on failure, passing an error message.</param>
        /// <param name="token">Optional API token for authentication.</param>
        /// <param name="timeout">The timeout in seconds for the request. Default is 20.</param>
        public static IEnumerator ApiGetJson<T>(string url, UnityAction<T> callbackOnSuccess, UnityAction<string> callbackOnFail, string token = "", int timeout = 20)
        {
            var www = UnityWebRequest.Get(url);
            www.timeout = timeout;
            if (!string.IsNullOrEmpty(token))
            {
                string authorizationToken = "Bearer " + token;
                www.SetRequestHeader("Authorization", authorizationToken);
            }
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (IsJsonString(www.downloadHandler.text))
                {
                    Error result = JsonConvert.DeserializeObject<Error>(www.downloadHandler.text);
                    if (result != null)
                    {
                        callbackOnFail?.Invoke(result.error);
                    }
                    else
                    {
                        callbackOnFail?.Invoke("Failed to get result");
                    }
                }
                else
                {
                    callbackOnFail?.Invoke(www.error);
                }
            }
            else
            {
                ParseResponse(www.downloadHandler.text, callbackOnSuccess, callbackOnFail);
            }
        }

        /// <summary>
        /// Coroutine for making a POST request with JSON data to the API.
        /// Sends data to the API and calls the appropriate callback on success or failure.
        /// </summary>
        /// <typeparam name="T">The type of the data to send.</typeparam>
        /// <typeparam name="U">The type of the response data.</typeparam>
        /// <param name="url">The API URL.</param>
        /// <param name="data">The data to send in the request body.</param>
        /// <param name="callbackOnSuccess">Callback to invoke on a successful request, passing the parsed data.</param>
        /// <param name="callbackOnFail">Callback to invoke on failure, passing an error message.</param>
        /// <param name="token">Optional API token for authentication.</param>
        public static IEnumerator ApiPostJson<T, U>(string url, T data, UnityAction<U> callbackOnSuccess, UnityAction<string> callbackOnFail, string token = "")
        {
            string dataJson = JsonConvert.SerializeObject(data);
            var www = new UnityWebRequest(url, "POST");
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("accept", "text/plain");

            if (!string.IsNullOrEmpty(token))
            {
                string authorizationToken = "Bearer " + token;
                www.SetRequestHeader("Authorization", authorizationToken);
            }

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(dataJson);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.uploadHandler.contentType = "application/json";

            var cert = new ForceAcceptAll();
            www.certificateHandler = cert;

            yield return www.SendWebRequest();
            cert?.Dispose();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (IsJsonString(www.downloadHandler.text))
                {
                    Error result = JsonConvert.DeserializeObject<Error>(www.downloadHandler.text);
                    callbackOnFail?.Invoke(result.error);
                }
                else
                {
                    callbackOnFail?.Invoke(www.error);
                }
            }
            else
            {
                ParseResponse(www.downloadHandler.text, callbackOnSuccess, callbackOnFail);
            }
        }

        /// <summary>
        /// Asynchronously makes a GET request with JSON response and returns the parsed response data.
        /// </summary>
        /// <typeparam name="T">The type of the response data.</typeparam>
        /// <param name="url">The API URL.</param>
        /// <param name="token">Optional API token for authentication.</param>
        /// <param name="timeout">Timeout in seconds. Default is 20.</param>
        /// <returns>Parsed response data of type T.</returns>
        public static async Task<T> ApiGetJsonAsync<T>(string url, string token = "", int timeout = 20)
        {
            using (var www = UnityWebRequest.Get(url))
            {
                www.timeout = timeout;
                if (!string.IsNullOrEmpty(token))
                {
                    string authorizationToken = "Bearer " + token;
                    www.SetRequestHeader("Authorization", authorizationToken);
                }

                try
                {
                    await www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    {
                        if (IsJsonString(www.downloadHandler.text))
                        {
                            Error result = JsonConvert.DeserializeObject<Error>(www.downloadHandler.text);
                            throw new Exception(result?.error ?? "Failed to get result");
                        }
                        throw new Exception(www.error);
                    }

                    return await ParseResponseAsync<T>(www.downloadHandler.text);
                }
                catch (Exception e)
                {
                    throw new Exception($"API request failed: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Asynchronously makes a POST request with JSON data to the API and returns the parsed response data.
        /// </summary>
        /// <typeparam name="T">The type of the data to send.</typeparam>
        /// <typeparam name="U">The type of the response data.</typeparam>
        /// <param name="url">The API URL.</param>
        /// <param name="data">The data to send in the request body.</param>
        /// <param name="token">Optional API token for authentication.</param>
        /// <returns>Parsed response data of type U.</returns>
        public static async Task<U> ApiPostJsonAsync<T, U>(string url, T data, string token = "")
        {
            string dataJson = JsonConvert.SerializeObject(data);
            using (var www = new UnityWebRequest(url, "POST"))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("accept", "text/plain");

                if (!string.IsNullOrEmpty(token))
                {
                    string authorizationToken = "Bearer " + token;
                    www.SetRequestHeader("Authorization", authorizationToken);
                }

                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(dataJson);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.uploadHandler.contentType = "application/json";

                using (var cert = new ForceAcceptAll())
                {
                    www.certificateHandler = cert;

                    try
                    {
                        await www.SendWebRequest();

                        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                        {
                            if (IsJsonString(www.downloadHandler.text))
                            {
                                Error result = JsonConvert.DeserializeObject<Error>(www.downloadHandler.text);
                                throw new Exception(result?.error ?? "Failed to get result");
                            }
                            throw new Exception(www.error);
                        }

                        return await ParseResponseAsync<U>(www.downloadHandler.text);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"API request failed: {e.Message}");
                    }
                }
            }
        }

        public static async Task<AudioClip> GetAudioAsync<T>(string url, T data, string token = "", string userId = "")
        {
            string dataJson = JsonConvert.SerializeObject(data);
            using (var www = new UnityWebRequest(url, "POST"))
            {
                www.SetRequestHeader("Content-Type", "application/json");

                if (!string.IsNullOrEmpty(token))
                {
                    string authorizationToken = "Bearer " + token;
                    www.SetRequestHeader("Authorization", authorizationToken);
                }

                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(dataJson);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);
                www.SetRequestHeader("content-type", "application/json");
                www.SetRequestHeader("X-USER-ID", userId);

                using (var cert = new ForceAcceptAll())
                {
                    www.certificateHandler = cert;

                    try
                    {
                        await www.SendWebRequest();

                        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                        {
                            throw new Exception(www.error);
                        }
                        AudioClip audioclip = DownloadHandlerAudioClip.GetContent(www);
                        return audioclip;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"API request failed: {e.Message}");
                    }
                }
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Parses the API response data and invokes the appropriate callback.
        /// </summary>
        private static void ParseResponse<T>(string data, UnityAction<T> callbackOnSuccess, UnityAction<string> callbackOnFail)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var parsedData = JsonConvert.DeserializeObject<T>(data, settings);
                callbackOnSuccess?.Invoke(parsedData);
            }
            catch (JsonSerializationException e)
            {
                Logger.Log(e.Message, Logger.LogLevel.Error);
                callbackOnFail?.Invoke("Failed to parse data " + e.Message);
            }
        }

        /// <summary>
        /// Asynchronously parses the API response data and returns the result.
        /// </summary>
        private static async Task<T> ParseResponseAsync<T>(string data)
        {
            return await Task.Run(() =>
            {
                try
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return JsonConvert.DeserializeObject<T>(data, settings);
                }
                catch (JsonSerializationException e)
                {
                    Logger.Log(e.Message, Logger.LogLevel.Error);
                    throw new Exception($"Failed to parse data: {e.Message}");
                }
            });
        }

        /// <summary>
        /// Checks if the given string is a valid JSON.
        /// </summary>
        private static bool IsJsonString(string jsonString)
        {
            try
            {
                object deserializedObject = JsonUtility.FromJson(jsonString, typeof(object));
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Error response class
    /// </summary>
    public class Error
    {
        public string error { get; set; }
    }

    /// <summary>
    /// Certificate handler that accepts all certificates
    /// </summary>
    public class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
