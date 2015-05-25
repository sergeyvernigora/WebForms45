<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="WebApplication1.UC.WebUserControl1" %>
<script type="text/x-jquery-tmpl" id="jqTemplate">
        <div class="question" data-reasonid="${Question.CRid}" data-questionid="${Question.QuestionId}">
            <div class="qHead">
                <strong class="qTitle">${Question.QuestionData.Text}</strong>
            </div>

            {{if Question.QuestionData.QuestionType == 1}}    
            <div>
                <select  data-questionid="${Question.QuestionId}" 
                        {{each(i, option) Question.QuestionData.Options}}
                            {{if option.EnableFreeText == false}} 
                             data-enablefreetext="false"
                            {{else}}
                             data-enablefreetext="true"
                            {{/if}}
                        {{/each}}>
                    <option value="" disabled="disabled" selected="selected"><%= GetLocalResourceObject("ChooseAnOption") %></option>
                    {{each(i, option) Question.QuestionData.Options}}
                    <option value="${option.Id}" data-answerid="${option.Id}" data-enablefreetext="${option.EnableFreeText}"
                        {{if Answer && Answer.QuestionOptionID && Answer.QuestionOptionID == option.Id}} 
                        selected
                        {{/if}}
                        >${option.Text}</option>
                    {{/each}}
                </select>
                <input type="text"  maxlength="100"  data-questionid="${Question.QuestionId}"  
                        {{each(i, option) Question.QuestionData.Options}}
                            {{if option.EnableFreeText == false || (option.EnableFreeText == true && Answer == null )  }} 
                             style="display: none"
                            {{/if}}
                        {{/each}}  
                             
                        {{if Answer && Answer.QuestionOptionID  }} 
                            value="${Answer.TextAnswer}" data-answerid="${Answer.QuestionOptionID}"
                        {{/if}}>
                {{if Answer && Answer.IsValid == false }} 
                   <span class="val"><%= GetLocalResourceObject("AnswerTheQuestion") %></span> 
                {{/if}}
            </div>
            {{/if}}
        
            {{if Question.QuestionData.QuestionType == 2}} 
            <div>
                <input type="text" maxlength="100" data-questionid="${Question.QuestionId}" 
                        {{if Answer && Answer.TextAnswer}} 
                            value="${Answer.TextAnswer}"
                        {{/if}}>
                {{if Answer && Answer.IsValid == false }} 
                   <span class="val"><%= GetLocalResourceObject("AnswerTheQuestion") %></span>  
                {{/if}}
            </div>
            {{/if}}
            
            {{if Question.QuestionData.QuestionType == 3}} 
            <div>
                <input type="checkbox"   data-questionid="${Question.QuestionId}"                      
                        {{if Answer && Answer.TextAnswer && Answer.TextAnswer == true}} 
                            checked
                        {{/if}}>

            </div>
            {{/if}}
            
          </div>
</script>
<script type="text/x-jquery-tmpl" id="jqLoader">
           <img id="loader" src="/Images/loader/loader0.gif" style="padding-bottom: 8px;">
</script>




<script src="/Scripts/jquery.tmpl.js" type="text/javascript"></script>
<script type="text/javascript">
    var ajaxCache = {};


    //$('#ddlCollectionReason').change(getSubreasons);
    //$('#ddlModel').attr('disabled', true);
    //$('#ddlColour').attr('disabled', true);


    //function CheckedRadioButton() {
    //    $find("AuthorisationBehaviour").show();
    //}




    $(function () {
        $("[name$=ddlCollectionReason]").change(loadSubreasons);

        $("[name$=ddlCollectionReason]").each(function () {
            var $item = $(this);
  
            if ($item.val() > 0) {
                $item.trigger('change');
            }
        });
    });

    function loadCachedSubreasons(id, sendBox, callback) {
        if (ajaxCache[id] !== undefined) {
            callback(ajaxCache[id], sendBox);
            return;
        }
        
        $.ajax({
            type: "POST",
            url: "/CollectionReasonService.asmx/GetQuestions",
            data: id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                ajaxCache[id] = response;
                callback(response, sendBox);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    }


    function loadSubreasons() {
        var reason = $(this);
        var sendBox = $(this).closest('.reasonSandbox');
        var hidId = sendBox.find("[name$=ID]");
        var questionsDiv = sendBox.find(".questions");

        var ajaxDataKey = "{orderLineId:" + hidId.val() + ", reasonId: " + reason.val() + " }";
        
        var dumbData =  ['dumb'];
        questionsDiv.empty();
        $("#jqLoader").tmpl(dumbData).appendTo(questionsDiv);

        loadCachedSubreasons(ajaxDataKey, sendBox, loadSubreasonsCallback);
    }

    function loadSubreasonsCallback(response, sendBox) {
        var questions = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;

        var questionsDiv = sendBox.find(".questions");

        questionsDiv.empty();

        var hidAns = sendBox.find("[name$=hidQuestionAnswers]");
        var valReason = sendBox.find(".reasonVal");
        valReason.hide();

        questions = mergeAnswers(questions, hidAns.val());
        $("#jqTemplate").tmpl(questions).appendTo(questionsDiv);
        bindQuestions(questionsDiv);
        saveQuestions(questionsDiv);            
    }

    function mergeAnswers(questions, answersString) {
        if (answersString == '')
            return questions;

        var answers =  JSON.parse(answersString);
        $(questions).each(function () {
            var $item = $(this)[0];

            var result = $.grep(answers, function (e) { return e.QuestionID == $item.Question.QuestionId; });
            if (result.length > 0) {
                $item.Answer = result[0];
            }
        });

        return questions;
    }

    function bindQuestions(qDiv) {
        $(document).on('change', "#" + qDiv.attr('id') + " select", changeQuestions);
        $(document).on('change', "#" + qDiv.attr('id') + " select", saveQuestions);

        $(document).on('change', "#" + qDiv.attr('id') + " :input[type=text]", saveQuestions);
        $(document).on('change', "#" + qDiv.attr('id') + " :input[type=checkbox]", saveQuestions);
    }
    
    function changeQuestions() {
       
        var select = $(this);
        var selectedItem = $("option:selected", select);
        select.attr('data-enablefreetext', 'false');
        
        var freeText = select.siblings(':input[type=text]', select).first();

        freeText.val('');
      //  freeText.prop('required', false);
        freeText.hide();

        if (selectedItem.data('enablefreetext') === true) {
            select.attr('data-enablefreetext', true);

            freeText.show();

            freeText.attr('data-questionid', select.data('questionid'));
            freeText.attr('data-answerid', selectedItem.val());
      //      freeText.prop('required', true);
        }
    }

    function saveQuestions(qDiv) {
        var parent;
        if (!$(qDiv).hasClass(".questions")) {
            var el = $(this);
            parent = el.closest('.questions');
        } else {
            parent = qDiv;
        }
       
        var olId = parent.data('orderlineid');
        
        var textCases = $(':input[type=text]:visible', parent);
        var checkCases = $(':input[type=checkbox]:visible', parent);
        var selectCases = $('select[data-enablefreetext=false]:visible', parent);

        var answers = [];

        var textAnswers = processAnswers(textCases, olId);
        var checkAnswers = processAnswers(checkCases, olId);
        var selectAnswers = processAnswers(selectCases, olId);

        answers = $.merge(answers, textAnswers);
        answers = $.merge(answers, checkAnswers);
        answers = $.merge(answers, selectAnswers);

        var sendBox = parent.closest('.reasonSandbox');
        var hidden = sendBox.find($("[name$=hidQuestionAnswers]"));

        hidden.val(JSON.stringify(answers));
    }

    function processAnswers(coll, olId) {
        var answers = [];

        coll.each(function () {
            var $item = $(this);

            var optionId = '';
            var textAnswer = '';

            if ($item.is(':checkbox')) {
                textAnswer = $item.prop("checked");
            } else if ($item.is('select')) {
                optionId = $item.val();
            }
            else {
                optionId = $item.data('answerid');
                textAnswer = $item.val();
            }

            answers.push({
                "OLIPhysicalItemId": olId,
                "QuestionID": $item.data('questionid'),
                "QuestionOptionID": optionId,
                "TextAnswer": textAnswer
            });
        });

        return answers;
    }



 
</script>    



<asp:Repeater ID="rptProductsToBeCollected" runat="server" OnItemDataBound="ProductsToBeCollected_ItemDataBound">

    <ItemTemplate>
        <div class="reasonSandbox">
        <tr id="trOrderLine"  runat="server">
            <td id="Td1" runat="server">
                <asp:HiddenField ID="ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
            </td>
            <td id="Td2" runat="server">
                <%# DataBinder.Eval(Container.DataItem, "Description")%>
            </td>
            <td id="Td3" class='alignCenter' runat="server">

                <asp:DropDownList ID="ddlCollectionReason" runat="server"  />
                <asp:Label ID="lbCollectionReasonError" runat="server" Text="*" Visible="False" CssClass="reasonVal"></asp:Label>
                <asp:HiddenField ID="hidQuestionAnswers" runat="server" />
                <div class="questions" <%# Eval("Id", "id=ol{0} data-orderlineid={0}") %> >
                </div>
            </td>
        </tr>
             </div>
    </ItemTemplate>
</asp:Repeater>

<input type="submit">
