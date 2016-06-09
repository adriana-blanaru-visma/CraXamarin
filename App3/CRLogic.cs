using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using Nancy.Json;

namespace App3
{
    public class CRLogic
    {
        public static bool InsertCR(string user, string manager, string product, decimal price)
        {
            return PostCR(user, manager, product, price);
        }

        public static List<CostRequest> GetAllCRs()
        {
            var request = WebRequest.Create("http://10.11.1.68:3579/cra/api/cr");
            ((HttpWebRequest)request).Accept = "application/json";
            request.Method = "GET";
            var httpResponse = (HttpWebResponse)request.GetResponse();
            var respStream = httpResponse.GetResponseStream();
            if (respStream != null)
            {
                using (var streamReader = new StreamReader(respStream))
                {
                    var result = streamReader.ReadToEnd();
                    var actualBody = new JavaScriptSerializer().Deserialize<CostRequest[]>(result);
                    if (actualBody != null && actualBody.Length > 0)
                        return actualBody.ToList();
                }
            }
            return null;
        }

        private static bool PostCR(string user, string manager, string product, decimal price)
        {
            var request = WebRequest.Create("http://10.11.1.68:3579/cra/api/cr");
            request.ContentType = "application/json";
            ((HttpWebRequest)request).Accept = "application/json";
            request.Headers.Add("Authorization", "TenantId 96444A56-C549-4911-A437-97A1C6E0300A");
            request.Method = "POST";
            var newCr = new CostRequest(user, manager, product, price);
            var arr = new[] { newCr };
            var jsonObj = new JavaScriptSerializer().Serialize(arr);
            using (var streamWriter = new StreamWriter(((HttpWebRequest)request).GetRequestStream()))
            {
                streamWriter.Write(jsonObj);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();
            var respStream = httpResponse.GetResponseStream();
            if (respStream != null)
            {
                using (var streamReader = new StreamReader(respStream))
                {
                    var result = streamReader.ReadToEnd();
                    var actualBody = new JavaScriptSerializer().Deserialize<CostRequest[]>(result);
                    if (actualBody != null && actualBody.Length > 0)
                        return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class CostRequest
    {
        public string UserName { get; set; }
        public string ManagerName { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public CostRequest()
        { }
        public CostRequest(string user, string manager, string product, decimal price)
        {
            UserName = user;
            ManagerName = manager;
            ProductName = product;
            ProductPrice = price;
        }
    }
}