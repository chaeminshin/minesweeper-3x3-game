using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GameLogicN
{
    class GameLogic
    {
        private List<Hashtable> mineList;
        private int specificRow;
        private int specificColumn;

        public GameLogic(List<Hashtable> list)
        {
            this.mineList = list; //has mines from xml
        }

        public void getClickLocation(int row, int column)
        {
            this.specificRow = row;
            this.specificColumn = column;
        }
        public string logic()
        {
            int mineRow = 0;
            int mineColumn = 0;
            int buttonValue = 0;

            foreach (Hashtable ht in mineList)
            {
                mineRow = (int)ht["row"];
                mineColumn = (int)ht["column"];
                
                if (mineRow == this.specificRow && mineColumn == this.specificColumn)
                {
                    return "M";
                }
                else
                {
                    if ((this.specificRow - 1 == mineRow && this.specificColumn == mineColumn)
                        || (this.specificRow + 1 == mineRow && this.specificColumn == mineColumn)
                        || (this.specificColumn - 1 == mineColumn && this.specificRow == mineRow)
                        || (this.specificColumn + 1 == mineColumn && this.specificRow == mineRow)
                        || (this.specificRow - 1 == mineRow && this.specificColumn - 1 == mineColumn)
                        || (this.specificRow + 1 == mineRow && this.specificColumn + 1 == mineColumn)
                        || (this.specificRow - 1 == mineRow && this.specificColumn + 1 == mineColumn)
                        || (this.specificRow + 1 == mineRow && this.specificColumn - 1 == mineColumn)
                        )
                    {
                        buttonValue++;
                    }
                }
            }
            return buttonValue + "";
        }


    }
}
