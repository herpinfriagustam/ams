using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic
{

    class InitData
    {
        public void setDataStatus()
        {
            List<Tuple<string,string>> listStat = new List<Tuple<string,string>>();
            listStat.Clear();
            listStat.Add(new Tuple<string, string> ( "PRE", "Preparation" ));
            listStat.Add(new Tuple<string, string> ( "RSV", "Reservation"));
            listStat.Add(new Tuple<string, string> ( "NUR", "First Inspection"));
            listStat.Add(new Tuple<string, string> ( "INS", "Inspection"));
            listStat.Add(new Tuple<string, string> ( "OBS", "Observation"));
            listStat.Add(new Tuple<string, string> ( "MED", "Medicine"));
            listStat.Add(new Tuple<string, string> ( "CLS", "Completed"));
            listStat.Add(new Tuple<string, string> ( "CAN", "Cancel"));
        }

        public static Tuple<string, int, bool>[] getTuple()
        {
            // Create a new tuple.
            Tuple<string, int, bool>[] aTuple =
            {
                new Tuple<string, int, bool>("One", 1, true),
                new Tuple<string, int, bool>("Two", 2, false),
                new Tuple<string, int, bool>("Three", 3, true)
            };

            // Return a list of values using the tuple.
            return aTuple;
        }
    }
}
