using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Siemens.Engineering;
using WpfTiaProject.Model;

namespace WpfTiaProject.ViewModel
{
    public class ProjectInfoViewModel : ViewModelBase
    {
        private string _name;
        private string _creationTime;
        private string _lastChange;
        private string _version;
        private string _comment;
        private string _cpu;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string CreationTime
        {
            get { return _creationTime; }
            set
            {
                _creationTime = value;
            }
        }
        public string LastChange
        {
            get { return _lastChange; }
            set
            {
                _lastChange = value;
            }
        }
        public string Version
        {
            get
            { return _version; }
            set
            {
                _version = value;
            }
        }
        public string Comment
        {
            get
            { return _comment; }
            set
            { _comment = value; }
        }
        public string CPU
        {
            get
            { return _cpu; }
            set
            { _cpu = value; }
        }
        public ProjectInfoViewModel() { }
        public ProjectInfoViewModel(Project project)
        {
            Name = project.Name;
            CreationTime = project.CreationTime.ToLongDateString();
            LastChange = project.LastModified.ToLongDateString();
            Version = project.Version;
            Language englishLanguage = project.LanguageSettings.Languages.Find(new CultureInfo("en-US"));
            MultilingualText comment = project.Comment;
            MultilingualTextItemComposition mltItemComposition = comment.Items;
            MultilingualTextItem englishComment = mltItemComposition.Find(englishLanguage);
            Comment = englishComment.Text;
            CPU = TiaProject.GetCurrentCPUList(project)[0].Name;
        }
    }
}
