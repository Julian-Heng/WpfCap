using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace WpfCap
{
    public partial class MainPage : Page
    {
        public static RoutedUICommand CustomRoutedCommand = new RoutedUICommand();
        
        public MainPage()
        {
            InitializeComponent();
            StartAnnotations();
            ViewFile();
        }

        public void ViewFile()
        {
            string fileName = "file-sample_1MB.xps";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\", fileName);
            XpsDocument xpsDoc = new XpsDocument(path, FileAccess.Read);
            
            var docSequence = xpsDoc.GetFixedDocumentSequence();
            var xpsDocRef = docSequence.References.First();

            //get the fixed document to enumerate
            FixedDocument xpsFixedDoc = xpsDocRef.GetDocument(false);

            Viewer.Document = docSequence;
            Viewer.UpdateLayout();
        }

        private void StartAnnotations()
        {
            var _annot = new AnnotationService(Viewer);
            
            if (_annot.IsEnabled)
                _annot.Disable();
            
            var _annotStream = new FileStream("annots.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            
            AnnotationStore _annotStore = new XmlStreamStore(_annotStream);
            _annot.Enable(_annotStore);
        }
        
        public string SelectedText {
            get {
                var selProperty = Viewer.GetType ().GetProperty ("TextSelection", BindingFlags.Instance | BindingFlags.NonPublic);
                var sel = selProperty.GetValue (Viewer, null) as TextSelection;
                return sel?.Text;
            }
        }

        private void ExecutedCustomCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(SelectedText);
        }

        private void CanExecuteCustomCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedText != null;
        }
    }
}