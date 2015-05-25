using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApplication1.UC
{
    public class Reason
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
    }


    public class AnswerModel
    {
        public int OrderLineId { get; set; }
        public int ReasonId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int? AnswerId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerValue { get; set; }
    }

    [Serializable]
    public class SOCollectionAnswer
    {
        public virtual int Id { get; set; }

        public virtual int OLIPhysicalItemId { get; set; }
        public virtual int QuestionID { get; set; }
        public virtual int? QuestionOptionID { get; set; }

        public virtual string TextAnswer { get; set; }
    }

    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        public class OrderLine
        {
            public virtual int Id { get; set; }
            public virtual string Description { get; set; }
        }

        [Serializable]
        public class SOCollectionAnswerClient : SOCollectionAnswer
        {
            public virtual bool IsValid  { get; set; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptProductsToBeCollected.DataSource = new List<OrderLine>()
            {
                new OrderLine()
                {
                    Id = 1,
                    Description = "Order Line 1"
                },
                new OrderLine()
                {
                    Id = 2,
                    Description = "Order Line 2"
                }
            };
                rptProductsToBeCollected.DataBind();
            }
            else
            {
                foreach (RepeaterItem i in rptProductsToBeCollected.Items)
                {
                    HiddenField hidQuestionAnswers = (HiddenField)i.FindControl("hidQuestionAnswers");
                    if (hidQuestionAnswers != null)
                    {
                        var answers = new JavaScriptSerializer().Deserialize<List<SOCollectionAnswerClient>>(hidQuestionAnswers.Value);

                        // AddQuestionaryNote(answers);
                    }
                }
            }
        }

        protected void ProductsToBeCollected_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var reasonsToCollectControl = (DropDownList)e.Item.FindControl("ddlCollectionReason");

                reasonsToCollectControl.DataSource = new List<Reason>()
            {
                new Reason()
                {
                    Id = 1,
                    DisplayText = "Reason 1"
                },
                new Reason()
                {
                    Id = 2,
                    DisplayText = "Reason 2"
                }
            };

                reasonsToCollectControl.DataTextField = "DisplayText";
                reasonsToCollectControl.DataValueField = "Id";
                reasonsToCollectControl.DataBind();
                reasonsToCollectControl.Items.Insert(0, new ListItem(" - Select - ", "0", true));
                reasonsToCollectControl.SelectedIndex = 0;

                //PopulateCollectionReasonDropdown(reasonsToCollectControl);
                //var currentItem = (OrderlineItemPhysicalItem)e.Item.DataItem;

                //AssignRepeaterTextBoxValues(e, currentItem);

                //if (reasonsToCollectControl.Items.FindByValue(currentItem.CollectionReasonCodeId.ToString()) != null)
                //{
                //    reasonsToCollectControl.SelectedValue = currentItem.CollectionReasonCodeId.ToString();

                //    if (currentItem.CollectionReasonCodeId > 0)
                //    {
                //        _currentUpliftNumberEntry = e.Item.FindControl("UpliftNumberEntry");
                //        _currentPromptIfApplianceUsedEntry = e.Item.FindControl("PromptUserIfApplianceUsedEntry");
                //        _currentSerialNumberEntry = e.Item.FindControl("SerialNumberEntry");

                //        SelectedCollectionReason = currentItem.CollectionReasonCodeId;

                //        UpdateSelectedCollectionReason();
                //    }
                //}
            }
        }

        public string GetLocalResourceObject(string loc)
        {
            return loc;
        }

        public bool IsValid()
        {
            var isValid = true;
            foreach (RepeaterItem i in rptProductsToBeCollected.Items)
            {
                var ddlCollectionReason = (DropDownList)i.FindControl("ddlCollectionReason");
                var ddlCollectionReasonError = (Label)i.FindControl("lbCollectionReasonError");
                if (ddlCollectionReason != null && ddlCollectionReasonError !=null)
                {
                    ddlCollectionReasonError.Visible = false;
                    if (ddlCollectionReason.SelectedIndex == 0)
                    {
                        ddlCollectionReasonError.Visible = true;
                        isValid = false;
                    }
                }
                
                HiddenField hidQuestionAnswers = (HiddenField)i.FindControl("hidQuestionAnswers");
                if (hidQuestionAnswers != null)
                {
                    var answers = new JavaScriptSerializer().Deserialize<List<SOCollectionAnswerClient>>(hidQuestionAnswers.Value);
                    if (answers != null)
                    {
                        var aa = answers.Cast<SOCollectionAnswer>().ToList();

                        foreach (var ans in answers)
                        {
                            ans.IsValid = !(string.IsNullOrEmpty(ans.TextAnswer) && ans.QuestionOptionID == null);

                            if (!ans.IsValid)
                            {
                                isValid = false;
                            }

                           
                        }

                        hidQuestionAnswers.Value = JsonConvert.SerializeObject(answers);
                    }
                }
            }

            return isValid;
        }


        public void Save()
        {
            //read the values and output them
            foreach (RepeaterItem i in rptProductsToBeCollected.Items)
            {
                HiddenField hidQuestionAnswers = (HiddenField)i.FindControl("hidQuestionAnswers");
                if (hidQuestionAnswers != null)
                {
                    var answers = new JavaScriptSerializer().Deserialize<List<SOCollectionAnswer>>(hidQuestionAnswers.Value);
                   // AddQuestionaryNote(answers);
                }
            }
        }

        private string AddQuestionaryNote(List<AnswerModel> answers)
        {
            var sb = new StringBuilder();
            if (answers != null && answers.Any())
            {
                var orderLineId = answers.First().OrderLineId;
                var reasonId = answers.First().ReasonId;

                sb.AppendFormat("Collection info for OrderLine: {0}", orderLineId).AppendLine();
                sb.AppendFormat("Collection reason: {0}", reasonId).AppendLine();
                sb.AppendLine("Questions: ");
                sb.AppendLine();

                foreach (var ans in answers.GroupBy(g => new { g.OrderLineId, g.ReasonId, g.QuestionId }).ToList())
                {
                    if (ans.Count() == 1)
                    {
                        // simple case select/textbox/checkbox
                        sb.AppendFormat("* {0}: {1}", ans.First().QuestionText, string.IsNullOrEmpty(ans.First().AnswerText) ? ans.First().AnswerValue : ans.First().AnswerText).AppendLine();
                    }
                    else if (ans.Count() == 2)
                    {
                        // select + textbox
                        var sel = ans.FirstOrDefault(x => !string.IsNullOrEmpty(x.AnswerValue));
                        var text = ans.FirstOrDefault(x => !string.IsNullOrEmpty(x.AnswerText));

                        if (sel != null && text != null)
                        {
                            sb.AppendFormat("* {0}: {1} - {2}", sel.QuestionText, text.AnswerText, sel.AnswerValue).AppendLine();
                        }
                    }
                }
            }

            return sb.ToString();
        }




    }
}