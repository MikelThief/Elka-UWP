using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace ElkaUWP.Modularity.GradesModule.Models.DataTemplateSelectors
{
    public class PartialGradeNodeTreeViewItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RootNodeTreeViewItemDataTemplate { get; set; }
        public DataTemplate GradeNodeTreeViewItemDataTemplate { get; set; }
        public DataTemplate TaskNodeTreeViewItemDataTemplate { get; set; }

        /// <inheritdoc />
        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is PartialGradeNode node)
            {
                switch (node.Type)
                {
                    case NodeType.Root:
                        return RootNodeTreeViewItemDataTemplate;
                    case NodeType.Folder:
                    case NodeType.Grade:
                        return GradeNodeTreeViewItemDataTemplate;
                    case NodeType.Task:
                        return TaskNodeTreeViewItemDataTemplate;
                    default:
                        throw new ArgumentOutOfRangeException(paramName: "Node.Type");
                }
            }

            throw new ArgumentException(message: "Argument is not " + nameof(PartialGradeNode));
        }
    }
}
