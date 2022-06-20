using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer.TourAttributes;
using TourPlanner.Models;
using System;
using System.Collections.Generic;
using TourPlanner.BusinessLayer.Exceptions;

namespace TourPlanner.BusinessLayer.APIRequest
{
    public static class MapquestAPIRequest
    {
        public static async Task<TourAPIData?> RequestDirection(string from, string to, string transportType)
        {

            var apiKey = ConfigurationManager.AppSettings["MapquestAPIKey"];
            var mapQuestAPIDirectionLink = ConfigurationManager.AppSettings["MapQuestAPIDirection"];
            HttpClient client = new HttpClient();

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
            $"{mapQuestAPIDirectionLink}" +
            $"key={apiKey}" +
            $"&unit=k" +
            $"&routeType={transportTypeOnUrl}" +
            $"&from={from}" +
            $"&to={to}");

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Root? myDeserializedClass = JsonConvert.DeserializeObject<Root>(responseBody);

            int statusCode = myDeserializedClass.info.statuscode;
            List<object> messages = myDeserializedClass.info.messages;

            if (int.Parse(statusCode.ToString().Substring(0, 1)) >= 4 || messages.Count != 0)
            {
                throw new MapquestAPIErrorException("Mapquest Status Error", statusCode, messages[0].ToString());
            }

            double distance = myDeserializedClass.route.distance;
            int time = myDeserializedClass.route.time;
            if (distance <= 0.0 || time <= 0)
            {
                throw new MapquestAPIInvalidValuesException("Mapquest Invalid Values Error", distance, time);
            }

            return new TourAPIData(statusCode, messages, distance, time);
        }
    }
}
