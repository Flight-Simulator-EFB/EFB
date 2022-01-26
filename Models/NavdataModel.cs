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

        public NavdataModel(int id, string name, int frequency, string latitude, string longitude){
            Id = id;
            Name = name;
            Frequency = frequency;
            Latitude = latitude;
            Longitude = longitude;
        }

        public NavdataModel[] Populate(){
            MySqlConnection con = new MySqlConnection("root:XXXXXXX@XXX.XXX.XXX.XXX:3306/EFB");
            con.Open();
            
            


        }
        
        
        
        
        
        
        
    }
}