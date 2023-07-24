using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace RMS
{
    public class GridViewTemplate : ITemplate
    {

        //A variable to hold the type of ListItemType.
        ListItemType _templateType;

        //A variable to hold the column name.
        string _columnName;

        string _type;



        //Constructor where we define the template type and column name.

        public GridViewTemplate(ListItemType type, string colname, string colType)
        {
            _templateType = type;
            _columnName = colname;
            _type = colType;

        }



        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {

            switch (_templateType)
            {

                case ListItemType.Header:

                    //Creates a new label control and add it to the container.

                    Label lbl = new Label();            //Allocates the new label object.

                    lbl.Text = _columnName;             //Assigns the name of the column in the lable.

                    container.Controls.Add(lbl);        //Adds the newly created label control to the container.

                    break;



                case ListItemType.Item:

                    //Creates a new text box control and add it to the container.

                    //TextBox tb1 = new TextBox();                            //Allocates the new text box object.
                    //tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                    //tb1.Columns = 4;                                        //Creates a column with size 4.
                    //container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                    //break;
                    if (_type.Equals("ArtId"))
                    {
                        HiddenField hdnField = new HiddenField();
                        hdnField.ID = "hdn" + _columnName;
                        hdnField.DataBinding += new EventHandler(hdn_DataBinding);
                        container.Controls.Add(hdnField);

                        Label lblCol = new Label();
                        lblCol.ID = "lbl" + _columnName;
                        lblCol.DataBinding += new EventHandler(lbl_DataBinding);
                        //lblCol.Text = _columnName;             //Assigns the name of the column in the lable.

                        container.Controls.Add(lblCol);        //Adds the newly created label control to the container.

                    }
                    else
                    {
                        RadioButton rdoButton = new RadioButton();
                        rdoButton.ID = "rdo" + _columnName;
                        rdoButton.GroupName = "rdo";
                        // if (_columnName.Equals(Convert.ToString(0)))
                        //     rdoButton.Checked = true;
                        rdoButton.DataBinding += new EventHandler(rdo_DataBinding);
                        container.Controls.Add(rdoButton);
                    }
                    break;

                case ListItemType.EditItem:

                    break;

                case ListItemType.Footer:

                    //As, I am not using any EditItem, I didnot added any code here.

                    break;

            }

        }

        #region Binding Events
        
        void rdo_DataBinding(object sender, EventArgs e)
        {

            RadioButton rdoB = (RadioButton)sender;
            GridViewRow container = (GridViewRow)rdoB.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            //if (dataValue != DBNull.Value)
            //{
            //    rdoB.Text = dataValue.ToString();

            //}

        }

        void hdn_DataBinding(object sender, EventArgs e)
        {

            HiddenField hdn = (HiddenField)sender;
            GridViewRow container = (GridViewRow)hdn.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _type);
            if (dataValue != DBNull.Value)
            {
                hdn.Value = dataValue.ToString();

            }

        }

        void lbl_DataBinding(object sender, EventArgs e)
        {

            Label lbl = (Label)sender;
            GridViewRow container = (GridViewRow)lbl.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            if (dataValue != DBNull.Value)
            {
                lbl.Text = dataValue.ToString();

            }

        }

        #endregion
    }
}
