using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace CaptionCombiner
{
    /// <summary>
    /// SortingPage.xaml 的交互逻辑
    /// 需要设置AllowDrop为True
    /// </summary>

    public class CaptionSnapshot
    {
        private string _Title;
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        private BitmapImage _ImageData;
        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }
    }

    public partial class SortingPage : Page
    {
        // 把数据类传递给ListBox
        ObservableCollection<CaptionSnapshot> captionSnapshotsList = new ObservableCollection<CaptionSnapshot>();

        // 用于拖拽
        private Point _dragStartPoint;

        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            return FindVisualParent<T>(parentObject);
        }

        public SortingPage(String[] imagePaths)
        {
            InitializeComponent();

            CaptionSnapshot[] captionSnapshotArray = new CaptionSnapshot[imagePaths.Length];
            for (var i = 0; i < imagePaths.Length; i++)
            {
                CaptionSnapshot c = new CaptionSnapshot
                {
                    Title = imagePaths[i],
                    ImageData = new BitmapImage(new Uri(imagePaths[i]))
                };
                captionSnapshotsList.Add(c);
            }

            this.ImageListBox.ItemsSource = captionSnapshotsList;
            this.ImageListBox.PreviewMouseMove += ImageListBox_PreviewMouseMove;

            Style itemContainerStyle = new Style(typeof(ListBoxItem));

            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(ImageListView_MouseLeftButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent,
                new DragEventHandler(ImageListView_Drop)));
            ImageListBox.ItemContainerStyle = itemContainerStyle;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ImageListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var lb = sender as ListBox;
                var lbi = FindVisualParent<ListBoxItem>(((DependencyObject)e.OriginalSource));
                if (lbi != null)
                {
                    DragDrop.DoDragDrop(lbi, lbi.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void ImageListView_Drop(object sender, DragEventArgs e) {
            //CaptionSnapshot droppedData = e.Data.GetData(typeof(CaptionSnapshot)) as CaptionSnapshot;
            //CaptionSnapshot target = ((ListBoxItem)(sender)).DataContext as CaptionSnapshot;

            //int removeIdx = ImageListBox.Items.IndexOf(droppedData);
            //int targetIdx = ImageListBox.Items.IndexOf(target);

            //if (removeIdx < targetIdx)
            //{
            //    this.captionSnapshotsList.Insert(targetIdx + 1, droppedData);
            //    this.captionSnapshotsList.RemoveAt(removeIdx);
            //}
            //else
            //{
            //    int remIdx = removeIdx + 1;
            //    if (this.captionSnapshotsList.Count + 1 > remIdx)
            //    {
            //        this.captionSnapshotsList.Insert(targetIdx, droppedData);
            //        this.captionSnapshotsList.RemoveAt(remIdx);
            //    }
            //}
                if (sender is ListBoxItem)
                {
                    var source = e.Data.GetData(typeof(CaptionSnapshot)) as CaptionSnapshot;
                    var target = ((ListBoxItem)(sender)).DataContext as CaptionSnapshot;

                    int sourceIndex = ImageListBox.Items.IndexOf(source);
                    int targetIndex = ImageListBox.Items.IndexOf(target);

                    Move(source, sourceIndex, targetIndex);
                }
        }

        private void Move(CaptionSnapshot source, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                this.captionSnapshotsList.Insert(targetIndex + 1, source);
                this.captionSnapshotsList.RemoveAt(sourceIndex);
            }
            else
            {
                int removeIndex = sourceIndex + 1;
                if (this.captionSnapshotsList.Count + 1 > removeIndex)
                {
                    this.captionSnapshotsList.Insert(targetIndex, source);
                    this.captionSnapshotsList.RemoveAt(removeIndex);
                }
            }
        }

        private void ImageListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListBoxItem)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        private void ImageListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var lb = sender as ListBox;
                var lbi = FindVisualParent<ListBoxItem>(((DependencyObject)e.OriginalSource));
                if (lbi != null)
                {
                    DragDrop.DoDragDrop(lbi, lbi.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void ImageListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

    }// End of class CaptionSnapshot
}
