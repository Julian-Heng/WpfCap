using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
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
        public static readonly RoutedUICommand DebugSelectedTextCommand = new RoutedUICommand();
        
        public MainPage()
        {
            InitializeComponent();
            StartAnnotations();
            ViewFile();
        }

        public void ViewFile()
        {
            var fileName = "file-sample_1MB.xps";
            var path = Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\", fileName);
            var xpsDoc = new XpsDocument(path, FileAccess.Read);
            var docSequence = xpsDoc.GetFixedDocumentSequence();
            var xpsDocRef = docSequence?.References.First();

            // Get the fixed document to enumerate
            var xpsFixedDoc = xpsDocRef?.GetDocument(false);

            Viewer.Document = docSequence;
            Viewer.UpdateLayout();
        }

        private void StartAnnotations()
        {
            var annotate = new AnnotationService(Viewer);
            
            if (annotate.IsEnabled)
                annotate.Disable();
            
            var annotateStream = new FileStream("annots.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            
            AnnotationStore annotateStore = new XmlStreamStore(annotateStream);
            annotate.Enable(annotateStore);
        }
        
        private string SelectedText =>
            (Viewer.GetType().GetProperty("TextSelection", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(Viewer, null) as
                TextSelection)?.Text;
        private void ExecutedDebugSelectedText(object sender, ExecutedRoutedEventArgs e) => Trace.WriteLine(SelectedText);
        private void CanExecuteDebugSelectedText(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ! string.IsNullOrEmpty(SelectedText);
    }
}