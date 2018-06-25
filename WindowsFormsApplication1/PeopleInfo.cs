using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YjkInspectClient
{
    public class PeopleInfo
    {
        public string cardId { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string age { get; set; }
        public string serialId { get; set; }
        public string addTime { get; set; }

        public void clearInfo()
        {
            cardId = null;
            name = null;
            sex = null;
            age = null;
            addTime = null;
            serialId = null;
        }
    }
}
