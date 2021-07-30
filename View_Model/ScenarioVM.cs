using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WroseModel.View_Model
{
    public class ScenarioVM
    {
        public int ID { get; set; }
        public int Project_ID { get; set; }
        public string Project_Name { get; set; }
        public string Scenario_Name { get; set; }
        public int Waste_Tech_ID { get; set; }
        public string Waste_Tech_Name { get; set; }
        public byte Active { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<byte> IsDefault { get; set; }
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public int Waste_Tech_Order { get; set; } 
    }
}