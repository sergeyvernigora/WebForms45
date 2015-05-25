using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using WebApplication1.UC;

namespace WebApplication1
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CollectionReasonService : WebService
    {

        public CollectionReasonService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        public class Reason
        {
            public int Id { get; set; }
            public string DisplayText { get; set; }
        }

        public enum QuestionType
        {
            Options = 1,
            Textbox = 2,
            Checkbox = 3
        }

        //public class Question
        //{
        //    public int Id { get; set; }
        //    public int ParentId { get; set; }
        //    public string DisplayText { get; set; }
        //    public List<SubReasonAnswer> Answers { get; set; }
        //}

        //public class SubReasonAnswer
        //{
        //    public int Id { get; set; }
        //    public int ParentId { get; set; }
        //    public string DisplayText { get; set; }
        //    public bool Mandatory { get; set; }
        //    public bool IsFreeText { get; set; }
        //}

        public struct CollectionReasonQuestionAnswer
        {
            public CollectionReasonQuestion Question;
            public SOCollectionAnswer Answer;
        }

        [Serializable]
        public class CollectionReasonQuestion
        {
            public virtual int Id { get; set; }
            public virtual int CRid { get; set; }
            public virtual int QuestionId { get; set; }

            //private Question _questionData;

            public virtual Question QuestionData { get; set; }

        }

        [Serializable]
        public class Question
        {
            private IList<QuestionOption> _options = new List<QuestionOption>();

            public virtual int Id { get; set; }


            public virtual string Text { get; set; }
            public virtual QuestionType QuestionType { get; set; }

            public virtual IList<QuestionOption> Options { get; set; }

            public virtual int OrderLineId { get; set; }
            public virtual int ReasonId { get; set; }

        }

        [Serializable]
        public class QuestionOption
        {
            public virtual int Id { get; set; }
            public virtual int QuestionId { get; set; }
            public virtual string Text { get; set; }
            public virtual bool EnableFreeText { get; set; }
            public virtual bool Mandatory { get; set; }
        }



        [WebMethod]
        public IList<CollectionReasonQuestionAnswer> GetQuestions(int orderLineId, int reasonId)
        {

            Thread.Sleep(2000);
          //  "[{"OLIPhysicalItemId":1,"QuestionID":2,"QuestionOptionID":1,"TextAnswer":"asd"},{"OLIPhysicalItemId":1,"QuestionID":3,"QuestionOptionID":"5","TextAnswer":""},{"OLIPhysicalItemId":1,"QuestionID":4,"QuestionOptionID":"7","TextAnswer":""}]"
            var r1 = @"[{'Question':{'Id':1,'CRid':2,'QuestionId':2,'QuestionData':{'Id':2,'Text':'What did we get wrong?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':1,'QuestionId':2,'Text':'Specification','EnableFreeText':true},{'Id':2,'QuestionId':2,'Text':'Incorrect Image','EnableFreeText':true},{'Id':3,'QuestionId':2,'Text':'Incorrect Video','EnableFreeText':true},{'Id':4,'QuestionId':2,'Text':'Incorrect dimensions','EnableFreeText':true}]}},'Answer':null},{'Question':{'Id':2,'CRid':2,'QuestionId':3,'QuestionData':{'Id':3,'Text':'Fed back to products / content team?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':5,'QuestionId':3,'Text':'Yes','EnableFreeText':false},{'Id':6,'QuestionId':3,'Text':'No - Issue already resolved','EnableFreeText':false}]}},'Answer':null},{'Question':{'Id':3,'CRid':2,'QuestionId':4,'QuestionData':{'Id':4,'Text':'Appliance Used?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':7,'QuestionId':4,'Text':'Yes','EnableFreeText':false},{'Id':8,'QuestionId':4,'Text':'No - Not plugged in','EnableFreeText':false},{'Id':9,'QuestionId':4,'Text':'No - Plugged in','EnableFreeText':false}]}},'Answer':null}]";
            var r1_a = @"[{'Question':{'Id':1,'CRid':2,'QuestionId':2,'QuestionData':{'Id':2,'Text':'What did we get wrong?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':1,'QuestionId':2,'Text':'Specification','EnableFreeText':true},{'Id':2,'QuestionId':2,'Text':'Incorrect Image','EnableFreeText':true},{'Id':3,'QuestionId':2,'Text':'Incorrect Video','EnableFreeText':true},{'Id':4,'QuestionId':2,'Text':'Incorrect dimensions','EnableFreeText':true}]}},'Answer':{'OLIPhysicalItemId':1,'QuestionID':2,'QuestionOptionID':1,'TextAnswer':'asd'}},{'Question':{'Id':2,'CRid':2,'QuestionId':3,'QuestionData':{'Id':3,'Text':'Fed back to products / content team?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':5,'QuestionId':3,'Text':'Yes','EnableFreeText':false},{'Id':6,'QuestionId':3,'Text':'No - Issue already resolved','EnableFreeText':false}]}},'Answer':null},{'Question':{'Id':3,'CRid':2,'QuestionId':4,'QuestionData':{'Id':4,'Text':'Appliance Used?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':7,'QuestionId':4,'Text':'Yes','EnableFreeText':false},{'Id':8,'QuestionId':4,'Text':'No - Not plugged in','EnableFreeText':false},{'Id':9,'QuestionId':4,'Text':'No - Plugged in','EnableFreeText':false}]}},'Answer':null}]";
            var r2 = @"[{'Question':{'Id':4,'CRid':8,'QuestionId':5,'QuestionData':{'Id':5,'Text':'Who delivered the item?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':10,'QuestionId':5,'Text':'DPD','EnableFreeText':false},{'Id':11,'QuestionId':5,'Text':'Expert','EnableFreeText':false}]}},{'Id':5,'CRid':8,'QuestionId':6,'QuestionData':{'Id':6,'Text':'Packaging condition?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':12,'QuestionId':6,'Text':'Packaging Fine','EnableFreeText':false},{'Id':13,'QuestionId':6,'Text':'Packaging Damaged','EnableFreeText':false}]}},{'Id':6,'CRid':8,'QuestionId':7,'QuestionData':{'Id':7,'Text':'Description of damage','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]}}},'Answer':null}]";
            var r3 = @"[{'Question':{'Id':7,'CRid':13,'QuestionId':9,'QuestionData':{'Id':9,'Text':'Who provided uplift Number?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':14,'QuestionId':9,'Text':'Uplift given by manufacturer','EnableFreeText':false},{'Id':15,'QuestionId':9,'Text':'Uplift given by customer - manu confirmed','EnableFreeText':false}]}},'Answer':null},{'Question':{'Id':8,'CRid':13,'QuestionId':10,'QuestionData':{'Id':10,'Text':'Serial Number','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]}},'Answer':null},{'Question':{'Id':9,'CRid':13,'QuestionId':11,'QuestionData':{'Id':11,'Text':'Uplift Number','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]}},'Answer':null},{'Question':{'Id':10,'CRid':13,'QuestionId':12,'QuestionData':{'Id':12,'Text':'Engineer / Service Centre visits','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':16,'QuestionId':12,'Text':'0','EnableFreeText':false},{'Id':17,'QuestionId':12,'Text':'1','EnableFreeText':false},{'Id':18,'QuestionId':12,'Text':'2','EnableFreeText':false},{'Id':19,'QuestionId':12,'Text':'3+','EnableFreeText':false}]}},'Answer':null},{'Question':{'Id':11,'CRid':13,'QuestionId':13,'QuestionData':{'Id':13,'Text':'Description of fault','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]}},'Answer':null},{'Question':{'Id':12,'CRid':13,'QuestionId':14,'QuestionData':{'Id':14,'Text':'Faulty process followed?','QuestionType':3,'OrderLineId':0,'ReasonId':0,'Options':[]}},'Answer':null}]";
            if (reasonId == 1)
            {
                return new JavaScriptSerializer().Deserialize<List<CollectionReasonQuestionAnswer>>(r1); 
            }
            else
            {
                return new JavaScriptSerializer().Deserialize<List<CollectionReasonQuestionAnswer>>(r3); 
            }

            //if (reasonId == 1)
            //{
            //    #region 1

            //    return new List<Question>()
            //{
            //    new Question()
            //    {
            //        Id = 1,
            //        ReasonId  = reasonId,
            //        OrderLineId = orderLineId,
            //        Text = "What did we get wrong",

            //        Options = new List<QuestionOption>()
            //        {
            //            new QuestionOption()
            //            {
            //                Id = 1,
            //                QuestionId = 1,
            //                Text = "Specification",
            //                EnableFreeText = true,
            //                Mandatory = true,
            //            },
            //            new QuestionOption()
            //            {
            //                Id = 2,
            //                QuestionId = 1,
            //                Text = "Incorrect Image",
            //                EnableFreeText = true,
            //                Mandatory = true,
            //            },
            //            new QuestionOption()
            //            {
            //                Id = 3,
            //                 QuestionId = 1,
            //                Text = "Incorrect Video",
            //                Mandatory = true,
            //            }
            //        }
            //    },
            //    new Question()
            //    {
            //        Id = 2,
            //                            ReasonId  = reasonId,
            //        OrderLineId = orderLineId,
            //        Text = "Fed back to products / content team?",
            //        Options = new List<QuestionOption>()
            //        {
            //            new QuestionOption()
            //            {
            //                Id = 1,
            //                QuestionId = 2,
            //                Text = "Yes",
            //                Mandatory = true,
            //                EnableFreeText =  false,
            //            },
            //            new QuestionOption()
            //            {
            //                Id = 2, 
            //                QuestionId = 2,
            //                Text = "No - Issue already resolved",
            //                Mandatory = true,
            //                EnableFreeText = false,
            //            },
            //        }
            //    },
            //};
            //    #endregion
            //}
            //else
            //{
            //    return new List<Question>()
            //{
            //    new Question()
            //    {
            //        Id = 4,
            //                            ReasonId  = reasonId,
            //        OrderLineId = orderLineId,
            //        Text = "Who provided uplift Number?",
            //        Options = new List<QuestionOption>()
            //        {
            //            new QuestionOption()
            //            {
            //                Id = 1,
            //                QuestionId = 4,
            //                Text = "Uplift given by manufacturer",
            //                Mandatory = true,
            //                EnableFreeText = false,
            //            },
            //            new QuestionOption()
            //            {
            //                Id = 2,
            //                QuestionId = 4,
            //                Text = "Uplift given by customer - manu confirmed",
            //                Mandatory = true,
            //                EnableFreeText = false,
            //            },
            //        }
            //    },
            //    new Question()
            //    {
            //        Id = 5,
            //                            ReasonId  = reasonId,
            //        OrderLineId = orderLineId,
            //        Text = "Serial Number",
            //    },
            //    new Question()
            //    {
            //        Id = 6,
            //                            ReasonId  = reasonId,
            //        OrderLineId = orderLineId,
            //        Text = "Description of fault",
            //        Options = new List<QuestionOption>()
            //        {
            //            new QuestionOption()
            //            {
            //                Id = 1,
            //                QuestionId = 4,
            //                Text = "N/A",
            //                Mandatory = true,
            //                EnableFreeText = true,
            //            },
            //        }
            //    },
            //    new Question()
            //    {
            //        Id = 6,
            //                           ReasonId  = reasonId,
            //        OrderLineId = orderLineId,
            //        Text = "Faulty process followed?",
            //        Options = new List<QuestionOption>()
            //        {
            //            new QuestionOption()
            //            {
            //                Id = 1,
            //               Text = "",
            //               Mandatory = true,
            //               EnableFreeText =  false,
            //            },
            //        }
            //    }
            //};
            //}
        }

        [WebMethod]
        public List<Question> GetQuestions1(int orderLineId, int reasonId)
        {
            var r1 = @"[{'Id':2,'Text':'What did we get wrong?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':1,'QuestionId':2,'Text':'Specification','EnableFreeText':true},{'Id':2,'QuestionId':2,'Text':'Incorrect Image','EnableFreeText':true},{'Id':3,'QuestionId':2,'Text':'Incorrect Video','EnableFreeText':true},{'Id':4,'QuestionId':2,'Text':'Incorrect dimensions','EnableFreeText':true}]},{'Id':3,'Text':'Fed back to products / content team?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':5,'QuestionId':3,'Text':'Yes','EnableFreeText':false},{'Id':6,'QuestionId':3,'Text':'No - Issue already resolved','EnableFreeText':false}]},{'Id':4,'Text':'Appliance Used?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':7,'QuestionId':4,'Text':'Yes','EnableFreeText':false},{'Id':8,'QuestionId':4,'Text':'No - Not plugged in','EnableFreeText':false},{'Id':9,'QuestionId':4,'Text':'No - Plugged in','EnableFreeText':false}]}]";
            var r2 = @"[{'Id':5,'Text':'Who delivered the item?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':10,'QuestionId':5,'Text':'DPD','EnableFreeText':false},{'Id':11,'QuestionId':5,'Text':'Expert','EnableFreeText':false}]},{'Id':6,'Text':'Packaging condition?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':12,'QuestionId':6,'Text':'Packaging Fine','EnableFreeText':false},{'Id':13,'QuestionId':6,'Text':'Packaging Damaged','EnableFreeText':false}]},{'Id':7,'Text':'Description of damage','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]}]";
            var r3 =
                @"{    'ReasonId':1,
    'OrderLineId':7,
    'Questions':[{'Id':9,'Text':'Who provided uplift Number?','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':14,'QuestionId':9,'Text':'Uplift given by manufacturer','EnableFreeText':false},{'Id':15,'QuestionId':9,'Text':'Uplift given by customer - manu confirmed','EnableFreeText':false}]},{'Id':10,'Text':'Serial Number','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]},{'Id':11,'Text':'Uplift Number','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]},{'Id':12,'Text':'Engineer / Service Centre visits','QuestionType':1,'OrderLineId':0,'ReasonId':0,'Options':[{'Id':16,'QuestionId':12,'Text':'0','EnableFreeText':false},{'Id':17,'QuestionId':12,'Text':'1','EnableFreeText':false},{'Id':18,'QuestionId':12,'Text':'2','EnableFreeText':false},{'Id':19,'QuestionId':12,'Text':'3+','EnableFreeText':false}]},{'Id':13,'Text':'Description of fault','QuestionType':2,'OrderLineId':0,'ReasonId':0,'Options':[]},{'Id':14,'Text':'Faulty process followed?','QuestionType':3,'OrderLineId':0,'ReasonId':0,'Options':[]}]}";

            return new JavaScriptSerializer().Deserialize<List<Question>>(r3);

            if (reasonId == 1)
            {
                #region 1

                return new List<Question>()
            {
                new Question()
                {
                    Id = 1,
                    ReasonId  = reasonId,
                    OrderLineId = orderLineId,
                    Text = "What did we get wrong",

                    Options = new List<QuestionOption>()
                    {
                        new QuestionOption()
                        {
                            Id = 1,
                            QuestionId = 1,
                            Text = "Specification",
                            EnableFreeText = true,
                            Mandatory = true,
                        },
                        new QuestionOption()
                        {
                            Id = 2,
                            QuestionId = 1,
                            Text = "Incorrect Image",
                            EnableFreeText = true,
                            Mandatory = true,
                        },
                        new QuestionOption()
                        {
                            Id = 3,
                             QuestionId = 1,
                            Text = "Incorrect Video",
                            Mandatory = true,
                        }
                    }
                },
                new Question()
                {
                    Id = 2,
                                        ReasonId  = reasonId,
                    OrderLineId = orderLineId,
                    Text = "Fed back to products / content team?",
                    Options = new List<QuestionOption>()
                    {
                        new QuestionOption()
                        {
                            Id = 1,
                            QuestionId = 2,
                            Text = "Yes",
                            Mandatory = true,
                            EnableFreeText =  false,
                        },
                        new QuestionOption()
                        {
                            Id = 2, 
                            QuestionId = 2,
                            Text = "No - Issue already resolved",
                            Mandatory = true,
                            EnableFreeText = false,
                        },
                    }
                },
            };
                #endregion
            }
            else
            {
                return new List<Question>()
            {
                new Question()
                {
                    Id = 4,
                                        ReasonId  = reasonId,
                    OrderLineId = orderLineId,
                    Text = "Who provided uplift Number?",
                    Options = new List<QuestionOption>()
                    {
                        new QuestionOption()
                        {
                            Id = 1,
                            QuestionId = 4,
                            Text = "Uplift given by manufacturer",
                            Mandatory = true,
                            EnableFreeText = false,
                        },
                        new QuestionOption()
                        {
                            Id = 2,
                            QuestionId = 4,
                            Text = "Uplift given by customer - manu confirmed",
                            Mandatory = true,
                            EnableFreeText = false,
                        },
                    }
                },
                new Question()
                {
                    Id = 5,
                                        ReasonId  = reasonId,
                    OrderLineId = orderLineId,
                    Text = "Serial Number",
                },
                new Question()
                {
                    Id = 6,
                                        ReasonId  = reasonId,
                    OrderLineId = orderLineId,
                    Text = "Description of fault",
                    Options = new List<QuestionOption>()
                    {
                        new QuestionOption()
                        {
                            Id = 1,
                            QuestionId = 4,
                            Text = "N/A",
                            Mandatory = true,
                            EnableFreeText = true,
                        },
                    }
                },
                new Question()
                {
                    Id = 6,
                                       ReasonId  = reasonId,
                    OrderLineId = orderLineId,
                    Text = "Faulty process followed?",
                    Options = new List<QuestionOption>()
                    {
                        new QuestionOption()
                        {
                            Id = 1,
                           Text = "",
                           Mandatory = true,
                           EnableFreeText =  false,
                        },
                    }
                }
            };
            }
        }
    }
}
