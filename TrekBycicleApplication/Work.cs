using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace TrekBycicleApplication
{
    /// <summary>
    ///     Brian Shaw 2017
    ///     Trek Code Example
    /// </summary>
    class Work
    {
        public List<Models.FamilyBikes> distinctListOfFamilies { get; set; }

        // reach out to the url and pick the JSON object that lives there.
        public string GetJSONStringData()
        {
            var trekJsonString = string.Empty;

            var endpoint = @"https://trekhiringassignments.blob.core.windows.net/interview/bikes.json";

            WebRequest trekWebRequest = WebRequest.Create(endpoint);

            using (var trekWebResponse = (HttpWebResponse)trekWebRequest.GetResponse())
            {
                using (var dataStream = trekWebResponse.GetResponseStream())
                {
                    using (var dataStreamReader = new StreamReader(dataStream))
                    {
                        trekJsonString = dataStreamReader.ReadToEnd();
                    }
                }
            }

            return trekJsonString;
        }

        public List<Models.FamilyBikes> ParseJSONStringToArray(string input)
        {
            var trekBikesJArray = JsonConvert.DeserializeObject<JArray>(input);

            var surveyFamilyBikes = new List<Models.FamilyBikes>();

            foreach (JObject item in trekBikesJArray)
            {
                var familyBike = new Models.FamilyBikes
                {
                    bikes = new List<string>(),
                    PopularityCount = 0
                };

                var jsonBikes = item.SelectTokens("bikes");

                foreach (var ee in jsonBikes.Children())
                {
                    familyBike.bikes.Add(ee.Value<string>());
                }

                familyBike.bikes = familyBike.bikes.OrderBy(x => x).ToList();

                surveyFamilyBikes.Add(familyBike);

            }

            return surveyFamilyBikes;
        }

        public List<Models.FamilyBikes> DetermineTopTwentyMostPopularBikeCombinations(List<Models.FamilyBikes> families)
        {
            distinctListOfFamilies = new List<Models.FamilyBikes>();            

            foreach (var item in families)
            {
                if (DoesTheFamilyExist(item) == false)
                {
                    distinctListOfFamilies.Add(item);
                }
            }

            foreach (var distinct in distinctListOfFamilies)
            {
                for (int i = 0; i < families.Count(); i++)
                {
                    if (families[i].bikes.Count() == distinct.bikes.Count())
                    {
                        var minicounter = 0;

                        for (int b = 0; b < families[i].bikes.Count(); b++)
                        {
                            if (families[i].bikes.Exists(x => x == distinct.bikes[b]))
                            {
                                minicounter++;
                            }
                        }

                        if (minicounter == families[i].bikes.Count())
                        {
                            distinct.PopularityCount++;
                        }

                    }
                }
            }

            distinctListOfFamilies = distinctListOfFamilies.OrderByDescending(x => x.PopularityCount).ToList();

            return distinctListOfFamilies;
        }

        public bool DoesTheFamilyExist(Models.FamilyBikes oneFamily)
        {
            var exists = false;

            // If the distinct list has no items, add the oneFamily item being passed into the method.
            if (distinctListOfFamilies.Count() == 0)
            {
                Models.FamilyBikes newby = new Models.FamilyBikes { bikes = new List<string>(), PopularityCount = 0 };
                newby.bikes.AddRange(oneFamily.bikes);
                distinctListOfFamilies.Add(newby);                
            }

            foreach (var distinctFamily in distinctListOfFamilies)
            {
                // A counter to keep track of how many bikes are found between the distinct list and the oneFamily item.
                var count = 0;

                foreach (var distinctBike in distinctFamily.bikes)
                {
                    if (oneFamily.bikes.Exists(x => x == distinctBike))
                    {
                        count++;
                    }

                }
                if (count == oneFamily.bikes.Count())
                {
                    exists = true;
                    break;
                }
                else if (count == 0)
                {
                    exists = false;
                }
            }
            return exists;

        }
    }
}
