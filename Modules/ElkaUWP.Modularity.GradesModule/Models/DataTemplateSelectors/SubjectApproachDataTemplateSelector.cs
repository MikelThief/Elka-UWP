using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.DataLayer.Propertiary.Converters;
using ElkaUWP.DataLayer.Propertiary.Entities;

namespace ElkaUWP.Modularity.GradesModule.Models.DataTemplateSelectors
{
    public class SubjectApproachDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InProgressSubjectApproachDataTemplate { get; set; }
        public DataTemplate FinishedSubjectApproachDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);

            var castedItem = item as SubjectApproach;

            var subjectApproachesList = itemsControl.ItemsSource as IEnumerable<SubjectApproach>;

            return IsHighestSemester(subjectApproaches: subjectApproachesList.ToList(), item: castedItem) ? InProgressSubjectApproachDataTemplate : FinishedSubjectApproachDataTemplate;
        }

        private bool IsHighestSemester(List<SubjectApproach> subjectApproaches, SubjectApproach item)
        {
            var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();
            short highestSemester = default;
            foreach (var subjectApproach in subjectApproaches)
            {
                var literalAsShort = semesterLiteralAndShortConverter.LiteralToShort(semesterLiteral: subjectApproach.SemesterLiteral);
                if (literalAsShort > highestSemester)
                    highestSemester = literalAsShort;
            }

            return highestSemester == semesterLiteralAndShortConverter.LiteralToShort(semesterLiteral: item.SemesterLiteral);
        }
    }
}
