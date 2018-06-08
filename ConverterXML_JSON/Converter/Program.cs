using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Converter
{



    [DataContract]
    [Serializable]
    public class Hotel
    {
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public int HotelID { get; set; }
        [DataMember]
        public DateTime FoundedDate { get; set; }
        [DataMember]
        public int TouristCapacity { get; set; }
        [DataMember]
        public double Rating { get; set; }

        public Hotel()
        { }

        public Hotel(string name, int id, DateTime date, int capacity, double rating)
        {
            HotelName = name;
            HotelID = id;
            FoundedDate = date;
            TouristCapacity = capacity;
            Rating = rating;
        }
    }


   
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Input path and name of csv file:");  // hotels.csv
            String path = Console.ReadLine();

            string[] source = File.ReadAllLines(@path);
            string fullsource = "";

            for (int i = 0; i < source.Length; i++)
            {
                fullsource += source[i];
            }

            string[] values = fullsource.Split(';');
            
            int[] id = new int[5];
            int[] capacity = new int[5];
            DateTime[] date = new DateTime[5];
            double[] rating = new double[5];
            List<Hotel> hotelsList = new List<Hotel>();
            int a = 1, b = 3, c = 4, e = 0, d = 2;

            
            for (int i = 0; i < 5; i++)
            {
                id[i] = Convert.ToInt32(values[a]);
                capacity[i] = Convert.ToInt32(values[b]);

                string[] datesplit = values[d].Split('/'); string gooddate="";
                gooddate = datesplit[1] + "." + datesplit[0] + "." + datesplit[2];
                date[i] = DateTime.Parse(gooddate);
                if (values[c].Equals(""))
                    rating[i] = Convert.ToDouble(0);
                else
                    rating[i] = Convert.ToDouble(values[c]);
                Hotel hotel = new Hotel(values[e], id[i], date[i], capacity[i], rating[i]);
                a += 5; b += 5; c += 5; d += 5; e += 5;
                hotelsList.Add(hotel);
            }

            List<Hotel> SortedHotelList = hotelsList.OrderBy(o => o.Rating).ToList();


            Console.WriteLine("JSON or XML?");
            string format = Console.ReadLine();
            if (format.Equals("JSON") || format.Equals("1"))
            {
                
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Hotel[]));

                using (FileStream fs = new FileStream("hotel.json", FileMode.OpenOrCreate))
                {
                    jsonFormatter.WriteObject(fs, SortedHotelList.ToArray());
                }


                Console.WriteLine("File is converted to JSON");
                Console.ReadLine();

            }

            else if (format.Equals("XML") || format.Equals("2"))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Hotel[]));
                
                using (FileStream fs = new FileStream("hotel.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, SortedHotelList.ToArray());
                }

                Console.WriteLine("File is converted to XML");
                Console.ReadLine();
            }
        }
    }
}
