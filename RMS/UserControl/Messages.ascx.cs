using System;
using RMS.BL.Enums;
namespace RMS.UserControl
{
    public partial class Messages : System.Web.UI.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Div1.Attributes.Add("style", "display:none");
        }
        public void ShowMessage(string message, MessageType messageType)
        {
            Div1.Attributes.Add("style", "display:bock");
            lblInfo.Text = message;
            if (messageType.Equals(MessageType.Error))
            {
                Div1.Attributes.Add("class", "error");

            }
            else
                Div1.Attributes.Add("class", "info");
        }
    }
}