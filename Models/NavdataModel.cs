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
        
    }
}