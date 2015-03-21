using NaoCoopApp.Helpers;
using NaoCoopApp.Resources;

namespace NaoCoopApp.ViewModels
{
    public class BaseViewModel : NotificationObject
    {
        private string _viewTitle;
        private string _viewDescription;

        public virtual string ViewTitle
        {
            get
            {
                if (_viewTitle == null)
                {
                    _viewTitle = ViewModelStringResources.ResourceManager.GetString(string.Format("{0}_Title", this.GetType().Name), ViewModelStringResources.Culture);
                }

                return _viewTitle;
            }
        }

        public virtual string ViewDescription
        {
            get
            {
                if (_viewDescription == null)
                {
                    _viewDescription = ViewModelStringResources.ResourceManager.GetString(string.Format("{0}_Description", this.GetType().Name), ViewModelStringResources.Culture);
                }

                return _viewDescription;
            }
        }
    }
}