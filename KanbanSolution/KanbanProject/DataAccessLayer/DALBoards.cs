using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProject.DataAccessLayer
{
    public class DALBoards
    {
        private string email;
        private Dictionary<string, BoardData> boards;

        public DALBoards(string email)
        {
            this.email = email;
            this.boards = new Dictionary<string, BoardData>();
        }

        public Dictionary<string,BoardData> GetBoards()
        {
            return this.boards;
        }

        public void AddBoard(string boardName)
        {
            BoardData newBD = new BoardData();
            this.boards.Add(boardName, newBD);
        }
        

     }
}
