using System.Collections.Generic;

namespace KanbanProject.InterfaceLayer.DataObjects
{
    public class InterfaceLayerColumn
    {

        public IReadOnlyCollection<InterfaceLayerTask> Tasks { get; private set; }

        public InterfaceLayerColumn(IReadOnlyCollection<InterfaceLayerTask> tasks)
        {
            this.Tasks = tasks;
        }
    }
}
