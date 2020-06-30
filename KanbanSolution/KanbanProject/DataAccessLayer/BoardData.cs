using System;
using System.Collections.Generic;

namespace KanbanProject.DataAccessLayer
{
    public class BoardData
    {
        
        private Dictionary<int, DALColumn> board;

        //constructor
        public BoardData()
        {
            this.board = new Dictionary<int, DALColumn>();
        }

        public void AddColumn(string columnName)
        {
            this.board.Add(this.board.Count + 1, new DALColumn(columnName));
        }

        public Dictionary<int, DALColumn> getDictionary()
        {
            return this.board;
        }

        public DALColumn GetColumnByName(string columnName)
        {
            foreach (KeyValuePair<int, DALColumn> pair in this.board)
            {
                if (pair.Value.GetTitle().Equals(columnName))
                    return pair.Value;
            }
            return null;
        }


        
        

       
    }
}
