using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using EFB.Sessions;

namespace EFB.Models
{
    public class NavdataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Frequency { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        

        public NavdataModel(int id, string name, string type, string latitude, string longitude){
            Id = id;
            Name = name;
            Type = type;
            Frequency = null;
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonConstructor]
        public NavdataModel(int id, string name, string type, int? frequency, string latitude, string longitude){
            Id = id;
            Name = name;
            Type = type;
            Frequency = frequency;
            Latitude = latitude;
            Longitude = longitude;
        }

        public static async Task<NavdataModel[]> Populate(){
            MySqlConnection con = new MySqlConnection("server=server.luke-else.co.uk;userid=root;password=;database=EFB");
            con.Open();

            // Console.WriteLine($"MySQL version : {con.ServerVersion}");

            string query = "SELECT * FROM waypoints";
            MySqlCommand command = new MySqlCommand(query, con);

            MySqlDataReader reader = (MySqlDataReader) await command.ExecuteReaderAsync();

            List<NavdataModel> navdata = new List<NavdataModel>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string type = reader.GetString(2);
                string latitude = reader.GetString(4);
                string longitude = reader.GetString(5);

                if (type == "VOR" || type == "NDB")
                {
                    // int? frequency = reader.GetInt32(3);
                    int? frequency = null;
                    navdata.Add(
                        new NavdataModel(id, name, type, frequency, latitude, longitude)
                    );
                }else{
                    navdata.Add(
                        new NavdataModel(id, name, type, latitude, longitude)
                    );
                }
            }

            return navdata.ToArray<NavdataModel>();
        }

        public static NavdataModel BinarySearch(ref NavdataModel[] data, int start, int end, string target){
            int midpoint = start + ((end - start) / 2);
            target = target.ToUpper().Trim();

            string mid = data[midpoint].Name;

            if (start == end-1)
            {
                if (mid == target)
                {
                    return data[midpoint];
                }
                return null;
            }

            if (String.Compare(target, mid) < 0)
            {
                return BinarySearch(ref data, start, midpoint, target);
            }
            return BinarySearch(ref data, midpoint, end, target);
        }

        public static NavdataModel[] MergeSort(ref NavdataModel[] data, int start, int end)
        {
            if (start == end)
            {//If we have narrowed it down to a single Item
                return new NavdataModel[] { data[start] };
            }

            int midpoint = start + ((end - start) / 2);

            //Split the data down to the left and the right portions
            NavdataModel[] left = MergeSort(ref data, start, midpoint);
            NavdataModel[] right = MergeSort(ref data, midpoint+1, end);

            List<NavdataModel> combined = new List<NavdataModel>();

            int leftPointer = 0;
            int rightPointer = 0;

            while (leftPointer <= left.Length-1 || rightPointer <= right.Length-1)
            {
                if (leftPointer == left.Length)
                {//Take a value only from the right (left pointer had reached the end)
                    AddValue(ref combined, right[rightPointer], ref rightPointer);
                }else if (rightPointer == right.Length)
                {//Take a value only from the left (right pointer has reached the end)
                    AddValue(ref combined, left[leftPointer], ref leftPointer);
                }else{
                    if (String.Compare(left[leftPointer].Name, right[rightPointer].Name) <= 0)
                    {//Take a value from the left hand side of the list. (Left value is considered 'smaller')
                        AddValue(ref combined, left[leftPointer], ref leftPointer);
                    }else{//Take a value from the right (right value is considered smaller)
                        AddValue(ref combined, right[rightPointer], ref rightPointer);
                    }
                }
            }

            return combined.ToArray();

        }

        private static void AddValue(ref List<NavdataModel> data, NavdataModel value, ref int pointer){
            pointer++;
            data.Add(value);
        }
        
    }
}