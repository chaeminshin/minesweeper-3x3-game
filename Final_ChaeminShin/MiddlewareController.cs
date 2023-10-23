using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using BackEndN;

namespace MiddlewareControllerN
{
    class MiddlewareController
    {
        BackEnd backEnd = new BackEnd();

        public List<Hashtable> mineList { get; set; }

        public List<Hashtable> MinesForSave { get; set; }

        public List<Hashtable> loadGameList { get; set; }

        public void CallSaveMinefieldXml()
        {
            backEnd.SaveMineFieldXML(this.MinesForSave);
        }

        public void CallLoadMinefieldXml()
        {
            new List<Hashtable>();
            this.mineList = backEnd.LoadMineFieldXML();
        }

        public void CallSaveGameXml(Hashtable ht)
        {
            backEnd.GameXML(ht);
        }

        public void CallLoadGameXml()
        {
            new List<Hashtable>();
            this.loadGameList = backEnd.LoadGameXML();
        }
    }
}
