using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System.Text;
using System;

public class WordServiceController : MonoBehaviour {

    private string grant_type = "";
    private string client_id = "";
    private string client_secret = "";

	// Use this for initialization
	void Start () {
       // StartCoroutine(postWord());

    }

    IEnumerator postWWW()
    {
        ///<summary>
        /// Post using WWW class
        /// </summary>
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        //var jsonString = "{\"Id\":2,\"Name\":\"Mary\"}";
        //byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        var jsonString = "{\"Id\":2,\"Name\":\"Mary\"}";
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        WWW wwwPostRequest = new WWW("http://localhost:55376/api/values", byteData, headers);
        yield return wwwPostRequest;
        if (string.IsNullOrEmpty(wwwPostRequest.error))
        {
            Debug.Log("Form upload complete!");
        }
        else
        {
            Debug.Log(wwwPostRequest.error);
        }
    }

    IEnumerator postUnityWebRequest()
    {
        ///<summary>
        /// Post using UnityWebRequest class
        /// </summary>
        var jsonString = "{\"Id\":3,\"Name\":\"Roy\"}";
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:55376/api/values", "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.Send();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
        }
    }

    IEnumerator getWWW()
    {
        // First define the url, this should be a valid url
        string url = "http://localhost:8060/word/services/api/mobile/word/game/getWordsByLevel?level=1";

        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.text);
        }
        else
            Debug.Log(www.error);

    }

    // Update is called once per frame
    void Update () {
		
	}

    /*

   [Serializable]
   public class TokenClassName
   {
       public string access_token;
   }

   private static IEnumerator GetAccessToken(Action<string> result)
   {
       Dictionary<string, string> content = new Dictionary<string, string>();
       //Fill key and value
       content.Add("grant_type", "client_credentials");
       content.Add("client_id", "login-secret");
       content.Add("client_secret", "secretpassword");

       UnityWebRequest www = UnityWebRequest.Post("https://someurl.com//oauth/token", content);
       //Send request
       yield return www.Send();

       if (!www.isError)
       {
           string resultContent = www.downloadHandler.text;
           TokenClassName json = JsonUtility.FromJson<TokenClassName>(resultContent);

           //Return result
           result(json.access_token);
       }
       else
       {
           //Return null
           result("");
       }
   }

   [Serializable]
   public class MeasurementClassName
   {
       public string Measurements;
   }

   private static IEnumerator GetMeasurements(string id, DateTime from, DateTime to, Action<string> result)
   {
       Dictionary<string, string> content = new Dictionary<string, string>();
       //Fill key and value
       content.Add("MeasurePoints", id);
       content.Add("Sampling", "Auto");
       content.Add("From", from.ToString("yyyy-MM-ddTHH:mm:ssZ"));
       content.Add("To", to.ToString("yyyy-MM-ddTHH:mm:ssZ"));
       content.Add("client_secret", "secretpassword");

       UnityWebRequest www = UnityWebRequest.Post("https://someurl.com/api/v2/Measurements", content);

       string token = null;

       yield return GetAccessToken((tokenResult) => { token = tokenResult; });

       www.SetRequestHeader("Authorization", "Bearer " + token);
       www.Send();

       if (!www.isError)
       {
           string resultContent = www.downloadHandler.text;
           MeasurementClassName[] rootArray = JsonHelper.FromJson<MeasurementClassName>(resultContent);

           string measurements = "";
           foreach (MeasurementClassName item in rootArray)
           {
               measurements = item.Measurements;
           }

           //Return result
           result(measurements);
       }
       else
       {
           //Return null
           result("");
       }
   }


      IEnumerator getToken()
      {
          var client = new RestClient("https://service.endpoint.com/api/oauth2/token");
          var request = new RestRequest(Method.POST);
          request.AddHeader("cache-control", "no-cache");
          request.AddHeader("content-type", "application/x-www-form-urlencoded");
          request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=abc&client_secret=123", ParameterType.RequestBody);
          IRestResponse response = client.Execute(request);

          yield return    ;
      }

          IEnumerator postWord()
          {
              ///<summary>
              /// Post using WWW class
              /// </summary>
              Dictionary<string, string> headers = new Dictionary<string, string>();
              headers.Add("Content-Type", "application/json");
              var jsonString = "{\"Id\":2,\"Name\":\"Mary\"}";
              byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());
              WWW wwwPostRequest = new WWW("http://localhost:55376/api/values", byteData, headers);
              yield return wwwPostRequest;
              if (string.IsNullOrEmpty(wwwPostRequest.error))
              {
                  Debug.Log("Form upload complete!");
              }
              else
              {
                  Debug.Log(wwwPostRequest.error);
              }
          }

          IEnumerator postUnityWebRequest()
          {
              ///<summary>
              /// Post using UnityWebRequest class
              /// </summary>
              var jsonString = "{\"Id\":3,\"Name\":\"Roy\"}";
              byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

              UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:55376/api/values", "POST");
              unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
              unityWebRequest.SetRequestHeader("Content-Type", "application/json");
              yield return unityWebRequest.Send();

              if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
              {
                  Debug.Log(unityWebRequest.error);
              }
              else
              {
                  Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
              }
          }

          IEnumerator getUnityWebRequest()
          {
              UnityWebRequest www = UnityWebRequest.Get("http://localhost:55376/api/values/3");
              yield return www.Send();

              if (www.isNetworkError || www.isHttpError)
              {
                  Debug.Log(www.error);
              }
              else
              {
                  Debug.Log(www.downloadHandler.text);
              }
          }

          */
}
