using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.Models;
using System;
using System.Collections.Generic;

namespace TourPlanner.BusinessLayer
{
    public static class APIRequest
    {
        public static async Task<TourAPIData?> RequestDirection(string from, string to, string transportType)
        {
            HttpClient client = new HttpClient();

            var apikey = ConfigurationManager.AppSettings["MapquestAPIKey"];

            string transportTypeOnUrl;
            switch (transportType)
            {
                case "Car":
                    transportTypeOnUrl = "fastest"; 
                    break;
                case "Foot":
                    transportTypeOnUrl = "pedestrian"; 
                    break;
                case "Bicycle":
                    transportTypeOnUrl = "bicycle";
                    break;
                default:
                    transportTypeOnUrl = "fastest";

                    break;
            }

            HttpResponseMessage response = await client.GetAsync(
                $"http://www.mapquestapi.com/directions/v2/route?" +
                $"&key={apikey}" +
                $"&unit=k" +
                $"&routeType={transportTypeOnUrl}" +
                $"&from={from}" +
                $"&to={to}");

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);

            int statusCode = myDeserializedClass.info.statuscode;
            List<object> messages = myDeserializedClass.info.messages;
            double distance = myDeserializedClass.route.distance;
            int time = myDeserializedClass.route.time;

            return new TourAPIData(statusCode, messages, distance, time);
        }
    }
}
