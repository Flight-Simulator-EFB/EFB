using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace EFB.Models
{
    public class NavdataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Frequency { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        

        public NavdataModel(int id, string name, string latitude, string longitude){
            Id = id;
            Name = name;
            Frequency = null;
            Latitude = latitude;
            Longitude = longitude;
        }

        public NavdataModel(int id, string name, int? frequency, string latitude, string longitude){
            Id = id;
            Name = name;
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

                if (reader.GetString(2) == "VOR" || reader.GetString(2) == "NDB")
                {
                    // int? frequency = reader.GetInt32(3);
                    int? frequency = null;
                    navdata.Add(
                        new NavdataModel(id, name, frequency, latitude, longitude)
                    );
                }else{
                    navdata.Add(
                        new NavdataModel(id, name, latitude, longitude)
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
        
    }
}