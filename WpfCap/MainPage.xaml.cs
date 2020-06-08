using System.IO;
using System.Linq;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace WpfCap
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            StartAnnotations();
            ViewFile();
        }

        public void ViewFile()
        {
            XpsDocument xpsDoc = new XpsDocument(Constants.INPUTFILE, FileAccess.Read);
            
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
    }
}