using System.Collections.Generic;

namespace KanbanProject.InterfaceLayer.DataObjects
{
    public class InterfaceLayerBoard
    {
        public IReadOnlyCollection<InterfaceLayerColumn> Columns { get; private set; }

        public InterfaceLayerBoard(IReadOnlyCollection<InterfaceLayerColumn> columns)
        {
            this.Columns = columns;
        }

    }
}
