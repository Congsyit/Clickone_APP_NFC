using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_Reader_WPF
{
    class PUT1Bien
    {
        public class Tag
        {
            public string effect { get; set; }
            public string _id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
            public string account { get; set; }
            public string group { get; set; }
            public string icon { get; set; }
        }

        public class Recall
        {
            public List<string> tags { get; set; }
            public List<object> users { get; set; }
            public DateTime date { get; set; }
            public bool? skip { get; set; }
        }

        public class Tag2
        {
            public string effect { get; set; }
            public string _id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
            public string account { get; set; }
            public string group { get; set; }
            public string icon { get; set; }
        }

        public class User
        {
            public bool admin { get; set; }
            public string _id { get; set; }
            public DateTime updatedAt { get; set; }
            public DateTime createdAt { get; set; }
            public string name { get; set; }
            public string color { get; set; }
            public string username { get; set; }
            public string account { get; set; }
            public string group { get; set; }
            public bool hidden { get; set; }
            public string kana { get; set; }
            public string dentistCode { get; set; }
            public bool nopass { get; set; }
        }

        public class Card
        {
            public Recall recall { get; set; }
            public string gender { get; set; }
            public double risk { get; set; }
            public List<Tag2> tags { get; set; }
            public List<User> users { get; set; }
            public List<string> relations { get; set; }
            public string _id { get; set; }
            public string account { get; set; }
            public string cid { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public string name { get; set; }
            public string kana { get; set; }
            public string memo { get; set; }
            public string email { get; set; }
            public string birthday { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
            public object burdenRate { get; set; }
            public string insuranceNo { get; set; }
            public string insuredClass { get; set; }
            public string insuredCode { get; set; }
            public object insuredConfirmationDate { get; set; }
            public object insuredDate { get; set; }
            public string insuredPersonName { get; set; }
            public string insuredPersonNo { get; set; }
            public object insuredTimeLimit { get; set; }
            public string insuredType { get; set; }
            public string postcode { get; set; }
            public string publicExpenditureNo1 { get; set; }
            public string publicExpenditureNo2 { get; set; }
            public string publicExpenditurePensionerNo1 { get; set; }
            public string publicExpenditurePensionerNo2 { get; set; }
            public bool? needConfirmation { get; set; }
        }

        public class Logs
        {
            public DateTime waiting { get; set; }
            public DateTime consulting { get; set; }
            public DateTime accounting { get; set; }
            public DateTime done { get; set; }
            public DateTime none { get; set; }
        }

        public class MyArray
        {
            public List<object> users { get; set; }
            public List<Tag> tags { get; set; }
            public string note { get; set; }
            public string _id { get; set; }
            public string line { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public string type { get; set; }
            public string status { get; set; }
            public Card card { get; set; }
            public string account { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public int __v { get; set; }
            public Logs logs { get; set; }
            public int? rating { get; set; }
        }

        public class Root
        {
            public List<MyArray> MyArray { get; set; }
        }
    }
}
