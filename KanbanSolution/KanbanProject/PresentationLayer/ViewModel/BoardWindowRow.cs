using KanbanProject.InterfaceLayer.DataObjects;

namespace KanbanProject.PresentationLayer.ViewModel
{
    class BoardWindowRow
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string CreationTime { get; set; }
        public string Duedate { get; set; }

        public BoardWindowRow(InterfaceLayerTask t)
        {
            this.Title = t.Title;
            this.Description = t.Description;
            this.State = t.State;
            this.CreationTime = t.CreationTime;
            this.Duedate = t.DueDate;
        }

    }
}
