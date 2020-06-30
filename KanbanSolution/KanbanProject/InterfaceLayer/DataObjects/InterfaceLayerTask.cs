namespace KanbanProject.InterfaceLayer.DataObjects
{
    public class InterfaceLayerTask
    {
        public string CreationTime { get; private set; }
        public string DueDate { get; private set; }
        public string Description { get; private set; }
        public string Title { get; private set; }
        public string State { get; private set; }

        public InterfaceLayerTask(string creationTime, string dueDate, string description, string title, string state)
        {
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Description = description;
            this.Title = title;
            this.State = state;
        }

    }
}
