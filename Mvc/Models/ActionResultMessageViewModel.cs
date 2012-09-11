namespace Customer.Project.Mvc.Models
{
    public class ActionResultMessageViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string BackLinkTitle { get; set; }
        public string ReturnUrl { get; set; }
        public bool ShowBackButton { get; set; }

        public ActionResultMessageViewModel()
        {
            BackLinkTitle = "Back";
            ShowBackButton = true;
        }
        public ActionResultMessageViewModel(string message)
            : this()
        {
            Message = message;
        }
    }
}